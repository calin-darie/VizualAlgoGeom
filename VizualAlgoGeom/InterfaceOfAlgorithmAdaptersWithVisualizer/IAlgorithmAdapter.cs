using System.Collections.Generic;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace InterfaceOfAlgorithmAdaptersWithVisualizer
{
  public interface IAlgorithmAdapter
  {
    string Explanation { get; }
    List<IPseudocodeLine> Pseudocode { get; }

    void RunAlgorithm(
      IAlgorithmInput input,
      ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry);
  }
}