using GeometricElements;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace SegmentIntersectionAlgorithm
{
  public static class PointExtension
  {
    public static int Compare(this Point p1, Point p2)
    {
      if (p1.X == p2.X && p1.Y == p2.Y)
        return 0;
      if (p1.Y > p2.Y || (p1.Y == p2.Y && p1.X < p2.X))
        return -1;
      return 1;
    }
  }
}