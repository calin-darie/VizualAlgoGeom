using System;
using System.Collections.Generic;
using System.Linq;
using GeometricElements;
using Infrastructure;

namespace SegmentIntersectionAlgorithm
{
  public class SegmentIntersection
  {
    const double Epsilon = 0.01;
    double _sweepLineY;
    readonly EventQueue _eventQueue;
    readonly List<LineSegment> _lineStatus;
    readonly LineSegment[] _segments;

    public SegmentIntersection(LineSegment[] segments)
    {
      _segments = segments;
      _eventQueue = new EventQueue();
      _lineStatus = new List<LineSegment>();
      SetEndpoints();
      InitializeEventQueue();
    }

    double SweepLineY
    {
      get { return _sweepLineY; }
      set
      {
        _sweepLineY = value;
        OnSweepLineUpdated(value);
      }
    }

    void SetEndpoints()
    {
      foreach (LineSegment segment in _segments.Where(s => s.FirstPoint.Compare(s.SecondPoint) > 0))
      {
        Point aux = segment.FirstPoint;
        segment.FirstPoint = segment.SecondPoint;
        segment.SecondPoint = aux;
      }
    }

    void InitializeEventQueue()
    {
      foreach (LineSegment segment in _segments)
      {
        var segmentsHavingCurrentEventPointAsUpperEndEndpoint = new EventPoint(segment.FirstPoint);
        segmentsHavingCurrentEventPointAsUpperEndEndpoint.CorrespondingSegments.Add(segment);
        _eventQueue.Insert(segmentsHavingCurrentEventPointAsUpperEndEndpoint, EventType.UpperEndpoint);

        var segmentsHavingCurrentEventPointAsLowerEndEndpoint = new EventPoint(segment.SecondPoint);
        _eventQueue.Insert(segmentsHavingCurrentEventPointAsLowerEndEndpoint, EventType.LowerEndpoint);
      }
    }

    public event Action<double> SweepLineInitialized;
    public event Action<double> SweepLineUpdated;
    public event Action<Point> HandleEventPointBegan;
    public event Action HandleEventPointEnded;
    public event Action<Point> NewEventFound;
    public event Action<LineSegment, LineSegment> TestingSegmentIntersectionBegan;
    public event Action TestingSegmentIntersectionEnded;
    public event Action<List<LineSegment>> RestoringSegmentsContainingEventPoint;
    public event Action<List<LineSegment>> RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus;
    public event Action<List<LineSegment>> RemovingSegmentsContainingEventPointFromLineStatus;
    public event Action<Intersection> IntersectionPointFound;

    public void FindIntersections()
    {
      OnSweepLineInitialized(_eventQueue.Peek().Point.Y);
      while (!_eventQueue.IsEmpty())
      {
        EventPoint nextEventPoint = _eventQueue.NextEvent();
        OnHandleEventPointBegan(nextEventPoint.Point);

        SweepLineY = nextEventPoint.Point.Y;
        HandleEventPoint(nextEventPoint);

        OnHandleEventPointEnded();
      }
    }

