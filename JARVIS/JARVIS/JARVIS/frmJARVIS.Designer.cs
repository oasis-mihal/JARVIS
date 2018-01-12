namespace JARVIS
{
    partial class frmJARVIS
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
            this.lstVoice = new System.Windows.Forms.ListBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstVoice
            // 
            this.lstVoice.FormattingEnabled = true;
            this.lstVoice.ItemHeight = 20;
            this.lstVoice.Location = new System.Drawing.Point(46, 36);
            this.lstVoice.Name = "lstVoice";
            this.lstVoice.Size = new System.Drawing.Size(620, 324);
            this.lstVoice.TabIndex = 0;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(766, 86);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(402, 26);
            this.txtInput.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(766, 118);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(110, 46);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // frmJARVIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 573);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lstVoice);
            this.Name = "frmJARVIS";
            this.Text = "JARVIS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmJARVIS_FormClosing);
            this.Load += new System.EventHandler(this.frmJARVIS_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstVoice;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnSubmit;
    }
}

