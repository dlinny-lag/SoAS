using System;
using System.ComponentModel;
using System.Windows.Forms;
using SceneModel;
using SceneModel.ContactAreas;
using SceneModel.Creatures;

namespace ScenesEditor
{
    public sealed partial class ContactEditorControl : UserControl
    {
        private const int BodyDropDownWidth = 160;

        sealed class DummyAreaRef : AreaReference
        {
            public DummyAreaRef(ContactArea area) : base(area)
            {
            }
        }

        public Scene Scene { get; private set; }
        public bool IsEnvironmentContact { get; }
        public ContactEditorControl(bool isEnvironmentContact, bool createMode = false)
        {
            InitializeComponent();
            selfContactAreaSelector.DropDownWidth = BodyDropDownWidth;
            otherContactAreaSelector.DropDownWidth = BodyDropDownWidth;

            IsEnvironmentContact = isEnvironmentContact;
            selfContactAreaSelector.SelectedNodeChanged += SelfContactChanged;

            if (IsEnvironmentContact)
            {
                otherParticipantSelector.Visible = false;
                otherContactAreaSelector.Visible = false;
                otherStimEditBox.Visible = false;
                otherHoldEditBox.Visible = false;
                otherPainEditBox.Visible = false;
                otherTickleEditBox.Visible = false;
                otherComfortEditBox.Visible = false;
                otherPainTypeBox.Visible = false;
                deletePartBtn.Visible = false;

                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label12.Visible = false;

                environmentDirectionComboBox.Visible = true;
                environmentDirectionComboBox.Items.Add(EnvironmentDirections.FromEnvironment);
                environmentDirectionComboBox.Items.Add(EnvironmentDirections.FromActor);

                Width = (createMode ? environmentDirectionComboBox.Right : deleteEnvBtn.Right) + Margin.Horizontal;

                InitPainTypes(selfPainTypeBox);
            }
            else
            {
                environmentDirectionComboBox.Visible = false;
                deleteEnvBtn.Visible = false;

                otherParticipantSelector.Visible = true;
                otherContactAreaSelector.Visible = true;
                otherStimEditBox.Visible = true;
                otherHoldEditBox.Visible = true;
                otherPainEditBox.Visible = true;
                otherTickleEditBox.Visible = true;
                otherComfortEditBox.Visible = true;
                otherPainTypeBox.Visible = true;

                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label11.Visible = true;

                otherContactAreaSelector.SelectedNodeChanged += OtherContactChanged;
                otherParticipantSelector.SelectedIndexChanged += OtherParticipantChanged;

                Width = otherContactAreaSelector.Right + Margin.Horizontal;
                InitPainTypes(selfPainTypeBox);
                InitPainTypes(otherPainTypeBox);
            }

            if (createMode)
            {
                deleteEnvBtn.Visible = false;
                deletePartBtn.Visible = false;
            }
        }

        private static void InitPainTypes(ComboBox box)
        {
            box.Items.Clear();
            for (int i = 0; i <= (int)PainType.Crush; i++)
            {
                box.Items.Add(Enum.GetName(typeof(PainType), i));
            }
        }
        private void OtherParticipantChanged(object sender, EventArgs e)
        {
            if (otherParticipantSelector.SelectedIndex < 0)
                return;
            int prev = Other.ParticipantIndex;
            int curr = otherParticipantSelector.SelectedIndex;
            if (prev == curr)
                return;
            Other.ParticipantIndex = otherParticipantSelector.SelectedIndex;
            Other.Participant = Scene.Participants[Other.ParticipantIndex];
            Other.ResolveBodyPart(Other.Participant.GetCreatureTemplate());
            IndexChanged?.Invoke(prev, curr);
            FillContacts(otherContactAreaSelector, Other);
        }
        private void FillOtherParticipants()
        {
            for (int i = 0; i < Scene.Participants.Count; i++)
            {
                otherParticipantSelector.Items.Add(Scene.ParticipantTitle(i));
            }
        }

        private void UpdateOtherParticipantSelector()
        {
            otherParticipantSelector.SelectedIndex = Other.ParticipantIndex;
            Other.PropertyChanged += UpdateOtherOnChange;
        }

        private void OtherContactChanged(object sender, EventArgs e)
        {
            if (Other == null)
                return;

            Other.PropertyChanged -= UpdateOtherOnChange;
            try
            {
                HandleContactChanged(otherContactAreaSelector, Other);
            }
            finally
            {
                Other.PropertyChanged += UpdateOtherOnChange;
            }
        }

