using System;
using System.Collections.Generic;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public class BreakpointTracker : IBreakpointTracker
  {
    public IEdge Edge { get; private set; }

    public Point Breakpoint
    {
      get
      {
        // todo: test(s)
        Point currentBreakpoint = ComputeBreakpoint();

        if (PreviouslyKnownPoint == null ||
            (currentBreakpoint != null && _comparer.Compare(currentBreakpoint, _lastComputedBreakpoint) != 0))
        {
          PreviouslyKnownPoint = _lastComputedBreakpoint;
        }

        _lastComputedBreakpoint = currentBreakpoint;

        return _lastComputedBreakpoint;
      }
    }

    public Point BottomArcSite { get; private set; }
    public Point TopArcSite { get; private set; }

    public void SetEdgeEndpoint(Point endPoint)
    {
      EdgeSide side = IsGoingLeft ? EdgeSide.Left : EdgeSide.Right;
      Point existingEndPoint;
      if (!Edge.EndPoints.TryGetValue(side, out existingEndPoint) || existingEndPoint == null)
      {
        Edge.EndPoints[side] = endPoint;
      }
    }

    public bool IsGoingLeft { get; private set; }
    public Point PreviouslyKnownPoint { get; private set; }
    public Point LeftArcSite { get; private set; }
    public Point RightArcSite { get; private set; }
    Point _lastComputedBreakpoint;
    readonly IComparer<Point> _comparer;

    public BreakpointTracker(
      IComparer<Point> comparer,
      ISweepLine sweepLine,
      IEdge edge,
      Point leftArcSite,
      Point rightArcSite,
      bool isGoingLeft)
    {
      if (edge == null)
      {
        throw new ArgumentNullException("edge");
      }
      if (leftArcSite == null)
      {
        throw new ArgumentNullException("leftArcSite");
      }
      if (rightArcSite == null)
      {
        throw new ArgumentNullException("rightArcSite");
      }
      if (sweepLine == null)
      {
        throw new ArgumentNullException("sweepLine");
      }
      if (comparer == null)
      {
        throw new ArgumentNullException("comparer");
      }


      Edge = edge;

      LeftArcSite = leftArcSite;
      RightArcSite = rightArcSite;

      if (leftArcSite.Y > rightArcSite.Y)
      {
        TopArcSite = leftArcSite;
        BottomArcSite = rightArcSite;
      }
      else
      {
        TopArcSite = rightArcSite;
        BottomArcSite = leftArcSite;
      }

      SweepLine = sweepLine;
      IsGoingLeft = isGoingLeft;
      PreviouslyKnownPoint = Breakpoint;
      _comparer = comparer;
    }

    internal ISweepLine SweepLine { get; private set; }

    Point ComputeBreakpoint()
    {
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (TopArcSite.Y == SweepLine.Y)
      {
        return new Point(TopArcSite.X, GetY(TopArcSite.X, BottomArcSite));
      }
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (BottomArcSite.Y == SweepLine.Y)
      {
        return new Point(BottomArcSite.X, GetY(BottomArcSite.X, TopArcSite));
      }


      double eqCoefXSquared = 1/(TopArcSite.Y - SweepLine.Y) - 1/(BottomArcSite.Y - SweepLine.Y);
      double eqCoefX = 2*
                       (BottomArcSite.X/(BottomArcSite.Y - SweepLine.Y) - TopArcSite.X/(TopArcSite.Y - SweepLine.Y));

      double eqCoefFree =
        (TopArcSite.X*TopArcSite.X)/(TopArcSite.Y - SweepLine.Y)
        - (BottomArcSite.X*BottomArcSite.X)/(BottomArcSite.Y - SweepLine.Y)
        + TopArcSite.Y - BottomArcSite.Y;

      double delta = eqCoefX*eqCoefX - 4*eqCoefXSquared*eqCoefFree;
      double resultX1 = (-eqCoefX + Math.Sqrt(delta))/(2*eqCoefXSquared);
      double resultX2 = (-eqCoefX - Math.Sqrt(delta))/(2*eqCoefXSquared);

      double resultY1 = GetY(resultX1, TopArcSite);
      double resultY2 = GetY(resultX2, TopArcSite);

      Point result1;
      Point result2;
      try
      {
        result1 = new Point(resultX1, resultY1);
      }
      catch
      {
        result1 = null;
      }
      try
      {
        result2 = new Point(resultX2, resultY2);
      }
      catch
      {
        result2 = null;
      }
      if (result1 == null)
      {
        return result2;
      }
      if (result2 == null)
      {
        return result1;
      }
      bool firstIsLeftmost = new FrontLinePointComparer().Compare(result1, result2) < 0;
      return firstIsLeftmost ^ IsGoingLeft
        ? result2
        : result1;
    }

    double GetY(double x, Point focus)
    {
      return (focus.Y + SweepLine.Y)/2
             + (focus.X - x)*(focus.X - x)/(2*(focus.Y - SweepLine.Y));
    }

    public static Point GetIntersectionPoint(IBreakpointTracker el1, IBreakpointTracker el2)
    {
      Point v = el1.Edge.IntersectionPointWith(el2.Edge);

      return v == null || !(CanBreakpointReach(v, el1) && CanBreakpointReach(v, el2)) ? null : v;
    }

    static bool CanBreakpointReach(Point point, IBreakpointTracker breakpointTracker)
    {
      bool rightOfBreakpoint = point.X >= breakpointTracker.Breakpoint.X;
      bool isReachPossible = rightOfBreakpoint ^ breakpointTracker.IsGoingLeft;
      return isReachPossible;
    }

    public override string ToString()
    {
      return string.Format("{{{0}; left: {1}; right: {2}}}", Breakpoint, TopArcSite, BottomArcSite);
    }
  }
}