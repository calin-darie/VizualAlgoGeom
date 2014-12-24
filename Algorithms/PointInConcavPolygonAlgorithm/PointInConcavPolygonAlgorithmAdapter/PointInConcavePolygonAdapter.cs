using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using PointInConcavePolygonAlgorithm;

namespace PointInConcavePolygonAlgorithmAdapter
{
  public class PointInConcavePolygonAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _intersectionPoints = new List<Point>();
      _snapshotRecorder = snapshotRecorder;
      Point searchPoint = input.PointList[0];
      var pointInConcavePolygon = new PointInConcavePolygon(
        input.ClosedPolylineList[0].Points.ToArray(),
        searchPoint);
      pointInConcavePolygon.TestingEdge += PointInConvexPolygonOnTestingEdge;
      pointInConcavePolygon.IntersectionFound += PointInConvexPolygonOnIntersectionFound;

      using (_snapshotRecorder.Show(searchPoint, _visualStyles.SearchPoint))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.SearchPointNotation);
        using (_snapshotRecorder.Show(GetDrawableSearchLine(searchPoint), _visualStyles.SearchLine))
        {
          _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.IntroducingSearchLine);
          using (_snapshotRecorder.Show(_intersectionPoints, _visualStyles.IntersectionPoints))
          using (_currentEdge = _snapshotRecorder.Show(new LineSegment(), _visualStyles.CurrentEdge))
          {
            bool isInterior = pointInConcavePolygon.IsInterior();
            SnapshotDescription verdict = isInterior
              ? _snapshotDescriptions.VerdictPointIsInterior
              : _snapshotDescriptions.VerdictPointIsExterior;
            _snapshotRecorder.TakeSnapshot(verdict);
          }
        }
      }
    }

    IDrawableEntityTracker<LineSegment> _currentEdge;
    List<Point> _intersectionPoints;
    ISnapshotRecorder _snapshotRecorder;
    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public PointInConcavePolygonAdapter()
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

    Line GetDrawableSearchLine(Point searchPoint)
    {
      return new PointPair(
        new Point(searchPoint.X + 3, searchPoint.Y),
        new Point(searchPoint.X + 5, searchPoint.Y))
        .Line;
    }

    void PointInConvexPolygonOnIntersectionFound(Point point)
    {
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.IntersectionFound);
    }

    void PointInConvexPolygonOnTestingEdge(LineSegment edge)
    {
      _currentEdge.Update(edge);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.TestingEdge);
    }

    
  }
}