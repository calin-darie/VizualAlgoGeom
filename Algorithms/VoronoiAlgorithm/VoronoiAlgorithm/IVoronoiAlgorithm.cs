using System;
using System.Collections.Generic;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public interface IVoronoiAlgorithm
  {
    double SweepLineY { get; }
    IFrontLine FrontLine { get; }
    IEnumerable<IEdge> Edges { get; }
    event Action<Point> SiteEvent;
    event Action<Circle> CircleEvent;
    event Action<IBreakpointTracker> BreakpointAdded;
    event Action<IBreakpointTracker> BreakpointRemoved;
    event Action<Edge> EdgeDiscovered;
    event Action<IEdge, IEdge, Circle, Point> CircleEventScheduled;
    IFrontLine Run(IEnumerable<Point> sites);
  }
}