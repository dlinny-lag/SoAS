using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SceneModel;
using Shared.Utils;

namespace ScenesEditor
{
    public sealed partial class SceneTagsControl : UserControl
    {
        private readonly TagsEditorControl[] importedTagControls;
        private readonly TagsEditorControl[] sceneTagControls;

        private HashSet<TagsEditorControl> updatingControls;
        public SceneTagsControl()
        {
            InitializeComponent();
            sceneTagControls = new []
            {
                sceneTags,
                authorsTags,
                narrativeTags,
                feelingTags,
                serviceTags,
                attributeTags,
                otherTags,
            };

            importedTagControls = new []
            {
                allTags,
                furnitureTags,
                contactTags,
                numericTags,
                actorTypesTags,
                unknownTags,
            };
            updatingControls = new HashSet<TagsEditorControl>(sceneTagControls);
            updatingControls.AddRange(importedTagControls);

            SetTagsControlsWidth();
            tagsPanel.ClientSizeChanged += TagsPanelOnClientSizeChanged;

            foreach (var control in importedTagControls)
            {
                control.Updated += ControlOnUpdatedFirstTime;
            }
            foreach (var control in sceneTagControls)
            {
                control.Updated += ControlOnUpdatedFirstTime;
                control.Editable = true;
                control.TagsChanged += () => TagsChanged?.Invoke();
            }
        }

        private void ControlOnUpdatedFirstTime(TagsEditorControl initializedControl)
        {
            initializedControl.Updated -= ControlOnUpdatedFirstTime;
            updatingControls.Remove(initializedControl);
            if (updatingControls.Count == 0)
            {
                updatingControls = null; // sanity check
                ScheduleLocationsUpdate();
                foreach (var control in importedTagControls)
                {
                    control.Updated += _ => ScheduleLocationsUpdate();
                }
                foreach (var control in sceneTagControls)
                {
                    control.Updated += _ => ScheduleLocationsUpdate();
                }
            }
        }

        private bool needLocationUpdate = true;
        private bool scheduled = false;

        private void EnsureLocationsUpdated()
        {
            scheduled = false;
            if (!needLocationUpdate)
                return;
            UpdateTagsControlsLocation();
            needLocationUpdate = false;
        }
        
        void ScheduleLocationsUpdate() // method always called from UI thread
        {
            needLocationUpdate = true;
            if (!IsHandleCreated)
                return; // can't schedule right now
            
            if (scheduled)
                return;
            BeginInvoke(new Action(EnsureLocationsUpdated));
            scheduled = true;
        }

        private void TagsPanelOnClientSizeChanged(object sender, EventArgs e)
        {
            SetTagsControlsWidth();
            ScheduleLocationsUpdate();
        }

        private void SetTagsControlsWidth()
        {
            // imported
            importedGroupBox.Width = tagsPanel.Width - SystemInformation.VerticalScrollBarWidth - tagsPanel.Padding.Horizontal;
            int width = importedGroupBox.DisplayRectangle.Width - importedGroupBox.Padding.Horizontal;
            foreach (var ctrl in importedTagControls)
            {
                ctrl.Width = width;
            }

            width = importedGroupBox.DisplayRectangle.Width;
            foreach (var ctrl in sceneTagControls)
            {
                ctrl.Width = width;
            }
            
        }
        public void InitTags(Scene scene)
        {
            if (scene == null)
                return;

            authorsTags.Tags = scene.Authors;
            narrativeTags.Tags = scene.Narrative;
            feelingTags.Tags = scene.Feeling;
            serviceTags.Tags = scene.Service;
            attributeTags.Tags = scene.Attribute;
            otherTags.Tags = scene.Other;

            furnitureTags.Tags = scene.Imported.Furniture;
            contactTags.Tags = scene.Imported.Contact.SelectMany(t => t).ToArray();
            numericTags.Tags = scene.Imported.Numeric;
            actorTypesTags.Tags = scene.Imported.ActorTypes;
            unknownTags.Tags = scene.Imported.Unknown;
            allTags.Tags = scene.RawTags.SelectMany(t => t).ToArray();

            sceneTags.Tags = scene.Tags ?? Array.Empty<string>();
            ScheduleLocationsUpdate();
        }

        public event Action TagsChanged;
        public string[] Tags => sceneTags.Tags;
        public string[] Authors => authorsTags.Tags;
        public string[] Narrative => narrativeTags.Tags;
        public string[] Feeling => feelingTags.Tags;
        public string[] Service => serviceTags.Tags;
        public string[] Attribute => attributeTags.Tags;
        public string[] Other => otherTags.Tags;

        private void UpdateTagsControlsLocation()
        {
            int left = tagsPanel.Padding.Left + importedGroupBox.DisplayRectangle.Left;
            int y = tagsPanel.Padding.Top;
            foreach (TagsEditorControl sceneTagControl in sceneTagControls)
            {
                sceneTagControl.Location = new Point(left, y);
                y += sceneTagControl.Height + Padding.Vertical;
            }

            importedGroupBox.Location = new Point(tagsPanel.Padding.Left, y + Padding.Vertical);
            // update location after size change
            y = importedGroupBox.DisplayRectangle.Top + importedGroupBox.Padding.Top;
            for (int i = 0; i < importedTagControls.Length; i++)
            {
                importedTagControls[i].Location = new Point(importedTagControls[i].Location.X, y);
                y += importedTagControls[i].Height + importedGroupBox.Padding.Top;
            }
            importedGroupBox.Height = importedTagControls.Last().Bottom + importedGroupBox.Padding.Bottom;
        }

    }
}
