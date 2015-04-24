namespace VizualAlgoGeom
{
    partial class ToolboxControl
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolboxControl));
      this.ttPoint = new System.Windows.Forms.ToolTip(this.components);
      this.btPoint = new System.Windows.Forms.Button();
      this.ttLineSegment = new System.Windows.Forms.ToolTip(this.components);
      this.btLineSegment = new System.Windows.Forms.Button();
      this.ttRay = new System.Windows.Forms.ToolTip(this.components);
      this.btRay = new System.Windows.Forms.Button();
      this.ttLine = new System.Windows.Forms.ToolTip(this.components);
      this.btLine = new System.Windows.Forms.Button();
      this.ttLineLoop = new System.Windows.Forms.ToolTip(this.components);
      this.btLineLoop = new System.Windows.Forms.Button();
      this.ttComplex = new System.Windows.Forms.ToolTip(this.components);
      this.ttLineStrip = new System.Windows.Forms.ToolTip(this.components);
      this.btLineStrip = new System.Windows.Forms.Button();
      this.ttCenter = new System.Windows.Forms.ToolTip(this.components);
      this._btCenter = new System.Windows.Forms.Button();
      this.btDcel = new System.Windows.Forms.Button();
      this.ttSelect = new System.Windows.Forms.ToolTip(this.components);
      this.ttNewGroup = new System.Windows.Forms.ToolTip(this.components);
      this.ttWeightedPoint = new System.Windows.Forms.ToolTip(this.components);
      this.btWeightedPoint = new System.Windows.Forms.Button();
      this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.flowLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // ttPoint
      // 
      this.ttPoint.AutomaticDelay = 0;
      this.ttPoint.Tag = "";
      // 
      // btPoint
      // 
      this.btPoint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.btPoint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btPoint.BackgroundImage")));
      this.btPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btPoint.Location = new System.Drawing.Point(2, 2);
      this.btPoint.Margin = new System.Windows.Forms.Padding(2);
      this.btPoint.Name = "btPoint";
      this.btPoint.Size = new System.Drawing.Size(26, 28);
      this.btPoint.TabIndex = 19;
      this.ttPoint.SetToolTip(this.btPoint, "Point");
      this.btPoint.UseVisualStyleBackColor = true;
      this.btPoint.Click += new System.EventHandler(this.btPoint_Click);
      // 
      // btLineSegment
      // 
      this.btLineSegment.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btLineSegment.BackgroundImage")));
      this.btLineSegment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btLineSegment.Location = new System.Drawing.Point(62, 2);
      this.btLineSegment.Margin = new System.Windows.Forms.Padding(2);
      this.btLineSegment.Name = "btLineSegment";
      this.btLineSegment.Size = new System.Drawing.Size(26, 28);
      this.btLineSegment.TabIndex = 16;
      this.ttLineSegment.SetToolTip(this.btLineSegment, "Line segment");
      this.btLineSegment.UseVisualStyleBackColor = true;
      this.btLineSegment.Click += new System.EventHandler(this.btLineSegment_Click);
      // 
      // btRay
      // 
      this.btRay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btRay.BackgroundImage")));
      this.btRay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btRay.Location = new System.Drawing.Point(92, 2);
      this.btRay.Margin = new System.Windows.Forms.Padding(2);
      this.btRay.Name = "btRay";
      this.btRay.Size = new System.Drawing.Size(26, 28);
      this.btRay.TabIndex = 13;
      this.ttRay.SetToolTip(this.btRay, "Ray");
      this.btRay.UseVisualStyleBackColor = false;
      this.btRay.Click += new System.EventHandler(this.btRay_Click);
      // 
      // btLine
      // 
      this.btLine.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btLine.BackgroundImage")));
      this.btLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btLine.Location = new System.Drawing.Point(122, 2);
      this.btLine.Margin = new System.Windows.Forms.Padding(2);
      this.btLine.Name = "btLine";
      this.btLine.Size = new System.Drawing.Size(26, 28);
      this.btLine.TabIndex = 17;
      this.ttLine.SetToolTip(this.btLine, "Line");
      this.btLine.UseVisualStyleBackColor = true;
      this.btLine.Click += new System.EventHandler(this.btLine_Click);
      // 
      // btLineLoop
      // 
      this.btLineLoop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btLineLoop.BackgroundImage")));
      this.btLineLoop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btLineLoop.Location = new System.Drawing.Point(182, 2);
      this.btLineLoop.Margin = new System.Windows.Forms.Padding(2);
      this.btLineLoop.Name = "btLineLoop";
      this.btLineLoop.Size = new System.Drawing.Size(26, 28);
      this.btLineLoop.TabIndex = 18;
      this.ttLineLoop.SetToolTip(this.btLineLoop, "Line loop");
      this.btLineLoop.UseVisualStyleBackColor = true;
      this.btLineLoop.Click += new System.EventHandler(this.btLineLoop_Click);
      // 
      // btLineStrip
      // 
      this.btLineStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btLineStrip.BackgroundImage")));
      this.btLineStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btLineStrip.Location = new System.Drawing.Point(152, 2);
      this.btLineStrip.Margin = new System.Windows.Forms.Padding(2);
      this.btLineStrip.Name = "btLineStrip";
      this.btLineStrip.Size = new System.Drawing.Size(26, 28);
      this.btLineStrip.TabIndex = 14;
      this.ttLineStrip.SetToolTip(this.btLineStrip, "Line strip");
      this.btLineStrip.UseVisualStyleBackColor = true;
      this.btLineStrip.Click += new System.EventHandler(this.btLineStrip_Click);
      // 
      // _btCenter
      // 
      this._btCenter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_btCenter.BackgroundImage")));
      this._btCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this._btCenter.Location = new System.Drawing.Point(212, 2);
      this._btCenter.Margin = new System.Windows.Forms.Padding(2);
      this._btCenter.Name = "_btCenter";
      this._btCenter.Size = new System.Drawing.Size(26, 28);
      this._btCenter.TabIndex = 20;
      this.ttCenter.SetToolTip(this._btCenter, "Center origin");
      this._btCenter.UseVisualStyleBackColor = true;
      // 
      // btDcel
      // 
      this.btDcel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btDcel.BackgroundImage")));
      this.btDcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btDcel.Location = new System.Drawing.Point(242, 2);
      this.btDcel.Margin = new System.Windows.Forms.Padding(2);
      this.btDcel.Name = "btDcel";
      this.btDcel.Size = new System.Drawing.Size(26, 28);
      this.btDcel.TabIndex = 24;
      this.ttCenter.SetToolTip(this.btDcel, "Center origin");
      this.btDcel.UseVisualStyleBackColor = true;
      this.btDcel.Click += new System.EventHandler(this.btDcel_Click);
      // 
      // btWeightedPoint
      // 
      this.btWeightedPoint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btWeightedPoint.BackgroundImage")));
      this.btWeightedPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.btWeightedPoint.Location = new System.Drawing.Point(32, 2);
      this.btWeightedPoint.Margin = new System.Windows.Forms.Padding(2);
      this.btWeightedPoint.Name = "btWeightedPoint";
      this.btWeightedPoint.Size = new System.Drawing.Size(26, 28);
      this.btWeightedPoint.TabIndex = 23;
      this.ttWeightedPoint.SetToolTip(this.btWeightedPoint, "Weighted point");
      this.btWeightedPoint.UseVisualStyleBackColor = true;
      this.btWeightedPoint.Click += new System.EventHandler(this.btWeightedPoint_Click);
      // 
      // flowLayoutPanel
      // 
      this.flowLayoutPanel.Controls.Add(this.btPoint);
      this.flowLayoutPanel.Controls.Add(this.btWeightedPoint);
      this.flowLayoutPanel.Controls.Add(this.btLineSegment);
      this.flowLayoutPanel.Controls.Add(this.btRay);
      this.flowLayoutPanel.Controls.Add(this.btLine);
      this.flowLayoutPanel.Controls.Add(this.btLineStrip);
      this.flowLayoutPanel.Controls.Add(this.btLineLoop);
      this.flowLayoutPanel.Controls.Add(this._btCenter);
      this.flowLayoutPanel.Controls.Add(this.btDcel);
      this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
      this.flowLayoutPanel.Name = "flowLayoutPanel";
      this.flowLayoutPanel.Size = new System.Drawing.Size(273, 37);
      this.flowLayoutPanel.TabIndex = 0;
      // 
      // ToolboxControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.flowLayoutPanel);
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "ToolboxControl";
      this.Size = new System.Drawing.Size(273, 37);
      this.flowLayoutPanel.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip ttPoint;
        private System.Windows.Forms.ToolTip ttRay;
        private System.Windows.Forms.ToolTip ttLineSegment;
        private System.Windows.Forms.ToolTip ttLine;
        private System.Windows.Forms.ToolTip ttLineLoop;
        private System.Windows.Forms.ToolTip ttComplex;
        private System.Windows.Forms.ToolTip ttLineStrip;
        private System.Windows.Forms.ToolTip ttCenter;
        private System.Windows.Forms.ToolTip ttSelect;
        private System.Windows.Forms.ToolTip ttNewGroup;
        private System.Windows.Forms.ToolTip ttWeightedPoint;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button btWeightedPoint;
        private System.Windows.Forms.Button _btCenter;
        private System.Windows.Forms.Button btPoint;
        private System.Windows.Forms.Button btLineLoop;
        private System.Windows.Forms.Button btLine;
        private System.Windows.Forms.Button btLineSegment;
        private System.Windows.Forms.Button btLineStrip;
        private System.Windows.Forms.Button btRay;
        private System.Windows.Forms.Button btDcel;
    }
}
