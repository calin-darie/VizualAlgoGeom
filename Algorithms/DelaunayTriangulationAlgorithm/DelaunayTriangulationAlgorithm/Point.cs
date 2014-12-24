namespace DelaunayTriangulationAlgorithm
{
  public class Point
  {
    public Point(GeometricElements.Point point, int? specialIndex = null)
    {
      X = point.X;
      Y = point.Y;
      SpecialIndex = specialIndex;
    }

    internal int? SpecialIndex { get; set; }
    public double X { get; private set; }
    public double Y { get; private set; }

    public static implicit operator GeometricElements.Point(Point p)
    {
      return new GeometricElements.Point(p.X, p.Y);
    }
  }
}