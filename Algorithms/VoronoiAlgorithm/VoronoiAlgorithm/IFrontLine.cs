using System.Collections.Generic;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public interface IFrontLine
  {
    IEnumerable<IBreakpointTracker> Breakpoints { get; }
    Point FirstArcGenerator { get; }
    void Add(IBreakpointTracker bisector);
    void Remove(IBreakpointTracker lbnd);
    IBreakpointTracker GetBreakpointLeftOf(Point newSite);

    void PinpointSite(Point newSite, out Point siteGeneratingArcBelow, out IBreakpointTracker originalBreakpointLeft,
      out IBreakpointTracker originalBreakpointRight);

    void PinpointBreakpoint(IBreakpointTracker leftBreakpoint, out IBreakpointTracker llbnd, out IBreakpointTracker rbnd,
      out IBreakpointTracker rrbnd);
  }
}