using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AAF.Services.AAFImport;
using AAF.Services.Errors;

namespace Shared.Controls
{
    public abstract class IntegrityErrorsDialog<TE, TL, TD> : Form
        where TE: IntegrityError
        where TL: IntegrityErrorsList<TE>, new()
        where TD: IntegrityErrorsDialog<TE, TL, TD>, new()
    {
        private TL errorsList;
        private ColorsLegend legend;
        private Button okBtn;
        private void InitializeComponent()
        {
            legend = new ColorsLegend();
            legend.Dock = DockStyle.Top;

            errorsList = new TL();
            errorsList.Dock = DockStyle.Fill;

            okBtn = new Button();
            okBtn.Margin = new Padding(0, 10, 0, 10);
            okBtn.DialogResult = DialogResult.OK;
            okBtn.Dock = DockStyle.Bottom;
            okBtn.Text = "OK";

            Controls.Add(errorsList);
            Controls.Add(okBtn);
            Controls.Add(legend);

            Size = new Size(600, 300);

            errorsList.EditFinished += () => okBtn.Focus();
        }

        protected void SetData(IList<TE> data)
        {
            errorsList.Data = data;
        }
        protected IntegrityErrorsDialog()
        {
            InitializeComponent();
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

        public static void Show(IList<TE> errors)
        {
            if (errors.Count == 0)
                return;
            using (var dlg = new TD())
            {
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.Text += $" ({errors.Count})";
                dlg.SetData(errors);
                dlg.ShowDialog();
            }
        }
    }
}