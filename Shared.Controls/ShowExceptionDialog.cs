using System;
using System.Windows.Forms;

namespace Shared.Controls
{
    public partial class ShowExceptionDialog : Form
    {
        private string yesNote;
        private string noNote;
        private Exception exception;

        public ShowExceptionDialog()
        {
            InitializeComponent();
            yesNoteLbl.Visible = false;
            noNoteLbl.Visible = false;
            yesBtn.Visible = false;
            noBtn.Visible = false;
        }

        public Exception Exception
        {
            get => exception;
            set
            {
                exception = value;
                exceptionTextBox.Text = exception?.ToString() ?? string.Empty;
                exceptionTextBox.SelectAll();
            }
        }
        public string YesNote
        {
            get => yesNote;
            set
            {
                if (value == yesNote)
                    return;
                yesNote = value;
                SetupControls();
            }
        }
        public string NoNote
        {
            get => noNote;
            set
            {
                if (value == noNote)
                    return;
                noNote = value;
                SetupControls();
            }
        }

        public static DialogResult Show(Exception exception, string yesNote = null, string noNote = null, IWin32Window owner = null, FormStartPosition position = FormStartPosition.CenterScreen )
        {
            using (var dlg = new ShowExceptionDialog{StartPosition = position})
            {
                dlg.Exception = exception;
                dlg.YesNote = yesNote;
                dlg.NoNote = noNote;
                return dlg.ShowDialog(owner);
            }
        }

        private void SetupControls()
        {
            yesNoteLbl.Text = string.IsNullOrWhiteSpace(yesNote) ? "":$"Yes - {yesNote}";
            yesNoteLbl.Visible = yesBtn.Visible = !string.IsNullOrWhiteSpace(yesNote);

            noNoteLbl.Text = string.IsNullOrWhiteSpace(noNote) ? "":$"No - {noNote}";
            noNoteLbl.Visible = noBtn.Visible = !string.IsNullOrWhiteSpace(noNote);

            // TODO: handle location of buttons and dialog size
        }

        private void yesBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void noBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
