using System;
using System.Collections.Generic;
using System.Linq;
using GeometricElements;

namespace CubicSplineInterpolationAlgorithm
{
  public class CubicSplineInterpolation
  {
    // The X and Y co-ordinates of the control points.
    double[] _controlsX;
    double[] _controlsY;
    // Cubic spline segments.  Each segment is a polynomial over the
    // range x = [0-1].
    Cubic[] _cubicX;
    Cubic[] _cubicY;
    // ******************************************************************** //
    // Private Data.
    // ******************************************************************** //

    // The control points for this curve.
    readonly Point[] _controlPoints;

    public CubicSplineInterpolation(Point[] inputPoints)
    {
      _controlPoints = inputPoints;
    }

    Cubic[] ComputeNaturalCubic(int n, double[] x, double[] gamma)
    {
      var delta = new double[n + 1];
      delta[0] = 3*(x[1] - x[0])*gamma[0];
      for (var i = 1; i < n; ++i)
        delta[i] = (3*(x[i + 1] - x[i - 1]) - delta[i - 1])*gamma[i];
      delta[n] = (3*(x[n] - x[n - 1]) - delta[n - 1])*gamma[n];

      var D = new double[n + 1];
      D[n] = delta[n];
      for (int i = n - 1; i >= 0; --i)
      {
        D[i] = delta[i] - gamma[i]*D[i + 1];
      }

      // Calculate the cubic segments.
      var C = new Cubic[n];
      for (var i = 0; i < n; i++)
      {
        double a = x[i];
        double b = D[i];
        double c = 3*(x[i + 1] - x[i]) - 2*D[i] - D[i + 1];
        double d = 2*(x[i] - x[i + 1]) + D[i] + D[i + 1];
        C[i] = new Cubic(a, b, c, d);
      }
      return C;
    }

    /**
     * Interpolate the spline.
     * @param   steps   The number of steps to interpolate in each segment.
     * @return          The interpolated values.
     */

    public Point[] Interpolate(int steps)
    {
      var p = new Point[_cubicX.Length*steps + 1];
      var np = 0;

      /* very crude technique - just break each segment up into steps lines */
      p[np++] = new Point(_cubicX[0].Eval(0), _cubicY[0].Eval(0));
      for (var i = 0; i < _cubicX.Length; i++)
      {
        for (var j = 1; j <= steps; j++)
        {
          double x = j/(double) steps;
          p[np++] = new Point(_cubicX[i].Eval(x), _cubicY[i].Eval(x));
        }
      }
      return p;
    }

    public List<Point> CubicSpline()
    {
      var result = new List<Point>();
      int len = _controlPoints.Length;

      // Flatten out the control points array.
      _controlsX = new double[len];
      _controlsY = new double[len];
      for (var i = 0; i < len; ++i)
      {
        _controlsX[i] = _controlPoints[i].X;
        _controlsY[i] = _controlPoints[i].Y;
      }

      // Compute the gamma values just once.
      int n = _controlPoints.Length - 1;
      var gamma = new double[n + 1];
      gamma[0] = 1.0/2.0;
      for (var i = 1; i < n; ++i)
        gamma[i] = 1/(4 - gamma[i - 1]);
      gamma[n] = 1/(2 - gamma[n - 1]);

      //Compute the cubic segments.
      _cubicX = ComputeNaturalCubic(n, _controlsX, gamma);
      _cubicY = ComputeNaturalCubic(n, _controlsY, gamma);

      for (var i = 0; i < _cubicX.Length; i++)
      {
        for (double t = 0; t <= 1; t = t + 0.01)
        {
          double valX = _cubicX.ElementAt(i).Eval(t);
          double valY = _cubicY.ElementAt(i).Eval(t);
          result.Add(new Point(valX, valY));
        }
        OnResultUpdated(result);
      }

      return result;
    }

    public event Action<List<Point>> ResultUpdated;

    protected virtual void OnResultUpdated(List<Point> obj)
    {
      Action<List<Point>> handler = ResultUpdated;
      if (handler != null) handler(obj);
    }
  }
}