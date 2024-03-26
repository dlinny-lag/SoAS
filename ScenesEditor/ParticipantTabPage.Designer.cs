
namespace ScenesEditor
{
    partial class ParticipantTabPage
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
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.aggressorCheckBox = new System.Windows.Forms.CheckBox();
            this.victimCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.aggressorCheckBox);
            this.panel1.Controls.Add(this.victimCheckBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 386);
            this.panel1.TabIndex = 0;
            // 
            // aggressorCheckBox
            // 
            this.aggressorCheckBox.AutoSize = true;
            this.aggressorCheckBox.Location = new System.Drawing.Point(5, 28);
            this.aggressorCheckBox.Name = "aggressorCheckBox";
            this.aggressorCheckBox.Size = new System.Drawing.Size(73, 17);
            this.aggressorCheckBox.TabIndex = 7;
            this.aggressorCheckBox.Text = "Aggressor";
            this.aggressorCheckBox.UseVisualStyleBackColor = true;
            this.aggressorCheckBox.CheckedChanged += new System.EventHandler(this.aggressorCheckBox_CheckedChanged);
            // 
            // victimCheckBox
            // 
            this.victimCheckBox.AutoSize = true;
            this.victimCheckBox.Location = new System.Drawing.Point(5, 8);
            this.victimCheckBox.Name = "victimCheckBox";
            this.victimCheckBox.Size = new System.Drawing.Size(54, 17);
            this.victimCheckBox.TabIndex = 6;
            this.victimCheckBox.Text = "Victim";
            this.victimCheckBox.UseVisualStyleBackColor = true;
            this.victimCheckBox.CheckedChanged += new System.EventHandler(this.victimCheckBox_CheckedChanged);
            // 
            // ParticipantTabPage
            // 
            this.Controls.Add(this.panel1);
            this.Size = new System.Drawing.Size(637, 386);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox victimCheckBox;
        private System.Windows.Forms.CheckBox aggressorCheckBox;

    }
}
