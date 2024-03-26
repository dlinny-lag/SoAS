using System;
using System.Collections.Generic;
using System.Linq;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;
using Shared.Utils;

namespace AAF.Services.AAFImport
{
    public class PositionValidator
    {
        private readonly IDictionary<string, Position> positions;
        private readonly IDictionary<string, Animation> animations;
        private readonly IDictionary<string, AnimationGroup> animGroups;
        private readonly IDictionary<string, PositionTree> posTrees;
        private readonly IDictionary<string, Race> races;
        private readonly IDictionary<string, Defaults> defaults;
        private readonly IDictionary<string, FurnitureGroup> furniture;
        private readonly IDictionary<string, PositionDuplicationError> duplicates;
        public PositionValidator(
            IDictionary<string, Position> positions,
            IDictionary<string, Animation> animations,
            IDictionary<string, AnimationGroup> animGroups,
            IDictionary<string, PositionTree> posTrees,
            IDictionary<string, Race> races,
            IDictionary<string, Defaults> defaults,
            IDictionary<string, FurnitureGroup> furniture,
            IDictionary<string, PositionDuplicationError> duplicates )
        {
            this.positions = positions;
            this.animations = animations;
            this.animGroups = animGroups;
            this.posTrees = posTrees;
            this.races = races;
            this.defaults = defaults;
            this.furniture = furniture;
            this.duplicates = duplicates;
        }

        public IDictionary<string, List<PositionError>> ValidatePositions()
        {
            Dictionary<string, List<PositionError>> retVal = new Dictionary<string, List<PositionError>>();

            AnimationCompatibilityComparer comparer = new AnimationCompatibilityComparer(defaults, races);

            foreach (var position in positions.Values)
            {
                var errors = ValidatePosition(position, comparer);
                if (errors.Count > 0)
                    retVal.Add(position.Id, errors);
            }

            // validate trees
            foreach (var position in positions.Values)
            {
                if (position.ReferenceType != ReferenceType.PositionTree)
                    continue;

                var errors = ValidateTree(position, comparer, retVal);
                if (errors == null || errors.Count <= 0) 
                    continue;
                
                if (retVal.TryGetValue(position.Id, out var existing))
                    existing.AddRange(errors);
                else
                    retVal.Add(position.Id, errors);
            }

            return retVal;
        }

        private List<PositionError> ValidatePosition(Position position, AnimationCompatibilityComparer comparer)
        {
            List<PositionError> errors = new List<PositionError>();

            var missingFurniture = MissingFurniture(position);
            if (missingFurniture?.Count > 0)
                errors.Add(new MissingFurnitureError(position.Id, missingFurniture));

            PositionError err = ResolveReferenceType(position);
            if (err != null)
            {
                errors.Add(err);
                return errors;
            }

            ValidateReferencePhaseOne(position, comparer, errors);

            return errors;
        }

        private void ValidateReferencePhaseOne(Position position, AnimationCompatibilityComparer comparer, List<PositionError> errors)
        {
            PositionError err;
            switch (position.ReferenceType)
            {
                case ReferenceType.Animation:
                    err = ValidateAnimation(position);
                    if (err != null)
                        errors.Add(err);
                    break;
                case ReferenceType.AnimationGroup:
                    err = ValidateGroup(position, comparer);
                    if (err != null)
                        errors.Add(err);
                    break;
                case ReferenceType.PositionTree:
                    // skip. will be checked when all positions processed
                    break;
                default:
                    throw new InvalidOperationException("Undefined reference type");
            }
        }

        private List<string> MissingFurniture(Position position)
        {
            if (position.Locations == null)
                return null;

            List<string> missingFurniture = new List<string>();
            foreach (string location in position.Locations)
            {
                if (!furniture.ContainsKey(location.ToUpperInvariant()))
                    missingFurniture.Add(location);
            }

            return missingFurniture;
        }

        private PositionError ResolveReferenceType(Position position)
        {
            if (position.ReferenceType != ReferenceType.None)
                return null;

            // we need to resolve reference in position declaration

            animations.TryGetValue(position.Reference, out var animation);
            animGroups.TryGetValue(position.Reference, out var group);
            posTrees.TryGetValue(position.Reference, out var posTree);
            List<Referenceable> resolved = new List<Referenceable>(3);
            if (animation != null)
            {
                resolved.Add(animation);
            }

            if (group != null)
            {
                resolved.Add(group);
            }

            if (posTree != null)
            {
                resolved.Add(posTree);
            }

            if (resolved.Count == 0)
            {
                return new MissingReferenceError(position.Id, position.Id);
            }

            if (resolved.Count > 1)
            {
                return new AmbiguousReferenceError(position.Id, resolved.ToArray());
            }

            // resolve reference type
            position.ReferenceType = resolved[0].Type;

            // NOTE: it is possible that duplicates are identical to resolved position.
            if (duplicates.TryGetValue(position.Id, out var err))
            {
                err.UpdateReference(position); // NOTE: err stores the unmodified copy of this reference. see AAFImportHelper.Positions method
            }

            return null; // no error
        }

