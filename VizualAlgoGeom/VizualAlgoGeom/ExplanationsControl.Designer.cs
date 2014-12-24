namespace VizualAlgoGeom
{
    partial class ExplanationsControl
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
            this.textBoxExplanation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxExplanation
            // 
            this.textBoxExplanation.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxExplanation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxExplanation.Location = new System.Drawing.Point(0, 0);
            this.textBoxExplanation.Multiline = true;
            this.textBoxExplanation.Name = "textBoxExplanation";
            this.textBoxExplanation.ReadOnly = true;
            this.textBoxExplanation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExplanation.Size = new System.Drawing.Size(275, 150);
            this.textBoxExplanation.TabIndex = 0;
            // 
            // ExplanationsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxExplanation);
            this.Name = "ExplanationsControl";
            this.Size = new System.Drawing.Size(275, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxExplanation;
    }
}
