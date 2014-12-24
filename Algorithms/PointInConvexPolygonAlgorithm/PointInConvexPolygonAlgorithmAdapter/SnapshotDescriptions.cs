using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace PointInConvexPolygonAlgorithmAdapter
{
  public class SnapshotDescriptions
  {
    public SnapshotDescription VerdictPointIsExterior { get; set; }
    public SnapshotDescription VerdictPointIsInterior { get; set; }
    public SnapshotDescription SearchPointNotation { get; set; }
    public SnapshotDescription InteriorPointFound { get; set; }
    public SnapshotDescription EdgeToCompareFound { get; set; }
    public SnapshotDescription SearchingInZone { get; set; }
  }
}