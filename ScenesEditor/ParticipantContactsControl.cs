using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SceneModel;

namespace ScenesEditor
{
    public sealed partial class ParticipantContactsControl : UserControl
    {
        public Scene Scene { get; private set; }
        public int ParticipantIndex { get; private set; }

        public bool IsVictim
        {
            get => victimCheckBox.Checked;
            private set => victimCheckBox.Checked = value;
        }
        public bool IsAggressor
        {
            get => aggressorCheckBox.Checked;
            private set => aggressorCheckBox.Checked = value;
        }

        public bool TitleVisible
        {
            get => participantTitle.Visible;
            set => participantTitle.Visible = value;
        }

        public ParticipantContactsControl()
        {
            InitializeComponent();
        }

        private readonly List<ContactEditorControl> toControls = new List<ContactEditorControl>();
        private readonly List<ContactEditorControl> fromControls = new List<ContactEditorControl>();
        private readonly List<ContactEditorControl> envControls = new List<ContactEditorControl>();


        void ClearContactControls()
        {
            void ClearEditorControls(List<ContactEditorControl> controls, GroupBox group)
            {
                foreach (ContactEditorControl toDelete in controls)
                {
                    group.Controls.Remove(toDelete);
                    toDelete.Dispose();
                }
                controls.Clear();
            }

            ClearEditorControls(toControls, toGroup);
            ClearEditorControls(fromControls, fromGroup);
            ClearEditorControls(envControls, environmentGroup);
        }

        void FillContacts()
        {
            ClearContactControls();

            var froms = Scene.ActorsContacts.Where(ca => ca.From.ParticipantIndex == ParticipantIndex);
            var tos = Scene.ActorsContacts.Where(ca => ca.To.ParticipantIndex == ParticipantIndex);
            var envs = Scene.EnvironmentContacts.Where(ca => ca.Details.ParticipantIndex == ParticipantIndex);
            
            int top = addOutcomingBtn.Bottom + fromGroup.Margin.Top;
            int left = fromGroup.Margin.Left;
            int height = addOutcomingBtn.Bottom + fromGroup.Margin.Vertical;
            fromGroup.Location = new Point(fromGroup.Location.X, victimCheckBox.Bottom + Margin.Vertical);
            foreach (var from in froms)
            {
                var cc = new ContactEditorControl(false);
                cc.InitActors(Scene, from.From, from.To);
                fromControls.Add(cc);
                cc.Location = new Point(left, top);
                fromGroup.Controls.Add(cc);
                cc.Changed += Changed;
                top += cc.Height + fromGroup.Margin.Vertical;
                height += cc.Height + fromGroup.Margin.Vertical;
                cc.IndexChanged += ContactChanged;
                cc.Deleted += () => { DeleteParticipantContact(from); };
            }
            fromGroup.Height = height;

            top = addIncomingBtn.Bottom + toGroup.Margin.Top;
            left = toGroup.Margin.Left;
            toGroup.Location = new Point(toGroup.Location.X, fromGroup.Bottom + Margin.Vertical);
            height = addIncomingBtn.Bottom + toGroup.Margin.Vertical;
            foreach (var to in tos)
            {
                var cc = new ContactEditorControl(false);
                cc.InitActors(Scene, to.To, to.From);
                toControls.Add(cc);
                cc.Location = new Point(left, top);
                toGroup.Controls.Add(cc);
                cc.Changed += Changed;
                top += cc.Height + toGroup.Margin.Vertical;
                height += cc.Height + toGroup.Margin.Vertical;
                cc.IndexChanged += ContactChanged;
                cc.Deleted += () => { DeleteParticipantContact(to); };
            }
            toGroup.Height = height;

            top = addEnvironmentalBtn.Bottom + environmentGroup.Margin.Top;
            left = environmentGroup.Margin.Left;
            environmentGroup.Location = new Point(environmentGroup.Location.X, toGroup.Bottom + Margin.Vertical);
            height = addEnvironmentalBtn.Bottom + environmentGroup.Margin.Vertical;
            foreach (var env in envs)
            {
                var cc = new ContactEditorControl(true);
                cc.InitEnvironmentContact(Scene, env);
                envControls.Add(cc);
                cc.Location = new Point(left, top);
                environmentGroup.Controls.Add(cc);
                cc.Changed += Changed;
                cc.Deleted += () => { DeleteEnvironmentContact(env); };
                top += cc.Height + environmentGroup.Margin.Vertical;
                height += cc.Height + environmentGroup.Margin.Vertical;
            }
            environmentGroup.Height = height;
        }

