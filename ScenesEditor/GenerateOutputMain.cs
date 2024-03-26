using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SceneModel;
using ScenesEditor.Data;
using SceneServices;
using Shared.Utils;

namespace ScenesEditor
{
    public static class GenerateOutputMain
    {
        /// <summary>
        /// expected folders structure
        /// %FALLOUT 4%\
        ///   Data\
        ///     Scenes\ - scenes data
        ///       tools\ - this .exe
        /// </summary>

#if DEBUG
        public static readonly string DataPath =
 @"c:\Program Files (x86)\Steam\steamapps\common\Fallout 4\Data\Scenes\";
#else
        static string GetExecutableDir()
        {
            return Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        }
        public static readonly string DataPath = Path.GetDirectoryName(GetExecutableDir());
#endif

        private static string[] OrderedMods()
        {
            var appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var pluginsFile = Path.Combine(appDataLocal, "Fallout4", "Plugins.txt");
            if (!File.Exists(pluginsFile))
                return Array.Empty<string>();
            var lines = File.ReadAllLines(pluginsFile);
            return lines.Where(l => l.StartsWith("*")).Select(l => l.TrimStart('*').ToUpperInvariant()).ToArray();
        }

        /// <summary>
        /// returns false if scenes are incompatible
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private static bool MergeFromOther(this Scene self, Scene other)
        {
            if (!string.IsNullOrEmpty(other.Id) && self.Id != other.Id)
                return false;
            self.Furniture.AddRange(other.Furniture); // merge
            self.Participants = other.Participants; // overwrite, TODO: verify that participants are same
            self.ActorsContacts = other.ActorsContacts; // overwrite
            self.EnvironmentContacts = other.EnvironmentContacts; // overwrite
            self.RawTags = other.RawTags; // overwrite, TODO: merge?
            // normalizedTags - ignore
            // Imported - ignore
            self.Authors = self.Authors.Union(other.Authors).Distinct().ToArray(); // merge
            self.Tags = self.Tags.Union(other.Tags).Distinct().ToArray(); // merge
            self.Custom = self.Custom.Merge(other.Custom, true);
            return true;
        }

        private static Dictionary<string, Scene> ReadFromPack(string path)
        {
            Dictionary<string, Scene> retVal = new Dictionary<string, Scene>();
            if (!File.Exists(path))
                return retVal;
            using (var storage = new ZipStorage(path, true))
            {
                ReadFromStorage(storage, retVal);
            }
            return retVal;
        }

        private static Dictionary<string, Scene> ReadFromLooseFiles(string path)
        {
            Dictionary<string, Scene> retVal = new Dictionary<string, Scene>();
            if (!Directory.Exists(path))
                return retVal;
            var storage = new FileSystemFilesStorage(path, true);
            ReadFromStorage(storage, retVal);
            return retVal;
        }

        private static void ReadFromStorage(IFilesStorage storage, Dictionary<string, Scene> retVal)
        {
            foreach (var file in storage.GetFiles())
            {
                if (!file.Name.EndsWith(ProjectSerialization.SceneFileExtension))
                    continue;
                Scene scene = file.ReadAllText().ToScene();
                retVal.Add(scene.Id.ToUpperInvariant(), scene);
            }
        }

        private static void Merge(this Dictionary<string, Scene> self, Dictionary<string, Scene> other)
        {
            foreach (var pair in other)
            {
                if (self.TryGetValue(pair.Key, out var scene))
                {
                    if (!scene.MergeFromOther(pair.Value))
                        Console.WriteLine($@"Failed to merge scene {pair.Key}");
                }
                else
                    self.Add(pair.Key, pair.Value);
            }
        }

        private static void WriteToFile(this Dictionary<string, Scene> scenes, string outputFile)
        {
            try
            {
                using (FileStream stream = File.OpenWrite(outputFile))
                {
                    stream.Write(scenes.Values);
                }
            }
            catch (Exception e)
            {
                File.Delete(outputFile);
                using (FileStream stream = File.OpenWrite(outputFile))
                {
                    stream.WriteErrors(e.ToString());
                }
            }
        }

        public static void Run(string outputPath)
        {
            var result = ReadFromPack(Path.Combine(DataPath, "Default"));
            var allMods = OrderedMods();
            for (int i = 0; i < allMods.Length; i++)
            {
                string path = Path.Combine(DataPath, $"{allMods[i]}.{ProjectSerialization.ScenesPackFileExtension}");
                if (!File.Exists(path))
                    continue;
                var pack = ReadFromPack(path);
                result.Merge(pack);
            }

            var loose = ReadFromLooseFiles(DataPath);
            result.Merge(loose);

            File.Delete(outputPath);
            result.WriteToFile(outputPath);
        }
    }
}