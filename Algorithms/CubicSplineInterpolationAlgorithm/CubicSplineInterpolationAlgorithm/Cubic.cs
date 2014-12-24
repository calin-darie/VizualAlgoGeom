namespace CubicSplineInterpolationAlgorithm
{
  public class Cubic
  {
    // ******************************************************************** //
    // Private Data.
    // ******************************************************************** //

    // The coefficients.
    readonly double _a;
    readonly double _b;
    readonly double _c;
    readonly double _d;
    // ******************************************************************** //
    // Constructor.
    // ******************************************************************** //

    /**
             * Create a cubic polynomial of form a + b*x + c*x^2 + d*x^3.
             * 
             * @param   a       A coefficient.
             * @param   b       B coefficient.
             * @param   c       C coefficient.
             * @param   d       D coefficient.
             */

    public Cubic(double a, double b, double c, double d)
    {
      _a = a;
      _b = b;
      _c = c;
      _d = d;
    }

    // ******************************************************************** //
    // Evaluation.
    // ******************************************************************** //

    /**
             * Evaluate the polynomial for a given value.
             * 
             * @param   x       X value to evaluate for.
             * @return          The value of the polynomial for the given X.
             */

    public double Eval(double x)
    {
      return ((_d*x + _c)*x + _b)*x + _a;
    }
  }
}