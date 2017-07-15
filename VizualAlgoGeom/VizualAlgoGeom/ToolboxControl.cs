using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Services;
using System.Windows.Forms;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  internal delegate void ToolChangedEventHandler(object sender, ToolChangedEventArgs e);

  public partial class ToolboxControl : UserControl
  {
    internal ToolboxControl()
    {
      InitializeComponent();
      InitializeFactories();
    }

    internal PointFactory PointFactory { get; set; }
    internal WeightedPointFactory WeightedPointFactory { get; set; }
    internal LineSegmentFactory LineSegmentFactory { get; set; }
    internal RayFactory RayFactory { get; set; }
    internal LineFactory LineFactory { get; set; }
    internal PolylineFactory PolylineFactory { get; set; }
    internal ClosedPolylineFactory ClosedPolylineFactory { get; set; }
    internal DcelFactory DcelFactory { get; set; }

    internal Button BtCenter
    {
      get { return _btCenter; }
    }

    public List<Factory> Factories => new List<Factory>
    {
      PointFactory,
      LineSegmentFactory,
      RayFactory,
      LineFactory,
      PolylineFactory,
      ClosedPolylineFactory,
      WeightedPointFactory,
      DcelFactory
    };

    internal event ToolChangedEventHandler ToolChanged;

    void InitializeFactories()
    {
      PolylineFactory = new PolylineFactory();
      LineSegmentFactory = new LineSegmentFactory();
      PointFactory = new PointFactory();
      ClosedPolylineFactory = new ClosedPolylineFactory();
      RayFactory = new RayFactory();
      LineFactory = new LineFactory();
      WeightedPointFactory = new WeightedPointFactory();
      DcelFactory = new DcelFactory();
      AddFactoriesEnableControlsEventHandlers();
    }


    void AddFactoriesEnableControlsEventHandlers()
    {
      PointFactory.EnableControls += EnableAllButtons;
      LineSegmentFactory.EnableControls += EnableAllButtons;
      PolylineFactory.EnableControls += EnableAllButtons;
      ClosedPolylineFactory.EnableControls += EnableAllButtons;
      LineFactory.EnableControls += EnableAllButtons;
      RayFactory.EnableControls += EnableAllButtons;
      WeightedPointFactory.EnableControls += EnableAllButtons;
      DcelFactory.EnableControls += EnableAllButtons;
    }

    void btPoint_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(PointFactory));
    }

    void btWeightedPoint_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(WeightedPointFactory));
    }

    void btLineSegment_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(LineSegmentFactory));
    }

    void btLineStrip_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(PolylineFactory));
    }

    void btLineLoop_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(ClosedPolylineFactory));
    }

    void btRay_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(RayFactory));
    }

    void btLine_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(LineFactory));
    }
    private void btDcel_Click(object sender, EventArgs e)
    {
      ToolChanged(sender, new ToolChangedEventArgs(DcelFactory));
    }

    void EnableAllButtons(object sender, EnableControlsEventArgs e)
    {
      bool enable = e.Enable;
      btPoint.Enabled = enable;
      btLineSegment.Enabled = enable;
      btLine.Enabled = enable;
      btRay.Enabled = enable;
      btLineStrip.Enabled = enable;
      btLineLoop.Enabled = enable;
      btWeightedPoint.Enabled = enable;
      btDcel.Enabled = enable;
    }

    public void SetCurrentGroupForFactories(Group group)
    {
      ClosedPolylineFactory.Group = group;
      LineFactory.Group = group;
      LineSegmentFactory.Group = group;
      PointFactory.Group = group;
      PolylineFactory.Group = group;
      RayFactory.Group = group;
      WeightedPointFactory.Group = group;
      DcelFactory.Group = group;
    }


  }
}