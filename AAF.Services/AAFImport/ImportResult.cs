using System.Collections.Generic;
using AAFModel;

namespace AAF.Services.AAFImport
{
    public class ImportResult
    {
        public readonly IDictionary<string, List<string[]>> AllTags;
        public readonly IDictionary<string, HashSet<string>> MergedLocations;
        public readonly IDictionary<string, Animation> Animations;
        public readonly IDictionary<string, AnimationGroup> AnimationGroups;
        public readonly IDictionary<string, Position> Positions;
        public readonly IDictionary<string, PositionTree> PositionsTrees;
        public readonly IDictionary<string, Race> Races;

        public readonly AAFImportErrors Errors;

        private ImportResult(IDictionary<string, List<string[]>> tags, IDictionary<string, HashSet<string>> mergedLocations, IDictionary<string, Animation> animations, IDictionary<string, AnimationGroup> animationGroups, IDictionary<string, Position> positions, IDictionary<string, PositionTree> positionsTrees, IDictionary<string, Race> races, AAFImportErrors errors)
        {
            AllTags = tags;
            MergedLocations = mergedLocations;
            Animations = animations;
            AnimationGroups = animationGroups;
            Positions = positions;
            PositionsTrees = positionsTrees;
            Races = races;
            Errors = errors;
        }

        public static ImportResult Import(AAFData aafModel)
        {
            Dictionary<string, List<string[]>> mergedTags = aafModel.MergeTags();
            Dictionary<string, HashSet<string>> mergedLocations = aafModel.MergeLocations();
            IDictionary<string, Animation> animations = aafModel.Animations(out var animationDuplications);
            IDictionary<string, AnimationGroup> animGroups = aafModel.AnimationGroups(out var animGroupDuplications);
            IDictionary<string, Position> positions = aafModel.Positions(out var positionDuplications);
            IDictionary<string, PositionTree> posTrees = aafModel.PositionTrees(out var posTreeDuplications);
            IDictionary<string, Race> races = aafModel.Races(out var racesDuplications);

            var validator = new PositionValidator(positions, animations, animGroups, posTrees, races, aafModel.Files, aafModel.Furniture, positionDuplications);
            var errors = validator.ValidatePositions();

            return new ImportResult(mergedTags, mergedLocations, animations, animGroups, positions, posTrees, races,
                new AAFImportErrors(animationDuplications, animGroupDuplications, positionDuplications,
                    posTreeDuplications, racesDuplications, errors));
        }
    }
}