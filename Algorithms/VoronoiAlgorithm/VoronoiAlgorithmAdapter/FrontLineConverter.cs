/*
 * more than one parabolic arc for the same site - done
 * more than one breakpoint at the same point - done
 * 
 * 
 * 
 * wrong breakpoint marked for deletion on circle event? yes.
 * wrong site assigned to breakpoint tracker?
 * 
 * find another way to express edge sides. left and right need an orientation besides a direction
 */

using System.Collections.Generic;
using System.Linq;
using VoronoiAlgorithm;
using VoronoiAlgorithmAdapter.Geometry;

namespace VoronoiAlgorithmAdapter
{
  internal class FrontLineConverter
  {
    double _sweepLineY;

    internal ParabolicArc[] GetFrontLineParabolicArcs(IVoronoiAlgorithm algorithm)
    {
      var parabolas = new List<ParabolicArc>();
      _sweepLineY = algorithm.SweepLineY;
      IBreakpointTracker previousBreakpointTracker = null;

      foreach (IBreakpointTracker tracker in algorithm.FrontLine.Breakpoints)
      {
        parabolas.Add(
          new ParabolicArc
          {
            Focus = tracker.LeftArcSite,
            DirectrixY = _sweepLineY,
            XLeft = previousBreakpointTracker != null
              ? previousBreakpointTracker.Breakpoint.X
              : double.MinValue,
            XRight = tracker.Breakpoint.X
          }
          );

        previousBreakpointTracker = tracker;
      }

      if (previousBreakpointTracker != null)
      {
        parabolas.Add(
          new ParabolicArc
          {
            Focus = previousBreakpointTracker.RightArcSite,
            DirectrixY = _sweepLineY,
            XLeft = previousBreakpointTracker.Breakpoint.X,
            XRight = double.MaxValue
          }
          );
      }

      return parabolas.Any()
        ? parabolas.ToArray()
        : algorithm.FrontLine.FirstArcGenerator != null
          ? new[]
          {
            new ParabolicArc
            {
              DirectrixY = _sweepLineY,
              Focus = algorithm.FrontLine.FirstArcGenerator
            }
          }
          : new ParabolicArc[0];
    }
  }
}