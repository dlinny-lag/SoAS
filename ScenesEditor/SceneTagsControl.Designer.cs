
namespace ScenesEditor
{
    partial class SceneTagsControl
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
            this.tagsPanel = new System.Windows.Forms.Panel();
            this.authorsTags = new TagsEditorControl();
            this.attributeTags = new TagsEditorControl();
            this.sceneTags = new TagsEditorControl();
            this.serviceTags = new TagsEditorControl();
            this.importedGroupBox = new System.Windows.Forms.GroupBox();
            this.allTags = new TagsEditorControl();
            this.contactTags = new TagsEditorControl();
            this.unknownTags = new TagsEditorControl();
            this.furnitureTags = new TagsEditorControl();
            this.numericTags = new TagsEditorControl();
            this.actorTypesTags = new TagsEditorControl();
            this.otherTags = new TagsEditorControl();
            this.narrativeTags = new TagsEditorControl();
            this.feelingTags = new TagsEditorControl();
            this.tagsPanel.SuspendLayout();
            this.importedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagsPanel
            // 
            this.tagsPanel.AutoScroll = true;
            this.tagsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tagsPanel.Controls.Add(this.authorsTags);
            this.tagsPanel.Controls.Add(this.attributeTags);
            this.tagsPanel.Controls.Add(this.sceneTags);
            this.tagsPanel.Controls.Add(this.serviceTags);
            this.tagsPanel.Controls.Add(this.importedGroupBox);
            this.tagsPanel.Controls.Add(this.otherTags);
            this.tagsPanel.Controls.Add(this.narrativeTags);
            this.tagsPanel.Controls.Add(this.feelingTags);
            this.tagsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagsPanel.Location = new System.Drawing.Point(0, 0);
            this.tagsPanel.Name = "tagsPanel";
            this.tagsPanel.Padding = new System.Windows.Forms.Padding(3);
            this.tagsPanel.Size = new System.Drawing.Size(744, 478);
            this.tagsPanel.TabIndex = 12;
            // 
            // authorsTags
            // 
            this.authorsTags.Category = "Authors";
            this.authorsTags.Editable = false;
            this.authorsTags.Location = new System.Drawing.Point(11, 47);
            this.authorsTags.Name = "authorsTags";
            this.authorsTags.Size = new System.Drawing.Size(721, 28);
            this.authorsTags.TabIndex = 0;
            this.authorsTags.Tags = new string[0];
            // 
            // attributeTags
            // 
            this.attributeTags.Category = "Attribute";
            this.attributeTags.Editable = false;
            this.attributeTags.Location = new System.Drawing.Point(11, 149);
            this.attributeTags.Name = "attributeTags";
            this.attributeTags.Size = new System.Drawing.Size(721, 28);
            this.attributeTags.TabIndex = 5;
            this.attributeTags.Tags = new string[0];
            // 
            // sceneTags
            // 
            this.sceneTags.Category = "Tags";
            this.sceneTags.Editable = false;
            this.sceneTags.Location = new System.Drawing.Point(11, 13);
            this.sceneTags.Name = "sceneTags";
            this.sceneTags.Size = new System.Drawing.Size(721, 28);
            this.sceneTags.TabIndex = 12;
            this.sceneTags.Tags = new string[0];
            // 
            // serviceTags
            // 
            this.serviceTags.Category = "Service";
            this.serviceTags.Editable = false;
            this.serviceTags.Location = new System.Drawing.Point(11, 176);
            this.serviceTags.Name = "serviceTags";
            this.serviceTags.Size = new System.Drawing.Size(721, 28);
            this.serviceTags.TabIndex = 4;
            this.serviceTags.Tags = new string[0];
            // 
            // importedGroupBox
            // 
            this.importedGroupBox.Controls.Add(this.allTags);
            this.importedGroupBox.Controls.Add(this.contactTags);
            this.importedGroupBox.Controls.Add(this.unknownTags);
            this.importedGroupBox.Controls.Add(this.furnitureTags);
            this.importedGroupBox.Controls.Add(this.numericTags);
            this.importedGroupBox.Controls.Add(this.actorTypesTags);
            this.importedGroupBox.Location = new System.Drawing.Point(4, 244);
            this.importedGroupBox.Name = "importedGroupBox";
            this.importedGroupBox.Size = new System.Drawing.Size(734, 226);
            this.importedGroupBox.TabIndex = 12;
            this.importedGroupBox.TabStop = false;
            this.importedGroupBox.Text = "Imported Tags";
            // 
            // allTags
            // 
            this.allTags.Category = "All";
            this.allTags.Editable = false;
            this.allTags.Location = new System.Drawing.Point(7, 23);
            this.allTags.Name = "allTags";
            this.allTags.Size = new System.Drawing.Size(721, 28);
            this.allTags.TabIndex = 11;
            this.allTags.Tags = new string[0];
            // 
            // contactTags
            // 
            this.contactTags.Category = "Contact";
            this.contactTags.Editable = false;
            this.contactTags.Location = new System.Drawing.Point(7, 91);
            this.contactTags.Name = "contactTags";
            this.contactTags.Size = new System.Drawing.Size(721, 28);
            this.contactTags.TabIndex = 6;
            this.contactTags.Tags = new string[0];
            // 
            // unknownTags
            // 
            this.unknownTags.Category = "Unknown";
            this.unknownTags.Editable = false;
            this.unknownTags.Location = new System.Drawing.Point(7, 193);
            this.unknownTags.Name = "unknownTags";
            this.unknownTags.Size = new System.Drawing.Size(721, 28);
            this.unknownTags.TabIndex = 10;
            this.unknownTags.Tags = new string[0];
            // 
            // furnitureTags
            // 
            this.furnitureTags.Category = "Furniture";
            this.furnitureTags.Editable = false;
            this.furnitureTags.Location = new System.Drawing.Point(6, 57);
            this.furnitureTags.Name = "furnitureTags";
            this.furnitureTags.Size = new System.Drawing.Size(721, 28);
            this.furnitureTags.TabIndex = 1;
            this.furnitureTags.Tags = new string[0];
            // 
            // numericTags
            // 
            this.numericTags.Category = "Numeric";
            this.numericTags.Editable = false;
            this.numericTags.Location = new System.Drawing.Point(7, 125);
            this.numericTags.Name = "numericTags";
            this.numericTags.Size = new System.Drawing.Size(721, 28);
            this.numericTags.TabIndex = 7;
            this.numericTags.Tags = new string[0];
            // 
            // actorTypesTags
            // 
            this.actorTypesTags.Category = "Actor types";
            this.actorTypesTags.Editable = false;
            this.actorTypesTags.Location = new System.Drawing.Point(7, 159);
            this.actorTypesTags.Name = "actorTypesTags";
            this.actorTypesTags.Size = new System.Drawing.Size(721, 28);
            this.actorTypesTags.TabIndex = 8;
            this.actorTypesTags.Tags = new string[0];
            // 
            // otherTags
            // 
            this.otherTags.Category = "Other";
            this.otherTags.Editable = false;
            this.otherTags.Location = new System.Drawing.Point(11, 210);
            this.otherTags.Name = "otherTags";
            this.otherTags.Size = new System.Drawing.Size(721, 28);
            this.otherTags.TabIndex = 9;
            this.otherTags.Tags = new string[0];
            // 
            // narrativeTags
            // 
            this.narrativeTags.Category = "Narrative";
            this.narrativeTags.Editable = false;
            this.narrativeTags.Location = new System.Drawing.Point(11, 81);
            this.narrativeTags.Name = "narrativeTags";
            this.narrativeTags.Size = new System.Drawing.Size(721, 28);
            this.narrativeTags.TabIndex = 3;
            this.narrativeTags.Tags = new string[0];
            // 
            // feelingTags
            // 
            this.feelingTags.Category = "Feeling";
            this.feelingTags.Editable = false;
            this.feelingTags.Location = new System.Drawing.Point(11, 115);
            this.feelingTags.Name = "feelingTags";
            this.feelingTags.Size = new System.Drawing.Size(721, 28);
            this.feelingTags.TabIndex = 2;
            this.feelingTags.Tags = new string[0];
            // 
            // SceneTagsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tagsPanel);
            this.Name = "SceneTagsControl";
            this.Size = new System.Drawing.Size(744, 478);
            this.tagsPanel.ResumeLayout(false);
            this.importedGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel tagsPanel;
        private TagsEditorControl unknownTags;
        private TagsEditorControl furnitureTags;
        private TagsEditorControl otherTags;
        private TagsEditorControl actorTypesTags;
        private TagsEditorControl numericTags;
        private TagsEditorControl contactTags;
        private TagsEditorControl allTags;
        private TagsEditorControl sceneTags;
        private System.Windows.Forms.GroupBox importedGroupBox;
        private TagsEditorControl authorsTags;
        private TagsEditorControl attributeTags;
        private TagsEditorControl serviceTags;
        private TagsEditorControl narrativeTags;
        private TagsEditorControl feelingTags;
    }
}
