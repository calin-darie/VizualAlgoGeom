using System;
using System.Collections.Generic;
using System.Linq;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public class FrontLine : IFrontLine
  {
    public void Add(IBreakpointTracker newBreakPoint)
    {
      _breakpointTrackers.Add(newBreakPoint);
    }

    public void Remove(IBreakpointTracker breakpoint)
    {
      _breakpointTrackers.Remove(breakpoint);
    }

    public void PinpointSite(Point newSite, out Point siteGeneratingArcBelow,
      out IBreakpointTracker originalBreakpointLeft, out IBreakpointTracker originalBreakpointRight)
    {
      originalBreakpointLeft = GetBreakpointLeftOf(newSite);
      originalBreakpointRight = GetBreakpointRightOf(newSite);

      siteGeneratingArcBelow =
        originalBreakpointLeft != null
          ? originalBreakpointLeft.RightArcSite
          : originalBreakpointRight != null
            ? originalBreakpointRight.LeftArcSite
            : FirstArcGenerator;

      if (siteGeneratingArcBelow == null)
      {
        FirstArcGenerator = newSite;
      }
    }

    public void PinpointBreakpoint(IBreakpointTracker key,
      out IBreakpointTracker previous, out IBreakpointTracker next, out IBreakpointTracker secondNext)
    {
      if (!_breakpointTrackers.Any())
      {
        previous = null;
        next = null;
        secondNext = null;
        return;
      }


      SortedSet<IBreakpointTracker> rightSubset =
        _breakpointTrackers.GetViewBetween(key, _breakpointTrackers.Max);
      next = rightSubset.Skip(1).FirstOrDefault();
      secondNext = rightSubset.Skip(2).FirstOrDefault();


      SortedSet<IBreakpointTracker> leftSubset =
        _breakpointTrackers.GetViewBetween(_breakpointTrackers.Min, key);
      previous = leftSubset.Reverse().Skip(1).LastOrDefault();
    }

    public IBreakpointTracker GetBreakpointLeftOf(Point site)
    {
      if (!_breakpointTrackers.Any())
      {
        return null;
      }

      var siteComparableToBreakpoints = new FrontLinePointComparer.Limit(
        site, new Point(double.MaxValue, double.MaxValue),
        isGoingLeft: false);
      IBreakpointTracker leftmostBreakpoint = _breakpointTrackers.Min;
      if (_breakpointComparer.Compare(siteComparableToBreakpoints, leftmostBreakpoint) < 0)
      {
        return null;
      }

      SortedSet<IBreakpointTracker> leftSubset =
        _breakpointTrackers.GetViewBetween(leftmostBreakpoint, siteComparableToBreakpoints);

      IBreakpointTracker result = leftSubset.Max;

      return result;
    }

    public IEnumerable<IBreakpointTracker> Breakpoints
    {
      get { return _breakpointTrackers.ToArray(); }
    }

    public Point FirstArcGenerator { get; private set; }
    readonly IComparer<IBreakpointTracker> _breakpointComparer;
    readonly SortedSet<IBreakpointTracker> _breakpointTrackers;

    public FrontLine(IComparer<IBreakpointTracker> pointComparer)
    {
      if (pointComparer == null)
      {
        throw new ArgumentNullException("pointComparer");
      }
      _breakpointComparer = pointComparer;
      _breakpointTrackers = new SortedSet<IBreakpointTracker>(_breakpointComparer);
    }

    IBreakpointTracker GetBreakpointRightOf(Point site)
    {
      if (!_breakpointTrackers.Any())
      {
        return null;
      }

      var siteComparableToBreakpoints = new FrontLinePointComparer.Limit(
        site, new Point(double.MinValue, double.MinValue),
        isGoingLeft: true);
      IBreakpointTracker rightmostBreakpoint = _breakpointTrackers.Max;
      if (_breakpointComparer.Compare(siteComparableToBreakpoints, rightmostBreakpoint) > 0)
      {
        return null;
      }

      SortedSet<IBreakpointTracker> rightSubset =
        _breakpointTrackers.GetViewBetween(siteComparableToBreakpoints, rightmostBreakpoint);

      IBreakpointTracker result = rightSubset.Min;

      return result;
    }
  }
}