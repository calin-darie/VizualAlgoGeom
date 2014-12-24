using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using CubicSplineInterpolationAlgorithm;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace CubicSplineInterpolationAlgorithmAdapter
{
  public class CubicSplineInterpolationAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _snapshotRecorder = snapshotRecorder;
      using (_interpolationPoints = _snapshotRecorder.Show(new List<Point>(), _visualStyles.InterpolationPoints))
      {
        _splineInterpolator = new CubicSplineInterpolation(input.PointList.ToArray());
        _splineInterpolator.ResultUpdated += SplineInterpolatorOnResultUpdated;

        List<Point> result = _splineInterpolator.CubicSpline();

        ShowFinalResult(result);
      }
    }

    ISnapshotRecorder _snapshotRecorder;
    CubicSplineInterpolation _splineInterpolator;

    IDrawableEntityTracker<List<Point>> _interpolationPoints;

    void ShowFinalResult(List<Point> result)
    {
      using (_snapshotRecorder.Show(new PolyLine(result), _visualStyles.FinalCurve))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.CurveComputed);
      }
    }

    void SplineInterpolatorOnResultUpdated(List<Point> points)
    {
      _interpolationPoints.Update(points);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.CurveSegmentComputed);
    }

    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public CubicSplineInterpolationAdapter()
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
      get { return _pseudocode; }
    }
  }
}