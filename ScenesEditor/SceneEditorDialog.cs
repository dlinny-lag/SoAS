using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SceneModel;
using SceneServices;
using SceneServices.Scenes;
using SceneServices.TagCategories;

namespace ScenesEditor
{
    public sealed partial  class SceneEditorDialog : Form
    {
        public Scene Scene { get; private set; }
        private readonly List<ParticipantContactsControl> contactControls = new List<ParticipantContactsControl>();
        private readonly List<ParticipantTabPage> participantTabs = new List<ParticipantTabPage>();
        public SceneEditorDialog()
        {
            InitializeComponent();
            buttonsPanel.ClientSizeChanged += ButtonsPanelOnClientSizeChanged;
            MinimumSize = new Size((okBtn.Width / 2 + cancelBtn.Width) * 2 + (buttonsPanel.Width-cancelBtn.Left) + buttonsPanel.Margin.Horizontal, buttonsPanel.Height);
        }

        private void ButtonsPanelOnClientSizeChanged(object sender, EventArgs e)
        {
            int x = (buttonsPanel.Width - okBtn.Width) / 2;
            okBtn.Location = new Point(x, okBtn.Location.Y);
            readyForReleaseCheckBox.Location = new Point(okBtn.Right + Margin.Horizontal, readyForReleaseCheckBox.Location.Y);
        }

        void ClearParticipantControls()
        {
            foreach (var control in contactControls)
            {
                control.Dispose();
            }
            contactControls.Clear();
            foreach (ParticipantTabPage tabPage in participantTabs)
            {
                tabPage.Dispose();
            }
            participantTabs.Clear();
        }

        void UpdateContactControls()
        {
            foreach (ParticipantContactsControl control in contactControls)
            {
                //control.UpdateAfterChange();
                // schedule action as a work around of access violation issue 
                control.BeginInvoke(new Action(() => control.UpdateAfterChange()));
            }
        }

        void FillActors()
        {
            ClearParticipantControls();
            int left = Margin.Left;
            int top = Margin.Top;
            for (int i = 0; i < Scene.Participants.Count; i++)
            {
                var control = new ParticipantContactsControl();
                control.Init(Scene, i);
                control.Location = new Point(left, top);
                contactsPanel.Controls.Add(control);
                contactControls.Add(control);

                left += control.Width + control.Margin.Left;

                ParticipantTabPage tab = new ParticipantTabPage(Scene, i);
                tabControl.Controls.Add(tab);
                participantTabs.Add(tab);
                
                control.Changed += UpdateContactControls;
                control.ParticipantAttributeChanged += (index) => tab.UpdateAfterChange();

                tab.Changed += (p, index) =>
                {
                    control.UpdateParticipantAttributes();
                };
            }
            
        }

        public void Init(Scene scene, bool readyForRelease)
        {
            var copy = scene.ToJson().ToScene();
            Scene = copy;
            Text = $"Scene - {copy.Id} ({copy.Type})";
            FillActors();
            sceneTagsControl.InitTags(copy);
            sceneAttributesControl.Init(copy);
            FillFurniture(copy);
            cancelBtn.Focus();
            readyForReleaseCheckBox.Checked = readyForRelease;
        }

        private void FillFurniture(Scene scene)
        {
            furnitureTreeView.Nodes.Clear();
            List<string> furniture = new List<string>(scene.Furniture);
            furniture.Sort();
            foreach (string furn in furniture)
            {
                var tagNode = furnitureTreeView.Nodes.Add(furn, furn);
                var bySource = ApplicationSettings.FurnitureLibrary.GetForms(furn);
                List<ModFile> sources = bySource.Keys.ToList();
                sources.Sort();
                foreach (ModFile source in sources)
                {
                    var sourceNode = tagNode.Nodes.Add(source.ToString(), source.ToString());
                    var items = bySource[source];
                    items.Sort();
                    foreach (var item in items)
                    {
                        sourceNode.Nodes.Add(item.FormId.ToString(), $"{item.FormId:X8}");
                    }
                }
            }
        }

        public bool IsReadyForRelease => readyForReleaseCheckBox.Checked;

        private void okBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            ApplyChanges();
            Close();
        }

        private void ApplyChanges()
        {
            List<ActorsContact> actorContacts = new List<ActorsContact>();
            List<EnvironmentContact> envContacts = new List<EnvironmentContact>();
            List<ParticipantSettings> flags = new List<ParticipantSettings>(contactControls.Count);
            foreach (var control in contactControls)
            {
                actorContacts.AddRange(control.FromContacts);
                envContacts.AddRange(control.FromEnvironment);
                flags.Add(new ParticipantSettings{IsAggressor = control.IsAggressor, IsVictim = control.IsVictim});
            }

            Scene.ActorsContacts = actorContacts;
            Scene.EnvironmentContacts = envContacts;
            for (int i = 0; i < flags.Count; i++)
            {
                Scene.Participants[i].IsVictim = flags[i].IsVictim;
                Scene.Participants[i].IsAggressor = flags[i].IsAggressor;
            }

            Scene.Tags = sceneTagsControl.Tags;
            Scene.Authors = sceneTagsControl.Authors;
            Scene.Narrative = sceneTagsControl.Narrative;
            Scene.Feeling = sceneTagsControl.Feeling;
            Scene.Service = sceneTagsControl.Service;
            Scene.Attribute = sceneTagsControl.Attribute;
            Scene.Other = sceneTagsControl.Other;

            Scene.SetCustomAttributes(sceneAttributesControl.CustomAttributes);
        }

        private void importFromTagsBtn_Click(object sender, EventArgs e)
        {
            Scene.FillAttributes();
            Scene.GuessContacts();

            FillActors();
            sceneTagsControl.InitTags(Scene);
            sceneAttributesControl.Init(Scene);
        }

        private void categorizeTagsBtn_Click(object sender, EventArgs e)
        {
            Scene.FillCategories();
            Scene.FillAttributes();
            sceneTagsControl.InitTags(Scene);
        }
    }
}
