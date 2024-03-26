using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ScenesEditor
{
    public sealed partial class TagsListEditDialog : Form
    {
        private List<string> tags = new List<string>();
        public TagsListEditDialog()
        {
            InitializeComponent();
            tagsList.Columns.Add(new DataGridViewTextBoxColumn
            {
                ValueType = typeof(string),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle{WrapMode = DataGridViewTriState.False}
            });
            tagsList.BackgroundColor = tagsList.DefaultCellStyle.BackColor;
            SetButtonsLocation();
            tagsList.CellValueChanged += TagsList_CellValueChanged;
        }

        private static string ValidateTag(string tag)
        {
            return tag.Replace(";", "").Trim();
        }

        private void TagsList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string value = tagsList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
            if (string.IsNullOrWhiteSpace(value))
            {
                tagsList.Rows.RemoveAt(e.RowIndex);
                return;
            }

            value = ValidateTag(value);
            tagsList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            SetButtonsLocation();
        }

        public string[] Tags
        {
            get => tags.ToArray();
            set
            {
                tags = value == null ? new List<string>() : new List<string>(value);
                tagsList.Rows.Clear();
                tagsList.Rows.AddRange(tags.Select(t =>
                        new DataGridViewRow{Cells = { new DataGridViewTextBoxCell{Value = t} }}).ToArray());
            }
        }

        void SetButtonsLocation()
        {
            okBtn.Location = new Point((ClientSize.Width-okBtn.Width)/2, tagsList.Bottom + Margin.Vertical*2);
            cancelBtn.Location = new Point(ClientSize.Width - cancelBtn.Width - Margin.Horizontal, tagsList.Bottom + Margin.Vertical*2);
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            tags = tagsList.Rows.Cast<DataGridViewRow>().Select(r => (string)r.Cells[0].Value).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
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
