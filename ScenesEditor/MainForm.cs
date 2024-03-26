using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ScenesEditor.Data;
using Shared.Controls;

namespace ScenesEditor
{
    public sealed partial class MainForm : Form
    {
        private Project currentProject;
        private const string Title = "Semantics of Animation Scenes";
        private bool dirty = false;
        public MainForm()
        {
            InitializeComponent();
            Text = Title;
            projectWorkspace.Visible = false;
            projectsListControl.Visible = true;

            projectsListControl.SetData(ProjectSerialization.List());
            UpdateTitle();
            projectWorkspace.Changed += () =>
            {
                dirty = true;
                UpdateTitle();
            };
            ApplicationSettings.FurnitureLibrary.Load();
        }

        private void UpdateTitle()
        {
            StringBuilder sb = new StringBuilder();
            if (dirty)
                sb.Append("* ");
            sb.Append(Title);
            if (currentProject != null)
                sb.Append(" - ").Append(currentProject.Name);
            Text = sb.ToString();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            projectsListControl.Selected += ProjectsListControlOnSelected;
        }

        private void ProjectsListControlOnSelected(ProjectHeader project)
        {
            DialogResult result = DialogResult.OK;
            try
            {
                var loaded = ProjectSerialization.Load(project.Path);
                OpenProject(loaded);
            }
            catch (Exception e)
            {
                result = ShowExceptionDialog.Show(e,
                    "Delete project from the list and continue",
                    "Keep project in the list and continue", 
                    this, FormStartPosition.CenterParent);
            }
            switch (result)
            {
                case DialogResult.OK:
                    return; // do nothing
                case DialogResult.No:
                    BeginInvoke(new Action(projectsListControl.ResetSelection)); // tricky. we are inside of selection change handler
                    return;
                case DialogResult.Cancel:
                    Close(); return;
                case DialogResult.Yes:
                    var projects = ProjectSerialization.UnregisterProject(project);
                    BeginInvoke(new Action( () => { projectsListControl.SetData(projects); }));
                    return;
                default:
                    throw new NotImplementedException("Unexpected result");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project newProject = new Project();
            string folder;
            using (var dlg = new FolderBrowserDialog() { ShowNewFolderButton = true, SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) }) 
            {
                if (DialogResult.OK != dlg.ShowDialog())
                    return;
                folder = dlg.SelectedPath;
            }

            bool FileNotExist(string projectName)
            {
                return !File.Exists(Path.Combine(folder, projectName + $@".{ProjectSerialization.ProjectFileExtension}"));
            }

            using (ProjectEditDialog dlg = new ProjectEditDialog(FileNotExist){StartPosition = FormStartPosition.CenterParent})
            {
                dlg.Project = newProject;
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    newProject.Created = DateTimeOffset.Now;
                    newProject.Save(folder);
                    OpenProject(newProject);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject == null)
                return;
            currentProject.Save(Path.GetDirectoryName(currentProject.Path));
            dirty = false;
            UpdateTitle();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // OpenProject()
        }

        void OpenProject(Project project)
        {
            currentProject = project;
            projectWorkspace.Project = currentProject;
            projectWorkspace.Visible = true;
            projectsListControl.Visible = false; // TODO: dispose?
            UpdateTitle();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new AboutDialog{StartPosition = FormStartPosition.CenterParent})
            {
                dlg.ShowDialog(this);
            }
        }

        private void furnitureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new FurnitureManagerForm{StartPosition = FormStartPosition.CenterParent})
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
