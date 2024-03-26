using System.Collections.Generic;

namespace SceneModel
{
    public class ImportedTags
    {
        public string[] Author { get; set; }
        public string[] Furniture { get; set; }
        public string[] Narrative { get; set; }
        public string[] Feeling { get; set; }
        public string[] Service { get; set; }
        public string[] Attribute { get; set; }
        public List<string[]> Contact { get; set; }
        public string[] Numeric { get; set; }
        public string[] ActorTypes { get; set; }
        public string[] Other { get; set; }
        public string[] Unknown { get; set; }
    }
}