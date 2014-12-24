using System;
using System.Collections.Generic;
using GeometricElements;

namespace deCasteljauAlgorithm
{
  public class DeCasteljau
  {
    readonly Point[] _controlPoints;

    public DeCasteljau(Point[] controlPoints)
    {
      _controlPoints = controlPoints;
    }

    public List<Point> Bezier()
    {
      var curve = new List<Point>();

      int n = _controlPoints.Length;
      var c = new Point[n, n];
      double t;
      int i, j;

      for (i = 0; i < n; i++)
        c[i, 0] = _controlPoints[i];

      for (t = 0; t <= 1; t = t + 0.01)
      {
        for (i = 1; i < n; i++)
          for (j = 1; j < n; j++)
          {
            if (i <= j)
            {
              double k = (1 - t)*c[j - 1, i - 1].X + t*c[j, i - 1].X;
              double l = (1 - t)*c[j - 1, i - 1].Y + t*c[j, i - 1].Y;
              c[j, i] = new Point(k, l);
            }
          }
        curve.Add(c[n - 1, n - 1]);
        OnCurveUpdated(curve);
      }
      return curve;
    }

    public event Action<List<Point>> CurveUpdated;

    protected virtual void OnCurveUpdated(List<Point> obj)
    {
      Action<List<Point>> handler = CurveUpdated;
      if (handler != null) handler(obj);
    }
  }
}