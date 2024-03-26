
namespace AAFXmlScanner
{
    partial class MainForm
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
            this.selectAAFFolderBtn = new System.Windows.Forms.Button();
            this.aafFolderPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnFileErrors = new System.Windows.Forms.Button();
            this.btnAnimationDuplications = new System.Windows.Forms.Button();
            this.btnAnimationGroupDuplications = new System.Windows.Forms.Button();
            this.btnPositionDuplications = new System.Windows.Forms.Button();
            this.btnPositionTreeDuplications = new System.Windows.Forms.Button();
            this.btnValidationErrors = new System.Windows.Forms.Button();
            this.btnRaceDuplications = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnParseWarnings = new System.Windows.Forms.Button();
            this.btnParseErrors = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectAAFFolderBtn
            // 
            this.selectAAFFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAAFFolderBtn.Location = new System.Drawing.Point(410, 9);
            this.selectAAFFolderBtn.Name = "selectAAFFolderBtn";
            this.selectAAFFolderBtn.Size = new System.Drawing.Size(25, 23);
            this.selectAAFFolderBtn.TabIndex = 5;
            this.selectAAFFolderBtn.Text = "...";
            this.selectAAFFolderBtn.UseVisualStyleBackColor = true;
            this.selectAAFFolderBtn.Click += new System.EventHandler(this.selectAAFFolderBtn_Click);
            // 
            // aafFolderPath
            // 
            this.aafFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aafFolderPath.Location = new System.Drawing.Point(73, 12);
            this.aafFolderPath.Name = "aafFolderPath";
            this.aafFolderPath.ReadOnly = true;
            this.aafFolderPath.Size = new System.Drawing.Size(331, 20);
            this.aafFolderPath.TabIndex = 4;
            this.aafFolderPath.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "AAF folder";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(13, 47);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnFileErrors
            // 
            this.btnFileErrors.Enabled = false;
            this.btnFileErrors.Location = new System.Drawing.Point(6, 19);
            this.btnFileErrors.Name = "btnFileErrors";
            this.btnFileErrors.Size = new System.Drawing.Size(165, 23);
            this.btnFileErrors.TabIndex = 7;
            this.btnFileErrors.Text = "File parse errors";
            this.btnFileErrors.UseVisualStyleBackColor = true;
            this.btnFileErrors.Click += new System.EventHandler(this.btnFileErrors_Click);
            // 
            // btnAnimationDuplications
            // 
            this.btnAnimationDuplications.Enabled = false;
            this.btnAnimationDuplications.Location = new System.Drawing.Point(6, 19);
            this.btnAnimationDuplications.Name = "btnAnimationDuplications";
            this.btnAnimationDuplications.Size = new System.Drawing.Size(165, 23);
            this.btnAnimationDuplications.TabIndex = 8;
            this.btnAnimationDuplications.Text = "Animation duplications";
            this.btnAnimationDuplications.UseVisualStyleBackColor = true;
            this.btnAnimationDuplications.Click += new System.EventHandler(this.btnAnimationDuplications_Click);
            // 
            // btnAnimationGroupDuplications
            // 
            this.btnAnimationGroupDuplications.Enabled = false;
            this.btnAnimationGroupDuplications.Location = new System.Drawing.Point(6, 48);
            this.btnAnimationGroupDuplications.Name = "btnAnimationGroupDuplications";
            this.btnAnimationGroupDuplications.Size = new System.Drawing.Size(165, 23);
            this.btnAnimationGroupDuplications.TabIndex = 9;
            this.btnAnimationGroupDuplications.Text = "Animation group duplications";
            this.btnAnimationGroupDuplications.UseVisualStyleBackColor = true;
            this.btnAnimationGroupDuplications.Click += new System.EventHandler(this.btnAnimationGroupDuplications_Click);
            // 
            // btnPositionDuplications
            // 
            this.btnPositionDuplications.Enabled = false;
            this.btnPositionDuplications.Location = new System.Drawing.Point(6, 77);
            this.btnPositionDuplications.Name = "btnPositionDuplications";
            this.btnPositionDuplications.Size = new System.Drawing.Size(165, 23);
            this.btnPositionDuplications.TabIndex = 10;
            this.btnPositionDuplications.Text = "Position duplications";
            this.btnPositionDuplications.UseVisualStyleBackColor = true;
            this.btnPositionDuplications.Click += new System.EventHandler(this.btnPositionDuplications_Click);
            // 
            // btnPositionTreeDuplications
            // 
            this.btnPositionTreeDuplications.Enabled = false;
            this.btnPositionTreeDuplications.Location = new System.Drawing.Point(6, 106);
            this.btnPositionTreeDuplications.Name = "btnPositionTreeDuplications";
            this.btnPositionTreeDuplications.Size = new System.Drawing.Size(165, 23);
            this.btnPositionTreeDuplications.TabIndex = 11;
            this.btnPositionTreeDuplications.Text = "Position tree duplications";
            this.btnPositionTreeDuplications.UseVisualStyleBackColor = true;
            this.btnPositionTreeDuplications.Click += new System.EventHandler(this.btnPositionTreeDuplications_Click);
            // 
            // btnValidationErrors
            // 
            this.btnValidationErrors.Enabled = false;
            this.btnValidationErrors.Location = new System.Drawing.Point(19, 220);
            this.btnValidationErrors.Name = "btnValidationErrors";
            this.btnValidationErrors.Size = new System.Drawing.Size(165, 23);
            this.btnValidationErrors.TabIndex = 12;
            this.btnValidationErrors.Text = "Position validation errors";
            this.btnValidationErrors.UseVisualStyleBackColor = true;
            this.btnValidationErrors.Click += new System.EventHandler(this.btnValidationErrors_Click);
            // 
            // btnRaceDuplications
            // 
            this.btnRaceDuplications.Enabled = false;
            this.btnRaceDuplications.Location = new System.Drawing.Point(6, 135);
            this.btnRaceDuplications.Name = "btnRaceDuplications";
            this.btnRaceDuplications.Size = new System.Drawing.Size(165, 23);
            this.btnRaceDuplications.TabIndex = 13;
            this.btnRaceDuplications.Text = "Race duplications";
            this.btnRaceDuplications.UseVisualStyleBackColor = true;
            this.btnRaceDuplications.Click += new System.EventHandler(this.btnRaceDuplications_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnParseWarnings);
            this.groupBox1.Controls.Add(this.btnParseErrors);
            this.groupBox1.Controls.Add(this.btnFileErrors);
            this.groupBox1.Location = new System.Drawing.Point(13, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 111);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AAF XML Parsing";
            // 
            // btnParseWarnings
            // 
            this.btnParseWarnings.Enabled = false;
            this.btnParseWarnings.Location = new System.Drawing.Point(6, 77);
            this.btnParseWarnings.Name = "btnParseWarnings";
            this.btnParseWarnings.Size = new System.Drawing.Size(165, 23);
            this.btnParseWarnings.TabIndex = 9;
            this.btnParseWarnings.Text = "Content warnings";
            this.btnParseWarnings.UseVisualStyleBackColor = true;
            this.btnParseWarnings.Click += new System.EventHandler(this.btnParseWarnings_Click);
            // 
            // btnParseErrors
            // 
            this.btnParseErrors.Enabled = false;
            this.btnParseErrors.Location = new System.Drawing.Point(6, 48);
            this.btnParseErrors.Name = "btnParseErrors";
            this.btnParseErrors.Size = new System.Drawing.Size(165, 23);
            this.btnParseErrors.TabIndex = 8;
            this.btnParseErrors.Text = "Content errors";
            this.btnParseErrors.UseVisualStyleBackColor = true;
            this.btnParseErrors.Click += new System.EventHandler(this.btnParseErrors_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAnimationDuplications);
            this.groupBox2.Controls.Add(this.btnAnimationGroupDuplications);
            this.groupBox2.Controls.Add(this.btnRaceDuplications);
            this.groupBox2.Controls.Add(this.btnPositionDuplications);
            this.groupBox2.Controls.Add(this.btnPositionTreeDuplications);
            this.groupBox2.Location = new System.Drawing.Point(252, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 168);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Duplications";
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(360, 47);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(252, 47);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(75, 23);
            this.btnStatistics.TabIndex = 17;
            this.btnStatistics.Text = "Statistics";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 263);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnValidationErrors);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.selectAAFFolderBtn);
            this.Controls.Add(this.aafFolderPath);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AAF XML Scanner";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectAAFFolderBtn;
        private System.Windows.Forms.TextBox aafFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnFileErrors;
        private System.Windows.Forms.Button btnAnimationDuplications;
        private System.Windows.Forms.Button btnAnimationGroupDuplications;
        private System.Windows.Forms.Button btnPositionDuplications;
        private System.Windows.Forms.Button btnPositionTreeDuplications;
        private System.Windows.Forms.Button btnValidationErrors;
        private System.Windows.Forms.Button btnRaceDuplications;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnParseWarnings;
        private System.Windows.Forms.Button btnParseErrors;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnStatistics;
    }
}

