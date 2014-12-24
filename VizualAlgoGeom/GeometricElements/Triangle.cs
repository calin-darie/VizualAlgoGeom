using System;

namespace GeometricElements
{
  [Serializable]
  public class Triangle
  {
    readonly Point[] _points;

    public Triangle(Point p1, Point p2, Point p3)
    {
      if (p1 == null)
      {
        throw new ArgumentNullException("p1");
      }
      if (p2 == null)
      {
        throw new ArgumentNullException("p2");
      }
      if (p3 == null)
      {
        throw new ArgumentNullException("p3");
      }

      if (AreColinear(p1, p2, p3))
      {
        throw new ArgumentException("colinear points do not form a triangle.");
      }

      _points = new[] {p1, p2, p3};
    }

    public LineSegment[] Edges
    {
      get
      {
        // to ensure encapsulation, we have to create an array every time
        // so don't bother keeping a collection of edges in a field

        // this situation will change if ever the collection of edges is to be used by other methods of this class
        return new[]
        {
          new LineSegment(_points[0], _points[1]),
          new LineSegment(_points[1], _points[2]),
          new LineSegment(_points[2], _points[0])
        };
      }
    }

    static bool AreColinear(Point p1, Point p2, Point p3)
    {
      return new PointPair(p1, p2).Line.Contains(p3);
    }
  }
}