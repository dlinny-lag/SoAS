using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using AAFModel;
using Shared.Utils;

namespace AAF.Services
{
    public class AAFReader
    {
        private static readonly char[] comma = ",".ToCharArray();

        private readonly IFilesStorage aafFolder;
        public AAFReader(IFilesStorage storage)
        {
            aafFolder = storage;
        }
        
        private static Action<AAFData, XmlDocument, string> GetFileHandler(IFileDescriptor fi)
        {
            if (!fi.EndsWith(".xml"))
                return null;

            if (fi.Has("animationData")) return ProcessAnimationData;
            if (fi.Has("animationGroupData")) return ProcessAnimationGroupData;
            if (fi.Has("positionData")) return ProcessPositionData;
            if (fi.Has("positionTree")) return ProcessPositionTreeData;
            if (fi.Has("raceData")) return ProcessRaceData;
            if (fi.Has("furnitureData")) return ProcessFurnitureData;
            if (fi.Has("tagData")) return ProcessTagData;
            return ProcessAny;
        }

        public AAFData ReadAll(bool showXmlErrors)
        { // TODO: optimize
            AAFData retVal = new AAFData();
            foreach (IFileDescriptor fi in aafFolder.GetFiles())
            {
                var handler = GetFileHandler(fi);
                if (handler == null)
                    continue; // not a file to proceed
                string fileName = fi.FullName;

                XmlDocument doc;
                LoadException error = null;
                try
                {
                    string content = File.ReadAllText(fileName);
                    doc = XmlFileLoader.LoadString(content, ref error);
                    if (doc == null)
                        continue; // empty file. silently skip it
                }
                catch (LoadException e)
                {
                    retVal.FailedFiles.Add(fileName, e);
                    continue;
                }

                if (error != null && showXmlErrors)
                {
                    retVal.FailedFiles.Add(fileName, error);
                }
                Defaults defs = GetDefaults(doc);
                retVal.Files.Add(fileName, defs);
                handler(retVal, doc, fileName);
            }
            
            retVal.Animations.Sort(new AAFEntityComparer<Animation>(retVal.Files));
            retVal.AnimationGroups.Sort(new AAFEntityComparer<AnimationGroup>(retVal.Files));
            retVal.Positions.Sort(new AAFEntityComparer<Position>(retVal.Files));
            retVal.PositionTrees.Sort(new AAFEntityComparer<PositionTree>(retVal.Files));
            retVal.Races.Sort(new AAFEntityComparer<Race>(retVal.Files));

            return retVal;
        }

        static Defaults GetDefaults(XmlDocument doc)
        {
            Defaults retVal = new Defaults();
            foreach (var element in doc.Elements("defaults"))
            {
                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.TryMap("source", source => retVal.Source = source)) continue;
                    if (attr.TryMapInt("loadPriority", priority => retVal.Priority = priority)) continue;
                    if (attr.TryMap("skeleton", skeleton => retVal.Skeleton = skeleton)) continue;
                    if (attr.TryMap("idleSource", idleSource => retVal.IdleSource = idleSource)) continue;
                }

                return retVal;
            }

