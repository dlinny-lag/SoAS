
namespace ScenesEditor
{
    partial class ContactEditorControl
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
            OnDispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.otherContactAreaSelector = new ComboTreeBox();
            this.otherParticipantSelector = new System.Windows.Forms.ComboBox();
            this.selfContactAreaSelector = new ComboTreeBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selfStimEditBox = new System.Windows.Forms.NumericUpDown();
            this.selfHoldEditBox = new System.Windows.Forms.NumericUpDown();
            this.selfTickleEditBox = new System.Windows.Forms.NumericUpDown();
            this.selfPainEditBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.environmentDirectionComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.otherPainEditBox = new System.Windows.Forms.NumericUpDown();
            this.otherTickleEditBox = new System.Windows.Forms.NumericUpDown();
            this.otherHoldEditBox = new System.Windows.Forms.NumericUpDown();
            this.otherStimEditBox = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.deletePartBtn = new System.Windows.Forms.Button();
            this.deleteEnvBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.otherComfortEditBox = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.selfComfortEditBox = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.selfPainTypeBox = new System.Windows.Forms.ComboBox();
            this.otherPainTypeBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.selfStimEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfHoldEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfTickleEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfPainEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherPainEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherTickleEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherHoldEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherStimEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherComfortEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfComfortEditBox)).BeginInit();
            this.SuspendLayout();
            // 
            // otherContactAreaSelector
            // 
            this.otherContactAreaSelector.DroppedDown = false;
            this.otherContactAreaSelector.Location = new System.Drawing.Point(136, 3);
            this.otherContactAreaSelector.Name = "otherContactAreaSelector";
            this.otherContactAreaSelector.SelectedNode = null;
            this.otherContactAreaSelector.ShowGlyphs = false;
            this.otherContactAreaSelector.Size = new System.Drawing.Size(123, 23);
            this.otherContactAreaSelector.TabIndex = 6;
            // 
            // otherParticipantSelector
            // 
            this.otherParticipantSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.otherParticipantSelector.FormattingEnabled = true;
            this.otherParticipantSelector.Location = new System.Drawing.Point(136, 167);
            this.otherParticipantSelector.Name = "otherParticipantSelector";
            this.otherParticipantSelector.Size = new System.Drawing.Size(123, 21);
            this.otherParticipantSelector.TabIndex = 5;
            // 
            // selfContactAreaSelector
            // 
            this.selfContactAreaSelector.DroppedDown = false;
            this.selfContactAreaSelector.Location = new System.Drawing.Point(3, 3);
            this.selfContactAreaSelector.Name = "selfContactAreaSelector";
            this.selfContactAreaSelector.SelectedNode = null;
            this.selfContactAreaSelector.ShowGlyphs = false;
            this.selfContactAreaSelector.Size = new System.Drawing.Size(125, 23);
            this.selfContactAreaSelector.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Stimulation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Hold";
            // 
            // selfStimEditBox
            // 
            this.selfStimEditBox.Location = new System.Drawing.Point(72, 31);
            this.selfStimEditBox.Name = "selfStimEditBox";
            this.selfStimEditBox.Size = new System.Drawing.Size(53, 20);
            this.selfStimEditBox.TabIndex = 9;
            this.selfStimEditBox.ValueChanged += new System.EventHandler(this.selfStimEditBox_ValueChanged);
            // 
            // selfHoldEditBox
            // 
            this.selfHoldEditBox.Location = new System.Drawing.Point(72, 52);
            this.selfHoldEditBox.Name = "selfHoldEditBox";
            this.selfHoldEditBox.Size = new System.Drawing.Size(53, 20);
            this.selfHoldEditBox.TabIndex = 10;
            this.selfHoldEditBox.ValueChanged += new System.EventHandler(this.selfHoldEditBox_ValueChanged);
            // 
            // selfTickleEditBox
            // 
            this.selfTickleEditBox.Location = new System.Drawing.Point(72, 119);
            this.selfTickleEditBox.Name = "selfTickleEditBox";
            this.selfTickleEditBox.Size = new System.Drawing.Size(53, 20);
            this.selfTickleEditBox.TabIndex = 11;
            this.selfTickleEditBox.ValueChanged += new System.EventHandler(this.selfTickleEditBox_ValueChanged);
            // 
            // selfPainEditBox
            // 
            this.selfPainEditBox.Location = new System.Drawing.Point(72, 74);
            this.selfPainEditBox.Name = "selfPainEditBox";
            this.selfPainEditBox.Size = new System.Drawing.Size(53, 20);
            this.selfPainEditBox.TabIndex = 12;
            this.selfPainEditBox.ValueChanged += new System.EventHandler(this.selfPainEditBox_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Pain";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Tickle";
            // 
            // environmentDirectionComboBox
            // 
            this.environmentDirectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.environmentDirectionComboBox.FormattingEnabled = true;
            this.environmentDirectionComboBox.Location = new System.Drawing.Point(3, 167);
            this.environmentDirectionComboBox.Name = "environmentDirectionComboBox";
            this.environmentDirectionComboBox.Size = new System.Drawing.Size(125, 21);
            this.environmentDirectionComboBox.TabIndex = 15;
            this.environmentDirectionComboBox.SelectedIndexChanged += new System.EventHandler(this.environmentDirectionComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Tickle";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(140, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Pain";
            // 
            // otherPainEditBox
            // 
            this.otherPainEditBox.Location = new System.Drawing.Point(203, 74);
            this.otherPainEditBox.Name = "otherPainEditBox";
            this.otherPainEditBox.Size = new System.Drawing.Size(53, 20);
            this.otherPainEditBox.TabIndex = 21;
            this.otherPainEditBox.ValueChanged += new System.EventHandler(this.otherPainEditBox_ValueChanged);
            // 
            // otherTickleEditBox
            // 
            this.otherTickleEditBox.Location = new System.Drawing.Point(203, 119);
            this.otherTickleEditBox.Name = "otherTickleEditBox";
            this.otherTickleEditBox.Size = new System.Drawing.Size(53, 20);
            this.otherTickleEditBox.TabIndex = 20;
            this.otherTickleEditBox.ValueChanged += new System.EventHandler(this.otherTickleEditBox_ValueChanged);
            // 
            // otherHoldEditBox
            // 
            this.otherHoldEditBox.Location = new System.Drawing.Point(203, 52);
            this.otherHoldEditBox.Name = "otherHoldEditBox";
            this.otherHoldEditBox.Size = new System.Drawing.Size(53, 20);
            this.otherHoldEditBox.TabIndex = 19;
            this.otherHoldEditBox.ValueChanged += new System.EventHandler(this.otherHoldEditBox_ValueChanged);
            // 
            // otherStimEditBox
            // 
            this.otherStimEditBox.Location = new System.Drawing.Point(203, 31);
            this.otherStimEditBox.Name = "otherStimEditBox";
            this.otherStimEditBox.Size = new System.Drawing.Size(53, 20);
            this.otherStimEditBox.TabIndex = 18;
            this.otherStimEditBox.ValueChanged += new System.EventHandler(this.otherStimEditBox_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(140, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Hold";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(140, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Stimulation";
            // 
            // deletePartBtn
            // 
            this.deletePartBtn.Location = new System.Drawing.Point(28, 165);
            this.deletePartBtn.Name = "deletePartBtn";
            this.deletePartBtn.Size = new System.Drawing.Size(75, 23);
            this.deletePartBtn.TabIndex = 24;
            this.deletePartBtn.Text = "Delete";
            this.deletePartBtn.UseVisualStyleBackColor = true;
            this.deletePartBtn.Click += new System.EventHandler(this.OnDeleteContactClick);
            // 
            // deleteEnvBtn
            // 
            this.deleteEnvBtn.Location = new System.Drawing.Point(158, 43);
            this.deleteEnvBtn.Name = "deleteEnvBtn";
            this.deleteEnvBtn.Size = new System.Drawing.Size(75, 23);
            this.deleteEnvBtn.TabIndex = 25;
            this.deleteEnvBtn.Text = "Delete";
            this.deleteEnvBtn.UseVisualStyleBackColor = true;
            this.deleteEnvBtn.Click += new System.EventHandler(this.OnDeleteContactClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(140, 141);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Comfort";
            // 
            // otherComfortEditBox
            // 
            this.otherComfortEditBox.Location = new System.Drawing.Point(203, 141);
            this.otherComfortEditBox.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.otherComfortEditBox.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.otherComfortEditBox.Name = "otherComfortEditBox";
            this.otherComfortEditBox.Size = new System.Drawing.Size(53, 20);
            this.otherComfortEditBox.TabIndex = 28;
            this.otherComfortEditBox.ValueChanged += new System.EventHandler(this.otherComfortEditBox_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Comfort";
            // 
            // selfComfortEditBox
            // 
            this.selfComfortEditBox.Location = new System.Drawing.Point(72, 141);
            this.selfComfortEditBox.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.selfComfortEditBox.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.selfComfortEditBox.Name = "selfComfortEditBox";
            this.selfComfortEditBox.Size = new System.Drawing.Size(53, 20);
            this.selfComfortEditBox.TabIndex = 26;
            this.selfComfortEditBox.ValueChanged += new System.EventHandler(this.selfComfortEditBox_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Pain Type";
            // 
            // selfPainTypeBox
            // 
            this.selfPainTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selfPainTypeBox.FormattingEnabled = true;
            this.selfPainTypeBox.Location = new System.Drawing.Point(72, 96);
            this.selfPainTypeBox.Name = "selfPainTypeBox";
            this.selfPainTypeBox.Size = new System.Drawing.Size(53, 21);
            this.selfPainTypeBox.TabIndex = 31;
            this.selfPainTypeBox.SelectedIndexChanged += new System.EventHandler(this.selfPainTypeBox_SelectedIndexChanged);
            // 
            // otherPainTypeBox
            // 
            this.otherPainTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.otherPainTypeBox.FormattingEnabled = true;
            this.otherPainTypeBox.Location = new System.Drawing.Point(203, 96);
            this.otherPainTypeBox.Name = "otherPainTypeBox";
            this.otherPainTypeBox.Size = new System.Drawing.Size(52, 21);
            this.otherPainTypeBox.TabIndex = 33;
            this.otherPainTypeBox.SelectedIndexChanged += new System.EventHandler(this.otherPainTypeBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(140, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Pain Type";
            // 
            // ContactEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.otherPainTypeBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.selfPainTypeBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.otherComfortEditBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.selfComfortEditBox);
            this.Controls.Add(this.deleteEnvBtn);
            this.Controls.Add(this.deletePartBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.otherPainEditBox);
            this.Controls.Add(this.otherTickleEditBox);
            this.Controls.Add(this.otherHoldEditBox);
            this.Controls.Add(this.otherStimEditBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.environmentDirectionComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.selfPainEditBox);
            this.Controls.Add(this.selfTickleEditBox);
            this.Controls.Add(this.selfHoldEditBox);
            this.Controls.Add(this.selfStimEditBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.otherContactAreaSelector);
            this.Controls.Add(this.otherParticipantSelector);
            this.Controls.Add(this.selfContactAreaSelector);
            this.Name = "ContactEditorControl";
            this.Size = new System.Drawing.Size(262, 191);
            ((System.ComponentModel.ISupportInitialize)(this.selfStimEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfHoldEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfTickleEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfPainEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherPainEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherTickleEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherHoldEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherStimEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherComfortEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selfComfortEditBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboTreeBox otherContactAreaSelector;
        private System.Windows.Forms.ComboBox otherParticipantSelector;
        private ComboTreeBox selfContactAreaSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown selfStimEditBox;
        private System.Windows.Forms.NumericUpDown selfHoldEditBox;
        private System.Windows.Forms.NumericUpDown selfTickleEditBox;
        private System.Windows.Forms.NumericUpDown selfPainEditBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox environmentDirectionComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown otherPainEditBox;
        private System.Windows.Forms.NumericUpDown otherTickleEditBox;
        private System.Windows.Forms.NumericUpDown otherHoldEditBox;
        private System.Windows.Forms.NumericUpDown otherStimEditBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button deletePartBtn;
        private System.Windows.Forms.Button deleteEnvBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown otherComfortEditBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown selfComfortEditBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox selfPainTypeBox;
        private System.Windows.Forms.ComboBox otherPainTypeBox;
        private System.Windows.Forms.Label label12;
    }
}
