using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using C5;
using GeometricElements;

namespace VoronoiAlgorithm
{
  internal class VoronoiEventScheduler : IVoronoiEventScheduler
  {
    public void AddCircleEvent(VoronoiAlgorithm algorithm, IBreakpointTracker breakpoint,
      Point eventPoint, Point generatingSite, Circle circle)
    {
      IPriorityQueueHandle<IEvent> handle = null;
      bool isAddedSuccessfully = _eventQueue.Add(
        ref handle,
        new CircleEvent(algorithm)
        {
          BreakpointTracker = breakpoint,
          Point = eventPoint,
          GeneratingSite = generatingSite,
          Circle = circle
        });
      if (isAddedSuccessfully && handle != null)
      {
        _circleEventHandle[breakpoint] = handle;
      }
    }

    public void TryDeleteCircleEventOf(IBreakpointTracker breakpointTracker)
    {
      if (_circleEventHandle.Keys.Contains(breakpointTracker))
      {
        try
        {
          _eventQueue.Delete(_circleEventHandle[breakpointTracker]);
        }
        catch (Exception ex)
        {
          Trace.WriteLine(ex);
        }
      }
    }

    public bool IsEventAvailable
    {
      get { return !_eventQueue.IsEmpty; }
    }

    public IEvent ExtarctNextEvent()
    {
      return _eventQueue.DeleteMin();
    }

    readonly Dictionary<IBreakpointTracker, IPriorityQueueHandle<IEvent>> _circleEventHandle;
    readonly IntervalHeap<IEvent> _eventQueue;

    public VoronoiEventScheduler(IEnumerable<SiteEvent> events)
    {
      _eventQueue = new IntervalHeap<IEvent>(new EventComparer());
      _circleEventHandle = new Dictionary<IBreakpointTracker, IPriorityQueueHandle<IEvent>>();

      _eventQueue.AddAll(events);
    }
  }
}