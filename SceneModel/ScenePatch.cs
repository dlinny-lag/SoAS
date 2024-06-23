using System;
using System.Collections.Generic;

namespace SceneModel
{
    public enum CollectionChangeType
    {
        None = 0, // invalid
        Add = 1,
        Remove = 2,
        Patch = 3
    }

    public enum IdentifierType
    {
        None, // invalid 
        Property,
        Index,
        PK
    }

    public struct Identifier
    {
        public IdentifierType Type;
        public string Value;
    }
    
    public class Change
    {
        
    }

    public class ScenePatch
    {
        public string SceneId { get; set; }
        public List<Change> Changes { get; set; } = new List<Change>();
    }

    public static class ScenePatchExtension
    {
        private static void ValidateScenesCompatibility(Scene a, Scene b)
        {
            if (a.Id != b.Id)
                throw new InvalidOperationException("Scene IDs mismatch");
            if (a.Type != b.Type)
                throw new InvalidOperationException("Scene types mismatch");
            if (a.Participants.Count != b.Participants.Count)
                throw new InvalidOperationException("Scene participants mismatch");
            for (int i = 0; i < a.Participants.Count; i++)
            {
                Participant ap = a.Participants[i];
                Participant bp = b.Participants[i];
                if (!ap.Skeleton.Equals(bp.Skeleton, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Participant skeleton mismatch");
                if (ap.Sex != bp.Sex)
                    throw new InvalidOperationException("Participant sex mismatch");
            }
        }

        public static ScenePatch GeneratePatch(this Scene original, Scene changed)
        {
            ValidateScenesCompatibility(original, changed);
            throw new NotImplementedException();
        }
    }
}