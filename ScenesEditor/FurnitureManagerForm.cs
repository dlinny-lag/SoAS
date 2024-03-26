using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AAF.Services;
using Shared.Utils;

namespace ScenesEditor
{
    public sealed partial class FurnitureManagerForm : Form
    {
        public FurnitureManagerForm()
        {
            InitializeComponent();
            fallout4PathText.Text = ApplicationSettings.FurnitureLibrary.Fallout4Path ?? "";
            FillTree();
        }

        private void FillTree()
        {
            furnitureTreeView.SuspendLayout();
            furnitureTreeView.Nodes.Clear();
            var sources = ApplicationSettings.FurnitureLibrary.Sources;
            sources.Sort();
            foreach (ModFile source in sources)
            {
                var node = furnitureTreeView.Nodes.Add(source.ToString(), source.ToString());
                List<FormItem> items = ApplicationSettings.FurnitureLibrary.GetForms(source);
                items.Sort();
                foreach (FormItem item in items)
                {
                    node.Nodes.Add(item.FormId.ToString(), item.IdTagsView);
                }
            }
            furnitureTreeView.ResumeLayout(true);
        }

        private void selectF4FolderBtn_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog(){ShowNewFolderButton = false})
            {
                if (DialogResult.OK != dlg.ShowDialog())
                    return;
                fallout4PathText.Text = dlg.SelectedPath;
                ApplicationSettings.FurnitureLibrary.Fallout4Path = fallout4PathText.Text;
            }
        }

        private void scanBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ApplicationSettings.FurnitureLibrary.Fallout4Path))
                return;

            var reader = new AAFReader(new FileSystemFilesStorage(Path.Combine(ApplicationSettings.FurnitureLibrary.Fallout4Path, "Data", "AAF"))); // TODO: NAF folder

            var data = reader.ReadAll();
            ApplicationSettings.FurnitureLibrary.Register(data.Furniture.Values);
            ApplicationSettings.FurnitureLibrary.Save();
            FillTree();
        }

        private void releaseBtn_Click(object sender, EventArgs e)
        {
            var packagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "furniture.zip");
            ApplicationSettings.FurnitureLibrary.Release(packagePath);
            string argument = $"/select, \"{packagePath}\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
    }
}
