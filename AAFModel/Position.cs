using System.Collections.Generic;

namespace AAFModel
{
    public enum ReferenceType
    {
        None = 0, // not defined
        Animation,
        AnimationGroup,
        PositionTree,
    }

    public sealed class Position : Declared
    {
        /// <summary>
        /// Name of <see cref="Animation"/>, <see cref="AnimationGroup"/> or <see cref="PositionTree"/>>
        /// </summary>
        public string Reference { get; set; }
        public ReferenceType ReferenceType { get; set; }
        /// <summary>
        /// names of furniture
        /// </summary>
        public string[] Locations { get; set; }
        public bool IsHidden { get; set; }
        public List<string[]> Tags { get; set; } = new List<string[]>();

        public Position(string file, int order) : base(file, order)
        {
        }
    }
}