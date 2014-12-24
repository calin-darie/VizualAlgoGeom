using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using InterfaceOfSnapshotsWithVisualizer;
using Snapshots;
using VizualAlgoGeom.DTO;

namespace VizualAlgoGeom.AssemblyLoading
{
  public class AlgorithmLoader : MarshalByRefObject
  {
    public List<IPseudocodeLine> PseudocodeLines { get; private set; }
    public string Explanation { get; private set; }
    public AlgorithmInput Input { get; set; }
    public SnapshotRecorder SnapshotRecorder { get; set; }
    public CanvasViewRegistry CanvasViewRegistry { get; set; }

    public void RunAlgorithm(string fileName)
    {
      IAlgorithmAdapter algorithmFactory = GetAlgorithmFactoryInAssembly(fileName);

      algorithmFactory.RunAlgorithm(Input, SnapshotRecorder, CanvasViewRegistry);
      PseudocodeLines = algorithmFactory.Pseudocode;
      Explanation = algorithmFactory.Explanation;
    }

    IAlgorithmAdapter GetAlgorithmFactoryInAssembly(string assemblyFileName)
    {
      Assembly algorithmAssembly = Assembly.LoadFrom(assemblyFileName);
      Type algorithmFactoryType =
        algorithmAssembly.GetTypes()
          .FirstOrDefault(typeof (IAlgorithmAdapter).IsAssignableFrom);

      if (algorithmFactoryType == null)
      {
        throw new AlgorithmFactoryNotFoundException();
      }

      var algorithmFactory = (IAlgorithmAdapter) Activator.CreateInstance(algorithmFactoryType);
      return algorithmFactory;
    }

    internal IEnumerable<IPendingDraw> GetDrawableObjects(ISnapshot snapshot)
    {
      var wrappers = new List<IPendingDraw>();
      foreach (KeyValuePair<int, IObjectSnapshot> kvp in snapshot)
      {
        kvp.Value.CollectDrawableWrappers(CanvasViewRegistry, wrappers);
      }
      return wrappers;
    }
  }
}