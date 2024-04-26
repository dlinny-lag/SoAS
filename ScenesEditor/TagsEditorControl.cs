using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ScenesEditor
{
    public sealed partial class TagsEditorControl : UserControl
    {
        private List<string> tags = new List<string>();
        private string category = string.Empty;
        public TagsEditorControl()
        {
            InitializeComponent();
            tagValuesBox.HandleCreated += TagValuesBoxOnHandleCreated;
        }

        public bool Editable { get; set; }
        public event Action TagsChanged;

        public string Category
        {
            get => category;
            set
            {
                category = value;
                UpdateControls();
            }
        }

        public string[] Tags
        {
            get => tags.ToArray();
            set
            {
                tags = new List<string>(value ?? Array.Empty<string>());
                UpdateControls();
            }
        }

        private void TagValuesBoxOnHandleCreated(object sender, EventArgs e)
        {
            UpdateControls();
        }

        public event Action<TagsEditorControl> Updated;

        private void UpdateControls()
        {
            tagLabel.Text = category;
            tagValuesBox.Location = new Point(tagLabel.Right + tagLabel.Margin.Horizontal + Padding.Horizontal, tagValuesBox.Top);
            if (DesignMode)
                return;

            tagValuesBox.Width = Width - Padding.Horizontal - tagValuesBox.Margin.Horizontal - tagValuesBox.Location.X;

            if (!tagValuesBox.IsHandleCreated)
                return;

            int expectedHeight = 0;
            int currentHeight = tagValuesBox.Height;
            void OnContentResized(object sender, ContentsResizedEventArgs e)
            {
                expectedHeight = e.NewRectangle.Height;
            }

            tagValuesBox.Text = "-"; // non empty value to force ContentsResized triggering
            tagValuesBox.ContentsResized += OnContentResized;
            tagValuesBox.Text = string.Join("; ", tags);
            tagValuesBox.ContentsResized -= OnContentResized;
            if (expectedHeight > 0 && expectedHeight != currentHeight)
            {
                tagValuesBox.Height = expectedHeight;
            }
            Height = tagValuesBox.Height + tagValuesBox.Top*2;
            Updated?.Invoke(this);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            UpdateControls();
        }


        private void tagValuesBox_DoubleClick(object sender, System.EventArgs e)
        {
            if (!Editable)
            {
                using (var dlg = new TagsListViewDialog{StartPosition = FormStartPosition.CenterParent})
                {
                    dlg.Text = Category;
                    dlg.Tags = tags.ToArray();
                    dlg.Text = tagLabel.Text;
                    dlg.ShowDialog(this);
                }
            }
            else
            {
                using (var dlg = new TagsListEditDialog{StartPosition = FormStartPosition.CenterParent})
                {
                    dlg.Text = $@"Edit {Category}";
                    dlg.Tags = Tags;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Tags = dlg.Tags;
                        TagsChanged?.Invoke();
                    }
                }
            }
        }
    }
}
