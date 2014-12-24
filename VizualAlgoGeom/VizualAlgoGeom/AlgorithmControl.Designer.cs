namespace VizualAlgoGeom
{
    partial class AlgorithmControl
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
            this.treeViewPseudocode = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewPseudocode
            // 
            this.treeViewPseudocode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPseudocode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewPseudocode.Location = new System.Drawing.Point(0, 0);
            this.treeViewPseudocode.Name = "treeViewPseudocode";
            this.treeViewPseudocode.Size = new System.Drawing.Size(250, 275);
            this.treeViewPseudocode.TabIndex = 0;
            // 
            // AlgorithmControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewPseudocode);
            this.Name = "AlgorithmControl";
            this.Size = new System.Drawing.Size(250, 275);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewPseudocode;
    }
}
