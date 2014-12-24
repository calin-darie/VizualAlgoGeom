using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace VoronoiAlgorithmAdapter
{
  public class VisualStyles
  {
    public VisualStyle Diagram { get; set; }
    public VisualStyle FrontLine { get; set; }
    public VisualStyle SweepLine { get; set; }
    public VisualStyle EdgeIntersection { get; set; }
    public VisualStyle ScheduledEvent { get; set; }
    public VisualStyle AddedBreakpoint { get; set; }
    public VisualStyle RemovedBreakpoint { get; set; }
    public VisualStyle LeftArcSite { get; set; }
    public VisualStyle RightArcSite { get; set; }
  }
}