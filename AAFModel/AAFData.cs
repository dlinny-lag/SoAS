using System.Collections.Generic;

namespace AAFModel
{
    public class AAFData
    {
        public Dictionary<string, Defaults> Files { get; } = new Dictionary<string, Defaults>();
        public Dictionary<string, LoadException> FailedFiles { get; } = new Dictionary<string, LoadException>();

        public Dictionary<string, IList<string>> Warnings { get; } = new Dictionary<string, IList<string>>();
        public Dictionary<string, IList<string>> Errors { get; } = new Dictionary<string, IList<string>>();

        public void AddError(string fileName, string message)
        {
            if (!Errors.TryGetValue(fileName, out var errors))
            {
                errors = new List<string>();
                Errors.Add(fileName, errors);
            }
            errors.Add(message);
        }

        public void AddWarning(string fileName, string message)
        {
            if (!Warnings.TryGetValue(fileName, out var warnings))
            {
                warnings = new List<string>();
                Warnings.Add(fileName, warnings);
            }
            warnings.Add(message);
        }

        public List<Race> Races { get; } = new List<Race>();
        public Dictionary<string, FurnitureGroup> Furniture { get; } = new Dictionary<string, FurnitureGroup>();
        public List<Animation> Animations { get;} = new List<Animation>();
        public List<AnimationGroup> AnimationGroups { get; } = new List<AnimationGroup>();
        public List<Position> Positions { get; } = new List<Position>();
        public List<PositionTree> PositionTrees { get; } = new List<PositionTree>();
        public List<TagsEntry> Tags { get; } = new List<TagsEntry>();
    }
}