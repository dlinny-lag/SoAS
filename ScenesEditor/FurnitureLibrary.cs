using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AAFModel;
using Newtonsoft.Json;
using ScenesEditor.Data;
using SceneServices;
using Shared.Controls;
using Shared.Utils;

namespace ScenesEditor
{
    // TODO: move to shared utils?

    public enum ModFileStatus
    {
        Normal = 0,
        ESL = 1,
        Unknown = 2
    }

    [DebuggerDisplay("{DebugView}")]
    public readonly struct ModFile : IComparable<ModFile>
    {
        public ModFile(string filename, ModFileStatus status)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));
            ModFilename = filename;
            Status = status;
        }
        public readonly string ModFilename;
        public readonly ModFileStatus Status;
        public override bool Equals(object obj)
        {
            if (!(obj is ModFile other))
                return false;
            return Equals(other);
        }

        public bool Equals(ModFile other)
        {
            return ModFilename == other.ModFilename && Status == other.Status;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ModFilename != null ? ModFilename.GetHashCode() : 0) * 397) ^ Status.GetHashCode();
            }
        }

        private const string ESLMark = ",ESL";
        private const string UnknownMark = ",UNK";
        public int CompareTo(ModFile other)
        {
            int byName = ModFilename.CompareTo(other.ModFilename);
            if (byName != 0)
                return byName;
            return Status.CompareTo(other.Status);
        }

        public override string ToString()
        {
            switch (Status)
            {
                case ModFileStatus.ESL: return ModFilename + ESLMark;
                case ModFileStatus.Unknown: return ModFilename + UnknownMark;
            }
            return ModFilename;
        }
        public string DebugView => ToString();

        public static ModFile FromString(string formatted)
        {
            int markIndex = formatted.LastIndexOf(ESLMark, StringComparison.Ordinal);
            if (markIndex > 0)
            {
                if (markIndex != formatted.Length - ESLMark.Length)
                    throw new ArgumentOutOfRangeException(nameof(formatted), $"Invalid mod file name format. [{formatted}]");
                return new ModFile(formatted.Substring(0, formatted.Length - ESLMark.Length), ModFileStatus.ESL);
            }
            markIndex = formatted.LastIndexOf(UnknownMark, StringComparison.Ordinal);
            if (markIndex > 0)
            {
                if (markIndex != formatted.Length - UnknownMark.Length)
                    throw new ArgumentOutOfRangeException(nameof(formatted), $"Invalid mod file name format. [{formatted}]");
                return new ModFile(formatted.Substring(0, formatted.Length - ESLMark.Length), ModFileStatus.Unknown);
            }

            return new ModFile(formatted, ModFileStatus.Normal);
        }
    }

    [DebuggerDisplay("{FullView}")]
    public sealed class FormItem : IComparable<FormItem>
    {
        public FormItem(ModFile source, int formId)
        {
            Source = source;
            FormId = formId;
        }
        public ModFile Source { get; }
        public int FormId { get; }
        public List<string> Tags { get; } = new List<string>();

        public void AddTag(string tag)
        {
            if (Tags.Contains(tag))
                return;
            Tags.Add(tag);
        }

        public string IdTagsView
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{FormId:X8}");
                for (int i = 0; i < Tags.Count; i++)
                {
                    sb.Append(i == 0 ? ": " : ", ");
                    sb.Append(Tags[i]);
                }

                return sb.ToString();
            }
        }

        public string SourceIdView
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Source.ToString()).Append("-").Append($"{FormId:X8}");
                return sb.ToString();
            }
        }

        public string FullView
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Source.ToString()).Append("-").Append(IdTagsView);
                return sb.ToString();
            }
        }

        public int CompareTo(FormItem other)
        {
            return FormId.CompareTo(other.FormId); // ascending
        }
    }

    class ModFileStatusResolver
    {
        private readonly Dictionary<string, ModFileStatus> statusCache = new Dictionary<string, ModFileStatus>();
        public string Fallout4Path { get; set; } = FoldersExtension.FindFo4Folder();

        public ModFileStatus FindStatus(string modFileName)
        {
            string ext = Path.GetExtension(modFileName).ToUpperInvariant();
            if (ext == ".ESL")
                return ModFileStatus.ESL;
            if (ext == ".ESM")
                return ModFileStatus.Normal;
            if (string.IsNullOrWhiteSpace(Fallout4Path))
                return ModFileStatus.Unknown;

            if (statusCache.TryGetValue(modFileName.ToUpperInvariant(), out ModFileStatus status))
                return status;

            bool isEsl = Path.Combine(Fallout4Path, "Data", modFileName).IsESLFile(out bool found);
            if (isEsl)
                status = ModFileStatus.ESL;
            else
                status = found ? ModFileStatus.Normal : ModFileStatus.Unknown;

            statusCache.Add(modFileName.ToUpperInvariant(), status);
            return status;
        }
    }

    public class FurnitureLibrary
    {
        private readonly ModFileStatusResolver statusResolver = new ModFileStatusResolver();

        public string Fallout4Path
        {
            get => statusResolver.Fallout4Path;
            set => statusResolver.Fallout4Path = value;
        }

        private readonly Dictionary<ModFile, Dictionary<int, FormItem>> furniture = new Dictionary<ModFile, Dictionary<int, FormItem>>();
        private Dictionary<string, Dictionary<ModFile, List<FormItem>>> groupByTag = new Dictionary<string, Dictionary<ModFile, List<FormItem>>>();

        public List<ModFile> Sources => furniture.Keys.ToList();

        public List<FormItem> GetForms(ModFile source)
        {
            if (!furniture.TryGetValue(source, out var items))
                return null;
            return items.Values.ToList();
        }

        public List<string> Tags => groupByTag.Keys.ToList();

        public IDictionary<ModFile, List<FormItem>> GetForms(string tag)
        {
            tag = tag.ToUpperInvariant();
            groupByTag.TryGetValue(tag, out var result);
            return result; // TODO: make readonly
        }

        /// <summary>
        /// init from AAF's furniture data model.
        /// </summary>
        /// <param name="groups"></param>
        public void Register(IEnumerable<FurnitureGroup> groups)
        {
            foreach (FurnitureGroup gr in groups)
            {
                string tag = gr.Name;
                foreach (Furniture furn in gr.Furnitures)
                {
                    ModFileStatus status = statusResolver.FindStatus(furn.ModFileName);
                    ModFile file = new ModFile(furn.ModFileName, status);
                    if (!furniture.TryGetValue(file, out var items))
                    {
                        if (status != ModFileStatus.Unknown)
                        {   // perhaps mod file was appear. try to detect it
                            ModFile withUnkStatus = new ModFile(furn.ModFileName, ModFileStatus.Unknown);
                            if (furniture.TryGetValue(withUnkStatus, out var existing))
                            { // detected! file status changed from unknown to actual
                                // 1. make a copy of existing items, but with the new file reference
                                items = new Dictionary<int, FormItem>(existing.Count);
                                foreach (var form in existing.Values)
                                {
                                    var copy = new FormItem(file, form.FormId);
                                    copy.Tags.AddRange(form.Tags);
                                    items.Add(copy.FormId, copy);
                                }
                                // 2. remove old entry
                                furniture.Remove(withUnkStatus);
                            }
                            else
                            {
                                items = new Dictionary<int, FormItem>();
                            }
                        }
                        else
                        {
                            items = new Dictionary<int, FormItem>();
                        }

                        furniture.Add(file, items);
                    }

                    if (!items.TryGetValue(furn.BaseFormId, out var item))
                    {
                        item = new FormItem(file, furn.BaseFormId);
                        items.Add(item.FormId, item);
                    }
                    item.Tags.Add(tag);
                }
            }
            UpdateGroupByTag();
        }

        private sealed class SerializableFormItem
        {
            public int SourceIndex { get; set; }
            public int FormId { get; set; }
            public List<int> TagIndices { get; set; } = new List<int>();
        }
        private sealed class SerializableLibrary
        {
            public int Version { get; set; } = 1;
            public List<string> Sources { get; set; } = new List<string>();
            public List<string> Tags { get; set; } = new List<string>();
            public List<SerializableFormItem> Items { get; set; } = new List<SerializableFormItem>();
        }

        private static readonly string StoragePath = Path.Combine(ApplicationSettings.ApplicationDataFolder, "furniture.json");

        public void Save()
        {
            var toSave = ToSerializable();
            string json = JsonConvert.SerializeObject(toSave, JsonSerialization.Formatted);
            File.WriteAllText(StoragePath, json);
        }

        private SerializableLibrary ToSerializable()
        {
            SerializableLibrary toSave = new SerializableLibrary();
            toSave.Sources.AddRange(furniture.Keys.Select(s => s.ToString()));
            toSave.Sources.Sort();
            HashSet<string> allTags = furniture.Values.SelectMany(p => p.Values.SelectMany(item => item.Tags)).ToHashSet();
            toSave.Tags.AddRange(allTags);
            toSave.Tags.Sort();
            foreach (FormItem item in furniture.Values.SelectMany(p => p.Values))
            {
                SerializableFormItem savingItem = new SerializableFormItem
                {
                    FormId = item.FormId,
                    SourceIndex = toSave.Sources.IndexOf(item.Source.ToString()),
                    TagIndices = item.Tags.Select(t => toSave.Tags.IndexOf(t)).ToList(),
                };
                toSave.Items.Add(savingItem);
            }

            return toSave;
        }

        public void Load()
        {
            if (!File.Exists(StoragePath))
                return;
            string json = File.ReadAllText(StoragePath);
            var deserialized = JsonConvert.DeserializeObject<SerializableLibrary>(json);
            if (deserialized == null)
                return; // TODO: handle this case
            if (deserialized.Version != 1)
                return; // unable to decode from future version
            furniture.Clear(); // TODO: debatable
            Register(deserialized, out bool statusUpdated);
            if (statusUpdated)
                Save();
        }

        private void Register(SerializableLibrary deserialized, out bool altered)
        {
            altered = false;
            foreach (var savedItem in deserialized.Items)
            {
                ModFile file = ModFile.FromString(deserialized.Sources[savedItem.SourceIndex]);
                ModFileStatus actualStatus = statusResolver.FindStatus(file.ModFilename);
                if (file.Status == ModFileStatus.Unknown && file.Status != actualStatus)
                {
                    altered = true;
                    file = new ModFile(file.ModFilename, actualStatus);
                }
                if (!furniture.TryGetValue(file, out var items))
                {
                    items = new Dictionary<int, FormItem>();
                    furniture.Add(file, items);
                }

                if (!items.TryGetValue(savedItem.FormId, out var item))
                {
                    item = new FormItem(file, savedItem.FormId);
                    items.Add(item.FormId, item);
                }

                foreach (int tagIndex in savedItem.TagIndices)
                {
                    item.AddTag(deserialized.Tags[tagIndex]);
                }
            }
            UpdateGroupByTag();
        }

        private void UpdateGroupByTag()
        {
            const string noTags = "";
            var result = new Dictionary<string, Dictionary<ModFile, List<FormItem>>>();

            void AddByTag(FormItem item, string tag)
            {
                tag = tag.ToUpperInvariant();
                if (!result.TryGetValue(tag, out var bySource))
                {
                    bySource = new Dictionary<ModFile, List<FormItem>>();
                    result.Add(tag, bySource);
                }

                if (!bySource.TryGetValue(item.Source, out var items))
                {
                    items = new List<FormItem>();
                    bySource.Add(item.Source, items);
                }
                items.Add(item);
            }

            foreach (FormItem item in furniture.Values.SelectMany(p => p.Values))
            {
                if (item.Tags.Count == 0)
                {
                    AddByTag(item, noTags);
                }
                foreach (string tag in item.Tags)
                {
                    AddByTag(item, tag);
                }
            }

            groupByTag = result;
        }

        public void Release(string outputPath)
        {
            var tmpPath = Path.GetTempFileName();
            using (ZipStorage storage = new ZipStorage(tmpPath, false))
            {
                Export(storage, storage.Encoding);
            }

            File.Delete(outputPath);
            File.Move(tmpPath, outputPath);
            File.Delete(tmpPath);
        }

        public const string FurnitureSourceFileExt = ".furn";
        private const char IdTagsSeparator = ':';
        private const char TagsSeparator = ',';
        private void Export(IFilesStorage storage, Encoding encoding)
        {
            foreach (var pair in furniture)
            {
                using (var stream = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(stream, encoding))
                    {
                        foreach (var formItem in pair.Value.Values)
                        {
                            sw.Write($"{formItem.FormId:X8}{IdTagsSeparator}");
                            bool added = false;
                            foreach (var tag in formItem.Tags)
                            {
                                if (added)
                                    sw.Write(TagsSeparator);
                                else
                                    added = true;
                                sw.Write(tag);
                            }

                            sw.WriteLine();
                        }

                        string path = $@"Data\Scenes\Furniture\{pair.Key.ToString()}{FurnitureSourceFileExt}";
                        sw.Flush();
                        stream.Position = 0;
                        storage.AddFile(path.ValidateFilename(), stream);
                    }
                }
            }
        }

        private static readonly char[] NewLineSeparator = "\n\r".ToCharArray();
        public static FurnitureLibrary Import(IFilesStorage storage)
        {
            FurnitureLibrary retVal = new FurnitureLibrary();
            foreach (var fd in storage.GetFiles())
            {
                if (string.Compare(Path.GetExtension(fd.Name), FurnitureSourceFileExt, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;
                
                string source = fd.Name.Substring(0, fd.Name.Length - FurnitureSourceFileExt.Length);
                ModFile file = ModFile.FromString(source);

                Dictionary<int, FormItem> items = new Dictionary<int, FormItem>();
                
                retVal.furniture.Add(file, items);

                string content = fd.ReadAllText();
                string[] lines = content.Split(NewLineSeparator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(IdTagsSeparator);
                    if (parts.Length != 2)
                    {
                        Console.WriteLine($"Invalid line in {fd.FullName}: {line}");
                        continue;
                    }

                    if (!int.TryParse(parts[0], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int formId))
                    {
                        Console.WriteLine($"Invalid form id in {fd.FullName}: {parts[0]}");
                        continue;
                    }
                    var tags = parts[1].Split(TagsSeparator);
                    FormItem fi = new FormItem(file, formId);
                    fi.Tags.AddRange(tags);
                    items.Add(fi.FormId, fi);
                }
            }

            return retVal;
        }
    }
}