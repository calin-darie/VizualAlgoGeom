namespace VizualAlgoGeom
{
    partial class RemarksControl
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
            this.TextBoxRemark = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _textBoxRemark
            // 
            this.TextBoxRemark.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxRemark.Location = new System.Drawing.Point(0, 0);
            this.TextBoxRemark.Multiline = true;
            this.TextBoxRemark.Name = "textBoxRemark";
            this.TextBoxRemark.ReadOnly = true;
            this.TextBoxRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxRemark.Size = new System.Drawing.Size(275, 100);
            this.TextBoxRemark.TabIndex = 0;
            // 
            // RemarksControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextBoxRemark);
            this.Name = "RemarksControl";
            this.Size = new System.Drawing.Size(275, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
