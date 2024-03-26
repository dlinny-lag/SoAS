
namespace ScenesEditor
{
    partial class ParticipantContactsControl
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
            Changed = null;
            ParticipantAttributeChanged = null;
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.participantTitle = new System.Windows.Forms.Label();
            this.toGroup = new System.Windows.Forms.GroupBox();
            this.addIncomingBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fromGroup = new System.Windows.Forms.GroupBox();
            this.addOutcomingBtn = new System.Windows.Forms.Button();
            this.environmentGroup = new System.Windows.Forms.GroupBox();
            this.addEnvironmentalBtn = new System.Windows.Forms.Button();
            this.victimCheckBox = new System.Windows.Forms.CheckBox();
            this.aggressorCheckBox = new System.Windows.Forms.CheckBox();
            this.toGroup.SuspendLayout();
            this.fromGroup.SuspendLayout();
            this.environmentGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // participantTitle
            // 
            this.participantTitle.AutoSize = true;
            this.participantTitle.Location = new System.Drawing.Point(6, 9);
            this.participantTitle.Name = "participantTitle";
            this.participantTitle.Size = new System.Drawing.Size(54, 13);
            this.participantTitle.TabIndex = 0;
            this.participantTitle.Text = "Paticipant";
            // 
            // toGroup
            // 
            this.toGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toGroup.Controls.Add(this.addIncomingBtn);
            this.toGroup.Location = new System.Drawing.Point(9, 31);
            this.toGroup.Name = "toGroup";
            this.toGroup.Size = new System.Drawing.Size(272, 50);
            this.toGroup.TabIndex = 2;
            this.toGroup.TabStop = false;
            this.toGroup.Text = "Incoming";
            // 
            // addIncomingBtn
            // 
            this.addIncomingBtn.Location = new System.Drawing.Point(7, 20);
            this.addIncomingBtn.Name = "addIncomingBtn";
            this.addIncomingBtn.Size = new System.Drawing.Size(75, 23);
            this.addIncomingBtn.TabIndex = 0;
            this.addIncomingBtn.Text = "Add";
            this.addIncomingBtn.UseVisualStyleBackColor = true;
            this.addIncomingBtn.Click += new System.EventHandler(this.addIncomingBtn_Click);
            // 
            // fromGroup
            // 
            this.fromGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fromGroup.Controls.Add(this.addOutcomingBtn);
            this.fromGroup.Location = new System.Drawing.Point(9, 83);
            this.fromGroup.Name = "fromGroup";
            this.fromGroup.Size = new System.Drawing.Size(272, 50);
            this.fromGroup.TabIndex = 3;
            this.fromGroup.TabStop = false;
            this.fromGroup.Text = "Outcoming";
            // 
            // addOutcomingBtn
            // 
            this.addOutcomingBtn.Location = new System.Drawing.Point(7, 19);
            this.addOutcomingBtn.Name = "addOutcomingBtn";
            this.addOutcomingBtn.Size = new System.Drawing.Size(75, 23);
            this.addOutcomingBtn.TabIndex = 1;
            this.addOutcomingBtn.Text = "Add";
            this.addOutcomingBtn.UseVisualStyleBackColor = true;
            this.addOutcomingBtn.Click += new System.EventHandler(this.addOutcomingBtn_Click);
            // 
            // environmentGroup
            // 
            this.environmentGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentGroup.Controls.Add(this.addEnvironmentalBtn);
            this.environmentGroup.Location = new System.Drawing.Point(9, 139);
            this.environmentGroup.Name = "environmentGroup";
            this.environmentGroup.Size = new System.Drawing.Size(272, 50);
            this.environmentGroup.TabIndex = 4;
            this.environmentGroup.TabStop = false;
            this.environmentGroup.Text = "From environment";
            // 
            // addEnvironmentalBtn
            // 
            this.addEnvironmentalBtn.Location = new System.Drawing.Point(7, 19);
            this.addEnvironmentalBtn.Name = "addEnvironmentalBtn";
            this.addEnvironmentalBtn.Size = new System.Drawing.Size(75, 23);
            this.addEnvironmentalBtn.TabIndex = 2;
            this.addEnvironmentalBtn.Text = "Add";
            this.addEnvironmentalBtn.UseVisualStyleBackColor = true;
            this.addEnvironmentalBtn.Click += new System.EventHandler(this.addEnvironmentalBtn_Click);
            // 
            // victimCheckBox
            // 
            this.victimCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.victimCheckBox.AutoSize = true;
            this.victimCheckBox.Location = new System.Drawing.Point(157, 8);
            this.victimCheckBox.Name = "victimCheckBox";
            this.victimCheckBox.Size = new System.Drawing.Size(54, 17);
            this.victimCheckBox.TabIndex = 6;
            this.victimCheckBox.Text = "Victim";
            this.victimCheckBox.UseVisualStyleBackColor = true;
            this.victimCheckBox.CheckedChanged += new System.EventHandler(this.victimCheckBox_CheckedChanged);
            // 
            // aggressorCheckBox
            // 
            this.aggressorCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.aggressorCheckBox.AutoSize = true;
            this.aggressorCheckBox.Location = new System.Drawing.Point(208, 8);
            this.aggressorCheckBox.Name = "aggressorCheckBox";
            this.aggressorCheckBox.Size = new System.Drawing.Size(73, 17);
            this.aggressorCheckBox.TabIndex = 7;
            this.aggressorCheckBox.Text = "Aggressor";
            this.aggressorCheckBox.UseVisualStyleBackColor = true;
            this.aggressorCheckBox.CheckedChanged += new System.EventHandler(this.aggressorCheckBox_CheckedChanged);
            // 
            // ParticipantContactsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toGroup);
            this.Controls.Add(this.environmentGroup);
            this.Controls.Add(this.aggressorCheckBox);
            this.Controls.Add(this.fromGroup);
            this.Controls.Add(this.victimCheckBox);
            this.Controls.Add(this.participantTitle);
            this.Name = "ParticipantContactsControl";
            this.Size = new System.Drawing.Size(289, 203);
            this.toGroup.ResumeLayout(false);
            this.fromGroup.ResumeLayout(false);
            this.environmentGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label participantTitle;
        private System.Windows.Forms.GroupBox toGroup;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox fromGroup;
        private System.Windows.Forms.GroupBox environmentGroup;
        private System.Windows.Forms.CheckBox victimCheckBox;
        private System.Windows.Forms.CheckBox aggressorCheckBox;
        private System.Windows.Forms.Button addIncomingBtn;
        private System.Windows.Forms.Button addOutcomingBtn;
        private System.Windows.Forms.Button addEnvironmentalBtn;
    }
}
