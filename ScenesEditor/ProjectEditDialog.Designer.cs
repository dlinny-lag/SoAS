
namespace ScenesEditor
{
    partial class ProjectEditDialog
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.authorTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.versionMajorTextBox = new System.Windows.Forms.TextBox();
            this.versionMinorTextBox = new System.Windows.Forms.TextBox();
            this.versionBuildTextBox = new System.Windows.Forms.TextBox();
            this.espNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(55, 13);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(146, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(15, 155);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(126, 155);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // authorTextBox
            // 
            this.authorTextBox.Location = new System.Drawing.Point(55, 39);
            this.authorTextBox.Name = "authorTextBox";
            this.authorTextBox.Size = new System.Drawing.Size(146, 20);
            this.authorTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Author";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Version";
            // 
            // versionMajorTextBox
            // 
            this.versionMajorTextBox.Location = new System.Drawing.Point(55, 71);
            this.versionMajorTextBox.Name = "versionMajorTextBox";
            this.versionMajorTextBox.Size = new System.Drawing.Size(45, 20);
            this.versionMajorTextBox.TabIndex = 7;
            // 
            // versionMinorTextBox
            // 
            this.versionMinorTextBox.Location = new System.Drawing.Point(105, 71);
            this.versionMinorTextBox.Name = "versionMinorTextBox";
            this.versionMinorTextBox.Size = new System.Drawing.Size(45, 20);
            this.versionMinorTextBox.TabIndex = 8;
            // 
            // versionBuildTextBox
            // 
            this.versionBuildTextBox.Location = new System.Drawing.Point(156, 71);
            this.versionBuildTextBox.Name = "versionBuildTextBox";
            this.versionBuildTextBox.Size = new System.Drawing.Size(45, 20);
            this.versionBuildTextBox.TabIndex = 9;
            // 
            // espNameTextBox
            // 
            this.espNameTextBox.Location = new System.Drawing.Point(75, 102);
            this.espNameTextBox.Name = "espNameTextBox";
            this.espNameTextBox.Size = new System.Drawing.Size(126, 20);
            this.espNameTextBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "ESP name";
            // 
            // ProjectEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 196);
            this.Controls.Add(this.espNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.versionBuildTextBox);
            this.Controls.Add(this.versionMinorTextBox);
            this.Controls.Add(this.versionMajorTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.authorTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectEditDialog";
            this.ShowIcon = false;
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.TextBox authorTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox versionMajorTextBox;
        private System.Windows.Forms.TextBox versionMinorTextBox;
        private System.Windows.Forms.TextBox versionBuildTextBox;
        private System.Windows.Forms.TextBox espNameTextBox;
        private System.Windows.Forms.Label label4;
    }
}