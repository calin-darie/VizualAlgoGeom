using System;
using System.Collections.Generic;
using System.Linq;
using GeometricElements;

namespace GrahamScanAlgorithm
{
  public class GrahamScan
  {
    readonly Point[] _points;

    public GrahamScan(Point[] points)
    {
      _points = points;
    }

    public IEnumerable<Point> PointsSortedLexicographically
    {
      get { return _points.OrderBy(q => q.X).ThenBy(q => q.Y).ToArray(); }
    }

    /* Input:  three points P0, P1, and P2
     * Return: >0 for P2 left of the line through P0 and P1
     *         =0 for P2 on the line
     *         <0 for P2 right of the line*/

    public double IsLeft(Point p0, Point p1, Point p2)
    {
      return p1.X*p2.Y + p0.X*p1.Y + p2.X*p0.Y
             - p1.X*p0.Y - p2.X*p1.Y - p0.X*p2.Y;
    }

    public List<Point> Graham()
    {
      List<Point> upperHull = GetHalfHull(PointsSortedLexicographically,
        OnUpperHullPointInserted, OnUpperHullPointRemoved);

      List<Point> lowerHull = GetHalfHull(PointsSortedLexicographically.Reverse(),
        OnLowerHullPointInserted, OnLowerHullPointRemoved);

      if (upperHull.Count > 0)
        upperHull.RemoveAt(0);

      if (upperHull.Count > 0)
        upperHull.RemoveAt(upperHull.Count - 1);

      var hull = new List<Point>(upperHull.Concat(lowerHull));
      return hull;
    }

    List<Point> GetHalfHull(IEnumerable<Point> points,
      Action<List<Point>> pointInserted,
      Action<List<Point>> pointRemoved)
    {
      var halfHull = new List<Point>();
      foreach (Point point in points)
      {
        halfHull.Insert(0, point);
        pointInserted(halfHull);
        while (halfHull.Count > 2 && IsLeft(halfHull.ElementAt(2), halfHull.ElementAt(1), halfHull.ElementAt(0)) > 0)
        {
          // if we're here, it means 
          // there were at least three points to find the convex hull of
          // (_points.Length >= 3)
          halfHull.RemoveAt(1);
          pointRemoved(halfHull);
        }
      }
      return halfHull;
    }

    public event Action<List<Point>> UpperHullPointInserted;

    protected virtual void OnUpperHullPointInserted(List<Point> upperHull)
    {
      Action<List<Point>> handler = UpperHullPointInserted;
      if (handler != null) handler(upperHull);
    }

    public event Action<List<Point>> LowerHullPointInserted;

    protected virtual void OnLowerHullPointInserted(List<Point> lowerHull)
    {
      Action<List<Point>> handler = LowerHullPointInserted;
      if (handler != null) handler(lowerHull);
    }

    public event Action<List<Point>> UpperHullPointRemoved;

    protected virtual void OnUpperHullPointRemoved(List<Point> obj)
    {
      Action<List<Point>> handler = UpperHullPointRemoved;
      if (handler != null) handler(obj);
    }

    public event Action<List<Point>> LowerHullPointRemoved;

    protected virtual void OnLowerHullPointRemoved(List<Point> obj)
    {
      Action<List<Point>> handler = LowerHullPointRemoved;
      if (handler != null) handler(obj);
    }
  }
}