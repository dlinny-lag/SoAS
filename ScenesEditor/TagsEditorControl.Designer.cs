
namespace ScenesEditor
{
    partial class TagsEditorControl
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
            this.tagLabel = new System.Windows.Forms.Label();
            this.tagValuesBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tagLabel
            // 
            this.tagLabel.AutoSize = true;
            this.tagLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tagLabel.Location = new System.Drawing.Point(3, 3);
            this.tagLabel.Name = "tagLabel";
            this.tagLabel.Size = new System.Drawing.Size(40, 13);
            this.tagLabel.TabIndex = 0;
            this.tagLabel.Text = "[TAG]";
            this.tagLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tagValuesBox
            // 
            this.tagValuesBox.BackColor = System.Drawing.SystemColors.Window;
            this.tagValuesBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tagValuesBox.DetectUrls = false;
            this.tagValuesBox.Location = new System.Drawing.Point(44, 3);
            this.tagValuesBox.Margin = new System.Windows.Forms.Padding(0);
            this.tagValuesBox.Name = "tagValuesBox";
            this.tagValuesBox.ReadOnly = true;
            this.tagValuesBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.tagValuesBox.ShortcutsEnabled = false;
            this.tagValuesBox.Size = new System.Drawing.Size(253, 16);
            this.tagValuesBox.TabIndex = 1;
            this.tagValuesBox.Text = "TAGS";
            this.tagValuesBox.DoubleClick += new System.EventHandler(this.tagValuesBox_DoubleClick);
            // 
            // TagsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tagValuesBox);
            this.Controls.Add(this.tagLabel);
            this.Name = "TagsEditorControl";
            this.Size = new System.Drawing.Size(301, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tagLabel;
        private System.Windows.Forms.RichTextBox tagValuesBox;
    }
}
