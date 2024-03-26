using System;
using System.Windows.Forms;
using JsonTreeView.Editors;
using Newtonsoft.Json.Linq;

namespace JsonTreeView
{
    sealed class  JObjectContextMenuStrip : JTokenContextMenuStrip
    {

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JObjectContextMenuStrip"/> class.
        /// </summary>
        public JObjectContextMenuStrip()
        {
            Items.Add(new ToolStripSeparator());
            Items.Add(new ToolStripMenuItem(Resources.Lang.InsertPropertyAsValue, null, InsertProperty_Click));
            Items.Add(new ToolStripMenuItem(Resources.Lang.InsertPropertyAsArray, null, InsertPropertyAsArray_Click));
            Items.Add(new ToolStripMenuItem(Resources.Lang.InsertPropertyAsObject, null, InsertPropertyAsObject_Click));
        }

        #endregion

        private void InsertProperty(string name, JToken value)
        {
            var jObjectTreeNode = JTokenNode as JObjectTreeNode;

            if (jObjectTreeNode == null)
            {
                return;
            }

            var newJProperty = new JProperty(name, value);
            jObjectTreeNode.JObjectTag.AddFirst(newJProperty);

            var jPropertyTreeNode = (JPropertyTreeNode)JsonTreeNodeFactory.Create(newJProperty);
            jObjectTreeNode.Nodes.Insert(0, jPropertyTreeNode);

            jObjectTreeNode.TreeView.SelectedNode = jPropertyTreeNode;
        }

        /// <summary>
        /// Click event handler for <see cref="InsertPropertyAsValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertProperty_Click(Object sender, EventArgs e)
        {
            using (var dlg = new NameValueDialog(){StartPosition = FormStartPosition.CenterParent})
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    InsertProperty(dlg.ValueName, dlg.JsonValue);
                }
            }
        }

        /// <summary>
        /// Click event handler for <see cref="InsertPropertyAsArrayToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertPropertyAsArray_Click(object sender, EventArgs e)
        {
            using (var dlg = new NameDialog { StartPosition = FormStartPosition.CenterParent })
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    InsertProperty(dlg.Value, new JArray());
                }
            }
        }

        /// <summary>
        /// Click event handler for <see cref="InsertPropertyAsObjectToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertPropertyAsObject_Click(object sender, EventArgs e)
        {
            using (var dlg = new NameDialog { StartPosition = FormStartPosition.CenterParent })
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    InsertProperty(dlg.Value, new JObject());
                }
            }
        }
    }
}
