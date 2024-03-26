using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AAFModel;
using SceneModel;
using SceneServices;
using Shared.Controls;

namespace ScenesEditor
{
    public sealed partial class ScenesEditorControl : UserControl
    {
        private class SceneNameBoxCell : DataGridViewTextBoxCell
        {
            public ScenesEditorControl Owner { get; set; }
            public static Color HighlightedColor { get; set; } = Color.DarkGreen;
            public override DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
            {
                var retVal = base.GetInheritedStyle(inheritedCellStyle, rowIndex, includeColors);
                if (Owner == null)
                    return retVal;
                RowSettings settings = Owner.GetRowSettings(rowIndex);
                if (settings == null)
                    return retVal;

                if (settings.IsHighLighted)
                {
                    retVal.ForeColor = HighlightedColor;
                    retVal.SelectionForeColor = HighlightedColor;
                }
                retVal.BackColor = settings.SceneTypeColor;

                return retVal;
            }

            public override object Clone()
            {
                SceneNameBoxCell retVal = (SceneNameBoxCell)base.Clone();
                retVal.Owner = Owner;
                return retVal;
            }
        }

        class RowSettings
        {
            public bool IsHighLighted;
            public Color SceneTypeColor;
        }

        private static Color TypeColor(SceneType type)
        {
            switch (type)
            {
                case SceneType.Sequence : return Color.LightBlue;
                case SceneType.Tree : return Color.LightPink;
                case SceneType.Single: return Color.White;
                default: return Color.White;
            }
        }
        private RowSettings GetRowSettings(int rowIndex)
        {
            if (filtered == null)
                return null;
            if (rowIndex < 0 || rowIndex >= filtered.Count)
                return null;
            RowSettings retVal = new RowSettings();
            Scene scene = filtered[rowIndex];
            retVal.SceneTypeColor = TypeColor(scene.Type);
            if (IsHighlighted != null)
                retVal.IsHighLighted = IsHighlighted(scene);
            return retVal;
        }


        private AAFData aafModel; // TODO: show errors/warnings
        private BuildResult scenesModel;
        private IList<Scene> filtered;
        public ScenesEditorControl()
        {
            InitializeComponent();
            scenesView.UpdateStyle(ControlStyles.OptimizedDoubleBuffer, true);
            scenesView.UpdateStyle(ControlStyles.ResizeRedraw, true);
            scenesView.Columns.Clear();
            scenesView.Columns.Add(new DataGridViewTextBoxColumn
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                CellTemplate = new SceneNameBoxCell {Owner = this},
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            });
            scenesView.BackgroundColor = scenesView.DefaultCellStyle.BackColor;
            scenesView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            scenesView.SelectionChanged += (sender, args) => SelectionChanged?.Invoke();
        }

        public void Init(AAFData aafData, BuildResult buildResult, bool keepSelection = false)
        {
            aafModel = aafData;
            scenesModel = buildResult;
            SetData(keepSelection);
        }

        public bool MultiSelect
        {
            get => scenesView.MultiSelect;
            set => scenesView.MultiSelect = value;
        }

        [Browsable(false)]
        public event Action<Scene> SceneDoubleClick;

        [Browsable(false)]
        public event Action SelectionChanged; 

        public ICollection<Scene> Selected
        {
            get
            {
                List<Scene> retVal = new List<Scene>(scenesView.SelectedRows.Count);
                List<int> indices = scenesView.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).ToList();
                indices.Sort();
                foreach (int index in indices)
                {
                    retVal.Add(filtered[index]);
                }

                return retVal;
            }
        }
        private void scenesView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Scene scene = filtered[e.RowIndex];
            SceneDoubleClick?.Invoke(scene);
        }

        private void scenesView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (filtered == null)
                return;
            e.Value = filtered[e.RowIndex].Id;
        }

        private Func<Scene, bool> customFilter;
        [Browsable(false)]
        public Func<Scene, bool> CustomFilter
        {
            get => customFilter;
            set
            {
                if (customFilter == value)
                    return;
                customFilter = value;
                SetData(false);
            }
        }

        public Func<Scene, bool> IsHighlighted { get; set; }

        public void NotifyFilterUpdated()
        {
            SetData(true);
        }

        public void NotifyHighlightChanged()
        {
            scenesView.Invalidate();
        }

        private void SetData(bool keepSelection)
        {
            int firstSelected = -1;
            if (keepSelection)
            {
                if (scenesView.SelectedRows.Count > 0)
                    firstSelected = scenesView.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).Min();
            }
            scenesView.RowCount = 0;
            if (scenesModel == null)
                return;
            string pattern = filterText.Text;
            IEnumerable<Scene> filter = scenesModel.Scenes;
            if (!string.IsNullOrWhiteSpace(pattern))
                filter = filter.Where(s => s.Id.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) >= 0);
            if (CustomFilter != null)
                filter = filter.Where(s => CustomFilter(s));

            filtered = filter.ToArray();
            scenesView.RowCount = filtered.Count;

            // update selection if necessary
            if (firstSelected >= filtered.Count)
                firstSelected = filtered.Count - 1;
            if (firstSelected >= 0)
            {
                scenesView.ClearSelection();
                scenesView.Rows[firstSelected].Selected = true;
            }
        }

        private void filterText_TextChanged(object sender, System.EventArgs e)
        {
            SetData(false);
        }
    }
}
