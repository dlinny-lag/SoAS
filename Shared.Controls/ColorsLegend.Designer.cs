
namespace Shared.Controls
{
    partial class ColorsLegend
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
            this.animationLabel = new System.Windows.Forms.Label();
            this.positionLabel = new System.Windows.Forms.Label();
            this.groupLabel = new System.Windows.Forms.Label();
            this.treeLable = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // animationLabel
            // 
            this.animationLabel.AutoSize = true;
            this.animationLabel.Location = new System.Drawing.Point(67, 3);
            this.animationLabel.Name = "animationLabel";
            this.animationLabel.Size = new System.Drawing.Size(53, 13);
            this.animationLabel.TabIndex = 0;
            this.animationLabel.Text = "Animation";
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(126, 3);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(44, 13);
            this.positionLabel.TabIndex = 1;
            this.positionLabel.Text = "Position";
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Location = new System.Drawing.Point(176, 3);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(36, 13);
            this.groupLabel.TabIndex = 2;
            this.groupLabel.Text = "Group";
            // 
            // treeLable
            // 
            this.treeLable.AutoSize = true;
            this.treeLable.Location = new System.Drawing.Point(218, 3);
            this.treeLable.Name = "treeLable";
            this.treeLable.Size = new System.Drawing.Size(29, 13);
            this.treeLable.TabIndex = 3;
            this.treeLable.Text = "Tree";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Legend:";
            // 
            // ColorsLegend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeLable);
            this.Controls.Add(this.groupLabel);
            this.Controls.Add(this.positionLabel);
            this.Controls.Add(this.animationLabel);
            this.Name = "ColorsLegend";
            this.Size = new System.Drawing.Size(250, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label animationLabel;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.Label treeLable;
        private System.Windows.Forms.Label label1;
    }
}
