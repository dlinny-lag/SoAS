using System;
using System.Windows.Forms;
using JsonTreeView.Editors;
using Newtonsoft.Json.Linq;

namespace JsonTreeView
{
    sealed class JValueContextMenuStrip : JTokenContextMenuStrip
    {
        public JValueContextMenuStrip()
        {
            Items.Add(new ToolStripSeparator());
            Items.Add(new ToolStripMenuItem("Change", null, OnChange));
        }

        private void OnChange(object sender, EventArgs e)
        {
            JValueTreeNode node = (JValueTreeNode)JTokenNode;
            using (var dlg = new ValueDialog { StartPosition = FormStartPosition.CenterParent })
            {
                dlg.Value = node.JValueTag.Type == JTokenType.String ? node.JValueTag.ToString() : node.Text;
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    UpdateSourceTreeNode(dlg.JsonValue);
                }
            }
        }
    }
}
