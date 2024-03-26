using System;
using System.Drawing;
using System.Windows.Forms;
using JsonTreeView.Views;

namespace ScenesEditor
{
    public sealed partial class AboutDialog : Form
    {
        private static readonly LicenseReference[] licenses = new[]
        {
            new LicenseReference
            {
                Name = "Drop-Down Controls for .NET",
                Resource = "LICENSE.TXT",
                Assembly = typeof(ComboTreeBox).Assembly,
            },
            new LicenseReference
            {
                Name = "JSon-Editor by ZTn",
                Resource = "LICENSE",
                Assembly = typeof(JTokenTreeView).Assembly,
            },

        };

        public AboutDialog()
        {
            InitializeComponent();
            InitVersion();
            InitLicenseControls();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            MinimumSize = new Size(Width, (Height - ClientSize.Height) +
                licensesGroupBox.Bottom + Margin.Vertical * 2 + closeBtn.Height + Margin.Vertical);
        }

        private void InitVersion()
        {
            versionLabel.Text = typeof(AboutDialog).Assembly.GetName().Version.ToString(3);
        }

        private void InitLicenseControls()
        {
            int y = licensesGroupBox.DisplayRectangle.Y + Margin.Vertical;
            for (int i = 0; i < licenses.Length; i++)
            {
                var control = new ExternalLicenseControl { LicenseReference = licenses[i] };
                control.Location = new Point(Margin.Left, y);
                licensesGroupBox.Controls.Add(control);
                y += control.Height + Margin.Bottom;
            }

            licensesGroupBox.Height = y + Margin.Bottom;
        }

        private void closeBtn_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
