using System.Collections.Generic;

namespace AAFModel
{
    public sealed class AnimationGroup : Referenceable
    {
        public List<string> Animations { get; } = new List<string>();

        public AnimationGroup(string file, int order) : base(file, order)
        {
        }

        public override ReferenceType Type => ReferenceType.AnimationGroup;
    }
}