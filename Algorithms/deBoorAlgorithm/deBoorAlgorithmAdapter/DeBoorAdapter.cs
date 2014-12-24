using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using deBoorAlgorithm;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DeBoorAlgorithmAdapter
{
  public class DeBoorAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      var bSplineCurve = new DeBoor(input.PointList.ToArray());
      bSplineCurve.CurveUpdated += SplineCurveOnUpdated;

      _snapshotRecorder = snapshotRecorder;
      using (_interpolationPoints = _snapshotRecorder.Show(new List<Point>(), _visualStyles.InterpolationPoints))
      {
        List<Point> curve = bSplineCurve.BSpline();
        ShowFinalResult(curve);
      }
    }

    IDrawableEntityTracker<List<Point>> _interpolationPoints;
    ISnapshotRecorder _snapshotRecorder;

    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public DeBoorAdapter()
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

    void ShowFinalResult(List<Point> result)
    {
      using (_snapshotRecorder.Show(new PolyLine(result),  _visualStyles .FinalCurve))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.CurveComputed);
      }
    }

    void SplineCurveOnUpdated(List<Point> points)
    {
      _interpolationPoints.Update(points);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.CurveUpdated);
    }
  }
}