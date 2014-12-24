using System.Collections.Generic;
using GeometricElements;

namespace InterfaceOfAlgorithmAdaptersWithVisualizer
{
  public interface IAlgorithmInput
  {
    IList<Point> PointList { get; }
    IList<LineSegment> LineSegmentList { get; }
    IList<Ray> RayList { get; }
    IList<Line> LineList { get; }
    IList<PolyLine> ClosedPolylineList { get; }
    IList<PolyLine> PolyLineList { get; }
    IList<Weighted<Point>> WeightedPointList { get; }
  }
}