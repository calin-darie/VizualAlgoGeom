using System.Collections.Generic;
using System.Linq;
using DelaunayTriangulationAlgorithm.EdgeList;
using GeometricElements;

namespace DelaunayTriangulationAlgorithmAdapter
{
  public static class EdgeConversions
  {
    public static IEnumerable<LineSegment> ToLineSegments(this IEnumerable<Edge> edges)
    {
      return edges.Select(e => e.ToLineSegment()).ToArray();
    }
  }
}