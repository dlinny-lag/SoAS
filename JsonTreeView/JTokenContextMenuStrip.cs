using System;
using System.Linq;
using System.Windows.Forms;
using JsonTreeView.Extensions;
using JsonTreeView.Linq;
using Newtonsoft.Json.Linq;

namespace JsonTreeView
{
    class JTokenContextMenuStrip : ContextMenuStrip
    {
        /// <summary>
        /// Source <see cref="TreeNode"/> at the origin of this <see cref="ContextMenuStrip"/>
        /// </summary>
        protected JTokenTreeNode JTokenNode;

        protected ToolStripItem CollapseAllToolStripItem;
        protected ToolStripItem ExpandAllToolStripItem;
        protected ToolStripItem DeleteNodeToolStripItem;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JTokenContextMenuStrip"/> class.
        /// </summary>
        public JTokenContextMenuStrip()
        {
            CollapseAllToolStripItem = new ToolStripMenuItem(Resources.Lang.CollapseAll, null, CollapseAll_Click);
            ExpandAllToolStripItem = new ToolStripMenuItem(Resources.Lang.ExpandAll, null, ExpandAll_Click);

            DeleteNodeToolStripItem = new ToolStripMenuItem(Resources.Lang.DeleteNode, null, DeleteNode_Click);

            Items.Add(CollapseAllToolStripItem);
            Items.Add(ExpandAllToolStripItem);
            Items.Add(new ToolStripSeparator());
            Items.Add(DeleteNodeToolStripItem);
        }

        #endregion

        #region >> ContextMenuStrip

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                JTokenNode = FindSourceTreeNode<JTokenTreeNode>();

                // Collapse item shown if node is expanded and has children
                CollapseAllToolStripItem.Visible = JTokenNode.IsExpanded
                    && JTokenNode.Nodes.Cast<TreeNode>().Any();

                // Expand item shown if node if not expanded or has a children not expanded
                ExpandAllToolStripItem.Visible = !JTokenNode.IsExpanded
                    || JTokenNode.Nodes.Cast<TreeNode>().Any(t => !t.IsExpanded);

                // Remove item enabled if it is not the root or the value of a property
                DeleteNodeToolStripItem.Enabled = (JTokenNode.Parent != null)
                    && !(JTokenNode.Parent is JPropertyTreeNode);
            }

            base.OnVisibleChanged(e);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="CollapseAllToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollapseAll_Click(Object sender, EventArgs e)
        {
            if (JTokenNode != null)
            {
                JTokenNode.TreeView.BeginUpdate();

                var nodes = JTokenNode.EnumerateNodes().Take(1000);
                foreach (var treeNode in nodes)
                {
                    treeNode.Collapse();
                }

                JTokenNode.TreeView.EndUpdate();
            }
        }
        
        /// <summary>
        /// Click event handler for <see cref="DeleteNodeToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteNode_Click(Object sender, EventArgs e)
        {
            try
            {
                JTokenNode.EditDelete();
            }
            catch (JTokenTreeNodeDeleteException exception)
            {
                MessageBox.Show(exception.InnerException?.Message, Resources.Lang.DeletionActionFailed);
            }
        }

        /// <summary>
        /// Click event handler for <see cref="ExpandAllToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExpandAll_Click(Object sender, EventArgs e)
        {
            if (JTokenNode != null)
            {
                JTokenNode.TreeView.BeginUpdate();

                var nodes = JTokenNode.EnumerateNodes().Take(1000);
                foreach (var treeNode in nodes)
                {
                    treeNode.Expand();
                }

                JTokenNode.TreeView.EndUpdate();
            }
        }
        
        /// <summary>
        /// Identify the Source <see cref="TreeNode"/> at the origin of this <see cref="ContextMenuStrip"/>.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="TreeNode"/> to return.</typeparam>
        /// <returns></returns>
        public T FindSourceTreeNode<T>() where T : TreeNode
        {
            var treeView = SourceControl as TreeView;

            return treeView?.SelectedNode as T;
        }

        protected void UpdateSourceTreeNode(JToken newToken)
        {
            var treeView = SourceControl as TreeView;
            if (treeView == null)
                return;
            treeView.BeginUpdate();
            try
            {
                var parent = JTokenNode.JTokenTag.Parent;
                JTokenNode.JTokenTag.Replace(newToken);
                JTokenNode.Tag = newToken;
                JTokenNode.UpdateText();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }
    }
}
