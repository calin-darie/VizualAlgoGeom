namespace VoronoiAlgorithm
{
  public class SweepLine : ISweepLine
  {
    public double Y { get; private set; }

    public void AdvanceTo(IEvent currentEvent)
    {
      Y = currentEvent.Point.Y;
    }
  }
}