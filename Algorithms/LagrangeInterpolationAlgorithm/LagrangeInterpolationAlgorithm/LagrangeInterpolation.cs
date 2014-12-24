using System.Collections.Generic;
using System.Linq;
using GeometricElements;

namespace LagrangeInterpolationAlgorithm
{
  public class LagrangeInterpolation
  {
    List<List<double>> _operatorList;
    List<double> Xs;
    List<double> Ys;
    readonly Point[] _points;

    public LagrangeInterpolation(Point[] points)
    {
      _points = points;
    }

    public List<Point> Lagrange()
    {
      _operatorList = new List<List<double>>();
      Xs = new List<double>();

      if (_points.Length > 0)
      {
        //compute lagrange operator for each X coordinate
        for (int x = -1000; x < 1500; x++)
        {
          List<double> lagrangeOperator = Enumerable.Repeat(1d, _points.Length).ToList();

          for (var i = 0; i < lagrangeOperator.Count; i++)
          {
            for (var k = 0; k < _points.Length; k++)
            {
              if (i != k)
                lagrangeOperator[i] *= (x - _points[k].X)/(_points[i].X - _points[k].X);
            }
          }
          _operatorList.Add(lagrangeOperator);
          Xs.Add(x);
        }

        //Computing the Polynomial P(x) which is y in our curve
        Ys = new List<double>(_operatorList.Select(o => _points.Select((p, i) =>
          o[i]*p.Y)
          .Sum()));
      }

      List<Point> lagrange = Xs.Select((x, i) => new Point(x, Ys[i])).ToList();
      return lagrange;
    }
  }
}