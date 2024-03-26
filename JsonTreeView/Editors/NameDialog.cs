using System;
using System.Windows.Forms;

namespace JsonTreeView.Editors
{
    public partial class NameDialog : Form
    {
        public NameDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            nameBox.Focus();
        }

        public string Value
        {
            get => nameBox.Text;
            set => nameBox.Text = value ?? string.Empty;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (!Value.IsNameValid())
            {
                nameBox.SelectAll();
                nameBox.Focus();
                return;
            }
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
