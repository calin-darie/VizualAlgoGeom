using System.Collections.Generic;
using GeometricElements;

namespace SegmentIntersectionAlgorithm
{
  public class Intersection
  {
    readonly Point _intersectionPoint;
    readonly List<LineSegment> _intersectionSegments;

    public Intersection(Point intersection, List<LineSegment> intersectionSegments)
    {
      _intersectionPoint = intersection;
      _intersectionSegments = intersectionSegments;
    }

    public Point IntersectionPoint
    {
      get { return _intersectionPoint; }
    }

    public List<LineSegment> IntersectionSegments
    {
      get { return _intersectionSegments; }
    }
  }
}