using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JsonTreeView.Editors
{
    public partial class NameValueDialog : Form
    {
        public NameValueDialog()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            nameBox.Focus();
        }

        public string ValueName
        {
            get => nameBox.Text;
            set => nameBox.Text = value;
        }

        public string Value
        {
            get => valueBox.Text;
            set => valueBox.Text = value;
        }

        public JValue JsonValue => Value.ToJValue();

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (!ValueName.IsNameValid())
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
