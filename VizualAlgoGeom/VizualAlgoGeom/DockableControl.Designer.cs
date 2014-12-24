namespace VizualAlgoGeom
{
    partial class DockableControl
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
            this.CanvasControl = new VizualAlgoGeom.CanvasControl();
            this.SuspendLayout();
            // 
            // canvasControl
            // 
            this.CanvasControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CanvasControl.Location = new System.Drawing.Point(24, 24);
            this.CanvasControl.Name = "CanvasControl";
            this.CanvasControl.Size = new System.Drawing.Size(1033, 612);
            this.CanvasControl.TabIndex = 14;
            // 
            // DockableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.Controls.Add(this.CanvasControl);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "DockableControl";
            this.Controls.SetChildIndex(this.CanvasControl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
