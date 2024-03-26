using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AAF.Services.AAFImport;
using AAF.Services.Errors;

namespace Shared.Controls
{
    public abstract class IntegrityErrorsList<TE> : UserControl, IMessageFilter
        where TE : IntegrityError
    {
        class MyDataGridView : DataGridView
        {
            public bool LastCellSelectedByMouse;
            private bool mouseDowned;
            protected override void OnMouseDown(MouseEventArgs e)
            {
                mouseDowned = true;
                base.OnMouseDown(e);
                mouseDowned = false;
            }

            protected override void OnCurrentCellChanged(EventArgs e)
            {
                LastCellSelectedByMouse = mouseDowned;
                base.OnCurrentCellChanged(e);
            }

            protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
            {
                base.OnCellMouseClick(e);
                LastCellSelectedByMouse = false;
            }
        }

        private void InitializeComponent()
        {
            dataGrid = new MyDataGridView();
            ((ISupportInitialize)(dataGrid)).BeginInit();
            SuspendLayout();
            // 
            // dataGrid
            // 
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
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
            Name = "IntegrityErrorsList";
            Size = new Size(626, 367);
            ((ISupportInitialize)(dataGrid)).EndInit();
            ResumeLayout(false);
        }

        private MyDataGridView dataGrid;
        public event Action EditFinished;

        protected IntegrityErrorsList()
        {
            InitializeComponent();

            dataGrid.VirtualMode = true;
            dataGrid.MultiSelect = true;
            dataGrid.ReadOnly = false;
            dataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;

            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToOrderColumns = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = true;

            dataGrid.CancelRowEdit += DataGridOnCancelRowEdit;
            dataGrid.CellEndEdit += DataGridCellEndEdit;

            dataGrid.UpdateStyle(ControlStyles.OptimizedDoubleBuffer, true);
            dataGrid.UpdateStyle(ControlStyles.ResizeRedraw, true);
            dataGrid.Columns.Add(new DataGridViewColumn(new DataGridViewReportCell{Padding = new Padding(5)})
            {
                ValueType = typeof(string),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            });
            dataGrid.BackgroundColor = dataGrid.DefaultCellStyle.BackColor;
            dataGrid.CellValueNeeded += DataGridOnCellValueNeeded;

            dataGrid.CellMouseClick += DataGridOnCellMouseClick;
            dataGrid.CurrentCellChanged += DataGridOnCurrentCellChanged;
            dataGrid.MouseEnter += DataGridOnMouseEnter;
            dataGrid.MouseLeave += DataGridOnMouseLeave;
            Application.AddMessageFilter(this); // TODO: may cause problems when handle is not created yet
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Application.RemoveMessageFilter(this);
        }

        private void DataGridOnCurrentCellChanged(object sender, EventArgs e)
        {
            lastCellClicked = new Point(dataGrid.CurrentCell?.ColumnIndex ?? -1, dataGrid.CurrentCell?.RowIndex ?? -1);
            Console.WriteLine($"Current row = {dataGrid.CurrentCell?.RowIndex ?? -1}");
        }


        private Point lastCellClicked = new Point(-1, -1);

        private void DataGridOnCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!dataGrid.LastCellSelectedByMouse && lastCellClicked.X == e.ColumnIndex &&
                lastCellClicked.Y == e.RowIndex && 
                dataGrid.CurrentCell?.ColumnIndex == e.ColumnIndex &&
                dataGrid.CurrentCell?.RowIndex == e.RowIndex)
            {
                dataGrid.BeginEdit(false);
            }
            else
            {
                if (dataGrid.IsCurrentCellInEditMode)
                    dataGrid.EndEdit();
            }
            lastCellClicked = new Point(e.ColumnIndex, e.RowIndex);
            Console.WriteLine($"Clicked {lastCellClicked.Y}");
        }
        

        private void DataGridCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            EditFinished?.Invoke();
        }

        private void DataGridOnCancelRowEdit(object sender, QuestionEventArgs e)
        {
            EditFinished?.Invoke();
        }

        private void DataGridOnCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var err = data[e.RowIndex];
            e.Value = new CachedReport(err.Report(), dataGrid.Columns[0].DefaultCellStyle);
        }

        private IList<TE> data;
        public IList<TE> Data
        {
            get => data;
            set
            {
                data = value;
                dataGrid.RowCount = data.Count;
                lastCellClicked = new Point(-1, -1);
                Invalidate();
            }
        }

        private bool gridHover = false;
        private void DataGridOnMouseLeave(object sender, EventArgs e)
        {
            gridHover = false;
        }

        private void DataGridOnMouseEnter(object sender, EventArgs e)
        {
            gridHover = true;
        }
        public bool PreFilterMessage(ref Message m)
        {
            if (gridHover && m.Msg == WindowExtension.WM_MOUSEWHEEL && !dataGrid.Focused)
            {
                dataGrid.SendMessage(m.Msg, m.WParam, m.LParam);
                return true;
            }

            return false;
        }
    }
}