        private void SelfContactChanged(object sender, System.EventArgs e)
        {
            if (Self == null)
                return;
            Self.PropertyChanged -= UpdateSelfOnChange;
            try
            {
                HandleContactChanged(selfContactAreaSelector, Self);
            }
            finally
            {
                Self.PropertyChanged += UpdateSelfOnChange;
            }
            
        }

        private void HandleContactChanged(ComboTreeBox selector, ParticipantContactDetails details)
        {
            if (details == null)
                return;

            var contactRef = selector.SelectedNode?.Payload as AreaReference;
            if (contactRef == null)
                return;
            if (details.Contact == contactRef.Area)
                return;
            if (contactRef is BodyPart bodyPart)
                details.BodyPart = bodyPart;
            else if (contactRef is AttachmentReference attachment)
                details.Contact = attachment.Area;
            else
                throw new InvalidOperationException("Unexpected area reference object ");
            Changed?.Invoke();
        }

        private static void FillContacts(ComboTreeBox toFill, ParticipantContactDetails details)
        {
            var actor = details.Participant;
            
            AreaReference selected;
            bool exactEqual = false;
            if (details.BodyPart != null)
            {
                selected = details.BodyPart;
                exactEqual = true;
            }
            else if (details.Contact is Attachment attachment)
                selected = new AttachmentReference(attachment);
            else if (details.Contact != null)
                selected = new DummyAreaRef(details.Contact);
            else
                selected = null;

            Creature template;
            if (selected is DummyAreaRef)
                template = null; // do not try to find area in a template
            else
                template = actor.GetCreatureTemplate();

            // if there is no template, then we can display selected contact only.
            var areas = template == null ? new [] { selected } : template.GetContactAreas();

            ComboTreeNode selectedNode = null;
            toFill.Nodes.Clear();
            foreach (AreaReference reference in areas)
            {
                var node = BuildTree(reference, selected, exactEqual, ref selectedNode);
                toFill.Nodes.Add(node);
            }
            toFill.SelectedNode = selectedNode;
        }

        static ComboTreeNode  BuildTree(AreaReference reference, AreaReference selected, bool exactEqual, ref ComboTreeNode selectedNode)
        {
            string displayValue = reference.Area.DisplayId;
            if (reference.Area.IsAny && reference.Area.GetVariants().Length > 1)
                displayValue += " (any)";
            ComboTreeNode retVal = new ComboTreeNode(reference.Area.Id, displayValue, reference);

            if (selectedNode == null)
            {
                bool compareResult;

                if (exactEqual)
                    compareResult = reference == selected;
                else
                    compareResult = reference.Area == selected?.Area;

                if (compareResult)
                    selectedNode = retVal;
            }

            BodyPart bodyPart = reference as BodyPart;
            if (bodyPart == null)
                return retVal;
            foreach (BodyPart child in bodyPart.Children)
            {
                retVal.Nodes.Add(BuildTree(child, selected, exactEqual, ref selectedNode));
            }

            return retVal;
        }

        void InitSelf(ParticipantContactDetails self)
        {
            FillContacts(selfContactAreaSelector, self);
            selfStimEditBox.Text = self.Stimulation.ToString();
            selfHoldEditBox.Text = self.Hold.ToString();
            selfPainEditBox.Text = self.Pain.ToString();
            selfTickleEditBox.Text = self.Tickle.ToString();
            selfComfortEditBox.Text = self.Comfort.ToString();
            selfPainTypeBox.SelectedIndex = (int)self.PainType;
            Self = self;
            self.PropertyChanged += UpdateSelfOnChange;
        }

        void InitOther(ParticipantContactDetails other, bool firstInit)
        {
            otherStimEditBox.Text = other.Stimulation.ToString();
            otherHoldEditBox.Text = other.Hold.ToString();
            otherPainEditBox.Text = other.Pain.ToString();
            otherTickleEditBox.Text = other.Tickle.ToString();
            otherComfortEditBox.Text = other.Comfort.ToString();
            otherPainTypeBox.SelectedIndex = (int)other.PainType;
            Other = other;
            if (firstInit)
                FillOtherParticipants();
            UpdateOtherParticipantSelector();
        }

        void UpdateSelfOnChange(object o, PropertyChangedEventArgs a)
        {
            Self.PropertyChanged -= UpdateSelfOnChange;
            InitSelf(Self);
        }

        public void InitEnvironmentContact(Scene scene, EnvironmentContact contact)
        {
            if (!IsEnvironmentContact)
                throw new InvalidOperationException("Can't init actors contact with environment contact");
            InitSelf(contact.Details);
            Scene = scene;
            environmentDirectionComboBox.SelectedIndex = (int)contact.Direction;
        }

