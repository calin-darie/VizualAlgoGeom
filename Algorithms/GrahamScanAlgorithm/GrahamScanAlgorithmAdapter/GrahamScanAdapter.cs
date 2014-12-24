using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using GrahamScanAlgorithm;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace GrahamScanAlgorithmAdapter
{
  public class GrahamScanAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      Point[] points = input.PointList.ToArray();
      var grahamScanner = new GrahamScan(points);
      _snapshotRecorder = snapshotRecorder;

      List<Point> hull;
      using (_lowerConvexHullDrawable = _snapshotRecorder.Show(CreateOpenPolyLine(new List<Point>()), _visualStyles.LowerConvexHull))
      using (_upperConvexHullDrawable = _snapshotRecorder.Show(CreateOpenPolyLine(new List<Point>()), _visualStyles.UpperConvexHull))
      {
        grahamScanner.LowerHullPointInserted += LowerHullPointInserted;
        grahamScanner.UpperHullPointInserted += UpperHullPointInserted;
        grahamScanner.LowerHullPointRemoved += LowerHullPointRemoved;
        grahamScanner.UpperHullPointRemoved += UpperHullPointRemoved;
        hull = grahamScanner.Graham();
      }
      ShowFinalResult(hull);
    }

    IDrawableEntityTracker<PolyLine> _lowerConvexHullDrawable;
    IDrawableEntityTracker<PolyLine> _upperConvexHullDrawable;
    ISnapshotRecorder _snapshotRecorder;
    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public GrahamScanAdapter()
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

    void UpperHullPointRemoved(List<Point> points)
    {
      UpdateUpperHull(points);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.UpperHullPointRemoved);
    }

    void LowerHullPointRemoved(List<Point> points)
    {
      UpdateLowerHull(points);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.LowerHullPointRemoved);
    }

    void ShowFinalResult(List<Point> result)
    {
      using (_snapshotRecorder.Show(new PolyLine(result, closed: true), _visualStyles.ConvexHull))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.ConcatenateHulls);
      }
    }

    void UpperHullPointInserted(List<Point> points)
    {
      UpdateUpperHull(points);
      SnapshotDescription description;
      switch (points.Count)
      {
        case 1:
          description = _snapshotDescriptions.UpperHullFirstPointAdded;
          break;
        case 2:
          description = _snapshotDescriptions.UpperHullSecondPointAdded;
          break;
        default:
          description = _snapshotDescriptions.UpperHullPointAdded;
          break;
      }
      _snapshotRecorder.TakeSnapshot(description);
    }

    void UpdateUpperHull(List<Point> points)
    {
      _upperConvexHullDrawable.Update(CreateOpenPolyLine(points));
    }

    static PolyLine CreateOpenPolyLine(List<Point> points)
    {
      return new PolyLine(points, closed:false);
    }

    void LowerHullPointInserted(List<Point> points)
    {
      UpdateLowerHull(points);
      SnapshotDescription description;
      switch (points.Count)
      {
        case 1:
          description = _snapshotDescriptions.LowerHullFirstPointAdded;
          break;
        case 2:
          description = _snapshotDescriptions.LowerHullSecondPointAdded;
          break;
        default:
          description = _snapshotDescriptions.LowerHullPointAdded;
          break;
      }
      _snapshotRecorder.TakeSnapshot(description);
    }

    void UpdateLowerHull(List<Point> points)
    {
      _lowerConvexHullDrawable.Update(CreateOpenPolyLine(points));
    }
  }
}