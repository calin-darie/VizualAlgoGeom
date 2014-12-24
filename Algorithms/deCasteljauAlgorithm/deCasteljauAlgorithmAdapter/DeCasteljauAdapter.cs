using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using deCasteljauAlgorithm;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DeCasteljauAlgorithmAdapter
{
  public class DeCasteljauAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _bezierCurve = new DeCasteljau(input.PointList.ToArray());
      _bezierCurve.CurveUpdated += CurveOnUpdated;

      _snapshotRecorder = snapshotRecorder;
      using (_interpolationPoints = _snapshotRecorder.Show(new List<Point>(), _visualStyles.InterpolatedPoints))
      {
        List<Point> curve = _bezierCurve.Bezier();
        ShowFinalResult(curve);
      }
    }
    DeCasteljau _bezierCurve;
    ISnapshotRecorder _snapshotRecorder;

    

    IDrawableEntityTracker<List<Point>> _interpolationPoints;
    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public DeCasteljauAdapter()
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

    SnapshotDescriptions SnapshotDescriptions
    {
      get { return _snapshotDescriptions; }
    }

    void ShowFinalResult(List<Point> result)
    {
      using (_snapshotRecorder.Show(new PolyLine(result), _visualStyles.FinalCurve))
      {
        _snapshotRecorder.TakeSnapshot(SnapshotDescriptions.CurveComputed);
      }
    }

    void CurveOnUpdated(List<Point> points)
    {
      _interpolationPoints.Update(points);
      _snapshotRecorder.TakeSnapshot(SnapshotDescriptions.CurveUpdated);
    }
  }
}