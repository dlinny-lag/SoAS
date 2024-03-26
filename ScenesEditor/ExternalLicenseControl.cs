using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ScenesEditor
{
    public class LicenseReference
    {
        public string Name;
        public Assembly Assembly;
        public string Resource;
    }

    public sealed partial class ExternalLicenseControl : UserControl
    {
        private LicenseReference reference;
        public ExternalLicenseControl()
        {
            InitializeComponent();
            moduleNameLink.ClientSizeChanged += ModuleNameLinkClientSizeChanged;
            moduleNameLink.Location = new Point(Margin.Left, Margin.Top);
            ModuleNameLinkClientSizeChanged(moduleNameLink, null);
        }

        private void ModuleNameLinkClientSizeChanged(object sender, EventArgs e)
        {
            ClientSize = moduleNameLink.ClientSize + Margin.Size;
        }

        [Browsable(false)]
        public LicenseReference LicenseReference
        {
            get => reference;
            set
            {
                if (reference == value)
                    return;
                reference = value;
                UpdateControls();
            }
        }

        private void UpdateControls()
        {
            moduleNameLink.Text = reference?.Name ?? "";
        }

        private void openTextLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (reference == null)
                return;
            string text;
            string fullResourceName = $"{reference.Assembly.GetName().Name}.{reference.Resource}";
            using(var stream = reference.Assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException($"Failed to get resource {fullResourceName}");
                using (StreamReader reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }

            using (var dlg = new TextDialog(){Text = reference.Name, StartPosition = FormStartPosition.CenterParent})
            {
                dlg.Content = text;
                dlg.ShowDialog(this);
            }
        }
    }
}
