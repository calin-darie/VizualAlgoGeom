using System;
using System.Collections.Generic;
using GeometricElements;

namespace deBoorAlgorithm
{
  public class DeBoor
  {
    readonly Point[] _controlPoints;
    readonly List<Point> _curve = new List<Point>();
    readonly double[] _splineOutX = new double[1000];
    readonly double[] _splineOutY = new double[1000];

    public DeBoor(Point[] ctrlPoints)
    {
      _controlPoints = ctrlPoints;
    }

    public void BSpline(Point p1, Point p2, Point p3, Point p4, int divisions)
    {
      var a = new double[4];
      var b = new double[4];


      a[0] = (-p1.X + 3*p2.X - 3*p3.X + p4.X)/6.0;
      a[1] = (3*p1.X - 6*p2.X + 3*p3.X)/6.0;
      a[2] = (-3*p1.X + 3*p3.X)/6.0;
      a[3] = (p1.X + 4*p2.X + p3.X)/6.0;

      b[0] = (-p1.Y + 3*p2.Y - 3*p3.Y + p4.Y)/6.0;
      b[1] = (3*p1.Y - 6*p2.Y + 3*p3.Y)/6.0;
      b[2] = (-3*p1.Y + 3*p3.Y)/6.0;
      b[3] = (p1.Y + 4*p2.Y + p3.Y)/6.0;

      _splineOutX[0] = a[3];
      _splineOutY[0] = b[3];

      for (var i = 1; i < divisions; i++)
      {
        double t = Convert.ToSingle(i)/Convert.ToSingle(divisions);
        _splineOutX[i] = a[3] + t*(a[2] + t*(a[1] + t*a[0]));
        _splineOutY[i] = b[3] + t*(b[2] + t*(b[1] + t*b[0]));
      }
    }

    public List<Point> BSpline()
    {
      int n = _controlPoints.Length;

      for (var i = 1; i < n - 2; i++)
      {
        double temp =
          (Math.Sqrt(Math.Pow((_controlPoints[i - 1].X - _controlPoints[i].X), 2) +
                     Math.Pow((_controlPoints[i - 1].Y - _controlPoints[i].Y), 2)));
        int interpol = Convert.ToInt32(temp);
        BSpline(_controlPoints[i - 1], _controlPoints[i], _controlPoints[i + 1], _controlPoints[i + 2], interpol);
        for (var k = 0; k <= interpol - 1; k++)
        {
          var p = new Point(_splineOutX[k], _splineOutY[k]);
          _curve.Add(p);
          OnCurveUpdated(_curve);
        }
      }
      return _curve;
    }

    public event Action<List<Point>> CurveUpdated;

    protected virtual void OnCurveUpdated(List<Point> obj)
    {
      Action<List<Point>> handler = CurveUpdated;
      if (handler != null) handler(obj);
    }
  }
}