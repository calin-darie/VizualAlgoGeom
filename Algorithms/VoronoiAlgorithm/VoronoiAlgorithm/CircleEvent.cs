using System;
using GeometricElements;

namespace VoronoiAlgorithm
{
  internal class CircleEvent : IEvent
  {
    public Point Point { get; set; }

    public void Fire()
    {
      _algorithm.Handle(this);
    }

    readonly VoronoiAlgorithm _algorithm;

    internal CircleEvent(VoronoiAlgorithm algorithm)
    {
      if (algorithm == null)
      {
        throw new ArgumentNullException();
      }

      _algorithm = algorithm;
    }

    public IBreakpointTracker BreakpointTracker { get; set; }
    public Point GeneratingSite { get; set; }
    public Circle Circle { get; set; }
  }
}