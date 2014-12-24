using System.Collections.Generic;
using System.Linq;

namespace SegmentIntersectionAlgorithm
{
  public enum EventType
  {
    Intersection,
    LowerEndpoint,
    UpperEndpoint
  }

  internal class EventQueue
  {
    readonly List<EventPoint> _eventPointQueue;

    public EventQueue()
    {
      _eventPointQueue = new List<EventPoint>();
    }

    EventPoint NextEventPoint
    {
      get
      {
        EventPoint nextEventPoint = _eventPointQueue.First();
        return nextEventPoint;
      }
    }

    public EventPoint NextEvent()
    {
      EventPoint nextEventPoint = NextEventPoint;
      _eventPointQueue.Remove(nextEventPoint);
      return nextEventPoint;
    }

    public EventPoint Peek()
    {
      return NextEventPoint;
    }

    public void Insert(EventPoint newEventPoint, EventType eType)
    {
      var found = false;
      var inserted = false;
      int count = _eventPointQueue.Count;

      var index = 0;
      while (index < count && !found)
      {
        EventPoint eventPoint = _eventPointQueue.ElementAt(index);
        if (eventPoint.Point.Equals(newEventPoint.Point))
        {
          if (eType == EventType.UpperEndpoint)
            eventPoint.CorrespondingSegments.AddRange(newEventPoint.CorrespondingSegments);
          found = true;
          inserted = true;
        }
        else if (newEventPoint.Point.Compare(eventPoint.Point) < 0)
        {
          found = true;
        }
        else
          index++;
      }
      if (!inserted)
      {
        _eventPointQueue.Insert(index, newEventPoint);
      }
    }

    public bool IsEmpty()
    {
      return (_eventPointQueue.Count == 0);
    }
  }
}