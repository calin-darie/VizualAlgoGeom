using System;

namespace GeometricElements
{
  internal static class AssertArgument
  {
    public static void IsFiniteNumber(double argValue, string argName)
    {
      if (double.IsInfinity(argValue)
          || double.IsNaN(argValue))
      {
        throw new ArgumentException(
          string.Format("invalid value {0}. finite number expected.", argValue)
          , argName, null);
      }
    }

    internal static void AscendingOrderStrict(double val1, double val2, string expectation)
    {
      if (val1 >= val2)
      {
        throw new ArgumentOutOfRangeException(expectation + " should be in ascending order.");
      }
    }
  }
}