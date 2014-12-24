namespace VizualAlgoGeom
{
    partial class SelectionControl
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
            this.components = new System.ComponentModel.Container();
            this.TvSelection = new System.Windows.Forms.TreeView();
            this.treeviewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeviewContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tvSelection
            // 
            this.TvSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TvSelection.LabelEdit = true;
            this.TvSelection.Location = new System.Drawing.Point(0, 0);
            this.TvSelection.Name = "tvSelection";
            this.TvSelection.Size = new System.Drawing.Size(256, 270);
            this.TvSelection.TabIndex = 0;
            this.TvSelection.MouseDown += new System.Windows.Forms.MouseEventHandler(this._tvSelection_MouseDown);
            // 
            // treeviewContextMenuStrip
            // 
            this.treeviewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.deleteGroupToolStripMenuItem,
            this.addGroupToolStripMenuItem});
            this.treeviewContextMenuStrip.Name = "treeviewContextMenuStrip";
            this.treeviewContextMenuStrip.Size = new System.Drawing.Size(167, 98);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // deleteGroupToolStripMenuItem
            // 
            this.deleteGroupToolStripMenuItem.Name = "deleteGroupToolStripMenuItem";
            this.deleteGroupToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.deleteGroupToolStripMenuItem.Text = "Delete group";
            this.deleteGroupToolStripMenuItem.Click += new System.EventHandler(this.deleteGroupToolStripMenuItem_Click);
            // 
            // addGroupToolStripMenuItem
            // 
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.addGroupToolStripMenuItem.Text = "Add group";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            // 
            // SelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TvSelection);
            this.Name = "SelectionControl";
            this.Size = new System.Drawing.Size(256, 270);
            this.treeviewContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

      private System.Windows.Forms.ContextMenuStrip treeviewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addGroupToolStripMenuItem;
    }
}
