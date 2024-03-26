using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ScenesEditor.Data;

namespace ScenesEditor
{
    public sealed partial class ProjectsListControl : UserControl
    {
        private class ProjectView
        {
            internal readonly ProjectHeader Data;
            public ProjectView(ProjectHeader data)
            {
                Data = data;
            }

            public string Name => Data.Name;
            public string Path => Data.Path;
            public string Created => Data.Created.ToString("g");
            public string Updated => Data.Updated.ToString("g");
        }

        public ProjectsListControl()
        {
            InitializeComponent();
            projectsGrid.SelectionChanged += ProjectsGridOnSelectionChanged;
        }

        private bool parentSubscribed = false;
        private bool parentShown = false;
        private void OnParentFormShownFirstTime(object sender, EventArgs e)
        {
            ((Form)sender).Shown -= OnParentFormShownFirstTime;
            parentShown = true;
            ResetSelection();
        }

        public event Action<ProjectHeader> Selected;

        public void ResetSelection()
        {
            projectsGrid.CurrentCell = null;
            projectsGrid.ClearSelection();
        }
        private void ProjectsGridOnSelectionChanged(object sender, EventArgs e)
        {
            if (!parentSubscribed)
            { // it is called when parent form handle created as well
                Form parent = FindForm();
                if (parent == null)
                    return; 
                parent.Shown += OnParentFormShownFirstTime;
                parentSubscribed = true;
            }

            if (!parentShown)
                return; // do not process anything until control is visible

            var handler = Selected;
            if (handler == null)
                return;

            if (projectsGrid.SelectedRows.Count == 0)
                return;

            var view = projectsGrid.SelectedRows[0].DataBoundItem as ProjectView;
            if (view == null)
                throw new InvalidOperationException("No project defined for a row");

            handler(view.Data);
        }

        public void SetData(IEnumerable<ProjectHeader> data)
        {
            var sorted = new List<ProjectView>(data.Select(p => new ProjectView(p)));
            sorted.Sort((a,b) => b.Data.Updated.CompareTo(a.Data.Updated)); // reverse order

            projectsGrid.SelectionChanged -= ProjectsGridOnSelectionChanged;
            try
            {
                projectsGrid.DataSource = sorted;

                DataGridViewTextBoxColumn Column(string caption)
                {
                    return new DataGridViewTextBoxColumn
                    {
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                        DataPropertyName = caption,
                        HeaderText = caption,
                    };
                }

                projectsGrid.Columns.Clear();
                projectsGrid.Columns.AddRange(
                    Column(nameof(ProjectView.Name)),
                    Column(nameof(ProjectView.Path)),
                    Column(nameof(ProjectView.Updated)),
                    Column(nameof(ProjectView.Created))
                );
                ResetSelection();
            }
            finally
            {
                projectsGrid.SelectionChanged += ProjectsGridOnSelectionChanged;
            }
        }
    }
}
