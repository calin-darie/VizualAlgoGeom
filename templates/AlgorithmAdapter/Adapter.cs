using System.Collections.Generic;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace $safeprojectname$AlgorithmAdapter
{
  public class $safeprojectname$Adapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      //var algorithm = new $safeprojectname$();
      _snapshotRecorder = snapshotRecorder;

      //algorithm.ResultUpdted += AlgorithmOnResultUpdated

      using (_drawable = _snapshotRecorder.Show(new List<Point>(), _visualStyles.PartialOutput))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.OutputUpdated);
        //result = algorithm.Run();
      }
      //ShowFinalResult(result);
    }

    private IDrawableEntityTracker<List<Point>> _drawable;
    ISnapshotRecorder _snapshotRecorder;
    
    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public $safeprojectname$Adapter()
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
  }
}