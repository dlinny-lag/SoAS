using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    public class FailedFilesDialog<T, TL> : Form
        where T: class
        where TL: FailedFilesList<T>, new()
    {
        private FailedFilesList<T> errorsList;
        private Button okBtn;
        private void InitializeComponent()
        {
            errorsList = new TL();
            errorsList.Dock = DockStyle.Fill;

            okBtn = new Button();
            okBtn.Margin = new Padding(0, 10, 0, 10);
            okBtn.DialogResult = DialogResult.OK;
            okBtn.Dock = DockStyle.Bottom;
            okBtn.Text = "OK";

            Controls.Add(errorsList);
            Controls.Add(okBtn);

            Size = new Size(600, 300);
        }

        public FailedFilesDialog()
        {
            InitializeComponent();
            errorsList.EditFinished += () => okBtn.Focus();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            okBtn.Focus();
        }

        protected sealed override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            okBtn.Left = (ClientSize.Width - okBtn.Width) / 2 ;
            okBtn.Top = (ClientSize.Height - okBtn.Height) / 2;
        }
        public static void Show(IDictionary<string, T> errors)
        {
            if (errors == null || errors.Count == 0)
                return;
            using (var dlg = new FailedFilesDialog<T, TL>())
            {
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.Text = $"Files failed to parse ({errors.Count})";
                dlg.errorsList.SetData(errors);
                dlg.ShowDialog();
            }
        }
    }
}