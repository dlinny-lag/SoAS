using System;
using System.Windows.Forms;

namespace ScenesEditor
{
    public sealed partial class TextDialog : Form
    {
        public TextDialog()
        {
            InitializeComponent();
        }

        public string Content
        {
            get => contentTextBox.Text;
            set => contentTextBox.Text = value ?? "";
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
