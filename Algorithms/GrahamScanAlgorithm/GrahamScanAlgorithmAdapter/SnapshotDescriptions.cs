using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace GrahamScanAlgorithmAdapter
{
  public class SnapshotDescriptions
  {
    public SnapshotDescription UpperHullPointRemoved { get; set; }
    public SnapshotDescription LowerHullPointRemoved { get; set; }
    public SnapshotDescription UpperHullPointAdded { get; set; }
    public SnapshotDescription LowerHullPointAdded { get; set; }
    public SnapshotDescription ConcatenateHulls { get; set; }
    public SnapshotDescription UpperHullFirstPointAdded { get; set; }
    public SnapshotDescription UpperHullSecondPointAdded { get; set; }
    public SnapshotDescription LowerHullFirstPointAdded { get; set; }
    public SnapshotDescription LowerHullSecondPointAdded { get; set; }
  }
}