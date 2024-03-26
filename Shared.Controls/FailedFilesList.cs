using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    public abstract class FailedFilesList<T> : UserControl
        where T: class
    {
        private DataGridView dataGrid;
        private void InitializeComponent()
        {
            dataGrid = new DataGridView();
            ((ISupportInitialize)(dataGrid)).BeginInit();
            SuspendLayout();
            // 
            // dataGrid
            // 
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.Location = new Point(0, 0);
            dataGrid.Name = "dataGrid";
            dataGrid.RowHeadersVisible = false;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.ScrollBars = ScrollBars.Vertical;
            dataGrid.Size = new Size(626, 367);
            dataGrid.TabIndex = 0;
            // 
            // IntegrityErrorsList
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dataGrid);
            Name = "FailedFilesList";
            Size = new Size(626, 367);
            ((ISupportInitialize)(dataGrid)).EndInit();
            ResumeLayout(false);
        }

        protected FailedFilesList()
        {
            InitializeComponent();
            dataGrid.VirtualMode = true;
            dataGrid.MultiSelect = true;
            dataGrid.ReadOnly = false;

            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToOrderColumns = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = true;


            dataGrid.UpdateStyle(ControlStyles.OptimizedDoubleBuffer, true);
            dataGrid.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                ValueType = typeof(string),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            });
            dataGrid.BackgroundColor = dataGrid.DefaultCellStyle.BackColor;
            dataGrid.CellValueNeeded += DataGridOnCellValueNeeded;
            dataGrid.CellEndEdit += DataGridOnCellEndEdit;
            dataGrid.CancelRowEdit += DataGridOnCancelRowEdit;
        }

        private void DataGridOnCancelRowEdit(object sender, QuestionEventArgs e)
        {
            EditFinished?.Invoke();
        }

        private void DataGridOnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            EditFinished?.Invoke();
        }

        public event Action EditFinished;

        private void DataGridOnCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var err = data[e.RowIndex];
            e.Value = $"{err.File} \n {err.Error}"; // same as in FileExport.ExportText. TODO: avoid duplication
        }

        protected sealed class FailInfo
        {
            public string File;
            public string Error;
        }
        private IList<FailInfo> data;

        protected abstract IList<FailInfo> GetData(IDictionary<string, T> errors);

        public void SetData(IDictionary<string, T> errors)
        {
            data = GetData(errors);
            
            dataGrid.RowCount = data.Count;
            Invalidate();
        }
    }
}