using System;
using System.Collections.Generic;
using System.Linq;
using SceneModel;
using SceneModel.Creatures;

namespace SceneServices.TagCategories
{
    public static class CategoriesExtension
    {

        public static readonly string SAMPrefix = "SF:".ToUpperInvariant();

        private static bool IsNumeric(this string tag)
        {
            return tag.StartsWith(NumericCategory.StimPrefix, StringComparison.InvariantCultureIgnoreCase) ||
                   tag.StartsWith(NumericCategory.DomPrefix, StringComparison.InvariantCultureIgnoreCase) ||
                   tag.StartsWith(NumericCategory.HeldPrefix, StringComparison.InvariantCultureIgnoreCase) ||
                   tag.StartsWith(NumericCategory.LovePrefix, StringComparison.InvariantCultureIgnoreCase);
        }
        
        private sealed class CategorizerContact : GenericActorsContact<ContactDetails>
        {
        
        }
        

        private static bool IsContact(this string tag)
        {
            tag = tag.ToUpperInvariant();
            return tag.TryParseFromThemeTag<CategorizerContact, ContactDetails>(null, out _);
        }

        private static bool IsAAFInformerContact(this string tag)
        {
            return tag.TryParseFromAAFInformerTag<CategorizerContact, ContactDetails>(null, out _);
        }
        public static TagCategoryTypes GetCategory(this string tag)
        {
            if (simpleMap.TryGetValue(tag.ToUpperInvariant(), out var category))
                return category;
            if (tag.StartsWith(SAMPrefix, StringComparison.InvariantCultureIgnoreCase))
                return TagCategoryTypes.SAMEnhancedAnimations;
            if (tag.IsNumeric())
                return TagCategoryTypes.Numeric;
            if (tag.IsContact())
                return TagCategoryTypes.Contact;
            if (tag.IsAAFInformerContact())
                return TagCategoryTypes.AAFInformer;
            if (tag.IsActorTypes())
                return TagCategoryTypes.ActorTypes;

            return TagCategoryTypes.Unknown;
        }

        static CategoriesExtension()
        {
            simpleMap = new Dictionary<string, TagCategoryTypes>();

            var categories = new Category[]
            {
                Feeling.Instance,
                Narrative.Instance,
                Author.Instance,
                Furniture.Instance,
                Service.Instance,
                Attribute.Instance,
            };

            foreach (Category category in categories)
            {
                foreach (string tag in category.Tags)
                {
                    simpleMap.Add(tag, category.Type);
                }
            }
        }
        private static readonly Dictionary<string, TagCategoryTypes> simpleMap;


        public static void FillCategories(this Scene scene)
        {
            List<string> authors = new List<string>();
            List<string> furniture = new List<string>();
            List<string> narrative = new List<string>();
            List<string> feeling = new List<string>();
            List<string> service = new List<string>();
            List<string> attribute = new List<string>();
            List<List<string>> contact = new List<List<string>>();
            List<string> numeric = new List<string>();
            List<string> actorTypes = new List<string>();
            List<string> other = new List<string>();
            List<string> unknown = new List<string>();

            foreach (var tags in scene.NormalizedTags)
            {
                List<string> contactTags = new List<string>();
                foreach (string tag in tags)
                {
                    switch (tag.GetCategory())
                    {
                        case TagCategoryTypes.Author:
                            authors.Add(tag);
                        break;
                        case TagCategoryTypes.Furniture:
                            furniture.Add(tag);
                        break;
                        case TagCategoryTypes.Narrative:
                            narrative.Add(tag);
                            break;
                        case TagCategoryTypes.Feeling:
                            feeling.Add(tag);
                            break;
                        case TagCategoryTypes.Service:
                            service.Add(tag);
                            break;
                        case TagCategoryTypes.Attribute:
                            attribute.Add(tag);
                            break;
                        case TagCategoryTypes.Numeric:
                            numeric.Add(tag);
                            break;
                        case TagCategoryTypes.ActorTypes:
                            actorTypes.Add(tag);
                            break;
                        case TagCategoryTypes.Contact:
                        case TagCategoryTypes.AAFInformer:
                            contactTags.Add(tag);
                            break;
                        case TagCategoryTypes.SAMEnhancedAnimations:
                            other.Add(tag);
                            break;
                        case TagCategoryTypes.Unknown:
                            unknown.Add(tag);
                            break;
                        default:
                            throw new NotImplementedException("Undefined tag category");
                    }
                }
                if (contactTags.Count > 0)
                    contact.Add(contactTags);
            }

            scene.Imported.Author = authors.ToHashSet().ToArray();
            scene.Imported.Furniture = furniture.ToHashSet().ToArray();
            scene.Imported.Narrative = narrative.ToHashSet().ToArray();
            scene.Imported.Feeling = feeling.ToHashSet().ToArray();
            scene.Imported.Service = service.ToHashSet().ToArray();
            scene.Imported.Attribute = attribute.ToHashSet().ToArray();
            scene.Imported.Numeric = numeric.ToArray();
            scene.Imported.ActorTypes = actorTypes.ToHashSet().ToArray();
            scene.Imported.Contact = contact.Select(contacts => contacts.ToArray()).ToList();
            scene.Imported.Other = other.ToHashSet().ToArray();
            scene.Imported.Unknown = unknown.ToHashSet().ToArray();
        }

        public static void FillAttributes(this Scene scene)
        {
            scene.Authors = scene.Imported.Author ?? Array.Empty<string>();
            scene.Narrative = scene.Imported.Narrative ?? Array.Empty<string>();
            scene.Feeling = scene.Imported.Feeling ?? Array.Empty<string>();
            scene.Service = scene.Imported.Service ?? Array.Empty<string>();
            scene.Attribute = scene.Imported.Attribute ?? Array.Empty<string>();
            scene.Other = scene.Imported.Other ?? Array.Empty<string>();

            var tags = new List<string>();
            if (scene.Tags != null)
                tags.AddRange(tags);
            if (scene.Imported.Unknown != null)
                tags.AddRange(scene.Imported.Unknown); // all unrecognized tags moved to tags
            scene.Tags = tags.ToArray();
        }
    }
}