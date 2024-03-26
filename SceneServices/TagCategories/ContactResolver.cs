using System;
using System.Collections.Generic;
using System.Globalization;
using SceneModel;
using SceneModel.ContactAreas;
using SceneModel.Creatures;

namespace SceneServices.TagCategories
{
    
    public static class ContactResolver
    {
        public const string NullToSelf = "NULLTOSELF";

        private static readonly Dictionary<string, ContactArea[]> knownFrom = new Dictionary<string, ContactArea[]>()
        {
            {"FEET", new ContactArea[]{Foot.Any}},
            {"TONGUE", new ContactArea[]{Tongue.Any}},
            {"MOUTH", new ContactArea[]{Mouth.Any}},
            {"OVIPOS",new ContactArea[]{Ovipositor.Any}},
            {"LEFTHAND", new ContactArea[]{Hand.Any, Arm.Left}},
            {"HAND", new ContactArea[]{Hand.Any}},
            {"TENTACLE", new ContactArea[]{SingleTentacle.Any}},
            {"PENIS", new ContactArea[]{Penis.Any}},
            {"VAGINA", new ContactArea[]{Vagina.Any}},
            {"STRAPON", new ContactArea[]{Strapon.Any}},
            {"SPANK", new ContactArea[]{Hand.Any}},
            {"STICK", new ContactArea[]{Stick.Any}},
        };

        private static readonly Dictionary<string,ContactArea[]> knownTo = new Dictionary<string, ContactArea[]>
        {
            {"ANUS", new ContactArea[]{Anus.Any}},
            {"BUTT", new ContactArea[]{Butt.Any}},
            {"FEET", new ContactArea[]{Foot.Any}},
            {"FOOT", new ContactArea[]{Foot.Any}},
            {"HAND", new ContactArea[]{Hand.Any}},
            {"MOUTH", new ContactArea[]{Mouth.Any}},
            {"NIPPLE", new ContactArea[]{Nipple.Any}},
            {"NIPPLES", new ContactArea[]{Nipple.Any}},
            {"TIT", new ContactArea[]{Breast.Any}},
            {"PENIS", new ContactArea[]{Penis.Any}},
            {"VAGINA", new ContactArea[]{Vagina.Any}},
            {"EITHER", new ContactArea[]{Either.Any}},
            {"STICK", new ContactArea[]{Stick.Any}},
            {"FACE", new ContactArea[]{Face.Any}},
        };

        private const string TO = "TO";
        private const string InformerSeparator = ":";
        private static readonly char[] IndicesSeparator = "-".ToCharArray();

        private static ContactArea[] TryFrom(string tag, out int indexOfTO)
        {
            indexOfTO = tag.IndexOf(TO, 1, StringComparison.InvariantCultureIgnoreCase); // 1 to handle TONGUE
            if (indexOfTO == -1)
                return null;
            string from = tag.Substring(0, indexOfTO);
            if (knownFrom.TryGetValue(from, out var retVal))
                return retVal;
            return null;
        }

        private static ContactArea[] TryTo(string tag, int indexOfTO, out int separatorIndex)
        {
            string to = tag.Substring(indexOfTO + TO.Length);
            separatorIndex = to.IndexOf(InformerSeparator, StringComparison.InvariantCultureIgnoreCase);
            if (separatorIndex > 0)
            {
                to = to.Substring(0, separatorIndex);
                separatorIndex += indexOfTO + TO.Length; // we index in original tag
            }

            if (knownTo.TryGetValue(to, out var retVal))
                return retVal;
            return null;
        }

        public static bool TryParseFromThemeTag<TContact, TDetails>(this string tag, Creature expected, out TContact contact)
            where TDetails : ContactDetails, new()
            where TContact : GenericActorsContact<TDetails>, new()
        {
            if (NullToSelf == tag)
            {
                contact = new TContact
                {
                    From = new TDetails(),
                    To = new TDetails(),
                };
                return true;
            }
            contact = null;
            tag = tag.ToUpperInvariant();
            if (!TryParseTag(tag, out var from, out var to, out var separatorIndex))
                return false;
            if (separatorIndex > 0)
                return false; // tag from AAF Informer
            contact = new TContact
            {
                From = new TDetails{ Contact = from?[0], BodyPart = expected?.Find(from)},
                To = new TDetails { Contact = to?[0], BodyPart = expected?.Find(to)} 
                // ignore actors indices
            };
            
            return true;
        }

        private static bool TryParseTag(string tag, out ContactArea[] from, out ContactArea[] to, out int separatorIndex)
        {
            from = null;
            to = null;
            separatorIndex = -1;
            from = TryFrom(tag, out int indexOfTO);
            if (from == null)
                return false;

            to = TryTo(tag, indexOfTO, out separatorIndex);
            if (to == null)
                return false;
            return true;
        }

        public static bool TryParseFromAAFInformerTag<TContact, TDetails>(this string tag, Creature expected, out TContact contact)
            where TDetails : ContactDetails, new()
            where TContact : GenericActorsContact<TDetails>, new()
        {
            contact = null;
            tag = tag.ToUpperInvariant();
            if (!TryParseTag(tag, out var from, out var to, out var separatorIndex))
                return false;
            if (separatorIndex < 0)
                return false;
            string indicesLine = tag.Substring(separatorIndex + 1); // do not include separator
            string[] parts = indicesLine.Split(IndicesSeparator);
            if (parts.Length != 2)
                return false;
            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int index1))
                return false;
            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int index2))
                return false;
            contact = new TContact
            {
                From = new TDetails
                {
                    Contact = from?[0],
                    BodyPart = expected?.Find(from),
                    ParticipantIndex = index1
                },
                To = new TDetails
                {
                    Contact = to?[0],
                    BodyPart = expected?.Find(to),
                    ParticipantIndex = index2
                }
            };
            return true;
        }
    }
}