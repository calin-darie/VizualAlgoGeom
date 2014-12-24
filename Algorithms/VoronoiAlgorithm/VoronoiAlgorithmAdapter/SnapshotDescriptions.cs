using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace VoronoiAlgorithmAdapter
{
  public class SnapshotDescriptions
  {
    public SnapshotDescription Done { get; set; }
    public SnapshotDescription HandleSite { get; set; }
    public SnapshotDescription EdgeAdded { get; set; }
    public SnapshotDescription CircleScheduled { get; set; }
    public SnapshotDescription HandleCircle { get; set; }
    public SnapshotDescription Error { get; set; }
    public SnapshotDescription RemovedBreakpoint { get; set; }
    public SnapshotDescription AddedBreakpoint { get; set; }
  }
}