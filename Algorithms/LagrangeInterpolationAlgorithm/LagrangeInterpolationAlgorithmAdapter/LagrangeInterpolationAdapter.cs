using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using LagrangeInterpolationAlgorithm;

namespace LagrangeInterpolationAlgorithmAdapter
{
  public class LagrangeInterpolationAdapter : IAlgorithmAdapter
  {
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public LagrangeInterpolationAdapter()
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

    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _lagrangeInterpolator = new LagrangeInterpolation(input.PointList.ToArray());

      var result = new List<Point>();
      using (snapshotRecorder.Show(result, _visualStyles.InterpolationPoints))
      {
        List<Point> lagrange = _lagrangeInterpolator.Lagrange();
        var i = 0;
        lagrange.ForEach(point =>
        {
          if (i%250 == 0) snapshotRecorder.TakeSnapshot(_snapshotDescriptions.CurveUpdated);
          result.Add(point);
          i++;
        });
      }

      ShowFinalResult(snapshotRecorder, result);
    }

    LagrangeInterpolation _lagrangeInterpolator;

    void ShowFinalResult(ISnapshotRecorder snapshotRecorder, List<Point> result)
    {
      using (snapshotRecorder.Show(new PolyLine(result), _visualStyles.CurveComputed))
      {
        snapshotRecorder.TakeSnapshot(_snapshotDescriptions.FinalCurve);
      }
    }
  }
}