
using System;
using System.Windows.Forms;
using JsonTreeView.Editors;
using Newtonsoft.Json.Linq;

namespace JsonTreeView
{
    sealed class JPropertyContextMenuStrip : JTokenContextMenuStrip
    {
        public JPropertyContextMenuStrip()
        {
            Items.Add(new ToolStripSeparator());
            Items.Add(new ToolStripMenuItem("Rename", null, OnRename));
        }

        private void OnRename(object sender, EventArgs e)
        {
            JPropertyTreeNode node = (JPropertyTreeNode)JTokenNode;
            using (var dlg = new NameDialog { Value = node.JPropertyTag.Name, StartPosition = FormStartPosition.CenterParent })
            {
                if (DialogResult.OK != dlg.ShowDialog(this))
                    return;
                var tmp = new JProperty(dlg.Value, node.JPropertyTag.Value);
                UpdateSourceTreeNode(tmp);
            }
        }
    }
}
