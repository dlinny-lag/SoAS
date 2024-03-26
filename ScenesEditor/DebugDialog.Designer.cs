
namespace ScenesEditor
{
    partial class DebugDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.totalScenesLbl = new System.Windows.Forms.Label();
            this.unsupportedSkeletonsList = new System.Windows.Forms.DataGridView();
            this.closeBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.unsupportedSkeletonsList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Unsupported Skeletons";
            // 
            // totalScenesLbl
            // 
            this.totalScenesLbl.AutoSize = true;
            this.totalScenesLbl.Location = new System.Drawing.Point(13, 13);
            this.totalScenesLbl.Name = "totalScenesLbl";
            this.totalScenesLbl.Size = new System.Drawing.Size(68, 13);
            this.totalScenesLbl.TabIndex = 1;
            this.totalScenesLbl.Text = "Total scenes";
            // 
            // unsupportedSkeletonsList
            // 
            this.unsupportedSkeletonsList.AllowUserToAddRows = false;
            this.unsupportedSkeletonsList.AllowUserToDeleteRows = false;
            this.unsupportedSkeletonsList.AllowUserToOrderColumns = true;
            this.unsupportedSkeletonsList.AllowUserToResizeColumns = false;
            this.unsupportedSkeletonsList.AllowUserToResizeRows = false;
            this.unsupportedSkeletonsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unsupportedSkeletonsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unsupportedSkeletonsList.Location = new System.Drawing.Point(16, 59);
            this.unsupportedSkeletonsList.Name = "unsupportedSkeletonsList";
            this.unsupportedSkeletonsList.ReadOnly = true;
            this.unsupportedSkeletonsList.RowHeadersVisible = false;
            this.unsupportedSkeletonsList.ShowCellErrors = false;
            this.unsupportedSkeletonsList.ShowCellToolTips = false;
            this.unsupportedSkeletonsList.ShowEditingIcon = false;
            this.unsupportedSkeletonsList.ShowRowErrors = false;
            this.unsupportedSkeletonsList.Size = new System.Drawing.Size(451, 150);
            this.unsupportedSkeletonsList.TabIndex = 2;
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.closeBtn.Location = new System.Drawing.Point(194, 238);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 3;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // DebugDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 279);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.unsupportedSkeletonsList);
            this.Controls.Add(this.totalScenesLbl);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebugDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Debug";
            ((System.ComponentModel.ISupportInitialize)(this.unsupportedSkeletonsList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label totalScenesLbl;
        private System.Windows.Forms.DataGridView unsupportedSkeletonsList;
        private System.Windows.Forms.Button closeBtn;
    }
}