            return retVal;
        }
        static void ProcessAnimationData(AAFData data, XmlDocument doc, string fileName)
        {
            int order = 0;
            foreach (XmlElement element in doc.Elements("animation"))
            {
                if (!element.TryGetId(out string animId))
                {
                    data.AddWarning(fileName, $"Invalid animation declaration detected");
                    continue;
                }

                Animation animation = new Animation(fileName, order++){Id = animId};
                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.TryMapInt("frames", (frames) => animation.Frames = frames))
                        break;
                }
                foreach (XmlNode actorNode in element)
                {
                    if (actorNode.Name != "actor") 
                        continue;

                    Actor actor = new Actor();

                    if (actorNode.Attributes != null)
                    {
                        foreach (XmlAttribute attr in actorNode.Attributes)
                        {
                            if (attr.TryMap("gender", (gender)=> actor.Gender = gender)) continue;
                            if (attr.TryMap("skeleton", (skeleton)=> actor.Skeleton = skeleton)) continue;
                            if (attr.TryMap("race", (race)=> actor.Race = race)) continue;
                        }
                    }

                    foreach (XmlElement child in actorNode)
                    {
                        if (child.Name == "idle")
                        {
                            foreach (XmlAttribute attr in child.Attributes)
                            {
                                if (attr.TryMapHex("form", (formId) => actor.IdleFormId = formId)) break;
                            }
                            break;
                        }
                    }
                        
                    animation.Actors.Add(actor);
                }

                data.Animations.Add(animation);
            }
        }
        static void ProcessAnimationGroupData(AAFData data, XmlDocument doc, string fileName)
        {
            int order = 0;
            foreach (XmlElement element in doc.Elements("animationGroup"))
            {
                if (!element.TryGetId(out string groupId))
                {
                    data.AddWarning(fileName, $"Invalid animation group declaration detected");
                    continue;
                }

                AnimationGroup group = new AnimationGroup(fileName, order++){Id = groupId};

                foreach (XmlElement node in element)
                {
                    if (node.Name != "stage") 
                        continue;

                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        if (attr.Name == "animation")
                        {
                            group.Animations.Add(attr.Value);
                            break;
                        }
                    }
                }

                data.AnimationGroups.Add(group);
            }
        }
        static void ProcessPositionData(AAFData data, XmlDocument doc, string fileName)
        {
            int order = 0;
            foreach (XmlElement element in doc.Elements("position"))
            {
                if (!element.TryGetId(out string positionId))
                {
                    data.AddWarning(fileName, $"Invalid position declaration detected");
                    continue;
                }

                Position position = new Position(fileName, order++) { Id = positionId };
                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.TryMap("location", locations => position.Locations = locations.Split(comma,StringSplitOptions.RemoveEmptyEntries))) continue;
                    if (attr.TryMap("tags", tagsValue =>
                        {
                            var tags = tagsValue.Split(comma, StringSplitOptions.RemoveEmptyEntries).Intern();
                            if (tags.Length > 0)
                                position.Tags.Add(tags);
                        })
                    ) continue;
                    if (attr.TryMap("isHidden", isHidden => position.IsHidden = isHidden)) continue;
                    if (attr.TryMap("animation", animation => { position.Reference = animation; position.ReferenceType = ReferenceType.Animation;})) continue;
                    if (attr.TryMap("animationGroup", animGroup => { position.Reference = animGroup; position.ReferenceType = ReferenceType.AnimationGroup;})) continue;
                    if (attr.TryMap("positionTree", tree => { position.Reference = tree; position.ReferenceType = ReferenceType.PositionTree;})) continue;
                }

                if (string.IsNullOrWhiteSpace(position.Reference))
                {
                    position.Reference = positionId;
                    position.ReferenceType = ReferenceType.None; 
                }

                data.Positions.Add(position);
            }
        }
        static void ProcessPositionTreeData(AAFData data, XmlDocument doc, string fileName)
        {
            int order = 0;
            foreach (XmlElement element in doc.Elements("tree"))
            {
                if (!element.TryGetId(out string treeId))
                    return; // TODO: invalid definition?
                PositionTree tree = new PositionTree(fileName, order++) { Id = treeId };
                var children = GetChildren(element);
                if (children.Count != 1)
                {
                    if (children.Count > 1)
                        data.AddError(fileName, $"Positions tree [{treeId}] has more than one root branch");
                    else if (children.Count == 0)
                        data.AddWarning(fileName, $"Positions tree [{treeId}] hasn't any branch");
                    continue; 
                }

                tree.Root = children[0];
                data.PositionTrees.Add(tree);
            }
        }

        static List<PositionItem> GetChildren(XmlElement parent)
        {
            List<PositionItem> retVal = new List<PositionItem>();
            foreach (XmlElement branch in parent)
            {
                if (branch.Name == "branch")
                {
                    PositionItem item = new PositionItem();
                    foreach (XmlAttribute attr in branch.Attributes)
                    {
                        if (attr.TryMap("id", branchId => item.BranchId = branchId)) continue;
                        attr.TryMap("positionID", positionId => item.PositionId = positionId);
                    }

                    item.Children = GetChildren(branch);
                    retVal.Add(item);
                }
            }

            return retVal;
        }

        static void ProcessRaceData(AAFData data, XmlDocument doc, string fileName)
        {
            int order = 0;
            foreach (XmlElement element in doc.Elements("race"))
            {
                if (!element.TryGetId(out string raceId))
                {
                    data.AddWarning(fileName, $"Invalid race declaration detected");
                    continue;
                }

                Race race = new Race(fileName, order++){Id = raceId};
                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.TryMap("skeleton", skeleton => race.Skeleton = skeleton)) continue;
                    if (attr.TryMapHex("form", formId => race.FormId = formId)) continue;
                }
                
                data.Races.Add(race);
            }
        }
        static void ProcessFurnitureData(AAFData data, XmlDocument doc, string fileName)
        {
            Defaults defs = GetDefaults(doc);
            foreach (XmlElement element in doc.Elements("group"))
            {
                if (!element.TryGetId(out string furnitureGroupName))
                {
                    data.AddWarning(fileName, $"Invalid furniture declaration detected");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(furnitureGroupName))
                {
                    data.AddError(fileName, $"Empty furniture group name detected");
                    continue;
                }

                if (!data.Furniture.TryGetValue(furnitureGroupName.ToUpperInvariant(), out var furnitureGroup))
                {
                    furnitureGroup = new FurnitureGroup(furnitureGroupName);
                    data.Furniture.Add(furnitureGroupName.ToUpperInvariant(), furnitureGroup);
                }

                if (!element.HasChildNodes)
                {
                    data.AddWarning(fileName, $"No any form id defined in furniture group [{furnitureGroupName}]");
                    continue;
                }

                foreach (XmlElement furnitureNode in element)
                {
                    if (furnitureNode.Name != "furniture")
                        continue;

                    string source = null;
                    string formIdStr = null;
                    foreach (XmlAttribute attr in furnitureNode.Attributes)
                    {

                        if (attr.TryMap("form", (val) => formIdStr = val))
                            continue;
                        attr.TryMap("source", (val) => source = val);
                    }

                    if (formIdStr == null)
                    {
                        data.AddError(fileName, $"Missing furniture's form id detected");
                        continue;
                    }

                    if (!int.TryParse(formIdStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int formId))
                    {
                        data.AddError(fileName, $"Invalid furniture's form id [{formIdStr}]");
                        continue;
                    }

                    int normalizedFormId = formId.NormalizeFormId();
                    if (normalizedFormId != formId)
                    {
                        data.AddWarning(fileName, $"Malformed furniture form id [{formId:X}]. Considering [{normalizedFormId:X}]");
                    }
                    formId = normalizedFormId;

                    source = source ?? defs.Source;

                    if (source == null)
                    {
                        data.AddError(fileName, $"Can't get source file for furniture '{formId}'");
                        continue;
                    }
                    furnitureGroup.Furnitures.Add(new Furniture(formId, source));
                }
            }
        }
        static void ProcessTagData(AAFData data, XmlDocument doc, string fileName)
        {
            int order = 0;
            foreach (XmlElement element in doc.Elements("tag"))
            {
                TagsEntry tag = new TagsEntry(fileName, order++);
                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.TryMap("position", positionId => tag.Id = positionId)) continue;
                    if (attr.TryMap("tags", tags => tag.Tags = tags.Split(comma, StringSplitOptions.RemoveEmptyEntries).Intern())) continue;
                }

                if (tag.Id == null)
                    continue; // <equipmentRulesData> contains <tag> entry that has different meaning
                
                data.Tags.Add(tag);
            }
        }

        static void ProcessAny(AAFData data, XmlDocument doc, string fileName)
        {
            ProcessAnimationData(data, doc, fileName);
            ProcessAnimationGroupData(data, doc, fileName);
            ProcessPositionData(data, doc, fileName);
            ProcessPositionTreeData(data, doc, fileName);
            ProcessRaceData(data, doc, fileName);
            ProcessFurnitureData(data, doc, fileName);
            ProcessTagData(data, doc, fileName);
        }
    }
}