        private PositionError ValidateAnimation(Position position)
        {
            if (!animations.TryGetValue(position.Reference, out var animation))
                return new MissingReferenceError(position.Id, position.Reference, ReferenceType.Animation);
            if (!defaults.TryGetValue(animation.File, out var animDefault))
                animDefault = Defaults.Default;
            foreach (Actor actor in animation.Actors)
            {
                if (actor.GetSkeleton(animDefault, races) == null)
                    return new SkeletonUndefinedError(position.Id, animation.Id);
            }
            return null; // TODO: any other checks? missing ESP?
        }

        private PositionError ValidateGroup(Position position, AnimationCompatibilityComparer comparer)
        {
            if (!animGroups.TryGetValue(position.Reference, out var group))
                return new MissingReferenceError(position.Id, position.Reference, ReferenceType.AnimationGroup);
            List<string> missingAnims = new List<string>();
            List<Animation> groupAnimations = new List<Animation>();
            for (int i = 0; i < group.Animations.Count; i++)
            {
                if (!animations.TryGetValue(group.Animations[i], out var anim))
                    missingAnims.Add(group.Animations[i]);
                else
                    groupAnimations.Add(anim);
            }

            if (missingAnims.Count > 0)
                return new GroupMissingAnimation(position.Id, group.Id, missingAnims);

            var incompatibility = AreAnimationsCompatible(groupAnimations, comparer);
            if (!incompatibility.IsNone())
                return new GroupIncompatibleAnimationsError(position.Id, group.Id, incompatibility);

            return null; // TODO: any other checks?
        }

        public static AnimationCompatibilityDifference AreAnimationsCompatible(IList<Animation> animations, AnimationCompatibilityComparer comparer)
        {
            if (animations.Count <= 1)
                return AnimationCompatibilityDifference.None;
            var reference = animations[0];
            AnimationCompatibilityDifference retVal = AnimationCompatibilityDifference.None;
            for (int i = 1; i < animations.Count; i++)
            {
                retVal |= comparer.Same(reference, animations[i]);
            }

            return retVal;
        }
        private List<PositionError> ValidateTree(Position position, AnimationCompatibilityComparer comparer, IDictionary<string, List<PositionError>> badPositions)
        {
            if (!posTrees.TryGetValue(position.Reference, out PositionTree posTree))
                return new List<PositionError> {new MissingReferenceError(position.Id, position.Reference, ReferenceType.PositionTree)};

            List<BadPositionsTreeError> errors = new List<BadPositionsTreeError>();
            List<Animation> collectedAnimations = new List<Animation>();
            VerifyTree(position, posTree, posTree.Root, badPositions, errors, collectedAnimations);

            var diff = AreAnimationsCompatible(collectedAnimations, comparer);
            if (!diff.IsNone())
                errors.Add(new TreeIncompatibleAnimationsError(position.Id, position.Reference, diff));

            // TODO: any other checks?

            if (errors.Count > 0)
                return errors.Select(bpErr => (PositionError)bpErr).ToList();

            return null;
        }
        


        private void VerifyTree(Position position, PositionTree posTree, PositionItem root, IDictionary<string, List<PositionError>> badPositions, List<BadPositionsTreeError> collectedErrors, List<Animation> collectedAnimations)
        {
            if (!positions.TryGetValue(root.PositionId, out var treePosition))
                collectedErrors.Add(new TreeHasMissingPositionError(position.Id, posTree.Id, root.PositionId));
            else
            {
                if (treePosition.ReferenceType != ReferenceType.Animation && treePosition.ReferenceType != ReferenceType.AnimationGroup)
                    collectedErrors.Add(new TreeHasInvalidPositionError(position.Id, posTree.Id, treePosition.Id, treePosition.ReferenceType));
                else
                {
                    switch (treePosition.ReferenceType)
                    {
                        case ReferenceType.Animation:
                            if (animations.TryGetValue(treePosition.Reference, out var anim))
                                collectedAnimations.Add(anim);
                            break;
                        case ReferenceType.AnimationGroup:
                            if (animGroups.TryGetValue(treePosition.Reference, out var group))
                                foreach (var animId in group.Animations)
                                    if (animations.TryGetValue(animId, out var animInGroup))
                                        collectedAnimations.Add(animInGroup);
                            break;
                    }
                }

                if (badPositions.TryGetValue(treePosition.Id, out var errs) && errs.Any(e => e is MissingReferenceError))
                    collectedErrors.Add(new TreeHasPositionWithMissingReference(position.Id, posTree.Id, treePosition.Id));

                if (treePosition.Id == position.Id)
                    collectedErrors.Add(new PositionsCircleError(position.Id, posTree.Id, root));
            }
            

            foreach (PositionItem child in root.Children)
            {
                VerifyTree(position, posTree, child, badPositions, collectedErrors, collectedAnimations);
            }
        }
    }
}