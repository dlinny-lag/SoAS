using System.Collections.Generic;

namespace AAFModel
{
    public sealed class Furniture
    {
        public Furniture(int formId, string filename)
        {
            BaseFormId = formId;
            ModFileName = filename;
        }
        public int BaseFormId { get; }
        public string ModFileName { get;}
    }

    public sealed class FurnitureGroup
    {
        public FurnitureGroup(string name)
        {
            Name = name;
        }
        public string Name { get; }
        public List<Furniture> Furnitures { get; } = new List<Furniture>();
    }
}