using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace SegmentIntersectionAlgorithmAdapter
{
  public class SnapshotDescriptions
  {
    public SnapshotDescription SweepLineInitialized { get; set; }
    public SnapshotDescription ExtractingEventPointFromQueue { get; set; }
    public SnapshotDescription SweepLineUpdated { get; set; }
    public SnapshotDescription IntersectionsFound { get; set; }
    public SnapshotDescription RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus { get; set; }
    public SnapshotDescription RemovingSegmentsContainingEventPointFromLineStatus { get; set; }
    public SnapshotDescription IntersectionPointFound { get; set; }
    public SnapshotDescription RestoringSegmentsContainingEventPoint { get; set; }
    public SnapshotDescription NewEventFound { get; set; }
    public SnapshotDescription TestingSegmentIntersection { get; set; }
  }
}