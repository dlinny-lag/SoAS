using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SceneModel.Creatures;
using ScenesEditor.Data;

namespace ScenesEditor
{
    public sealed partial class DebugDialog : Form
    {
        private Project project;
        public DebugDialog()
        {
            InitializeComponent();
            unsupportedSkeletonsList.Columns.Add(new DataGridViewTextBoxColumn
            {
                ValueType = typeof(string),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle{WrapMode = DataGridViewTriState.False}
            });
            unsupportedSkeletonsList.BackgroundColor = unsupportedSkeletonsList.DefaultCellStyle.BackColor;
        }

        public Project Project
        {
            get => project;
            set
            {
                if (project == value)
                    return;
                project = value;
                UpdateControls();
            }
        }

        void UpdateControls()
        {
            var scenes = project?.Scenes;
            totalScenesLbl.Text = $@"Scenes count: {scenes?.Count ?? 0}";
            unsupportedSkeletonsList.DataSource = null;
            if (scenes == null)
                return;
            var usedSkeletons = scenes.SelectMany(s => s.Participants).Select(p => p.Skeleton).ToHashSet();
            List<string> unsupportedSkeletons = new List<string>();
            foreach (string usedSkeleton in usedSkeletons)
            {
                if (!usedSkeleton.IsSupportedSkeleton())
                    unsupportedSkeletons.Add(usedSkeleton);
            }
            
            unsupportedSkeletonsList.Rows.Clear();
            unsupportedSkeletonsList.Rows.AddRange(unsupportedSkeletons.Select(skeleton =>
                    new DataGridViewRow{Cells = { new DataGridViewTextBoxCell{Value = skeleton} }}).ToArray());
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
