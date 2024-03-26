
namespace ScenesEditor
{
    partial class ImportScenesDialog
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
            this.importingPanel = new System.Windows.Forms.GroupBox();
            this.importingList = new ScenesEditorControl();
            this.projectPanel = new System.Windows.Forms.GroupBox();
            this.projectsList = new ScenesEditorControl();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.importingFolder = new System.Windows.Forms.ComboBox();
            this.customFolderBtn = new System.Windows.Forms.Button();
            this.scanBtn = new System.Windows.Forms.Button();
            this.importingPanel.SuspendLayout();
            this.projectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // importingPanel
            // 
            this.importingPanel.Controls.Add(this.importingList);
            this.importingPanel.Location = new System.Drawing.Point(13, 37);
            this.importingPanel.Name = "importingPanel";
            this.importingPanel.Size = new System.Drawing.Size(490, 471);
            this.importingPanel.TabIndex = 0;
            this.importingPanel.TabStop = false;
            this.importingPanel.Text = "Importing";
            // 
            // importingList
            // 
            this.importingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.importingList.Location = new System.Drawing.Point(3, 16);
            this.importingList.MultiSelect = true;
            this.importingList.Name = "importingList";
            this.importingList.Size = new System.Drawing.Size(484, 452);
            this.importingList.TabIndex = 0;
            // 
            // projectPanel
            // 
            this.projectPanel.Controls.Add(this.projectsList);
            this.projectPanel.Location = new System.Drawing.Point(577, 37);
            this.projectPanel.Name = "projectPanel";
            this.projectPanel.Size = new System.Drawing.Size(492, 471);
            this.projectPanel.TabIndex = 1;
            this.projectPanel.TabStop = false;
            this.projectPanel.Text = "Project";
            // 
            // projectsList
            // 
            this.projectsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectsList.Location = new System.Drawing.Point(3, 16);
            this.projectsList.MultiSelect = true;
            this.projectsList.Name = "projectsList";
            this.projectsList.Size = new System.Drawing.Size(486, 452);
            this.projectsList.TabIndex = 0;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(499, 514);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(994, 514);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(506, 129);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(68, 23);
            this.addBtn.TabIndex = 4;
            this.addBtn.Text = ">>>";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(506, 167);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(68, 23);
            this.removeBtn.TabIndex = 5;
            this.removeBtn.Text = "<<<";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // importingFolder
            // 
            this.importingFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importingFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.importingFolder.FormattingEnabled = true;
            this.importingFolder.Location = new System.Drawing.Point(101, 10);
            this.importingFolder.Name = "importingFolder";
            this.importingFolder.Size = new System.Drawing.Size(923, 21);
            this.importingFolder.TabIndex = 7;
            this.importingFolder.SelectedIndexChanged += new System.EventHandler(this.importingFolder_SelectedIndexChanged);
            // 
            // customFolderBtn
            // 
            this.customFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.customFolderBtn.Location = new System.Drawing.Point(1030, 8);
            this.customFolderBtn.Name = "customFolderBtn";
            this.customFolderBtn.Size = new System.Drawing.Size(36, 23);
            this.customFolderBtn.TabIndex = 8;
            this.customFolderBtn.Text = "...";
            this.customFolderBtn.UseVisualStyleBackColor = true;
            this.customFolderBtn.Click += new System.EventHandler(this.customFolderBtn_Click);
            // 
            // scanBtn
            // 
            this.scanBtn.Location = new System.Drawing.Point(16, 10);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(75, 23);
            this.scanBtn.TabIndex = 9;
            this.scanBtn.Text = "Scan";
            this.scanBtn.UseVisualStyleBackColor = true;
            this.scanBtn.Click += new System.EventHandler(this.scanBtn_Click);
            // 
            // ImportScenesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 546);
            this.Controls.Add(this.scanBtn);
            this.Controls.Add(this.customFolderBtn);
            this.Controls.Add(this.importingFolder);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.projectPanel);
            this.Controls.Add(this.importingPanel);
            this.Name = "ImportScenesDialog";
            this.Text = "Import Scenes";
            this.importingPanel.ResumeLayout(false);
            this.projectPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox importingPanel;
        private System.Windows.Forms.GroupBox projectPanel;
        private ScenesEditorControl importingList;
        private ScenesEditorControl projectsList;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.ComboBox importingFolder;
        private System.Windows.Forms.Button customFolderBtn;
        private System.Windows.Forms.Button scanBtn;
    }
}