namespace VoronoiAlgorithm
{
  public enum EdgeSide
  {
    Left = 0,
    Right = 1
  }

  public static class EdgeListPmExtensions
  {
    public static EdgeSide OppositeSide(this EdgeSide pm)
    {
      return 1 - pm;
    }
  }
}