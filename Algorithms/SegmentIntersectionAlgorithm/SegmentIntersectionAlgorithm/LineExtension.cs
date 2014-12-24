using GeometricElements;

namespace SegmentIntersectionAlgorithm
{
  public static class LineExtension
  {
    public static bool DoLineSegmentsIntersect(this LineSegment l1, LineSegment l2, ref Point ptIntersection)
    {
      // Denominator for ua and ub are the same, so store this calculation
      double d =
        (l2.SecondPoint.Y - l2.FirstPoint.Y)*(l1.SecondPoint.X - l1.FirstPoint.X)
        -
        (l2.SecondPoint.X - l2.FirstPoint.X)*(l1.SecondPoint.Y - l1.FirstPoint.Y);

      //n_a and n_b are calculated as seperate values for readability
      double n_a =
        (l2.SecondPoint.X - l2.FirstPoint.X)*(l1.FirstPoint.Y - l2.FirstPoint.Y)
        -
        (l2.SecondPoint.Y - l2.FirstPoint.Y)*(l1.FirstPoint.X - l2.FirstPoint.X);

      double n_b =
        (l1.SecondPoint.X - l1.FirstPoint.X)*(l1.FirstPoint.Y - l2.FirstPoint.Y)
        -
        (l1.SecondPoint.Y - l1.FirstPoint.Y)*(l1.FirstPoint.X - l2.FirstPoint.X);

      // Make sure there is not a division by zero - this also indicates that
      // the lines are parallel.  
      // If n_a and n_b were both equal to zero the lines would be on top of each 
      // other (coincidental).  This check is not done because it is not 
      // necessary for this implementation (the parallel check accounts for this).
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (d == 0)
        return false;

      // Calculate the intermediate fractional point that the lines potentially intersect.
      double ua = n_a/d;
      double ub = n_b/d;

      // The fractional point will be between 0 and 1 inclusive if the lines
      // intersect.  If the fractional calculation is larger than 1 or smaller
      // than 0 the lines would need to be longer to intersect.
      if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
      {
        ptIntersection = new Point(l1.FirstPoint.X + (ua*(l1.SecondPoint.X - l1.FirstPoint.X)),
          l1.FirstPoint.Y + (ua*(l1.SecondPoint.Y - l1.FirstPoint.Y)));
        return true;
      }
      return false;
    }
  }
}