using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometricElements
{
  [Serializable]
  public class Ray : PointPair
  {
    public Ray(Point firstPoint, Point secondPoint)
      : base(firstPoint, secondPoint)
    {
    }

    public bool Contains(Point point)
    {
      return SegmentContains(point) ||
             (new LineSegment(FirstPoint, point)).SegmentContains(SecondPoint);
    }

    public IList<Point> IntersectionsWithRectangle(Rectangle rectangle)
    {
      return Line.IntersectionsWithRectangle(rectangle)
        .Where(Contains)
        .ToList();
    }
  }
}