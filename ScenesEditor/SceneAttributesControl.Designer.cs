
using JsonTreeView.Controls;

namespace ScenesEditor
{
    partial class SceneAttributesControl
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
            this.customAttributesJsonEditor = new JsonTreeView.Controls.JTokenTreeUserControl();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // customAttributesJsonEditor
            // 
            this.customAttributesJsonEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customAttributesJsonEditor.CollapsedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.customAttributesJsonEditor.ExpandedFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            this.customAttributesJsonEditor.Location = new System.Drawing.Point(4, 15);
            this.customAttributesJsonEditor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.customAttributesJsonEditor.Name = "customAttributesJsonEditor";
            this.customAttributesJsonEditor.Size = new System.Drawing.Size(512, 223);
            this.customAttributesJsonEditor.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Custom attributes:";
            // 
            // SceneAttributesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customAttributesJsonEditor);
            this.Name = "SceneAttributesControl";
            this.Size = new System.Drawing.Size(518, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private JTokenTreeUserControl customAttributesJsonEditor;
        private System.Windows.Forms.Label label1;
    }
}
