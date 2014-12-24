using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using Point = GeometricElements.Point;

namespace TestTriangleAlgorithmAdapter
{
  public class TestTriangleAdapter : IAlgorithmAdapter
  {
    readonly IExplanationsProvider _explanationsProvider = new DefaultExplanationsProvider(
      Assembly.GetExecutingAssembly().Location, "CubicSplineInterpolationResources");

    public TestTriangleAdapter()
    {
      _snapshotDescriptions = new Lazy<SnapshotDescriptions>(LoadSnapshotDescriptions);
    }

    public IExplanationsProvider ExplanationsProvider
    {
      get { return _explanationsProvider; }
    }

    readonly Lazy<SnapshotDescriptions> _snapshotDescriptions;
    private SnapshotDescriptions SnapshotDescriptions { get { return _snapshotDescriptions.Value; } }


    SnapshotDescriptions LoadSnapshotDescriptions()
    {
      SnapshotDescriptions result = _explanationsProvider.LoadSnapshotDescriptions<SnapshotDescriptions>()
        ?? new SnapshotDescriptions
        {
          PointAdded = new SnapshotDescription { PseudocodeLine = 1 },
          ErrorNotEnoughPoints = new SnapshotDescription { Remark ="at least three points needed" },
          Done = new SnapshotDescription { Remark = "YEY"}
        };
      return result;
    }
    public void RunAlgorithm(IAlgorithmInputData inputData,
      ISnapshotRecorder snapshotRecorder,
      IDrawableEntityFactory drawableEntityFactory,
      CanvasViewRegistry canvasViewRegistry)
    {
      Point[] inputPoints = inputData.PointList.Select(p => new Point(p.X, p.Y)).ToArray();
      var edges = new List<LineSegment>();
      if (inputPoints.Length < 3)
      {
        snapshotRecorder.TakeSnapshot(SnapshotDescriptions.ErrorNotEnoughPoints);
        return;
      }
      using (drawableEntityFactory.Show<IEnumerable<LineSegment>>(edges, 
        new VisualHints(Color.Green)))
      {
        edges.Add(new LineSegment(inputPoints[0], inputPoints[1]));
        snapshotRecorder.TakeSnapshot(SnapshotDescriptions.PointAdded);
        edges.Add(new LineSegment(inputPoints[0], inputPoints[2]));
        snapshotRecorder.TakeSnapshot(SnapshotDescriptions.PointAdded);
        edges.Add(new LineSegment(inputPoints[1], inputPoints[2]));
        snapshotRecorder.TakeSnapshot(SnapshotDescriptions.PointAdded);
      }
      snapshotRecorder.TakeSnapshot(SnapshotDescriptions.Done);
    }
  }


  public class SnapshotDescriptions
  {
    public SnapshotDescription PointAdded { get; set; }

    public SnapshotDescription Done { get; set; }
    public SnapshotDescription ErrorNotEnoughPoints { get; set; }
  }
}