using System;
using System.Collections.Generic;
using GeometricElements;

namespace VoronoiAlgorithm
{
  internal class FrontLinePointComparer : IComparer<IBreakpointTracker>, IComparer<Point>
  {
    public int Compare(IBreakpointTracker a, IBreakpointTracker b)
    {
      var result = 0;
      // to do: test to ensure comparison by breakpoint stays
      if (IsAnyNull(a.Breakpoint, b.Breakpoint, ref result))
      {
        return result;
      }

      if (IsAnyNull(a.PreviouslyKnownPoint, b.PreviouslyKnownPoint, ref result))
      {
        return result;
      }

      ThenCompareX(a.Breakpoint, b.Breakpoint, ref result);

      ThenCompareX(a.PreviouslyKnownPoint, b.PreviouslyKnownPoint, ref result);

      if (result == 0)
      {
        if (a.IsGoingLeft != b.IsGoingLeft)
        {
          result = a.IsGoingLeft ? -1 : 1;
        }
      }
      return result;
    }

    public int Compare(Point a, Point b)
    {
      var result = 0;

      if (IsAnyNull(a, b, ref result))
      {
        return result;
      }
      ThenCompareX(a, b, ref result);
      ThenCompareY(a, b, ref result);

      return result;
    }

    const int Precision = 4;

    static bool IsAnyNull(Point point1, Point point2, ref int result)
    {
      bool areNull = point1 == null || point2 == null;
      if (areNull)
      {
        result = (point1 == null && point2 == null)
          ? 0
          : (point1 == null) ? 1 : -1;
      }
      return areNull;
    }

    static void ThenCompareX(Point point1, Point point2, ref int result)
    {
      if (result == 0)
      {
        result = Compare(point1.X, point2.X);
      }
    }

    static int Compare(double n1, double n2)
    {
      return Math.Round(n1, Precision).CompareTo(Math.Round(n2, Precision));
    }

    static void ThenCompareY(Point point1, Point point2, ref int result)
    {
      if (result == 0)
      {
        result = Compare(point1.Y, point2.Y);
      }
    }

    public class Limit : IBreakpointTracker
    {
      public Point Breakpoint
      {
        get { return _point; }
      }

      public bool IsGoingLeft
      {
        get { return _isGoingLeft; }
      }

      public Point PreviouslyKnownPoint
      {
        get { return _previouslyKnownPoint; }
      }

      readonly bool _isGoingLeft;
      readonly Point _point;
      readonly Point _previouslyKnownPoint;

      public Limit(Point currentPoint, Point previouslyKnownPoint, bool isGoingLeft)
      {
        if (currentPoint == null)
        {
          throw new ArgumentNullException();
        }
        if (previouslyKnownPoint == null)
        {
          throw new ArgumentNullException();
        }
        _point = currentPoint;
        _isGoingLeft = isGoingLeft;
        _previouslyKnownPoint = previouslyKnownPoint;
      }

      #region not implemented

      public IEdge Edge
      {
        get { throw new NotImplementedException(); }
      }

      public Point LeftArcSite
      {
        get { throw new NotImplementedException(); }
      }

      public Point RightArcSite
      {
        get { throw new NotImplementedException(); }
      }

      public Point TopArcSite
      {
        get { throw new NotImplementedException(); }
      }

      public Point BottomArcSite
      {
        get { throw new NotImplementedException(); }
      }

      public void SetEdgeEndpoint(Point v)
      {
        throw new NotImplementedException();
      }

      #endregion
    }
  }
}