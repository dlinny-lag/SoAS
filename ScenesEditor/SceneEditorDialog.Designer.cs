
namespace ScenesEditor
{
    partial class SceneEditorDialog
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
            contactControls.Clear();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.contactsPanel = new System.Windows.Forms.Panel();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.readyForReleaseCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.contactsTab = new System.Windows.Forms.TabPage();
            this.tagsTab = new System.Windows.Forms.TabPage();
            this.sceneTagsControl = new SceneTagsControl();
            this.furnitureTab = new System.Windows.Forms.TabPage();
            this.furnitureTreeView = new System.Windows.Forms.TreeView();
            this.attributesTab = new System.Windows.Forms.TabPage();
            this.sceneAttributesControl = new SceneAttributesControl();
            this.importFromTagsBtn = new System.Windows.Forms.Button();
            this.categorizeTagsBtn = new System.Windows.Forms.Button();
            this.buttonsPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.contactsTab.SuspendLayout();
            this.tagsTab.SuspendLayout();
            this.furnitureTab.SuspendLayout();
            this.attributesTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // contactsPanel
            // 
            this.contactsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contactsPanel.AutoScroll = true;
            this.contactsPanel.BackColor = System.Drawing.Color.Transparent;
            this.contactsPanel.Location = new System.Drawing.Point(0, 0);
            this.contactsPanel.Name = "contactsPanel";
            this.contactsPanel.Size = new System.Drawing.Size(830, 400);
            this.contactsPanel.TabIndex = 0;
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.readyForReleaseCheckBox);
            this.buttonsPanel.Controls.Add(this.cancelBtn);
            this.buttonsPanel.Controls.Add(this.okBtn);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonsPanel.Location = new System.Drawing.Point(0, 455);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(838, 35);
            this.buttonsPanel.TabIndex = 1;
            // 
            // readyForReleaseCheckBox
            // 
            this.readyForReleaseCheckBox.AutoSize = true;
            this.readyForReleaseCheckBox.Location = new System.Drawing.Point(462, 11);
            this.readyForReleaseCheckBox.Name = "readyForReleaseCheckBox";
            this.readyForReleaseCheckBox.Size = new System.Drawing.Size(114, 17);
            this.readyForReleaseCheckBox.TabIndex = 2;
            this.readyForReleaseCheckBox.Text = "Ready for Release";
            this.readyForReleaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(751, 6);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(380, 6);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.contactsTab);
            this.tabControl.Controls.Add(this.tagsTab);
            this.tabControl.Controls.Add(this.furnitureTab);
            this.tabControl.Controls.Add(this.attributesTab);
            this.tabControl.Location = new System.Drawing.Point(0, 29);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(838, 426);
            this.tabControl.TabIndex = 3;
            // 
            // contactsTab
            // 
            this.contactsTab.Controls.Add(this.contactsPanel);
            this.contactsTab.Location = new System.Drawing.Point(4, 22);
            this.contactsTab.Name = "contactsTab";
            this.contactsTab.Padding = new System.Windows.Forms.Padding(3);
            this.contactsTab.Size = new System.Drawing.Size(830, 400);
            this.contactsTab.TabIndex = 0;
            this.contactsTab.Text = "Contacts";
            this.contactsTab.UseVisualStyleBackColor = true;
            // 
            // tagsTab
            // 
            this.tagsTab.Controls.Add(this.sceneTagsControl);
            this.tagsTab.Location = new System.Drawing.Point(4, 22);
            this.tagsTab.Name = "tagsTab";
            this.tagsTab.Padding = new System.Windows.Forms.Padding(3);
            this.tagsTab.Size = new System.Drawing.Size(830, 400);
            this.tagsTab.TabIndex = 1;
            this.tagsTab.Text = "Tags";
            this.tagsTab.UseVisualStyleBackColor = true;
            // 
            // sceneTagsControl
            // 
            this.sceneTagsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneTagsControl.Location = new System.Drawing.Point(3, 3);
            this.sceneTagsControl.Name = "sceneTagsControl";
            this.sceneTagsControl.Size = new System.Drawing.Size(824, 394);
            this.sceneTagsControl.TabIndex = 0;
            // 
            // furnitureTab
            // 
            this.furnitureTab.Controls.Add(this.furnitureTreeView);
            this.furnitureTab.Location = new System.Drawing.Point(4, 22);
            this.furnitureTab.Name = "furnitureTab";
            this.furnitureTab.Size = new System.Drawing.Size(830, 400);
            this.furnitureTab.TabIndex = 3;
            this.furnitureTab.Text = "Furniture";
            this.furnitureTab.UseVisualStyleBackColor = true;
            // 
            // furnitureTreeView
            // 
            this.furnitureTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.furnitureTreeView.Location = new System.Drawing.Point(0, 0);
            this.furnitureTreeView.Name = "furnitureTreeView";
            this.furnitureTreeView.Size = new System.Drawing.Size(830, 400);
            this.furnitureTreeView.TabIndex = 0;
            // 
            // attributesTab
            // 
            this.attributesTab.Controls.Add(this.sceneAttributesControl);
            this.attributesTab.Location = new System.Drawing.Point(4, 22);
            this.attributesTab.Name = "attributesTab";
            this.attributesTab.Size = new System.Drawing.Size(830, 400);
            this.attributesTab.TabIndex = 2;
            this.attributesTab.Text = "Scene attributes";
            this.attributesTab.UseVisualStyleBackColor = true;
            // 
            // sceneAttributesControl
            // 
            this.sceneAttributesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneAttributesControl.Location = new System.Drawing.Point(0, 0);
            this.sceneAttributesControl.Name = "sceneAttributesControl";
            this.sceneAttributesControl.Size = new System.Drawing.Size(830, 400);
            this.sceneAttributesControl.TabIndex = 0;
            // 
            // importFromTagsBtn
            // 
            this.importFromTagsBtn.Location = new System.Drawing.Point(4, 3);
            this.importFromTagsBtn.Name = "importFromTagsBtn";
            this.importFromTagsBtn.Size = new System.Drawing.Size(97, 23);
            this.importFromTagsBtn.TabIndex = 1;
            this.importFromTagsBtn.Text = "Import from tags";
            this.importFromTagsBtn.UseVisualStyleBackColor = true;
            this.importFromTagsBtn.Click += new System.EventHandler(this.importFromTagsBtn_Click);
            // 
            // categorizeTagsBtn
            // 
            this.categorizeTagsBtn.Location = new System.Drawing.Point(341, 3);
            this.categorizeTagsBtn.Name = "categorizeTagsBtn";
            this.categorizeTagsBtn.Size = new System.Drawing.Size(114, 23);
            this.categorizeTagsBtn.TabIndex = 4;
            this.categorizeTagsBtn.Text = "Categorize tags";
            this.categorizeTagsBtn.UseVisualStyleBackColor = true;
            this.categorizeTagsBtn.Click += new System.EventHandler(this.categorizeTagsBtn_Click);
            // 
            // SceneEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 490);
            this.Controls.Add(this.categorizeTagsBtn);
            this.Controls.Add(this.importFromTagsBtn);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonsPanel);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.Name = "SceneEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scene";
            this.buttonsPanel.ResumeLayout(false);
            this.buttonsPanel.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.contactsTab.ResumeLayout(false);
            this.tagsTab.ResumeLayout(false);
            this.furnitureTab.ResumeLayout(false);
            this.attributesTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel contactsPanel;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage contactsTab;
        private System.Windows.Forms.TabPage tagsTab;
        private System.Windows.Forms.TabPage attributesTab;
        private SceneTagsControl sceneTagsControl;
        private System.Windows.Forms.Button importFromTagsBtn;
        private SceneAttributesControl sceneAttributesControl;
        private System.Windows.Forms.CheckBox readyForReleaseCheckBox;
        private System.Windows.Forms.TabPage furnitureTab;
        private System.Windows.Forms.TreeView furnitureTreeView;
        private System.Windows.Forms.Button categorizeTagsBtn;
    }
}