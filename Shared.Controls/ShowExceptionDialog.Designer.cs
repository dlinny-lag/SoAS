
namespace Shared.Controls
{
    partial class ShowExceptionDialog
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
            this.exceptionTextBox = new System.Windows.Forms.RichTextBox();
            this.yesBtn = new System.Windows.Forms.Button();
            this.noBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.yesNoteLbl = new System.Windows.Forms.Label();
            this.noNoteLbl = new System.Windows.Forms.Label();
            this.closeNoteLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.Location = new System.Drawing.Point(2, 3);
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.ReadOnly = true;
            this.exceptionTextBox.Size = new System.Drawing.Size(449, 162);
            this.exceptionTextBox.TabIndex = 0;
            this.exceptionTextBox.Text = "";
            // 
            // yesBtn
            // 
            this.yesBtn.Location = new System.Drawing.Point(135, 262);
            this.yesBtn.Name = "yesBtn";
            this.yesBtn.Size = new System.Drawing.Size(75, 23);
            this.yesBtn.TabIndex = 1;
            this.yesBtn.Text = "Yes";
            this.yesBtn.UseVisualStyleBackColor = true;
            this.yesBtn.Click += new System.EventHandler(this.yesBtn_Click);
            // 
            // noBtn
            // 
            this.noBtn.Location = new System.Drawing.Point(249, 262);
            this.noBtn.Name = "noBtn";
            this.noBtn.Size = new System.Drawing.Size(75, 23);
            this.noBtn.TabIndex = 2;
            this.noBtn.Text = "No";
            this.noBtn.UseVisualStyleBackColor = true;
            this.noBtn.Click += new System.EventHandler(this.noBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(376, 262);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 3;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // yesNoteLbl
            // 
            this.yesNoteLbl.AutoSize = true;
            this.yesNoteLbl.Location = new System.Drawing.Point(13, 168);
            this.yesNoteLbl.Name = "yesNoteLbl";
            this.yesNoteLbl.Size = new System.Drawing.Size(25, 13);
            this.yesNoteLbl.TabIndex = 4;
            this.yesNoteLbl.Text = "Yes";
            // 
            // noNoteLbl
            // 
            this.noNoteLbl.AutoSize = true;
            this.noNoteLbl.Location = new System.Drawing.Point(13, 190);
            this.noNoteLbl.Name = "noNoteLbl";
            this.noNoteLbl.Size = new System.Drawing.Size(21, 13);
            this.noNoteLbl.TabIndex = 5;
            this.noNoteLbl.Text = "No";
            // 
            // closeNoteLbl
            // 
            this.closeNoteLbl.AutoSize = true;
            this.closeNoteLbl.Location = new System.Drawing.Point(13, 213);
            this.closeNoteLbl.Name = "closeNoteLbl";
            this.closeNoteLbl.Size = new System.Drawing.Size(143, 13);
            this.closeNoteLbl.TabIndex = 6;
            this.closeNoteLbl.Text = "Close - Terminate application";
            // 
            // ShowExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 297);
            this.Controls.Add(this.closeNoteLbl);
            this.Controls.Add(this.noNoteLbl);
            this.Controls.Add(this.yesNoteLbl);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.noBtn);
            this.Controls.Add(this.yesBtn);
            this.Controls.Add(this.exceptionTextBox);
            this.Name = "ShowExceptionDialog";
            this.Text = "Exception";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox exceptionTextBox;
        private System.Windows.Forms.Button yesBtn;
        private System.Windows.Forms.Button noBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label yesNoteLbl;
        private System.Windows.Forms.Label noNoteLbl;
        private System.Windows.Forms.Label closeNoteLbl;
    }
}