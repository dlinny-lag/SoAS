using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JsonTreeView
{
    sealed class JArrayContextMenuStrip : JTokenContextMenuStrip
    {

        #region >> Constructors

        public JArrayContextMenuStrip()
        {
            Items.Add(new ToolStripSeparator());
            Items.Add(new ToolStripMenuItem(Resources.Lang.InsertArray, null, InsertArray_Click));
            Items.Add(new ToolStripMenuItem(Resources.Lang.InsertObject, null, InsertObject_Click));
            Items.Add(new ToolStripMenuItem(Resources.Lang.InsertValue, null, InsertValue_Click));
        }

        #endregion

        private void InsertArray_Click(Object sender, EventArgs e)
        {
            InsertJToken(JArray.Parse("[]"));
        }

        private void InsertObject_Click(Object sender, EventArgs e)
        {
            InsertJToken(JObject.Parse("{}"));
        }

        private void InsertValue_Click(Object sender, EventArgs e)
        {
            InsertJToken(JToken.Parse("null"));
        }

        /// <summary>
        /// Add a new <see cref="JToken"/> instance in current <see cref="JArrayTreeNode"/>
        /// </summary>
        /// <param name="newJToken"></param>
        private void InsertJToken(JToken newJToken)
        {
            var jArrayTreeNode = JTokenNode as JArrayTreeNode;

            if (jArrayTreeNode == null)
            {
                return;
            }

            jArrayTreeNode.JArrayTag.AddFirst(newJToken);

            TreeNode newTreeNode = JsonTreeNodeFactory.Create(newJToken);
            jArrayTreeNode.Nodes.Insert(0, newTreeNode);

            jArrayTreeNode.TreeView.SelectedNode = newTreeNode;
        }
    }
}
