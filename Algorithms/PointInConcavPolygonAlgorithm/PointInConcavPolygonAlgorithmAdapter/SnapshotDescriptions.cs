using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace PointInConcavePolygonAlgorithmAdapter
{
  public class SnapshotDescriptions
  {
    public SnapshotDescription TestingEdge { get; set; }
    public SnapshotDescription IntersectionFound { get; set; }
    public SnapshotDescription SearchPointNotation { get; set; }
    public SnapshotDescription VerdictPointIsInterior { get; set; }
    public SnapshotDescription VerdictPointIsExterior { get; set; }
    public SnapshotDescription IntroducingSearchLine { get; set; }
  }
}