    void HandleEventPoint(EventPoint point)
    {
      List<LineSegment> segmentsHavingCurrentEventPointAsUpperEnd = point.CorrespondingSegments;
      var segmentsContainingCurrentEventPoint = new List<LineSegment>();
      var segmentsHavingCurrentEventPointAsLowerEnd = new List<LineSegment>();
      int count = _lineStatus.Count;
      for (var i = 0; i < count; i++)
      {
        LineSegment crtSegment = _lineStatus[i];
        if (crtSegment.SegmentContains(point.Point))
        {
          if (crtSegment.SecondPoint.Equals(point.Point))
            segmentsHavingCurrentEventPointAsLowerEnd.Add(crtSegment);
          else
            segmentsContainingCurrentEventPoint.Add(crtSegment);
        }
      }
      if ((segmentsHavingCurrentEventPointAsUpperEnd.Count +
           segmentsContainingCurrentEventPoint.Count +
           segmentsHavingCurrentEventPointAsLowerEnd.Count) > 1)
      {
        var all = new List<LineSegment>();
        all.AddRange(segmentsHavingCurrentEventPointAsUpperEnd);
        all.AddRange(segmentsHavingCurrentEventPointAsLowerEnd);
        all.AddRange(segmentsContainingCurrentEventPoint);
        var intersection = new Intersection(point.Point, all);
        OnIntersectionPointFound(intersection);
      }
      foreach (LineSegment l in segmentsContainingCurrentEventPoint)
      {
        _lineStatus.Remove(l);
        OnRemovingSegmentsContainingEventPointFromLineStatus(_lineStatus);
      }
      foreach (LineSegment l in segmentsHavingCurrentEventPointAsLowerEnd)
      {
        _lineStatus.Remove(l);
        OnRemovingSegmentsHavingEventPointAsLowerEndFromLineStatus(_lineStatus);
      }
      InsertInLineStatus(segmentsHavingCurrentEventPointAsUpperEnd, segmentsContainingCurrentEventPoint,
        point.Point);
      if (segmentsHavingCurrentEventPointAsUpperEnd.Count == 0 && segmentsContainingCurrentEventPoint.Count == 0)
      {
        var sl = new LineSegment();
        var sr = new LineSegment();
        if (FindNeighbours(point.Point, ref sl, ref sr))
          FindNewEvent(sl, sr, point.Point);
      }
      else
      {
        LineSegment sl = FindTheLeftmostSegment(segmentsHavingCurrentEventPointAsUpperEnd,
          segmentsContainingCurrentEventPoint);
        LineSegment sr = FindRightmostSegment(segmentsHavingCurrentEventPointAsUpperEnd,
          segmentsContainingCurrentEventPoint);
        LineSegment sll;
        LineSegment srr;
        if (FindLeftSegment(sl, out sll))
          FindNewEvent(sll, sl, point.Point);
        if (FindRightSegment(sr, out srr))
          FindNewEvent(sr, srr, point.Point);
      }
    }

    bool FindRightSegment(LineSegment sr, out LineSegment srr)
    {
      if (_lineStatus.Count == 0 || _lineStatus.Last().Equals(sr))
      {
        srr = new LineSegment();
        return false;
      }

      srr = _lineStatus.ElementAt(_lineStatus.IndexOf(sr) + 1);
      return true;
    }

    bool FindLeftSegment(LineSegment sl, out LineSegment leftSegment)
    {
      leftSegment = new LineSegment();
      if (_lineStatus.Count == 0 || _lineStatus.First().Equals(sl))
        return false;
      leftSegment = _lineStatus.ElementAt(_lineStatus.IndexOf(sl) - 1);
      return true;
    }

    LineSegment FindRightmostSegment(List<LineSegment> segmentsHavingCurrentEventPointAsUpperEnd,
      List<LineSegment> segmentsContainingCurrentEventPoint)
    {
      int count = _lineStatus.Count;
      for (int i = count - 1; i >= 0; i--)
        if (segmentsHavingCurrentEventPointAsUpperEnd.Contains(_lineStatus[i]) ||
            segmentsContainingCurrentEventPoint.Contains(_lineStatus[i]))
          return _lineStatus[i];
      return new LineSegment();
    }

    LineSegment FindTheLeftmostSegment(List<LineSegment> segmentsHavingCurrentEventPointAsUpperEnd,
      List<LineSegment> segmentsContainingCurrentEventPoint)
    {
      int count = _lineStatus.Count;
      for (var i = 0; i < count; i++)
        if (segmentsHavingCurrentEventPointAsUpperEnd.Contains(_lineStatus[i]) ||
            segmentsContainingCurrentEventPoint.Contains(_lineStatus[i]))
          return _lineStatus[i];
      return new LineSegment();
    }

    bool FindNeighbours(Point point, ref LineSegment sl, ref LineSegment sr)
    {
      int count = _lineStatus.Count;
      if (count == 0)
        return false;
      double x;
      _lineStatus.First().Line.IntersectHorizontal(point.Y, out x);
      if (point.X < x)
        return false;
      _lineStatus.Last().Line.IntersectHorizontal(point.Y, out x);
      if (x < point.X)
        return false;
      for (var i = 0; i < count - 1; i++)
      {
        _lineStatus[i].Line.IntersectHorizontal(point.Y, out x);
        if (x < point.X)
        {
          sl = _lineStatus[i];
          sr = _lineStatus[i + 1];
          return true;
        }
      }
      return false;
    }

