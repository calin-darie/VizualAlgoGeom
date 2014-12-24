using GeometricElements;

namespace VoronoiAlgorithm
{
  internal interface IVoronoiEventScheduler
  {
    bool IsEventAvailable { get; }

    void AddCircleEvent(VoronoiAlgorithm algorithm,
      IBreakpointTracker breakpoint,
      Point eventPoint,
      Point generatingSite,
      Circle circle);

    void TryDeleteCircleEventOf(IBreakpointTracker rbnd);
    IEvent ExtarctNextEvent();
  }
}