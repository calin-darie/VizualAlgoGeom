using System.Collections.Generic;
using GeometricElements;
using Infrastructure;

namespace VoronoiAlgorithm
{
  internal class EventComparer : IComparer<IEvent>
  {
    public int Compare(IEvent p, IEvent q)
    {
      int result = Compare(p.Point, q.Point);
      if (result == 0)
      {
        if (p is SiteEvent && q is CircleEvent)
          result = -1;
        else if (q is SiteEvent && p is CircleEvent)
          result = 1;
      }

      return result;
    }

    public int Compare(Point p, Point q)
    {
      if (p.Y.GreaterThanTolerant(q.Y)) return 1;
      if (q.Y.GreaterThanTolerant(p.Y)) return -1;
      return ComparePointsOnTheSameHorizontal(p, q);
    }

    private int ComparePointsOnTheSameHorizontal(Point p, Point q)
    {
      if (p.X.GreaterThanTolerant(q.X)) return 1;
      if (q.X.GreaterThanTolerant(p.X)) return -1;
      return 0;
    }
  }
}