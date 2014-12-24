using System.Collections.Generic;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public interface IEdge
  {
    Dictionary<EdgeSide, Point> EndPoints { get; set; }
    Line Line { get; }
    Point IntersectionPointWith(IEdge other);
  }
}