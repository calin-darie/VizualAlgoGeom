using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using VoronoiAlgorithm;
using VoronoiAlgorithmAdapter.CanvasViews;
using VoronoiAlgorithmAdapter.Geometry;

namespace VoronoiAlgorithmAdapter
{
  public class VoronoiAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _snapshotRecorder = snapshotRecorder;
      canvasViewRegistry.RegisterView<ParabolicArc[], FrontLineView>(new FrontLineView());
      canvasViewRegistry.RegisterView<Diagram, DiagramView>(new DiagramView(canvasViewRegistry.GetView<Line>(),
        canvasViewRegistry.GetView<IEnumerable<Ray>>(), canvasViewRegistry.GetView<IEnumerable<LineSegment>>()
        ));

      _algorithm = new VoronoiAlgorithm.VoronoiAlgorithm();
      _algorithm.SiteEvent += algorithm_SiteEvent;

      _algorithm.CircleEvent += algorithm_CircleEvent;

      _algorithm.BreakpointAdded += algorithm_BreakpointAdded;
      _algorithm.BreakpointRemoved += algorithm_BreakpointRemoved;

      _algorithm.EdgeDiscovered += _algorithm_EdgeDiscovered;

      _algorithm.CircleEventScheduled += _algorithm_CircleEventScheduled;

      try
      {
        using (_diagram = _snapshotRecorder.Show(GetDrawableDiagram, _visualStyles.Diagram))
        {
          using (_sweepLine = _snapshotRecorder.Show(GetDrawableSweepLine, _visualStyles.SweepLine))
          using (_frontLine = _snapshotRecorder.Show<ParabolicArc[]>((Func<ParabolicArc[]>)GetDrawableFrontLine
            , _visualStyles.FrontLine))
          {
            _algorithm.Run(input.PointList);
          }
          _diagram.Update();
          _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.Done);
        }
      }
      catch (Exception e)
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.Error.WithFormattedRemark(e.ToString()));
      }
    }
    IVoronoiAlgorithm _algorithm;
    IDrawableEntityTracker _diagram;
    IDrawableEntityTracker _frontLine;
    ISnapshotRecorder _snapshotRecorder;
    IDrawableEntityTracker _sweepLine;
    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public VoronoiAdapter()
    {
      var paths = new AlgorithmResourcePaths(Assembly.GetExecutingAssembly().Location);

      _visualStyles = new XmlIo<VisualStyles>().LoadFrom(paths.VisualStyles) ?? new VisualStyles();
      _snapshotDescriptions = new XmlIo<SnapshotDescriptions>().LoadFrom(paths.SnapshotDescriptions) ?? new SnapshotDescriptions();
      _explanation = new DefaultExplanationLoader().LoadFrom(paths.Explanation) ?? string.Empty;
      _pseudocode = new DefaultPseudocodeLoader().LoadFrom(paths.Pseudocode) ?? new List<IPseudocodeLine>();
    }

    public string Explanation
    {
      get { return _explanation; }
    }

    public List<IPseudocodeLine> Pseudocode
    {
      get { return _pseudocode;}
    }

    void _algorithm_CircleEventScheduled(IEdge edge1, IEdge edge2, Circle circle, Point eventPoint)
    {
      using (HighlightSubDiagram(edge1, edge2))
      using (_snapshotRecorder.Show(circle))
      using (_snapshotRecorder.Show(circle.Center, _visualStyles.ScheduledEvent))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.CircleScheduled);
      }
    }

    void _algorithm_EdgeDiscovered(Edge edge)
    {
      using (HighlightSubDiagram(edge))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.EdgeAdded);
      }
    }

    void algorithm_BreakpointAdded(IBreakpointTracker tracker)
    {
      using (
        _snapshotRecorder.Show(tracker.Breakpoint,
          _visualStyles.AddedBreakpoint.WithFormattedName(FormatBreakpointDirection(tracker.IsGoingLeft))))
      using (_snapshotRecorder.Show(tracker.LeftArcSite, _visualStyles.LeftArcSite))
      using (_snapshotRecorder.Show(tracker.RightArcSite, _visualStyles.RightArcSite))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.AddedBreakpoint.WithFormattedRemark(tracker.Breakpoint));
      }
    }

    void algorithm_BreakpointRemoved(IBreakpointTracker tracker)
    {
      Point point = tracker.Breakpoint;
      using (_snapshotRecorder.Show(point, _visualStyles.RemovedBreakpoint))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RemovedBreakpoint
          .WithFormattedRemark(FormatBreakpointDirection(tracker.IsGoingLeft), Math.Round(point.X, 2),
            Math.Round(point.Y, 2)));
      }
    }

    string FormatBreakpointDirection(bool isGoingLeft)
    {
      return (isGoingLeft ? "left" : "right");
    }

    void algorithm_CircleEvent(Circle circle)
    {
      Update();

      using (_snapshotRecorder.Show(circle))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.HandleCircle.WithFormattedRemark(BreakpointXList()));
      }
    }

    void algorithm_SiteEvent(Point site)
    {
      Update();

      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.HandleSite.WithFormattedRemark(BreakpointXList()));
    }

    void Update()
    {
      _sweepLine.Update();
      _frontLine.Update();
      _diagram.Update();
    }

    string BreakpointXList()
    {
      return string.Join("; ",
        _algorithm.FrontLine.Breakpoints.Select(breakpointTracker => breakpointTracker.Breakpoint.X));
    }

    Line GetDrawableSweepLine()
    {
      double y = _algorithm.SweepLineY;
      return _algorithm == null ? null : Line.HorizontalWithY(y);
    }

    ParabolicArc[] GetDrawableFrontLine()
    {
      ParabolicArc[] arcs = _algorithm == null
        ? new ParabolicArc[0]
        : new FrontLineConverter().GetFrontLineParabolicArcs(_algorithm);
      return arcs;
    }

    Diagram GetDrawableDiagram()
    {
      var diagram = new Diagram();
      foreach (IEdge edge in _algorithm.Edges)
      {
        diagram.Add(edge);
      }
      return diagram;
    }

    #region voronoi specific DrawableEntity

    IDrawableEntityTracker HighlightSubDiagram(params IEdge[] edges)
    {
      var diagram = new Diagram();
      foreach (IEdge edge in edges)
        diagram.Add(edge);
      return _snapshotRecorder.Show(diagram, _visualStyles.EdgeIntersection);
    }

    #endregion
  }
}