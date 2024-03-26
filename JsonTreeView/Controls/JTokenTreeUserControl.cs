using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JsonTreeView.Linq;
using Newtonsoft.Json.Linq;

namespace JsonTreeView.Controls
{
    [DefaultEvent("AfterSelect")]
    public partial class JTokenTreeUserControl : UserControl
    {
        private Font collapsedFont;
        private Font expandedFont;
        public const int LoadedTreeDepth = 3;

        #region >> Constructors

        /// <summary>
        /// Creates a new instance of the control.
        /// </summary>
        public JTokenTreeUserControl()
        {
            InitializeComponent();

            jsonTreeView.AfterCollapse += OnJsonTreeViewAfterCollapse;
            jsonTreeView.AfterExpand += OnJsonTreeViewAfterExpand;
            jsonTreeView.AfterSelect += OnJsonTreeViewAfterSelect;
        }

        #endregion

        #region >> Events

        [Category("Behavior")]
        [Description("Occurs when the selection has been changed")]
        public event EventHandler<AfterSelectEventArgs> AfterSelect;

        #endregion

        #region >> Properties

        private JTokenRoot JsonEditorItem =>
            jsonTreeView.Nodes.Count != 0 ? new JTokenRoot(((JTokenTreeNode)jsonTreeView.Nodes[0]).JTokenTag) : null;

        public JToken Root => JsonEditorItem?.JTokenValue;

        public Font ExpandedFont
        {
            get { return expandedFont ?? (expandedFont = new Font(Font, FontStyle.Underline)); }
            set { expandedFont = value; }
        }

        public Font CollapsedFont
        {
            get { return collapsedFont ?? (collapsedFont = Font); }
            set { collapsedFont = value; }
        }

        #endregion
        public void SetJsonSource(JObject root)
        {
            jsonTreeView.Nodes.Clear();
            jsonTreeView.Nodes.Add(JsonTreeNodeFactory.Create(root));
            jsonTreeView.Nodes
                .Cast<TreeNode>()
                .ForEach(n => n.Expand());
        }

        /// <summary>
        /// Replaces the current node value with the value of <paramref name="jsonString"/>.
        /// </summary>
        /// <param name="jsonString"></param>
        public void UpdateSelected(string jsonString)
        {
            var node = jsonTreeView.SelectedNode as IJsonTreeNode;
            if (node == null)
            {
                return;
            }

            jsonTreeView.BeginUpdate();

            try
            {
                jsonTreeView.SelectedNode = node.AfterJsonTextChange(jsonString);
            }
            finally
            {
                jsonTreeView.EndUpdate();
            }
        }

        private void OnJsonTreeViewAfterCollapse(object sender, TreeViewEventArgs eventArgs)
        {
            var node = eventArgs.Node as IJsonTreeNode;

            node?.AfterCollapse();
        }

        private void OnJsonTreeViewAfterExpand(object sender, TreeViewEventArgs eventArgs)
        {
            var node = eventArgs.Node as IJsonTreeNode;

            node?.AfterExpand();
        }

        private void OnJsonTreeViewAfterSelect(object sender, TreeViewEventArgs eventArgs)
        {
            AfterSelectImplementation((dynamic)eventArgs.Node, eventArgs);
        }

        /// <summary>
        /// Default catcher in case of a node of unattended type.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="e"></param>
        // ReSharper disable once UnusedParameter.Local
        private void AfterSelectImplementation(TreeNode node, TreeViewEventArgs e)
        {
            AfterSelect?.Invoke(this, new AfterSelectEventArgs(
                $"{JTokenType.Undefined}: {node.GetType().FullName}",
                $"{JTokenType.Undefined}: {node.GetType().FullName}",
                () => ""));
        }

        // ReSharper disable once UnusedParameter.Local
        private void AfterSelectImplementation(JTokenTreeNode node, TreeViewEventArgs e)
        {
            AfterSelect?.Invoke(this, new AfterSelectEventArgs(
               $"{node.JTokenTag.Type}",
               $"{node.JTokenTag.Type}",
               () => $"{node.JTokenTag}"));
        }

        // ReSharper disable once UnusedParameter.Local
        private void AfterSelectImplementation(JValueTreeNode node, TreeViewEventArgs e)
        {
            AfterSelect?.Invoke(this, new AfterSelectEventArgs(
               node.Tag.GetType().Name,
               $"{node.JValueTag.Type}",
               () =>
               {
                   switch (node.JValueTag.Type)
                   {
                       case JTokenType.String:
                           return $@"""{node.JValueTag}""";
                       case JTokenType.Boolean:
                           return $"{node.JValueTag}".ToLower();
                       default:
                           return $"{node.JValueTag}";
                   }
               }));
        }

        private void OnJsonTreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs eventArgs)
        {
            jsonTreeView.SelectedNode = eventArgs.Node;
        }
    }
}
