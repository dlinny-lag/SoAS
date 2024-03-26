
namespace ScenesEditor
{
    partial class ScenesEditorControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scenesView = new System.Windows.Forms.DataGridView();
            this.filterText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.scenesView)).BeginInit();
            this.SuspendLayout();
            // 
            // scenesView
            // 
            this.scenesView.AllowUserToAddRows = false;
            this.scenesView.AllowUserToDeleteRows = false;
            this.scenesView.AllowUserToResizeColumns = false;
            this.scenesView.AllowUserToResizeRows = false;
            this.scenesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenesView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scenesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scenesView.ColumnHeadersVisible = false;
            this.scenesView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.scenesView.EnableHeadersVisualStyles = false;
            this.scenesView.GridColor = System.Drawing.SystemColors.Control;
            this.scenesView.Location = new System.Drawing.Point(3, 30);
            this.scenesView.MultiSelect = false;
            this.scenesView.Name = "scenesView";
            this.scenesView.RowHeadersVisible = false;
            this.scenesView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.scenesView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.scenesView.ShowCellErrors = false;
            this.scenesView.ShowEditingIcon = false;
            this.scenesView.ShowRowErrors = false;
            this.scenesView.Size = new System.Drawing.Size(612, 361);
            this.scenesView.TabIndex = 0;
            this.scenesView.VirtualMode = true;
            this.scenesView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.scenesView_CellDoubleClick);
            this.scenesView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.scenesView_CellValueNeeded);
            // 
            // filterText
            // 
            this.filterText.Location = new System.Drawing.Point(4, 4);
            this.filterText.Name = "filterText";
            this.filterText.Size = new System.Drawing.Size(185, 20);
            this.filterText.TabIndex = 1;
            this.filterText.TextChanged += new System.EventHandler(this.filterText_TextChanged);
            // 
            // ScenesEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filterText);
            this.Controls.Add(this.scenesView);
            this.Name = "ScenesEditorControl";
            this.Size = new System.Drawing.Size(618, 394);
            ((System.ComponentModel.ISupportInitialize)(this.scenesView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView scenesView;
        private System.Windows.Forms.TextBox filterText;
    }
}