        void UpdateOtherOnChange(object o, PropertyChangedEventArgs a)
        {
            Other.PropertyChanged -= UpdateOtherOnChange;
            InitOther(Other, false);
        }
        public void InitActors(Scene scene, ParticipantContactDetails self, ParticipantContactDetails other)
        {
            if (IsEnvironmentContact)
                throw new InvalidOperationException("Can't init environment contact with actors contact");
            InitSelf(self);
            Scene = scene;
            FillContacts(otherContactAreaSelector, other);
            InitOther(other, true);
            
        }

        private ParticipantContactDetails _self;
        public ParticipantContactDetails Self
        {
            get => _self;
            private set
            {
                if (_self != null)
                    _self.PropertyChanged -= UpdateSelfOnChange;
                _self = value;
            }
        }

        private ParticipantContactDetails _other;
        public ParticipantContactDetails Other
        {
            get => _other;
            private set
            {
                if (_other != null)
                    _other.PropertyChanged -= UpdateOtherOnChange;
                _other = value;
            }
        }
        public EnvironmentDirections Direction { get; private set; }

        public event Action Changed;
        public event Action<int, int> IndexChanged;
        public event Action Deleted;

        private void OnDispose()
        {
            Changed = null;
            IndexChanged = null;
            Deleted = null;
            if (Self != null)
                Self.PropertyChanged -= UpdateSelfOnChange;
            if (Other != null)
                Other.PropertyChanged -= UpdateOtherOnChange;
        }

        private void selfStimEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Self == null)
                return; 
            if (Self.Stimulation == (int)selfStimEditBox.Value)
                return;
            Self.Stimulation = (int)selfStimEditBox.Value;
        }

        private void selfHoldEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Self == null)
                return;
            if (Self.Hold == (int)selfHoldEditBox.Value)
                return;
            Self.Hold = (int)selfHoldEditBox.Value;
        }

        private void selfPainEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Self == null)
                return;
            if (Self.Pain == (int)selfPainEditBox.Value)
                return;
            Self.Pain = (int)selfPainEditBox.Value;
        }

        private void selfTickleEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Self == null)
                return; 
            if (Self.Tickle == (int)selfTickleEditBox.Value)
                return;
            Self.Tickle = (int)selfTickleEditBox.Value;
        }
        private void selfComfortEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Self == null)
                return; 
            if (Self.Comfort == (int)selfComfortEditBox.Value)
                return;
            Self.Comfort = (int)selfComfortEditBox.Value;
        }
        private void selfPainTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Self == null || selfPainTypeBox.SelectedIndex < 0)
                return;
            if (Self.PainType == (PainType)selfPainTypeBox.SelectedIndex)
                return;
            Self.PainType = (PainType)selfPainTypeBox.SelectedIndex;
        }

        private void environmentDirectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Direction = (EnvironmentDirections)environmentDirectionComboBox.SelectedIndex;
        }

        private void otherStimEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Other == null)
                return; 
            if (Other.Stimulation == (int)otherStimEditBox.Value)
                return;
            Other.Stimulation = (int)otherStimEditBox.Value;
        }

        private void otherHoldEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Other == null)
                return; 
            if (Other.Hold == (int)otherHoldEditBox.Value)
                return;
            Other.Hold = (int)otherHoldEditBox.Value;
        }

        private void otherPainEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Other == null)
                return; 
            if (Other.Pain == (int)otherPainEditBox.Value)
                return;
            Other.Pain = (int)otherPainEditBox.Value;
        }

        private void otherTickleEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Other == null)
                return; 
            if (Other.Tickle == (int)otherTickleEditBox.Value)
                return;
            Other.Tickle = (int)otherTickleEditBox.Value;
        }
        private void otherComfortEditBox_ValueChanged(object sender, EventArgs e)
        {
            if (Other == null)
                return; 
            if (Other.Comfort == (int)otherComfortEditBox.Value)
                return;
            Other.Comfort = (int)otherComfortEditBox.Value;
        }

        private void otherPainTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Other == null || otherPainTypeBox.SelectedIndex < 0)
                return;
            if (Other.PainType == (PainType)otherPainTypeBox.SelectedIndex)
                return;
            Other.PainType = (PainType)otherPainTypeBox.SelectedIndex;
        }
        private void OnDeleteContactClick(object sender, EventArgs e)
        {
            Deleted?.Invoke();
        }
    }
}
