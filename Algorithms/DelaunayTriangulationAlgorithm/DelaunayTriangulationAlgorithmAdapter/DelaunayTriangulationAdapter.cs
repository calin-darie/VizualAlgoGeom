using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using DelaunayTriangulationAlgorithm;
using DelaunayTriangulationAlgorithm.EdgeList;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using Point = DelaunayTriangulationAlgorithm.Point;
using Triangle = DelaunayTriangulationAlgorithm.EdgeList.Triangle;

namespace DelaunayTriangulationAlgorithmAdapter
{
  public class DelaunayTriangulationAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _sites = input.PointList
        .Select(p => new Point(new GeometricElements.Point(p.X, p.Y)))
        .ToArray();

      _snapshotRecorder = snapshotRecorder;

      _algorithm = new DelaunayRandomizedIncrementalTriangulator();
      _algorithm.TriangleAdded += algorithm_TriangleAdded;
      _algorithm.TriangleRemoved += algorithm_TriangleRemoved;
      _algorithm.InsertingPoint += _algorithm_InsertingPoint;
      _algorithm.TestingCircle += _algorithm_TestingCircle;
      _algorithm.FlippingEdge += _algorithm_FlippingEdge;

      try
      {
        using (
          _supportingTriangles =
            _snapshotRecorder.Show(GetDrawableSupportingTriangles, _visualStyles.SupportingTriangles))
        using (_triangulation = _snapshotRecorder.Show(GetDrawableTriangulation, _visualStyles.Diagram))
        {
          _algorithm.Run(_sites);
          _triangulation.Update();
          _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.Done);
        }
      }
      catch (Exception e)
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.Error.WithFormattedRemark(e));
      }
    }

    public string Explanation
    {
      get { return _explanation; }
    }

    public List<IPseudocodeLine> Pseudocode
    {
      get { return _pseudocode; }
    }

    DelaunayRandomizedIncrementalTriangulator _algorithm;
    Point[] _sites;
    ISnapshotRecorder _snapshotRecorder;
    IDrawableEntityTracker _supportingTriangles;
    IDrawableEntityTracker _triangulation;
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public DelaunayTriangulationAdapter()
    {
      var paths = new AlgorithmResourcePaths(Assembly.GetExecutingAssembly().Location);

      _visualStyles = new XmlIo<VisualStyles>().LoadFrom(paths.VisualStyles) ?? new VisualStyles();
      _snapshotDescriptions = new XmlIo<SnapshotDescriptions>().LoadFrom(paths.SnapshotDescriptions) ?? new SnapshotDescriptions();
      _explanation = new DefaultExplanationLoader().LoadFrom(paths.Explanation) ?? string.Empty;
      _pseudocode = new DefaultPseudocodeLoader().LoadFrom(paths.Pseudocode) ?? new List<IPseudocodeLine>();
    }

    void _algorithm_TestingCircle(
      Triangle triangle,
      Circle circumcircle,
      GeometricElements.Point pointToTestForInclusion)
    {
      using (_snapshotRecorder.Show(triangle.Edges.ToLineSegments(), _visualStyles.TestLegalEdge))
      using (_snapshotRecorder.Show(circumcircle, _visualStyles.TestLegalEdge))
      using (_snapshotRecorder.Show(pointToTestForInclusion, _visualStyles.TestLegalEdge))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.TestingLegalEdgeCircle);
      }
    }

    void _algorithm_FlippingEdge(Edge edge, Point point)
    {
      using (_snapshotRecorder.Show<LineSegment>(edge, _visualStyles.IllegalEdge))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.FlippingEdge);
      }
    }

    void _algorithm_InsertingPoint(Point point)
    {
      Update();
      ShowPoint(point, _visualStyles.AddedItem, _snapshotDescriptions.PointInserted.WithFormattedRemark(point));
    }

    void algorithm_TriangleAdded(Triangle triangle)
    {
      Update();
      using (_snapshotRecorder.Show(triangle.Edges.ToLineSegments(), _visualStyles.AddedItem))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.TriangleAdded);
      }
    }

    void algorithm_TriangleRemoved(Triangle triangle)
    {
      Update();
      using (_snapshotRecorder.Show(triangle.Edges.ToLineSegments(), _visualStyles.RemovedTriangle))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.TriangleRemoved);
      }
    }

    void Update()
    {
      _triangulation.Update();
      _supportingTriangles.Update();
    }

    IEnumerable<LineSegment> GetDrawableTriangulation()
    {
      return GetDrawableTriangles(_algorithm.Triangles.Where(t => !t.IsSupporting));
    }

    IEnumerable<LineSegment> GetDrawableSupportingTriangles()
    {
      return GetDrawableTriangles(_algorithm.Triangles.Where(t => t.IsSupporting));
    }

    IEnumerable<LineSegment> GetDrawableTriangles(IEnumerable<Triangle> triangles)
    {
      IEnumerable<LineSegment> lines = triangles.SelectMany(t => t.Edges).ToLineSegments();
      return lines;
    }

    void ShowPoint(GeometricElements.Point point, VisualStyle style, SnapshotDescription description)
    {
      using (_snapshotRecorder.Show(point, style))
      {
        _snapshotRecorder.TakeSnapshot(description);
      }
    }
  }
}