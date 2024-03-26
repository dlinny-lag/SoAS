using System.Collections.Generic;
using AAF.Services.Errors;

namespace AAF.Services.AAFImport
{
    public class AAFImportErrors
    {
        public readonly IDictionary<string, AnimationDuplicationError> AnimationDuplications;
        public readonly IDictionary<string, AnimationGroupDuplicationError> AnimationGroupDuplications;
        public readonly IDictionary<string, PositionDuplicationError> PositionDuplications;
        public readonly IDictionary<string, PositionTreeDuplicationError> PositionTreeDuplications;
        public readonly IDictionary<string, RaceDuplicationError> RaceDuplications;
        public readonly IDictionary<string, List<PositionError>> ValidationErrors;

        public AAFImportErrors(
            IDictionary<string, AnimationDuplicationError> animationDuplications,
            IDictionary<string, AnimationGroupDuplicationError> animationGroupDuplications,
            IDictionary<string, PositionDuplicationError> positionDuplications,
            IDictionary<string, PositionTreeDuplicationError> positionTreeDuplications,
            IDictionary<string, RaceDuplicationError> raceDuplications,
            IDictionary<string, List<PositionError>> validationErrors)
        {
            AnimationDuplications = animationDuplications;
            AnimationGroupDuplications = animationGroupDuplications;
            PositionDuplications = positionDuplications;
            PositionTreeDuplications = positionTreeDuplications;
            RaceDuplications = raceDuplications;
            ValidationErrors = validationErrors;

        }

    }
}