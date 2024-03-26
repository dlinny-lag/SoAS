using System;
using System.Collections.Generic;
using System.Linq;
using SceneModel;
using SceneModel.ContactAreas;
using SceneModel.Creatures;
using SceneServices.TagCategories;
using Shared.Utils;
using Attribute = SceneServices.TagCategories.Attribute;

namespace SceneServices.TagsHandlers
{
    public class EnvironmentGuess
    {
        public HashSet<int> VictimIndices = new HashSet<int>();
        public List<EnvironmentContact> Contacts = new List<EnvironmentContact>();
    }

    public class ActorsGuess
    {
        public HashSet<int> AggressorIndices = new HashSet<int>();
        public HashSet<int> VictimIndices = new HashSet<int>();
        public List<ActorsContact> Contacts = new List<ActorsContact>();
    }
    public static class ContactsGuesser
    {
        private const int TagTargetActorIndex = 0; // all contacts points to 0 actor from all other
        private const int VictimActorIndex = 0; // in most cases actor 0 is a victim
        public static ActorsGuess GuessActorContacts(this Scene scene)
        {
            ActorsGuess retVal = new ActorsGuess();
            retVal.InitContacts(scene);
            retVal.FixCreaturesGenitalia(scene);
            retVal.InitVictimsAggressors(scene);
            retVal.InitNumeric(scene);
            retVal.InitMissing(scene);
            return retVal;
        }

        private static bool InitVictimsAggressors(this ActorsGuess retVal, Scene scene)
        {
            if (scene.Participants.Count < 1)
                return false;
            if (!scene.Imported.Feeling.ContainsAny(Feeling.Aggressive)) 
                return false;

            retVal.VictimIndices = retVal.Contacts.Select(c => c.To.ParticipantIndex).ToHashSet();
            retVal.AggressorIndices = retVal.Contacts.Select(c => c.From.ParticipantIndex).ToHashSet();

            return true;
        }

        private static void InitContacts(this ActorsGuess retVal, Scene scene)
        {
            bool informerTagDetected = false;
            List<ActorsContact> fromThemeTags = new List<ActorsContact>();
            foreach (string[] tagsSequence in scene.Imported.Contact)
            {
                for (int i = 0; i < tagsSequence.Length; i++)
                {
                    string tag = tagsSequence[i];
                    Participant p = scene.Participants.TryGet(i);
                    Creature expected = p?.GetCreatureTemplate();
                    if (tag.TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(expected, out var contact))
                    {
                        if (!informerTagDetected)
                        { // remove contacts retrieved from theme tags
                            foreach (ActorsContact fromTheme in fromThemeTags)
                            {
                                retVal.Contacts.Remove(fromTheme);
                            }
                            informerTagDetected = true;
                        }
                    }

                    if (!informerTagDetected && contact == null)
                    {
                        if (tag == ContactResolver.NullToSelf)
                            continue;
                        if (tag.TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(expected, out contact))
                        {
                            contact.To.ParticipantIndex = TagTargetActorIndex;
                            int otherActorIndex = (i > 0) ? (i) : (i + 1);
                            contact.From.ParticipantIndex = Math.Min(otherActorIndex, scene.Participants.Count-1);
                            fromThemeTags.Add(contact);
                        }
                    }

                    if (contact == null) 
                        continue;

                    retVal.Contacts.Add(contact);

                    if (contact.From.Contact is Stick)
                        contact.From.Contact = scene.Participants[contact.From.ParticipantIndex].Sex.ResolveStick();

                    if (!(contact.To.Contact is Either)) 
                        continue;

                    // handle multi target area
                    var either = scene.Participants[contact.To.ParticipantIndex].Sex.ResolveEither();
                    contact.To.Contact = either[0];

                    if (either.Length < 2)
                        continue;

                    // add a new contact
                    var additional = new ActorsContact()
                    {
                        From = contact.From.Copy(),
                        To = contact.To.Copy(either[1])
                    };
                    retVal.Contacts.Add(additional);
                }
            }
        }

        private static void FixCreaturesGenitalia(this ActorsGuess retVal, Scene scene)
        {
            // TODO: implement the wrong contact area for non-mammal creatures
            // TODO: there are multiple AAF tags definition when penis is used
        }

        private static void InitNumeric(this ActorsGuess retVal, Scene scene)
        {
            int painLevel = 0;
            foreach (string tag in scene.Imported.Numeric)
            {
                NumericValue val = tag.AsNumeric();
                if (val.Value == 0)
                    continue; // nothing noticeable
                int normalized = val.Value.NormalizeNumeric();
                switch (val.Type)
                {
                    case NumericType.Stim:
                        foreach (ActorsContact contact in retVal.Contacts)
                        {
                            contact.To.Stimulation = normalized;
                            contact.From.Stimulation = normalized;
                        }
                        break;
                    case NumericType.Dom:
                        UpdateComfort(retVal, -normalized/2); // negative dom increases comfort
                        break;
                    case NumericType.Held:
                        foreach (ActorsContact contact in retVal.Contacts)
                        {
                            if (contact.To.ParticipantIndex == VictimActorIndex || contact.To.ParticipantIndex.IsIn(retVal.VictimIndices))
                                contact.To.Hold = normalized;
                        }
                        break;
                    case NumericType.Love:
                        UpdateComfort(retVal, normalized/2); // positive love increases comfort
                        painLevel -= normalized; // and decreases pain
                        break;
                }
            }

            // increase pain for aggressive and rough scenes
            if (scene.Imported.Feeling.Contains(Feeling.Aggressive))
                painLevel += 50;
            if (scene.Imported.Feeling.Contains(Feeling.Rough))
                painLevel += 25;

            if (painLevel <= 0)
                return;

            foreach (ActorsContact contact in retVal.Contacts)
            {
                contact.To.Pain = painLevel;
            }
        }