    void InsertInLineStatus(List<LineSegment> segmentsHavingCurrentEventPointAsUpperEnd,
      List<LineSegment> segmentsContainingCurrentEventPoint, Point eventPoint)
    {
      foreach (LineSegment s in segmentsHavingCurrentEventPointAsUpperEnd)
        InsertInLineStatus(s, eventPoint.Y - Epsilon);
      foreach (LineSegment s in segmentsContainingCurrentEventPoint)
        InsertInLineStatus(s, eventPoint.Y - Epsilon);
    }

    void InsertInLineStatus(LineSegment segment, double y)
    {
      int count = _lineStatus.Count;
      var i = 0;
      double lx;
      segment.Line.IntersectHorizontal(y, out lx);
      if (count > 0)
      {
        double x;
        do
        {
          _lineStatus[i].Line.IntersectHorizontal(y, out x);
          i++;
        } while (i < count && x < lx);
        if (x < lx)
          _lineStatus.Add(segment);
        else
          _lineStatus.Insert(i - 1, segment);
      }
      else
        _lineStatus.Add(segment);
      OnRestoringSegmentsContainingEventPoint(_lineStatus);
    }

    void FindNewEvent(LineSegment s1, LineSegment s2, Point eventPoint)
    {
      var lTested1 = new LineSegment(s1.FirstPoint, s1.SecondPoint);
      var lTested2 = new LineSegment(s2.FirstPoint, s2.SecondPoint);

      OnTestingSegmentIntersectionBegan(lTested1, lTested2);

      var intersection = new Point();
      if (s1.DoLineSegmentsIntersect(s2, ref intersection) &&
          (intersection.Y < SweepLineY || (Numbers.EqualTolerant(intersection.Y, SweepLineY) && intersection.X.GreaterThanTolerant(eventPoint.X))))
      {
        _eventQueue.Insert(new EventPoint(intersection), EventType.Intersection);
        OnNewEventFound(intersection);
      }

      OnTestingSegmentIntersectionEnded();
    }

    protected virtual void OnSweepLineInitialized(double y)
    {
      Action<double> handler = SweepLineInitialized;
      if (handler != null) handler(y);
    }

    protected virtual void OnHandleEventPointEnded()
    {
      Action handler = HandleEventPointEnded;
      if (handler != null) handler();
    }

    protected virtual void OnHandleEventPointBegan(Point point)
    {
      Action<Point> handler = HandleEventPointBegan;
      if (handler != null) handler(point);
    }

    protected virtual void OnSweepLineUpdated(double obj)
    {
      Action<double> handler = SweepLineUpdated;
      if (handler != null) handler(obj);
    }

    protected virtual void OnIntersectionPointFound(Intersection obj)
    {
      Action<Intersection> handler = IntersectionPointFound;
      if (handler != null) handler(obj);
    }

    protected virtual void OnRemovingSegmentsContainingEventPointFromLineStatus(List<LineSegment> obj)
    {
      Action<List<LineSegment>> handler = RemovingSegmentsContainingEventPointFromLineStatus;
      if (handler != null) handler(obj);
    }

    protected virtual void OnRemovingSegmentsHavingEventPointAsLowerEndFromLineStatus(List<LineSegment> obj)
    {
      Action<List<LineSegment>> handler = RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus;
      if (handler != null) handler(obj);
    }

    protected virtual void OnRestoringSegmentsContainingEventPoint(List<LineSegment> obj)
    {
      Action<List<LineSegment>> handler = RestoringSegmentsContainingEventPoint;
      if (handler != null) handler(obj);
    }

    protected virtual void OnTestingSegmentIntersectionEnded()
    {
      Action handler = TestingSegmentIntersectionEnded;
      if (handler != null) handler();
    }

    protected virtual void OnTestingSegmentIntersectionBegan(LineSegment arg1, LineSegment arg2)
    {
      Action<LineSegment, LineSegment> handler = TestingSegmentIntersectionBegan;
      if (handler != null) handler(arg1, arg2);
    }

    protected virtual void OnNewEventFound(Point p)
    {
      Action<Point> handler = NewEventFound;
      if (handler != null) handler(p);
    }
  }
}