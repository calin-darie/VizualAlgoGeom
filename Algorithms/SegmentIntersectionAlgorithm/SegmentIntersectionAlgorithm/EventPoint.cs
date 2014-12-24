using System.Collections.Generic;
using GeometricElements;

namespace SegmentIntersectionAlgorithm
{
  internal class EventPoint
  {
    public EventPoint(Point point)
    {
      Point = point;
      CorrespondingSegments = new List<LineSegment>();
    }

    public Point Point { get; set; }
    public List<LineSegment> CorrespondingSegments { get; set; }
  }
}