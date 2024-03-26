using System.Linq;
using System.Windows.Forms;

namespace ScenesEditor
{
    public sealed partial class TagsListViewDialog : Form
    {
        private string[] tags;
        public TagsListViewDialog()
        {
            InitializeComponent();
            tagsList.Columns.Add(new DataGridViewTextBoxColumn
            {
                ValueType = typeof(string),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle{WrapMode = DataGridViewTriState.False}
                
            });
            tagsList.BackgroundColor = tagsList.DefaultCellStyle.BackColor;
        }

        public string[] Tags
        {
            get => tags;
            set
            {
                tags = value;
                tagsList.Rows.Clear();
                tagsList.Rows.AddRange(tags.Select(t =>
                    new DataGridViewRow{Cells = { new DataGridViewTextBoxCell{Value = t} }})
                    .ToArray());
            }
        }

        private void closeBtn_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
