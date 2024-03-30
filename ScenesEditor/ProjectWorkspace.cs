using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SceneModel;
using ScenesEditor.Data;
using SceneServices;

namespace ScenesEditor
{
    public sealed partial class ProjectWorkspace : UserControl
    {
        private Project project;
        public ProjectWorkspace()
        {
            InitializeComponent();
            scenesEditor.MultiSelect = false;
            scenesEditor.SceneDoubleClick += ScenesEditorOnSceneDoubleClick;
            scenesEditor.IsHighlighted = IsReadyForRelease;
        }

        private bool IsReadyForRelease(Scene scene)
        {
            if (scene == null)
                return false;
            if (project == null)
                return false;
            return project.IsReadyForRelease(scene);
        }

        public event Action Changed; 

        private void ScenesEditorOnSceneDoubleClick(Scene scene)
        {
            using (var dlg = new SceneEditorDialog())
            {
                dlg.Init(scene, project.IsReadyForRelease(scene));
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    scene.ShallowInitFromOther(dlg.Scene);

                    if (dlg.IsReadyForRelease)
                        project.MarkReadyForRelease(scene);
                    else
                        project.UnmarkReadyForRelease(scene);

                    scenesEditor.NotifyHighlightChanged();
                    Changed?.Invoke();
                }
            }
        }

        public Project Project
        {
            get => project;
            set
            {
                if (project == value)
                    return;
                project = value;
                RefreshList();
            }
        }

        private void RefreshList()
        {
            var scenes = new List<Scene>(project?.Scenes ?? Array.Empty<Scene>());
            scenesEditor.Init(null, new BuildResult(scenes, null));
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            if (project == null)
                return;
            using (var dlg = new ImportScenesDialog{StartPosition = FormStartPosition.CenterScreen})
            {
                dlg.Project = project;
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    if (dlg.NewScenes.Count == 0)
                        return;
                    project.Add(dlg.NewScenes);
                    RefreshList();
                    Changed?.Invoke();
                }
            }
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            if (project == null)
                return;

            using (var dlg = new ProjectEditDialog{StartPosition = FormStartPosition.CenterParent})
            {
                dlg.Project = project;
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    project.Updated = DateTimeOffset.Now;
                    if (project.UpdateSettings())
                        Changed?.Invoke();
                }
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (project == null)
                return;

            using (var dlg = new DeleteScenesDialog{StartPosition = FormStartPosition.CenterParent})
            {
                dlg.Project = project;
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    var toDelete = dlg.Selected;
                    if (toDelete.Count == 0)
                        return;

                    project.Remove(toDelete);
                    RefreshList();
                    Changed?.Invoke();
                }
            }
        }

        private void debugBtn_Click(object sender, EventArgs e)
        {
            if (project == null)
                return;

            using (var dlg = new DebugDialog())
            {
                dlg.Project = project;
                dlg.ShowDialog(this);
            }
        }

        private static string DefaultDataSetFolder;

        internal Project LoadFromDefaultDataSet()
        {
            if (!ApplicationSettings.EditDefaultDataSetMode)
                throw new InvalidOperationException("Edit default data set mode must be on");
            if (string.IsNullOrWhiteSpace(DefaultDataSetFolder) || !Directory.Exists(DefaultDataSetFolder))
            {
                using (FolderBrowserDialog dlg = new FolderBrowserDialog{ShowNewFolderButton = false, Description = @"Select Scenes folder"})
                {
                    if (DialogResult.OK != dlg.ShowDialog())
                        return null;

                    DefaultDataSetFolder = dlg.SelectedPath;
                }
            }
            return ProjectSerialization.LoadDefaultDataSetFromFolder(DefaultDataSetFolder);
        }

        private void releaseBtn_Click(object sender, EventArgs e)
        {
            if (project == null)
                return;

            if (ApplicationSettings.EditDefaultDataSetMode)
            {
                project.ReleaseDefaultDataSet(DefaultDataSetFolder);
                return;
            }

            if (project.Release(out string packagePath))
                Changed?.Invoke();

            string argument = $"/select, \"{packagePath}\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

    }
}
