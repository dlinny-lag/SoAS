
namespace ScenesEditor
{
    partial class ExternalLicenseControl
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
            this.moduleNameLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // moduleNameLink
            // 
            this.moduleNameLink.AutoSize = true;
            this.moduleNameLink.Location = new System.Drawing.Point(3, 0);
            this.moduleNameLink.Name = "moduleNameLink";
            this.moduleNameLink.Size = new System.Drawing.Size(89, 13);
            this.moduleNameLink.TabIndex = 0;
            this.moduleNameLink.TabStop = true;
            this.moduleNameLink.Text = "ComponentName";
            this.moduleNameLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.openTextLabel_LinkClicked);
            // 
            // ExternalLicenseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.moduleNameLink);
            this.Name = "ExternalLicenseControl";
            this.Size = new System.Drawing.Size(93, 16);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel moduleNameLink;
    }
}
