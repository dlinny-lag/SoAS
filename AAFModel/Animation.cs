using System.Collections.Generic;

namespace AAFModel
{
    public sealed class Animation : Referenceable
    {
        public int Frames { get; set; }
        public List<Actor> Actors { get; } = new List<Actor>();

        public Animation(string file, int order) : base(file, order)
        {
        }

        public override ReferenceType Type => ReferenceType.Animation;
    }
}