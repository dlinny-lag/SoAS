using System;
using System.Collections.Generic;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;
using Shared.Utils;

namespace AAF.Services.AAFImport
{
    public static class AAFImportHelper
    {
        internal static Sex ToSex(this string gender)
        {
            return gender.ToSex<Sex>();
        }
        public static string GetSkeleton(this Actor a, Defaults defaults, IDictionary<string, Race> races)
        {
            if (!string.IsNullOrWhiteSpace(a.Skeleton))
                return a.Skeleton;
            if (a.Race != null && races.TryGetValue(a.Race, out var race))
                return race.Skeleton;
            return defaults.Skeleton;
        }

        public static IDictionary<string, Race> Races(this AAFData aafModel, out Dictionary<string, RaceDuplicationError> duplicates)
        {
            return aafModel.Races.Collect<Race, RaceDifference, RaceDuplicationError>((id, race) => new RaceDuplicationError(id, race), out duplicates);
        }

        public static IDictionary<string, Animation> Animations(this AAFData aafModel, out Dictionary<string, AnimationDuplicationError> duplicates)
        {
            return aafModel.Animations.Collect<Animation, AnimationDifference, AnimationDuplicationError>((id, animation) => new AnimationDuplicationError(id, animation), out duplicates);
        }

        public static IDictionary<string, AnimationGroup> AnimationGroups(this AAFData aafModel, out Dictionary<string, AnimationGroupDuplicationError> duplicates)
        {
            return aafModel.AnimationGroups.Collect<AnimationGroup, AnimationGroupDifference, AnimationGroupDuplicationError>((id, group) => new AnimationGroupDuplicationError(id, group), out duplicates);
        }

        public static IDictionary<string, Position> Positions(this AAFData aafModel, out Dictionary<string, PositionDuplicationError> duplicates)
        {
            // we may update reference Position object in place, so use the copy that will remain untouched
            return aafModel.Positions.Collect<Position, PositionDifference, PositionDuplicationError>((id, p) => new PositionDuplicationError(id, p.Clone()), out duplicates);
        }

        public static IDictionary<string, PositionTree> PositionTrees(this AAFData aafModel, out Dictionary<string, PositionTreeDuplicationError> duplicates)
        {
            return aafModel.PositionTrees.Collect<PositionTree, PositionItemDifference, PositionTreeDuplicationError>((id, pt) => new PositionTreeDuplicationError(id, pt), out duplicates);
        }

        private static SequentialDictionary<string, T> Collect<T, TDiff, TDup>(this List<T> data, Func<string, T, TDup> constructor, out Dictionary<string, TDup> duplicates)
            where T: Declared
            where TDiff : Enum
            where TDup: DuplicationError<T, TDiff>
        {
            var retVal = new SequentialDictionary<string, T>();
            duplicates = new Dictionary<string, TDup>();
            foreach (T a in data)
            {
                if (retVal.TryGetValue(a.Id, out T existing))
                {
                    if (!duplicates.TryGetValue(a.Id, out var dupErr))
                    {
                        dupErr = constructor(a.Id, existing); // first collected object is the reference
                        duplicates.Add(a.Id, dupErr);
                    }
                    dupErr.AddDuplicates(a);
                    continue; // first collected object will be used
                }
                retVal.Add(a.Id, a);
            }

            return retVal;
        }

        public static Dictionary<string, List<string[]>> MergeTags(this AAFData aafModel)
        {
            Dictionary<string, List<string[]>> retVal = new Dictionary<string, List<string[]>>();
            foreach (TagsEntry t in aafModel.Tags)
            {
                if (!retVal.TryGetValue(t.Id, out var merged))
                {
                    merged = new List<string[]>();
                    retVal.Add(t.Id, merged);
                }
                merged.Add(t.Tags);
            }

            foreach (Position p in aafModel.Positions)
            {
                if (p.Tags == null)
                    continue;

                if (!retVal.TryGetValue(p.Id, out var merged))
                {
                    merged = new List<string[]>();
                    retVal.Add(p.Id, merged);
                }
                merged.AddRange(p.Tags);
            }

            return retVal;
        }

        public static Dictionary<string, HashSet<string>> MergeLocations(this AAFData aafModel)
        {
            Dictionary<string, HashSet<string>> retVal = new Dictionary<string, HashSet<string>>();
            foreach (Position p in aafModel.Positions)
            {
                if (p.Locations == null)
                    continue;
                if (!retVal.TryGetValue(p.Id, out var merged))
                {
                    merged = new HashSet<string>();
                    retVal.Add(p.Id, merged);
                }
                merged.AddRange(p.Locations);
            }

            return retVal;
        }

        public static Position Clone(this Position position)
        {
            return new Position(position.File, position.Order)
            {
                Id = position.Id,
                IsHidden = position.IsHidden,
                Locations = position.Locations,
                Tags = position.Tags,
                Reference = position.Reference,
                ReferenceType = position.ReferenceType,
            };
        }
    }
}