using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SceneModel.ContactAreas;
using SceneModel.Creatures;

namespace SceneModel
{
    public class ContactDetails : INotifyPropertyChanged
    {
        private ContactArea contact;
        private int participantIndex = -1;
        [Ignore]
        public ContactArea Contact
        {
            get => contact;
            set
            {
                if (contact == value)
                    return;
                contact = value;
                Area = contact.Id;
                OnPropertyChanged(nameof(ContactArea));
            }
        }

        [Mandatory]
        public string Area { get; set; }
        public int ParticipantIndex 
        {
            get => participantIndex;
            set
            {
                if (participantIndex == value)
                    return;
                participantIndex = value;
                OnPropertyChanged(nameof(ParticipantIndex));
            }
        }

        [Ignore]
        public virtual BodyPart BodyPart
        {
            get => null;
            set { }
        }
        

        private event PropertyChangedEventHandler propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => propertyChanged += value;
            remove => propertyChanged -= value;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"#{ParticipantIndex}'s {Contact.Id}";
        }

        public virtual void MakeAlive()
        {
            Contact = ContactArea.FromString(Area);
        }
    }

    public sealed class ParticipantContactDetails : ContactDetails, IHasDistance<ParticipantContactDetails>
    {
        private BodyPart bodyPart;
        [Ignore]
        public override BodyPart BodyPart
        {
            get => bodyPart;
            set
            {
                if (bodyPart == value)
                    return; // do not update path

                bodyPart = value;
                if (bodyPart == null)
                {
                    ReversePath = null;
                    OnPropertyChanged(nameof(BodyPart));
                    return;
                }

                ReversePath = bodyPart.ReversePath;
                Contact = bodyPart.Area;
                OnPropertyChanged(nameof(BodyPart));
            }
        }

        [Mandatory]
        public string ReversePath { get; private set; }

        public void ResolveBodyPart(Creature creature)
        {
            if (Contact == null || creature == null)
                return;

            bodyPart = null;
            foreach (BodyPart part in creature.Body)
            {
                var found = part.Find(Contact, ReversePath);
                if (found == null)
                    continue;
                bodyPart = found;
                return;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Validate(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Validate0_100(int value)
        {
            return Validate(value, 0, 100);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Validate_50_50(int value)
        {
            return Validate(value, -50, 50);
        }

        private int stimulation;
        private int hold;
        private int pain;
        private int tickle;
        private int comfort;
        private PainType painType;
        public int Stimulation
        {
            get => stimulation;
            set
            {
                if (stimulation == value)
                    return;
                stimulation = Validate0_100(value);
                OnPropertyChanged(nameof(Stimulation));
            }
        }
        public int Hold
        {
            get => hold;
            set
            {
                if (hold == value)
                    return;
                hold = Validate0_100(value);
                OnPropertyChanged(nameof(Hold));
            }
        }
        public int Pain
        {
            get => pain;
            set
            {
                if (pain == value)
                    return;
                pain = Validate0_100(value);
                OnPropertyChanged(nameof(Pain));
            }
        }
        public int Tickle  // TODO: useless?
        {
            get => tickle;
            set
            {
                if (tickle == value)
                    return;
                tickle = Validate0_100(value);
                OnPropertyChanged(nameof(Tickle));
            }
        }

        public int Comfort
        {
            get => comfort;
            set
            {
                if (comfort == value)
                    return;
                comfort = Validate_50_50(value);
                OnPropertyChanged(nameof(Comfort));
            }
        }

        public PainType PainType
        {
            get => painType;
            set
            {
                if (painType == value)
                    return;
                painType = value;
                OnPropertyChanged(nameof(PainType));
            }
        }

        [Ignore]
        public Participant Participant { get; set; }

        public ParticipantContactDetails Copy(ContactArea newArea = null)
        {
            return new ParticipantContactDetails
            {
                ParticipantIndex = ParticipantIndex,
                Contact = newArea ?? Contact,
                Stimulation = Stimulation,
                Hold = Hold,
                Pain = Pain,
                Tickle = Tickle,
                Comfort = Comfort,
                PainType = PainType,
                BodyPart = BodyPart,
            };
        }

        public ulong Distance(ParticipantContactDetails other)
        {
            ulong retVal = 0;
            retVal += ParticipantIndex.Diff(other.ParticipantIndex);
            retVal += Area.Diff(other.Area);
            retVal += ReversePath.Diff(other.ReversePath);
            retVal += stimulation.Diff(other.stimulation);
            retVal += hold.Diff(other.hold);
            retVal += pain.Diff(other.pain);
            retVal += tickle.Diff(other.tickle);
            retVal += comfort.Diff(other.comfort);
            retVal += PainType.Diff(other.PainType);

            return retVal;
        }
    }
}