        private static void UpdateComfort(ActorsGuess retVal, int normalized)
        {
            foreach (ActorsContact contact in retVal.Contacts)
            {
                if (contact.To.ParticipantIndex == TagTargetActorIndex)
                {
                    contact.To.Comfort += normalized;
                    contact.From.Comfort += Math.Abs(normalized);
                }

                if (contact.From.ParticipantIndex == TagTargetActorIndex)
                {
                    contact.From.Comfort += normalized;
                    contact.To.Comfort += Math.Abs(normalized);
                }
            }
        }

        static readonly List<ParticipantContactDetails> defaultForMissing = new List<ParticipantContactDetails>()
        {
            new ParticipantContactDetails{Contact = Vagina.Any, Stimulation = 50, PainType = PainType.Pierce},
            new ParticipantContactDetails{Contact = Anus.Any, Stimulation = 50, PainType = PainType.Pierce},
            new ParticipantContactDetails{Contact = Penis.Any, Stimulation = 50, PainType = PainType.Crush},
            new ParticipantContactDetails{Contact = Mouth.Any, Stimulation = 50, PainType = PainType.Pierce},
            new ParticipantContactDetails{Contact = Nipple.Any, Stimulation = 50, PainType = PainType.Crush},
            new ParticipantContactDetails{Contact = Strapon.Any, Stimulation = 50, PainType = PainType.Crush},
            new ParticipantContactDetails{Contact = Either.Any, Stimulation = 50, PainType = PainType.Pierce},
        };
        static ParticipantContactDetails TryGetDefault(this ParticipantContactDetails toInit)
        {
            ContactArea toFind = toInit.Contact;
            foreach (var def in defaultForMissing)
            {
                if (def.Contact == toFind)
                    return def;
            }
            return null;
        }
        private static void InitMissing(this ActorsGuess retVal, Scene scene)
        {
            // there are a lot of missing numeric values that can be used for
            // contact details filling
            // set default value if numeric tags was missing

            foreach (ActorsContact contact in retVal.Contacts)
            {
                ParticipantContactDetails defs = TryGetDefault(contact.From);
                if (defs != null)
                    InitFromDefs(contact.From, defs);
                defs = TryGetDefault(contact.To);
                if (defs != null)
                    InitFromDefs(contact.To, defs);
            }
        }
        static void InitFromDefs(ParticipantContactDetails details, ParticipantContactDetails defs)
        {
            if (details.Stimulation == 0)
                details.Stimulation = defs.Stimulation;
            if (details.Pain != 0 && details.PainType == PainType.None)
                details.PainType = defs.PainType;
        }

        public static EnvironmentGuess GuessEnvironmentContacts(this Scene scene)
        {

            var retVal = new EnvironmentGuess();
            if (scene.Participants.Count > 1)
                return retVal;

            if (scene.Tied(retVal.Contacts))
                retVal.VictimIndices.Add(0);

            if (scene.NeckFromBadEnd(retVal.Contacts))
                retVal.VictimIndices.Add(0);

            return retVal;
        }

        private static bool Tied(this Scene scene, List<EnvironmentContact> contacts)
        {
            bool added = false;
            if (scene.Attribute.Contains(Attribute.LegsTied))
            {
                contacts.Add(new EnvironmentContact
                {
                    Direction = EnvironmentDirections.FromEnvironment,
                    Details = new ParticipantContactDetails
                    {
                        ParticipantIndex = 0,
                        Contact = HumanoidLeg.Any,
                        PainType = PainType.Crush,
                        Pain = 10,
                        Hold = 100,
                    }
                });
                added = true;
            }

            if (scene.Attribute.ContainsAny(Attribute.Cuffed, Attribute.Bound))
            {
                contacts.Add(new EnvironmentContact
                {
                    Direction = EnvironmentDirections.FromEnvironment,
                    Details = new ParticipantContactDetails
                    {
                        ParticipantIndex = 0,
                        Contact = Hand.Any,
                        PainType = PainType.Crush,
                        Pain = 10,
                        Hold = 100,
                    }
                });
                added = true;
            }

            return added;
        }

        private static bool NeckFromBadEnd(this Scene scene, List<EnvironmentContact> contacts)
        {
            if (scene.Narrative.ContainsAny(Narrative.NeckHanging, Narrative.NeckStrangling))
            {
                contacts.Add(new EnvironmentContact
                {
                    Direction = EnvironmentDirections.FromEnvironment,
                    Details = new ParticipantContactDetails
                    {
                        ParticipantIndex = 0,
                        Contact = Neck.Any,
                        PainType = PainType.Crush,
                        Pain = 100,
                        Hold = 100,
                        Stimulation = 50,
                    }
                });
                return true;
            }

            return false;
        }
    }
}