using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SceneModel;

namespace ScenesEditor
{
    public sealed partial class SceneTagsControl : UserControl
    {
        private const int ImportedControlsCount = 6;
        private const int SceneControlsCount = 7;

        private readonly TagsEditorControl[] importedTagControls;
        private readonly TagsEditorControl[] sceneTagControls;
        private int handlesCreated = 0;

        public SceneTagsControl()
        {
            InitializeComponent();
            sceneTagControls = new TagsEditorControl[SceneControlsCount]
            {
                sceneTags,
                authorsTags,
                narrativeTags,
                feelingTags,
                serviceTags,
                attributeTags,
                otherTags,
            };

            importedTagControls = new TagsEditorControl[ImportedControlsCount]
            {
                allTags,
                furnitureTags,
                contactTags,
                numericTags,
                actorTypesTags,
                unknownTags,
            };
            UpdateTagsControlsWidth();
            tagsPanel.ClientSizeChanged += TagsPanelOnClientSizeChanged;

            foreach (var control in importedTagControls)
            {
                control.HandleCreated += ControlOnHandleCreated;
            }
            foreach (var control in sceneTagControls)
            {
                control.HandleCreated += ControlOnHandleCreated;
                control.Editable = true;
                control.TagsChanged += () =>
                {
                    UpdateTagsControlsLocation();
                    TagsChanged?.Invoke();
                };
            }
        }

        private void ControlOnHandleCreated(object sender, EventArgs e)
        {
            ((TagsEditorControl)sender).HandleCreated -= ControlOnHandleCreated;
            ++handlesCreated;
            if (handlesCreated == SceneControlsCount + ImportedControlsCount)
                UpdateTagsControlsLocation();
        }


        private void TagsPanelOnClientSizeChanged(object sender, EventArgs e)
        {
            UpdateTagsControlsWidth();
            UpdateTagsControlsLocation();
        }

        private void UpdateTagsControlsWidth()
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
            UpdateTagsControlsLocation();
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
