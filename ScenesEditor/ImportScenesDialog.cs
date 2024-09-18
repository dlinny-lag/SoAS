using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AAF.Services;
using AAFModel;
using SceneModel;
using ScenesEditor.Data;
using SceneServices;
using SceneServices.Scenes;
using Shared.Controls;
using Shared.Utils;

namespace ScenesEditor
{
    public sealed partial class ImportScenesDialog : Form
    {
        private HashSet<string> projectSceneNames = new HashSet<string>();
        private const string CustomImportingFolderKey = "CustomImportingFolder";
        private const string ImportingFolderKey = "ImportingFolder";
        private readonly List<Scene> importingScenes = new List<Scene>();

        enum FolderType
        {
            Custom,
            AAF,
            NAF,
        }
        class FolderRef
        {
            public FolderType Type { get; set; }
            public string Path { get; set; }
            public override string ToString()
            {
                return $"[{Type}] {Path}";
            }
        }

        private string ImportingFolder
        {
            get
            {
                var selected = importingFolder.SelectedItem as FolderRef;
                return selected?.Path ?? string.Empty;
            }
        }

        private readonly AppRegistry appRegistry = ApplicationSettings.AppRegistry;
        private Project project;
        public ImportScenesDialog()
        {
            InitializeComponent();
            FillPaths();
            SelectDefaultPath();
            importingList.CustomFilter = CanImport;
        }

        private void FillPaths()
        {
            var fo4 = FoldersExtension.FindFo4Folder();
            if (fo4 != null)
            {
                string aafPath = FoldersExtension.FindAAFFolder();
                if (!string.IsNullOrWhiteSpace(aafPath))
                {
                    importingFolder.Items.Add(new FolderRef { Path = aafPath, Type = FolderType.AAF });
                }

                string nafPath = Path.Combine(fo4, "Data", "NAF");
                if (!Directory.Exists(nafPath))
                    nafPath = null;
                if (!string.IsNullOrWhiteSpace(nafPath))
                {
                    importingFolder.Items.Add(new FolderRef { Path = nafPath, Type = FolderType.NAF });
                }
            }

            string customPath = appRegistry.StoredFolder(CustomImportingFolderKey);
            importingFolder.Items.Add(new FolderRef { Path = customPath ?? string.Empty, Type = FolderType.Custom });
        }

