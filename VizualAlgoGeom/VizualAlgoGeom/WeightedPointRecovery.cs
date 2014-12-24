using System.Collections.Generic;
using System.Linq;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  internal static class WeightedPointRecovery
  {
    internal static IList<WeightedPoint> GetWeightedPoints(this IEnumerable<Point> points)
    {
      return points.OfType<WeightedPoint>().ToList();
    }
  }
}