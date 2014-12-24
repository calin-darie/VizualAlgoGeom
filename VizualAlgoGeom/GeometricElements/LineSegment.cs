using System;

namespace GeometricElements
{
  [Serializable]
  public class LineSegment : PointPair
  {
    public LineSegment(Point firstPoint, Point secondPoint)
      : base(firstPoint, secondPoint)
    {
    }

    public LineSegment()
    {
    }
  }
}