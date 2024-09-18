using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AAF.Services;
using AAF.Services.AAFImport;
using AAFModel;
using Shared.Controls;
using Shared.Utils;

namespace AAFXmlScanner
{
    public sealed partial class MainForm : Form
    {
        private AAFReader reader;
        private AAFData aafModel;
        private ImportResult result;
        private readonly AppRegistry appRegistry = new AppRegistry("AAFXmlScanner");
        public MainForm()
        {
            InitializeComponent();
            var ver = GetType().Assembly.GetName().Version;
            Text += $" {ver.Major}.{ver.Minor}.{ver.Build}";
            aafFolderPath.TextChanged += AafFolderPathOnTextChanged;
            TryInitAAFFolder();
        }

        private void AafFolderPathOnTextChanged(object sender, EventArgs e)
        {
            reader = null;
            aafModel = null;
            result = null;
            EnableButtons();
        }

        void TryInitAAFFolder()
        {
            string aafFolder = appRegistry.StoredAAFFolder();
            if (aafFolder == null)
            {
                aafFolder = FoldersExtension.FindAAFFolder();
                if (aafFolder != null)
                    appRegistry.StoreAAFFolder(aafFolder);
            }

            aafFolderPath.Text = aafFolder ?? "";
        }

        AAFData ReadAll()
        {
            if (string.IsNullOrWhiteSpace(aafFolderPath.Text))
            {
                MessageBox.Show("Please specify AAF folder", "Error");
                return null;
            }

            if (reader == null)
                reader = new AAFReader(new FileSystemFilesStorage(aafFolderPath.Text));

            return reader.ReadAll(chkStrictMode.Checked);
        }

        private void selectAAFFolderBtn_Click(object sender, System.EventArgs e)
        {
            
            using (var dlg = new FolderBrowserDialog(){ShowNewFolderButton = false})
            {
                if (DialogResult.OK != dlg.ShowDialog())
                    return;
                appRegistry.StoreAAFFolder(dlg.SelectedPath);
                aafFolderPath.Text = dlg.SelectedPath;
            }
        }

        private void DisableButtons()
        {
            btnScan.Enabled = false;
            btnExport.Enabled = false;
            btnStatistics.Enabled = false;
            btnFileErrors.Enabled = false;
            btnParseErrors.Enabled = false;
            btnParseWarnings.Enabled = false;
            btnAnimationDuplications.Enabled = false;
            btnAnimationGroupDuplications.Enabled = false;
            btnPositionDuplications.Enabled = false;
            btnPositionTreeDuplications.Enabled = false;
            btnRaceDuplications.Enabled = false;
            btnValidationErrors.Enabled = false;
        }

        private void EnableButtons()
        {
            btnScan.Enabled = true;
            btnExport.Enabled = aafModel != null;
            btnStatistics.Enabled = aafModel != null && result != null;
            btnFileErrors.Enabled = aafModel?.FailedFiles?.Count > 0;
            btnParseErrors.Enabled = aafModel?.Errors?.Count > 0;
            btnParseWarnings.Enabled = aafModel?.Warnings?.Count > 0;
            btnAnimationDuplications.Enabled = result?.Errors?.AnimationDuplications?.Count > 0;
            btnAnimationGroupDuplications.Enabled = result?.Errors?.AnimationGroupDuplications?.Count > 0;
            btnPositionDuplications.Enabled = result?.Errors?.PositionDuplications?.Count > 0;
            btnPositionTreeDuplications.Enabled = result?.Errors?.PositionTreeDuplications?.Count > 0;;
            btnRaceDuplications.Enabled = result?.Errors?.RaceDuplications?.Count > 0;
            btnValidationErrors.Enabled = result?.Errors?.ValidationErrors?.Count > 0;
        }
        private void btnScan_Click(object sender, System.EventArgs e)
        {
            DisableButtons();
            aafModel = null;
            result = null;
            try
            {
                aafModel = ReadAll();
                result = ImportResult.Import(aafModel);
            }
            finally
            {
                EnableButtons();
            }
        }

        private void btnFileErrors_Click(object sender, System.EventArgs e)
        {
            FileLoadFailuresDialog.Show(aafModel?.FailedFiles);
        }

        private void btnParseErrors_Click(object sender, System.EventArgs e)
        {
            FailedFilesDialog<IList<string>, FileStringFailureList>.Show(aafModel?.Errors);
        }

        private void btnParseWarnings_Click(object sender, System.EventArgs e)
        {
            FailedFilesDialog<IList<string>, FileStringFailureList>.Show(aafModel?.Warnings);
        }

        private void btnAnimationDuplications_Click(object sender, System.EventArgs e)
        {
            AnimationsDuplicationDialog.Show(result?.Errors?.AnimationDuplications?.Values.ToArray());
        }

        private void btnAnimationGroupDuplications_Click(object sender, System.EventArgs e)
        {
            AnimationsGroupDuplicationDialog.Show(result?.Errors?.AnimationGroupDuplications?.Values.ToArray());
        }

        private void btnPositionDuplications_Click(object sender, System.EventArgs e)
        {
            PositionDuplicationDialog.Show(result?.Errors?.PositionDuplications?.Values.ToArray());
        }

        private void btnPositionTreeDuplications_Click(object sender, System.EventArgs e)
        {
            PositionTreeDuplicationDialog.Show(result?.Errors?.PositionTreeDuplications?.Values.ToArray());
        }

        private void btnRaceDuplications_Click(object sender, System.EventArgs e)
        {
            RaceDuplicationDialog.Show(result?.Errors?.RaceDuplications?.Values.ToArray());
        }

        private void btnValidationErrors_Click(object sender, System.EventArgs e)
        {
            PositionValidationErrorsDialog.Show(result?.Errors?.ValidationErrors?.SelectMany(p => p.Value).ToArray());
        }

        private void btnExport_Click(object sender, System.EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.AddExtension = true;
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    using(var stream = dlg.OpenFile())
                        stream.ExportText(aafModel, result);
                }
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            using (var dlg = new StatisticsDialog(aafModel, result))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
