using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using PointInConvexPolygonAlgorithm;

namespace PointInConvexPolygonAlgorithmAdapter
{
  public class PointInConvexPolygonAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _snapshotRecorder = snapshotRecorder;
      Point searchPoint = input.PointList[0];
      var pointInConvexPolygon = new PointInConvexPolygon(
        input.ClosedPolylineList[0].Points.ToArray(),
        searchPoint);

      pointInConvexPolygon.SearchingInZone += PointInConvexPolygonOnSearchingInZone;
      pointInConvexPolygon.InteriorPointFound += PointInConvexPolygonOnInteriorPointFound;
      pointInConvexPolygon.EdgeToCompareFound += PointInConvexPolygonOnEdgeToCompareFound;

      using (_snapshotRecorder.Show(searchPoint, _visualStyles.SearchPoint))
      using (
        _zone =
          _snapshotRecorder.Show(new Ray[0], _visualStyles.Rays))
      {
        bool isInterior = pointInConvexPolygon.IsInterior();
        SnapshotDescription verdict = isInterior
          ? _snapshotDescriptions.VerdictPointIsInterior
          : _snapshotDescriptions.VerdictPointIsExterior;
        _snapshotRecorder.TakeSnapshot(verdict);
      }
    }

    ISnapshotRecorder _snapshotRecorder;
    IDrawableEntityTracker<Ray[]> _zone;
    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public PointInConvexPolygonAdapter()
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

    void PointInConvexPolygonOnEdgeToCompareFound(LineSegment edge)
    {
      _snapshotRecorder.Show(edge, _visualStyles.EdgeToCompare);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.EdgeToCompareFound);
    }

    void PointInConvexPolygonOnSearchingInZone(Ray[] rays)
    {
      _zone.Update(rays);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.SearchingInZone);
    }

    void PointInConvexPolygonOnInteriorPointFound(Point point)
    {
      _snapshotRecorder.Show(point, _visualStyles.InteriorPoint);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.InteriorPointFound);
    }

  }
}