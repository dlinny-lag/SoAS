
namespace ScenesEditor
{
    partial class DeleteScenesDialog
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
            this.scenesList = new ScenesEditorControl();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.selectedCountLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // scenesList
            // 
            this.scenesList.CustomFilter = null;
            this.scenesList.Location = new System.Drawing.Point(13, 37);
            this.scenesList.MultiSelect = true;
            this.scenesList.Name = "scenesList";
            this.scenesList.Size = new System.Drawing.Size(775, 370);
            this.scenesList.TabIndex = 0;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(283, 414);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(455, 415);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // selectedCountLbl
            // 
            this.selectedCountLbl.AutoSize = true;
            this.selectedCountLbl.Location = new System.Drawing.Point(13, 13);
            this.selectedCountLbl.Name = "selectedCountLbl";
            this.selectedCountLbl.Size = new System.Drawing.Size(87, 13);
            this.selectedCountLbl.TabIndex = 3;
            this.selectedCountLbl.Text = "Nothing selected";
            // 
            // DeleteScenesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.selectedCountLbl);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.scenesList);
            this.Name = "DeleteScenesDialog";
            this.Text = "Delete Scenes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScenesEditorControl scenesList;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label selectedCountLbl;
    }
}