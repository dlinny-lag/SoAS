using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SceneModel;
using SceneServices;
using Shared.Utils;

namespace ScenesEditor.Data
{
    public static class ProjectSerialization
    {
        public static readonly string ProjectFileExtension = "scproj";
        public static readonly string SceneFileExtension = "scene";
        public static readonly string ScenesPackFileExtension = "scenes";
        public static readonly string ESPFileExtension = "esl";

        private static readonly string InfoFilename = "info";
        private static readonly string ProgressFilename = "progress";

        private static readonly string ProjectListPath =
            Path.Combine(ApplicationSettings.ApplicationDataFolder, "projects.list");

        private static bool IsFolderSameTo(this Project project, string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentNullException(nameof(folder));

            if (string.IsNullOrWhiteSpace(project.Path))
                return false;
            string projectPath = Path.GetDirectoryName(project.Path)?.TrimEnd('\\');
            if (string.IsNullOrWhiteSpace(projectPath))
                return false;
            folder = folder.TrimEnd('\\');
            return string.Compare(projectPath, folder, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static void Save(this Project project, string folderPath)
        {
            if (ApplicationSettings.EditDefaultDataSetMode)
                return; // TODO: redesign to avoid mix of responsibilities here
            folderPath = folderPath ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            bool pathChanged = InitPath(project, folderPath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string tmpFile = project.Path + ".tmp";
            File.Delete(tmpFile);
            using (var storage = new ZipStorage(tmpFile, false))
            {
                project.Updated = DateTimeOffset.Now;
                Save(project, storage, true, true, false);
            }
            File.Delete(project.Path);
            File.Move(tmpFile, project.Path);
            RegisterProject(project, pathChanged);
        }

        private static bool InitPath(Project project, string folderPath)
        {
            string projectFile = Path.Combine(folderPath, $"{project.Name}.{ProjectFileExtension}");
            bool pathChanged = !string.Equals(projectFile, project.Path, StringComparison.OrdinalIgnoreCase);
            project.Path = projectFile; // project's file name should be changed when name of project changes.
            return pathChanged;
        }

        public static bool UpdateSettings(this Project project)
        {
            bool pathChanged = InitPath(project, Path.GetDirectoryName(project.Path));
            using (var storage = new ZipStorage(project.Path, false))
            {
                var infoFile = storage.FindFile(InfoFilename);
                if (infoFile == null)
                    return false;

                string json = JsonConvert.SerializeObject(project, typeof(ProjectHeader), Formatting.Indented, JsonSerialization.Default);
                storage.UpdateFile(infoFile, new MemoryStream(storage.Encoding.GetBytes(json)));
            }
            RegisterProject(project, pathChanged);
            return true;
        }

        private static byte[] ToScenesPack(this Project project)
        {
            // TODO: avoid temp file usage
            var tmpFileName = Path.GetTempFileName();
            using (var storage = new ZipStorage(tmpFileName, false))
            {
                project.Save(storage, false, true, false);
            }
            var scenesContent = File.ReadAllBytes(tmpFileName);
            File.Delete(tmpFileName);
            return scenesContent;
        }

        public static void ReleaseDefaultDataSet(this Project project, string path)
        {
            IFilesStorage fileStorage = new FileSystemFilesStorage(path, false);
            project.Save(fileStorage, true, false, true);
        }

        public static bool Release(this Project project, out string  packagePath)
        {
            string folder = Path.GetDirectoryName(project.Path);
            string file = Path.GetFileNameWithoutExtension(project.Path);

            packagePath = $"{folder}\\{file}-{project.Version.ToString(3)}.zip";
            string tmpPath = packagePath + ".tmp";
            string backup = project.Path;
            project.Path = null; // do not share path
            
            try
            {
                try
                {
                    File.Delete(tmpPath);
                    using (ZipStorage storage = new ZipStorage(tmpPath, false))
                    {
                        if (!string.IsNullOrWhiteSpace(project.EspName))
                        {
                            var eslContent = Fallout4ModsHelper.GenerateEmptyESL(project.Author, $"Version {project.Version.ToString(3)}");
                            storage.AddFile($"Data\\{project.EspName}", new MemoryStream(eslContent));
                            storage.AddFile($"Data\\Scenes\\{project.EspName}.{ScenesPackFileExtension}", new MemoryStream(project.ToScenesPack()));
                        }
                        else
                        {
                            // loose files
                            string prefix = "Data\\Scenes\\";
                            if (!string.IsNullOrWhiteSpace(project.EspName))
                                prefix = Path.Combine(prefix, project.EspName) + "\\";
                            project.Save(storage, false, false, true, prefix);
                        }
                    }
                    File.Delete(packagePath);
                    File.Move(tmpPath, packagePath);
                }
                finally
                {
                    File.Delete(tmpPath);
                }
            }
            finally
            {
                project.Path = backup;
            }
            
            // save release date on success
            project.Released = DateTimeOffset.Now;
            return project.UpdateSettings();
        }

        private static void Save(this Project project, IFilesStorage storage, bool includeProgress, bool includeInfo, bool formatted, string prefix = "")
        {
            if (!string.IsNullOrWhiteSpace(prefix) && !prefix.EndsWith("\\"))
                throw new ArgumentException("Prefix must be a folder with \\ at the end");
            
            if (includeInfo)
            {
                string json = JsonConvert.SerializeObject(project, typeof(ProjectHeader), Formatting.Indented, JsonSerialization.Default);
                storage.AddFile($"{prefix}{InfoFilename}", new MemoryStream(storage.Encoding.GetBytes(json)));
            }

            if (includeProgress)
            {
                string json = JsonConvert.SerializeObject(project.ScenesReadyForRelease, typeof(IList<string>), Formatting.Indented, JsonSerialization.Default);
                storage.AddFile($"{prefix}{ProgressFilename}", new MemoryStream(storage.Encoding.GetBytes(json)));
            }

            foreach (var scene in project.Scenes ?? Array.Empty<Scene>())
            {
                string json = scene.ToJson(formatted);
                storage.AddFile($"{prefix}{scene.Id.ValidateFilename()}.{SceneFileExtension}", new MemoryStream(storage.Encoding.GetBytes(json)));
            }
        }

        public static Project LoadDefaultDataSetFromFolder(string path)
        {
            var storage = new FileSystemFilesStorage(path, true);
            return LoadFromStorage(storage);
        }

        public static Project Load(string filePath)
        {
            using (var storage = new ZipStorage(filePath, true))
            {
                return LoadFromStorage(storage);
            }
        }

        private static Project LoadFromStorage(IFilesStorage storage)
        {
            var files = storage.GetFiles();
            if (files.Length == 0)
                return null;

            Project retVal = null;
            string[] readyForeRelease = Array.Empty<string>();
            BatchDeserializer deserializer = new BatchDeserializer(files.Length - 1);
            deserializer.Start();
            foreach (var file in files)
            {
                if (file.Name == InfoFilename)
                {
                    retVal = JsonConvert.DeserializeObject<Project>(file.ReadAllText(), JsonSerialization.Default);
                    if (retVal == null)
                    {
                        Console.WriteLine($@"Unable to deserialize {storage.Path}\{file.FullName}");
                        return null;
                    }

                    retVal.Path = storage.Path;
                    RegisterProject(retVal);
                    continue;
                }

                if (file.Name == ProgressFilename)
                {
                    readyForeRelease = JsonConvert.DeserializeObject<string[]>(file.ReadAllText(), JsonSerialization.Default);
                    continue;
                }

                if (Path.GetExtension(file.Name).TrimStart('.') != SceneFileExtension)
                    continue;

                var content = file.ReadAllText();
                deserializer.AddJson(content, file.FullName);
            }

            deserializer.NotifyAllAdded();
            retVal?.Add(deserializer.GetResult());
            retVal?.SetReadyForRelease(readyForeRelease);
            return retVal;
        }

        public static ProjectHeader[] List(bool ignoreCache = true)
        {
            if (!ignoreCache && cache != null)
                return cache.ToArray();

            if (!File.Exists(ProjectListPath))
                return Array.Empty<ProjectHeader>();

            string json = File.ReadAllText(ProjectListPath);
            var projects = JsonConvert.DeserializeObject<ProjectHeader[]>(json, JsonSerialization.Default);
            if (projects == null)
                throw new JsonSerializationException("Unable to read projects list file");
            cache = new List<ProjectHeader>(projects);
            return projects;
        }

        private static List<ProjectHeader> cache;

        private static void RegisterProject(ProjectHeader header, bool ignoreCache = true)
        {
            if (ApplicationSettings.EditDefaultDataSetMode)
                return; // TODO: redesign to avoid mix of responsibilities here

            if (string.IsNullOrWhiteSpace(header.Path))
                throw new ArgumentException("Project's file path is undefined");
            
            List<ProjectHeader> existing = new List<ProjectHeader>(List(ignoreCache));
            bool found = false;
            for (int i = 0; i < existing.Count; i++)
            {
                if (string.Compare(existing[i].Path, header.Path, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    found = true;
                    existing[i] = header;
                    break;
                }
            }
            if (!found)
                existing.Add(header);

            string json = JsonConvert.SerializeObject(existing.ToArray(), typeof(ProjectHeader[]), Formatting.Indented, JsonSerialization.Default);
            Directory.CreateDirectory(Path.GetDirectoryName(ProjectListPath));
            File.WriteAllText(ProjectListPath, json);

            cache = existing;
        }

        public static IList<ProjectHeader> UnregisterProject(ProjectHeader project)
        {
            if (cache == null)
                throw new InvalidOperationException("Projects list is not ready yet");
            for(int i = 0; i < cache.Count; i++)
            {
                if (string.Equals(cache[i].Path, project.Path, StringComparison.OrdinalIgnoreCase))
                {
                    cache.RemoveAt(i);
                    string json = JsonConvert.SerializeObject(cache.ToArray(), typeof(ProjectHeader[]), Formatting.Indented, JsonSerialization.Default);
                    File.WriteAllText(ProjectListPath, json);
                    break;
                }
            }

            return cache.ToArray();
        }
    }
}