        private void DeleteParticipantContact(ActorsContact contact)
        {
            if (!Scene.ActorsContacts.Remove(contact))
                throw new InvalidOperationException("No actors contact to remove");
            Changed?.Invoke();
        }

        private void DeleteEnvironmentContact(EnvironmentContact contact)
        {
            if (!Scene.EnvironmentContacts.Remove(contact))
                throw new InvalidOperationException("No environmental contact to remove");
            Changed?.Invoke();
        }

        void ContactChanged(int previousActorIndex, int newActorIndex)
        {
            Changed?.Invoke();
        }

        void UpdateHeight()
        {
            Height = environmentGroup.Bottom + Margin.Vertical;
            MinimumSize = new Size(MinimumSize.Width, Height);
        }

        public void Init(Scene scene, int participantIndex)
        {
            Scene = scene;
            ParticipantIndex = participantIndex;
            UpdateParticipantAttributes();
            participantTitle.Text = scene.ParticipantTitle(participantIndex);
            MinimumSize = new Size(TitleVisible?participantTitle.Width:0 + victimCheckBox.Width + aggressorCheckBox.Width + Margin.Horizontal * 5, 130);
            FillContacts();
            UpdateHeight();
        }

        public void UpdateAfterChange()
        {
            FillContacts();
            UpdateHeight();
        }

        public void UpdateParticipantAttributes()
        {
            var p = Scene.Participants[ParticipantIndex];
            IsAggressor = Scene.Participants[ParticipantIndex].IsAggressor;
            IsVictim = Scene.Participants[ParticipantIndex].IsVictim;
        }

        public event Action Changed;
        public event Action<int> ParticipantAttributeChanged;

        public ActorsContact[] ToContacts
        {
            get
            {
                List<ActorsContact> retVal = new List<ActorsContact>(toControls.Count);
                for (int i = 0; i < toControls.Count; i++)
                {
                    retVal.Add(new ActorsContact
                    {
                        To = toControls[i].Self,
                        From = toControls[i].Other,
                    });
                }

                return retVal.ToArray();
            }
        }
        public ActorsContact[] FromContacts
        {
            get
            {
                List<ActorsContact> retVal = new List<ActorsContact>(fromControls.Count);
                for (int i = 0; i < fromControls.Count; i++)
                {
                    retVal.Add(new ActorsContact
                    {
                        From = fromControls[i].Self,
                        To = fromControls[i].Other,
                    });
                }

                return retVal.ToArray();
            }
        }

        public EnvironmentContact[] FromEnvironment
        {
            get
            {
                List<EnvironmentContact> retVal = new List<EnvironmentContact>(envControls.Count);
                for (int i = 0; i < envControls.Count; i++)
                {
                    retVal.Add(new EnvironmentContact
                    {
                        Direction = envControls[i].Direction, 
                        Details = envControls[i].Self
                    });
                }
                return retVal.ToArray();
            }
        }

        private void addIncomingBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddContactDialog(false))
            {
                dlg.FromActor = false;
                dlg.InitActorsContact(Scene, ParticipantIndex, ParticipantIndex);
                dlg.ShowDialog(this);
                if (DialogResult.OK == dlg.DialogResult)
                {
                    Scene.ActorsContacts.Add(dlg.ActorsContact);
                    FillContacts(); // TODO: optimize and ensure correct scroll position
                    UpdateHeight();
                    Changed?.Invoke();
                }
            }
        }

        private void addOutcomingBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddContactDialog(false))
            {
                dlg.FromActor = true;
                dlg.InitActorsContact(Scene, ParticipantIndex, ParticipantIndex);
                dlg.ShowDialog(this);
                if (DialogResult.OK == dlg.DialogResult)
                {
                    Scene.ActorsContacts.Add(dlg.ActorsContact);
                    FillContacts(); // TODO: optimize and ensure correct scroll position
                    UpdateHeight();
                    Changed?.Invoke();
                }
            }
        }

        private void addEnvironmentalBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddContactDialog(true))
            {
                dlg.InitEnvironmentContact(Scene, ParticipantIndex);
                dlg.ShowDialog(this);
                if (DialogResult.OK == dlg.DialogResult)
                {
                    Scene.EnvironmentContacts.Add(dlg.Environmental);
                    FillContacts(); // TODO: optimize and ensure correct scroll position
                    UpdateHeight();
                    Changed?.Invoke();
                }
            }
        }

        private void victimCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Scene.Participants[ParticipantIndex].IsVictim = IsVictim;
            ParticipantAttributeChanged?.Invoke(ParticipantIndex);
        }

        private void aggressorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Scene.Participants[ParticipantIndex].IsAggressor = IsAggressor;
            ParticipantAttributeChanged?.Invoke(ParticipantIndex);
        }
    }
}
