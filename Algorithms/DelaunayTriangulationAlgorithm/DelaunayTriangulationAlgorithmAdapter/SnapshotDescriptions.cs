using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DelaunayTriangulationAlgorithmAdapter
{
  public class SnapshotDescriptions
  {
    public SnapshotDescription Done { get; set; }
    public SnapshotDescription Error { get; set; }
    public SnapshotDescription TestingLegalEdgeCircle { get; set; }
    public SnapshotDescription TriangleRemoved { get; set; }
    public SnapshotDescription TriangleAdded { get; set; }
    public SnapshotDescription PointInserted { get; set; }
    public SnapshotDescription FlippingEdge { get; set; }
  }
}