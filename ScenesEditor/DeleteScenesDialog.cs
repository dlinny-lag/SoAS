using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SceneModel;
using ScenesEditor.Data;
using SceneServices;

namespace ScenesEditor
{
    public sealed partial class DeleteScenesDialog : Form
    {
        private Project project;
        public DeleteScenesDialog()
        {
            InitializeComponent();
            scenesList.SelectionChanged += () =>
            {
                selectedCountLbl.Text = $@"Selected {scenesList.Selected.Count} scenes of {(project == null?"nothing":project.Scenes.Count.ToString())}";
            };
            SetupLayout();
        }

        public Project Project
        {
            get => project;
            set
            {
                if (project == value)
                    return;
                project = value;
                scenesList.Init(null, new BuildResult(project?.Scenes.ToList() ?? (IList<Scene>)Array.Empty<Scene>(), null));
            }
        }

        public IList<Scene> Selected => scenesList.Selected.ToArray();

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void SetupLayout()
        {
            okBtn.Location = new Point((ClientSize.Width-okBtn.Width)/2, okBtn.Top);
            cancelBtn.Location = new Point(ClientSize.Width - cancelBtn.Width - Margin.Horizontal, cancelBtn.Top);
        }
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            SetupLayout();
        }
    }
}
