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
        private ProjectHeader currentProject;
        private const string Title = "Semantics of Animation Scenes";
        private bool dirty = false;
        public MainForm()
        {
            InitializeComponent();
            Text = Title;
            if (ApplicationSettings.EditDefaultDataSetMode)
            {
                projectWorkspace.Visible = false;
                projectsListControl.Visible = false;
            }
            else
            {
                projectWorkspace.Visible = false;
                projectsListControl.Visible = true;

                projectsListControl.SetData(ProjectSerialization.List());
                projectWorkspace.Changed += SetDirty;
            }

            UpdateTitle();
            ApplicationSettings.FurnitureLibrary.Load();
        }

        private void SetDirty()
        {
            dirty = true;
            UpdateTitle();
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
            if (ApplicationSettings.EditDefaultDataSetMode)
            {
                var project = projectWorkspace.LoadFromDefaultDataSet();
                if (project == null)
                {
                    Close();
                    return;
                }
                OpenProject(project);
            }
            else
            {
                projectsListControl.Selected += ProjectsListControlOnSelected;
            }
        }

        private void ProjectsListControlOnSelected(ProjectHeader project)
        {
            DialogResult result = DialogResult.OK;
            try
            {
                switch (project.Path.ProjectTypeFromFileExtension())
                {
                    case ProjectType.Overwrite:
                        {
                            var loaded = ProjectSerialization.Load<Project>(project.Path);
                            OpenProject(loaded);
                        }
                        break;
                    case ProjectType.Patch:
                        {
                            var loaded = ProjectSerialization.Load<PatchProject>(project.Path);
                            OpenProject(loaded.GenerateProject());
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Unrecognized project type");
                }
                
                
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
        
        private void NewProjectMenuItemClick(object sender, EventArgs e)
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

        private void NewPatchProjectMenuItemClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject == null)
                return;
            if (currentProject is Project p)
                p.Save(Path.GetDirectoryName(currentProject.Path));
            if (currentProject is PatchProject pp)
                pp.Save(Path.GetDirectoryName(currentProject.Path));
            dirty = false;
            UpdateTitle();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // OpenProject()
        }

        void OpenProject(ProjectHeader project)
        {
            Project editingProject = project as Project;
            PatchProject patchProject = project as PatchProject;
            
            if (editingProject.ValidateData())
                SetDirty();

            if (patchProject != null)
            {
                editingProject = patchProject.GenerateProject();
                if (editingProject.ValidateData())
                    return; // TODO: show a message about invalid project
            }

            currentProject = project;
            projectWorkspace.Project = editingProject;
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
