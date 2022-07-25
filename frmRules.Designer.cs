
namespace Absorb
{
    partial class frmRules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRules));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxRules = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Stencil", 40F);
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 80);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rules";
            // 
            // txtBoxRules
            // 
            this.txtBoxRules.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtBoxRules.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtBoxRules.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBoxRules.Location = new System.Drawing.Point(27, 100);
            this.txtBoxRules.Multiline = true;
            this.txtBoxRules.Name = "txtBoxRules";
            this.txtBoxRules.ReadOnly = true;
            this.txtBoxRules.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxRules.Size = new System.Drawing.Size(616, 501);
            this.txtBoxRules.TabIndex = 1;
            // 
            // frmRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Absorb.Properties.Resources.absorbmenuimage;
            this.ClientSize = new System.Drawing.Size(697, 628);
            this.Controls.Add(this.txtBoxRules);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmRules";
            this.Text = "Rules";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxRules;
    }
}