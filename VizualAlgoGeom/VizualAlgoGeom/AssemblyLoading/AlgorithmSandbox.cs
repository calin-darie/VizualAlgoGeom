using System;
using System.Security.Policy;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using Snapshots;

namespace VizualAlgoGeom.AssemblyLoading
{
  public class AlgorithmSandbox : IDisposable
  {
    public void Dispose()
    {
      if (_tempDomain != null)
      {
        AppDomain.Unload(_tempDomain);
      }
    }

    readonly AppDomain _tempDomain;

    public AlgorithmSandbox()
    {
      Evidence evidence = AppDomain.CurrentDomain.Evidence;
      _tempDomain = AppDomain.CreateDomain("algorithm sandbox " + Guid.NewGuid(),
        evidence,
        AppDomain.CurrentDomain.SetupInformation);
    }

    internal T Get<T>() where T : new()
    {
      var instance = (T) _tempDomain.CreateInstanceAndUnwrap(
        typeof (T).Assembly.FullName,
        typeof (T).FullName);
      return instance;
    }

    internal AlgorithmLoader GetAlgorithmLoaderWithDependencies()
    {
      var loader = Get<AlgorithmLoader>();
      var snapshotRecorder = Get<SnapshotRecorder>();
      loader.SnapshotRecorder = snapshotRecorder;
      loader.CanvasViewRegistry = Get<CanvasViewRegistry>();
      return loader;
    }
  }
}