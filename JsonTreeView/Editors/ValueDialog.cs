using System;
using System.Globalization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JsonTreeView.Editors
{
    public partial class ValueDialog : Form
    {
        public ValueDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            valueBox.Focus();
        }

        public string Value
        {
            get => valueBox.Text;
            set => valueBox.Text = value;
        }

        public JValue JsonValue => Value.ToJValue();

        private void okBtn_Click(object sender, EventArgs e)
        {
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
