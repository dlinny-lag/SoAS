
namespace ScenesEditor
{
    partial class FurnitureManagerForm
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
            this.scanBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.fallout4PathText = new System.Windows.Forms.TextBox();
            this.selectF4FolderBtn = new System.Windows.Forms.Button();
            this.furnitureTreeView = new System.Windows.Forms.TreeView();
            this.releaseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scanBtn
            // 
            this.scanBtn.Location = new System.Drawing.Point(12, 41);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(75, 23);
            this.scanBtn.TabIndex = 0;
            this.scanBtn.Text = "Import";
            this.scanBtn.UseVisualStyleBackColor = true;
            this.scanBtn.Click += new System.EventHandler(this.scanBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fallout 4 folder";
            // 
            // fallout4PathText
            // 
            this.fallout4PathText.Location = new System.Drawing.Point(94, 10);
            this.fallout4PathText.Name = "fallout4PathText";
            this.fallout4PathText.Size = new System.Drawing.Size(634, 20);
            this.fallout4PathText.TabIndex = 2;
            // 
            // selectF4FolderBtn
            // 
            this.selectF4FolderBtn.Location = new System.Drawing.Point(735, 10);
            this.selectF4FolderBtn.Name = "selectF4FolderBtn";
            this.selectF4FolderBtn.Size = new System.Drawing.Size(28, 20);
            this.selectF4FolderBtn.TabIndex = 3;
            this.selectF4FolderBtn.Text = "...";
            this.selectF4FolderBtn.UseVisualStyleBackColor = true;
            this.selectF4FolderBtn.Click += new System.EventHandler(this.selectF4FolderBtn_Click);
            // 
            // furnitureTreeView
            // 
            this.furnitureTreeView.Location = new System.Drawing.Point(12, 70);
            this.furnitureTreeView.Name = "furnitureTreeView";
            this.furnitureTreeView.Size = new System.Drawing.Size(751, 368);
            this.furnitureTreeView.TabIndex = 4;
            // 
            // releaseBtn
            // 
            this.releaseBtn.Location = new System.Drawing.Point(688, 41);
            this.releaseBtn.Name = "releaseBtn";
            this.releaseBtn.Size = new System.Drawing.Size(75, 23);
            this.releaseBtn.TabIndex = 5;
            this.releaseBtn.Text = "Release";
            this.releaseBtn.UseVisualStyleBackColor = true;
            this.releaseBtn.Click += new System.EventHandler(this.releaseBtn_Click);
            // 
            // FurnitureManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 450);
            this.Controls.Add(this.releaseBtn);
            this.Controls.Add(this.furnitureTreeView);
            this.Controls.Add(this.selectF4FolderBtn);
            this.Controls.Add(this.fallout4PathText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scanBtn);
            this.Name = "FurnitureManagerForm";
            this.Text = "Furniture";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button scanBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fallout4PathText;
        private System.Windows.Forms.Button selectF4FolderBtn;
        private System.Windows.Forms.TreeView furnitureTreeView;
        private System.Windows.Forms.Button releaseBtn;
    }
}