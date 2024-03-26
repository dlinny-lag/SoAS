using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using ScenesEditor.Data;
using Shared.Utils;

namespace ScenesEditor
{
    public sealed partial class ProjectEditDialog : Form
    {
        private Project project;
        private readonly Func<string, bool> projectNameVerifier;
        public ProjectEditDialog(Func<string, bool> nameVerifier = null)
        {
            InitializeComponent();
            projectNameVerifier = nameVerifier;
        }

        

        [Browsable(false)]
        public Project Project
        {
            get => project;
            set
            {
                bool changed = project != value;
                project = value;
                if (changed)
                    UpdateControls();
            }
        }

        bool VerifyControls()
        {
            if (!nameTextBox.Text.IsValidFilename() ||
                !(projectNameVerifier?.Invoke(nameTextBox.Text.Trim())??true))
            {
                nameTextBox.SelectAll();
                nameTextBox.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(authorTextBox.Text) && authorTextBox.Text.Length > Fallout4ModsHelper.MaxStringLength)
            {
                authorTextBox.SelectAll();
                authorTextBox.Focus();
                return false;
            }

            bool ValidateInt(TextBox textBox)
            {
                bool retVal = int.TryParse(textBox.Text, NumberStyles.None, CultureInfo.InvariantCulture, out var forDebug);
                if (!retVal)
                {
                    textBox.SelectAll();
                    textBox.Focus();
                }

                return retVal;
            }

            if (!ValidateInt(versionMajorTextBox))
                return false;
            if (!ValidateInt(versionMinorTextBox))
                return false;
            if (!ValidateInt(versionBuildTextBox))
                return false;

            if (string.IsNullOrWhiteSpace(espNameTextBox.Text))
                return true;

            if (!espNameTextBox.Text.IsValidFilename())
            {
                espNameTextBox.SelectAll();
                espNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(espNameTextBox.Text)))
            {
                espNameTextBox.Text = Path.ChangeExtension(espNameTextBox.Text, ProjectSerialization.ESPFileExtension);
                espNameTextBox.SelectAll();
                espNameTextBox.Focus();
                return false;
            }

            return true;
        }

        private void UpdateControls()
        {
            nameTextBox.Text = project?.Name ?? "";
            authorTextBox.Text = project?.Author ?? "";
            versionMajorTextBox.Text = project?.Version.Major.ToString() ?? "1";
            versionMinorTextBox.Text = project?.Version.Minor.ToString() ?? "0";
            versionBuildTextBox.Text = project?.Version.Build.ToString() ?? "0";
        }

        private void okBtn_Click(object sender, System.EventArgs e)
        {
            if (!VerifyControls())
                return;
            if (project == null)
                return;
            project.Name = nameTextBox.Text.Trim();
            project.Author = authorTextBox.Text.Trim();
            project.Version = new Version(
                int.Parse(versionMajorTextBox.Text, NumberStyles.None, CultureInfo.InvariantCulture),
                int.Parse(versionMinorTextBox.Text, NumberStyles.None, CultureInfo.InvariantCulture),
                int.Parse(versionBuildTextBox.Text, NumberStyles.None, CultureInfo.InvariantCulture));
            project.EspName = espNameTextBox.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
