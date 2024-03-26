using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SceneModel;
using Shared.Utils;

namespace ScenesEditor.Data
{
    public class ProjectHeader
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Released { get; set; }
        public Version Version { get; set; } = new Version(1, 0, 0, 0);
        public string EspName { get; set; }

        [JsonProperty]
        internal string Path { get; set; }
    }

    public sealed class Project : ProjectHeader
    {
        private readonly HashSet<string> sceneNames = new HashSet<string>();

        private readonly HashSet<string> readyForRelease = new HashSet<string>();

        [Ignore]
        public IList<string> ScenesReadyForRelease
        {
            get
            {
                List<string> ids = new List<string>(readyForRelease);
                ids.Sort();
                return ids;
            }
        } 

        public bool IsReadyForRelease(Scene scene)
        {
            if (scene == null)
                return false;
            return readyForRelease.Contains(scene.Id);
        }
        public void SetReadyForRelease(params string[] sceneIDs)
        {
            readyForRelease.Clear();
            readyForRelease.AddRange(sceneIDs);
        }
        public void MarkReadyForRelease(Scene scene)
        {
            readyForRelease.Add(scene.Id);
        }

        public void UnmarkReadyForRelease(Scene scene)
        {
            readyForRelease.Remove(scene.Id);
        }

        [Ignore]
        public ICollection<Scene> Scenes { get; private set; }

        public int Add(ICollection<Scene> scenes)
        {
            if (Scenes == null)
                Scenes = new List<Scene>();
            int added = 0;
            foreach (Scene newScene in scenes)
            {
                if (!sceneNames.Add(newScene.Id))
                    continue;
                Scenes.Add(newScene);
                ++added;
            }
            if (added > 0)
                Updated = DateTimeOffset.Now;
            return added;
        }

        public int Add(params Scene[] scenes)
        {
            return Add((ICollection<Scene>)scenes);
        }

        public int Remove(ICollection<Scene> toDelete)
        {
            if (Scenes == null)
                return 0;
            int deleted = 0;
            foreach (Scene scene in toDelete)
            {
                if (!sceneNames.Remove(scene.Id))
                    continue;
                Scenes.Remove(Scenes.First(s => s.Id == scene.Id));
                UnmarkReadyForRelease(scene);
                ++deleted;
            }

            if (deleted > 0)
                Updated = DateTimeOffset.Now;
            return deleted;
        }

        public int Remove(params Scene[] toDelete)
        {
            return Remove((ICollection<Scene>)toDelete);
        }
    }
}