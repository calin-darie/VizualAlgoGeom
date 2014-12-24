namespace VoronoiAlgorithm
{
  public interface ISweepLine
  {
    double Y { get; }
    void AdvanceTo(IEvent currentEvent);
  }
}