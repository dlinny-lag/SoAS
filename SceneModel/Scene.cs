using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Shared.Utils;

namespace SceneModel
{
    /// <summary>
    /// Combination of data from AAF's Position and Animation/AnimationGroup/PositionTree entries
    /// </summary>
    [DebuggerDisplay("{DebugDisplay}")]
    public sealed class Scene
    {
        public string Id { get; set; }
        public SceneType Type { get; set; }

        public HashSet<string> Furniture { get; set; } = new HashSet<string>();
        public IList<Participant> Participants { get; set; }
        public List<ActorsContact> ActorsContacts { get; set; } = new List<ActorsContact>();

        /// <summary>
        /// Contacts from environment. Furniture or some "virtual" devices that are not included to scene definition
        /// </summary>
        public List<EnvironmentContact> EnvironmentContacts { get; set; } = new List<EnvironmentContact>();

        private List<string[]> rawTags;
        public List<string[]> RawTags
        {
            get => rawTags;
            set
            {
                rawTags = value;
                normalizedTags = null;
            }
        }

        #region Attributes
        public string[] Authors { get; set; }
        public string[] Narrative { get; set; }
        public string[] Feeling { get; set; }
        public string[] Service { get; set; }
        public string[] Attribute { get; set; }
        public string[] Other { get; set; }
        #endregion

        public CustomAttributes Custom { get; set; }

        public string[] Tags { get; set; }

        public ImportedTags Imported { get; set; } = new ImportedTags();

        private List<string[]> normalizedTags;
        
        [Ignore]
        public List<string[]> NormalizedTags
        {
            get
            {
                if (rawTags == null)
                    return null;
                if (normalizedTags != null) 
                    return normalizedTags;

                normalizedTags = new List<string[]>();
                foreach (string[] tags in rawTags)
                {
                    string[] normalized = new string[tags.Length];
                    for(int i = 0; i < tags.Length; i++)
                    {
                        normalized[i] = string.Intern(tags[i].ToUpperInvariant());
                    }
                    normalizedTags.Add(normalized);
                }

                return normalizedTags;
            }
        }

        private string debug;
        private string DebugDisplay
        {
            get
            {
                if (debug != null)
                    return debug;
                StringBuilder sb = new StringBuilder();
                sb.Append(Id).Append(": ");
                if (NormalizedTags != null)
                {
                    List<string> tags = (new HashSet<string>(NormalizedTags.SelectMany(tagValues => tagValues))).ToList();
                    tags.Sort();
                    for (int i = 0; i < tags.Count; i++)
                    {
                        if (i > 0)
                            sb.Append(",");
                        sb.Append(tags[i]);
                    }
                }

                debug = sb.ToString();
                return debug;
            }
        }

        public void MakeAlive()
        {
            foreach (var ac in ActorsContacts)
            {
                ac.MakeAlive(Participants);
            }

            foreach (var ec in EnvironmentContacts)
            {
                ec.MakeAlive(Participants);
            }
        }

        public void ShallowInitFromOther(Scene other)
        {
            if (!string.IsNullOrEmpty(other.Id) && Id != other.Id)
                throw new InvalidOperationException("Id mismatch");
            Type = other.Type;
            Furniture = other.Furniture;
            Participants = other.Participants;
            ActorsContacts = other.ActorsContacts;
            EnvironmentContacts = other.EnvironmentContacts;
            rawTags = other.rawTags;
            normalizedTags = other.normalizedTags;
            debug = other.debug;
            Authors = other.Authors;
            Imported = other.Imported;
            Tags = other.Tags;
            Narrative = other.Narrative;
            Feeling = other.Feeling;
            Service = other.Service;
            Attribute = other.Attribute;
            Other = other.Other;
            Custom = other.Custom;
            MakeAlive();
        }
    }
}