        private void SelectDefaultPath()
        {
            var selected = appRegistry.StoredFolder(ImportingFolderKey);
            if (!string.IsNullOrWhiteSpace(selected))
            {
                for (int i = 0; i < importingFolder.Items.Count; i++)
                {
                    FolderRef folder = importingFolder.Items[i] as FolderRef;
                    if (folder == null)
                        throw new InvalidOperationException("Folder is not assigned to item");
                    if (string.Compare(folder.Path, selected, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        importingFolder.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (importingFolder.SelectedIndex < 0)
                importingFolder.SelectedIndex = importingFolder.Items.Count - 1;
        }

        private bool CanImport(Scene scene)
        {
            return !projectSceneNames.Contains(scene.Id);
        }
        public Project Project
        {
            get => project;
            set
            {
                project = value;
                UpdateProjectList();
            }
        }

        private void UpdateProjectList()
        {
            List<Scene> scenes = new List<Scene>(project?.Scenes ?? Array.Empty<Scene>());
            scenes.AddRange(importingScenes);
            projectSceneNames = scenes.Select(s => s.Id).ToHashSet();
            projectsList.Init(null, new BuildResult(scenes, null), true);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SetupLayout();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            SetupLayout();
        }

        void SetupLayout()
        {
            int panelTop = importingFolder.Bottom + Margin.Vertical;

            int panelWidth = (ClientSize.Width - Margin.Horizontal * 2 - addBtn.Width) / 2;
            int panelHeight = ClientSize.Height - panelTop - okBtn.Height - Margin.Vertical * 2;

            importingPanel.Location = new Point(Margin.Left, panelTop);
            importingPanel.Size = new Size(panelWidth, panelHeight);

            addBtn.Location = new Point(Margin.Horizontal + panelWidth, addBtn.Top);
            removeBtn.Location = new Point(Margin.Horizontal + panelWidth, removeBtn.Top);

            projectPanel.Location = new Point(addBtn.Right + Margin.Left, panelTop);
            projectPanel.Size = new Size(panelWidth, panelHeight);


            okBtn.Location = new Point((ClientSize.Width-okBtn.Width)/2, importingPanel.Bottom + Margin.Vertical);
            cancelBtn.Location = new Point(ClientSize.Width - cancelBtn.Width - Margin.Horizontal, importingPanel.Bottom + Margin.Vertical);
        }

        private void customFolderBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog() { ShowNewFolderButton = false })
            {
                if (DialogResult.OK != dlg.ShowDialog())
                    return;
                var customFolder = importingFolder.Items[importingFolder.SelectedIndex] as FolderRef;
                if (customFolder == null || customFolder.Type != FolderType.Custom)
                    throw new InvalidOperationException("Custom folder item is not selected");
                customFolder.Path = dlg.SelectedPath;
                importingFolder.Items[importingFolder.SelectedIndex] = customFolder; // enforce updating

                appRegistry.StoreFolder(CustomImportingFolderKey, ImportingFolder);
                appRegistry.StoreFolder(ImportingFolderKey, ImportingFolder);
            }
        }

        private void importingFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            customFolderBtn.Enabled = importingFolder.SelectedItem is FolderRef folder && folder.Type == FolderType.Custom;
            appRegistry.StoreFolder(ImportingFolderKey, ImportingFolder);
        }

        AAFData ReadAll()
        {
            if (string.IsNullOrWhiteSpace(ImportingFolder))
            {
                MessageBox.Show("Please specify folder", "Error");
                return null;
            }
            
            var reader = new AAFReader(new FileSystemFilesStorage(ImportingFolder));

            return reader.ReadAll(false);
        }

        private void scanBtn_Click(object sender, EventArgs e)
        {
            AAFData data = ReadAll();
            FileLoadFailuresDialog.Show(data.FailedFiles);

            BuildResult result = data.GenerateScenes();

            importingList.Init(data, result);

            HashSet<string> unknownTags = result.Scenes.SelectMany(scene => scene.Imported.Unknown).ToHashSet();
            var unknown = unknownTags.ToList();
            unknown.Sort();
            foreach (string tag in unknown)
            {
                Console.WriteLine(tag);
            }

            Console.WriteLine("Used skeletons: ");
            var skeletons = result.Scenes.SelectMany(s => s.Participants.Select(p => p.Skeleton)).Distinct().ToList();
            skeletons.Sort(CompareSkeletons);
            foreach (string s in skeletons)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();
            Console.WriteLine("Declared skeletons");
            
            var declaredSkeletons = data.Races.Select(r => GetSkeleton(r, data.Files)).Distinct().ToList();
            declaredSkeletons.Sort(CompareSkeletons);
            foreach (string s in declaredSkeletons)
            {
                Console.WriteLine(s);
            }
        }
        private static string GetSkeleton(Race r, IDictionary<string, Defaults> defaults)
        {
            if (!string.IsNullOrWhiteSpace(r.Skeleton))
                return r.Skeleton;
            return defaults[r.File].Skeleton;
        }

        private static int CompareSkeletons(string a, string b)
        {
            int byLength = a.Length.CompareTo(b.Length);
            if (byLength != 0)
                return byLength;
            return a.CompareTo(b);
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            var newScenes = importingList.Selected;
            if (newScenes.Count == 0)
                return;
            importingScenes.AddRange(newScenes);
            UpdateProjectList();
            importingList.NotifyFilterUpdated();
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            var removingScenes = projectsList.Selected;
            if (removingScenes.Count == 0)
                return;
            importingScenes.RemoveRange(removingScenes);
            UpdateProjectList();
            importingList.NotifyFilterUpdated();
        }


        public IList<Scene> NewScenes => importingScenes;
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
    }
}
