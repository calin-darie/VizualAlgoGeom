namespace Infrastructure
{
  public static class Numbers
  {
    const double DefaultTolerance = 1E-4;

    public static bool EqualTolerant(double a, double b, double tolerance = DefaultTolerance)
    {
      return System.Math.Abs(a - b) < tolerance;
    }

    public static bool GreaterThanTolerant(this double a, double b, double tolerance = DefaultTolerance)
    {
      return a - b >= tolerance;
    }
  }
}
