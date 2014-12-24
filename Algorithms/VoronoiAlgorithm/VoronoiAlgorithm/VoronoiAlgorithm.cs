using System;
using System.Collections.Generic;
using System.Linq;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public class VoronoiAlgorithm : IVoronoiAlgorithm
  {
    public event Action<Point> SiteEvent;
    public event Action<Circle> CircleEvent;

    public double SweepLineY
    {
      get { return _sweepLine.Y; }
    }

    public IFrontLine Run(IEnumerable<Point> sites)
    {
      // seed event queue with site events; sorting is implicit
      _eventScheduler = new VoronoiEventScheduler(sites.Select(site => new SiteEvent(this, site)));

      // main loop
      while (_eventScheduler.IsEventAvailable)
      {
        IEvent currentEvent = _eventScheduler.ExtarctNextEvent();

        _sweepLine.AdvanceTo(currentEvent);

        currentEvent.Fire();
      }

      return _frontLine;
    }

    public IEnumerable<IEdge> Edges
    {
      get { return _edges; }
    }

    public event Action<Edge> EdgeDiscovered;
    public event Action<IEdge, IEdge, Circle, Point> CircleEventScheduled;

    public IFrontLine FrontLine
    {
      get { return _frontLine; }
    }

    IVoronoiEventScheduler _eventScheduler;
    readonly HashSet<IEdge> _edges = new HashSet<IEdge>();
    readonly IFrontLine _frontLine;
    readonly FrontLinePointComparer _pointComparer;
    readonly SweepLine _sweepLine;

    public VoronoiAlgorithm()
    {
      _sweepLine = new SweepLine();
      _pointComparer = new FrontLinePointComparer();
      _frontLine = new FrontLine(_pointComparer);
    }

    void DoAsserts()
    {
      IBreakpointTracker previousBreakpointTracker = null;

      foreach (IBreakpointTracker tracker in _frontLine.Breakpoints)
      {
        if (previousBreakpointTracker == null)
        {
          previousBreakpointTracker = tracker;
          continue;
        }

        //bool isContinuous = Equals(tracker.TopArcSite, previousBreakpointTracker.BottomArcSite);
        //bool isMonotonous = _pointComparer.Compare(tracker, previousBreakpointTracker) >= 0;

        previousBreakpointTracker = tracker;
      }
    }

    internal void Handle(SiteEvent siteEvent)
    {
      Point newSite = siteEvent.Point;
      FireSiteEvent(newSite);

      IBreakpointTracker originalBreakpointLeft;
      IBreakpointTracker originalBreakpointRight;
      Point siteGeneratingArcBelow;
      _frontLine.PinpointSite(newSite, out siteGeneratingArcBelow, out originalBreakpointLeft,
        out originalBreakpointRight);

      if (siteGeneratingArcBelow == null)
      {
        return;
      }

      if (originalBreakpointLeft != null)
      {
        // delete false alarm
        _eventScheduler.TryDeleteCircleEventOf(originalBreakpointLeft);
      }

      Edge newEdge = CreateBisectorOf(newSite, siteGeneratingArcBelow);
      _edges.Add(newEdge);

      FireEdgeDiscovered(newEdge);
      var newBreakpointLeft = new BreakpointTracker(
        _pointComparer,
        _sweepLine,
        newEdge, siteGeneratingArcBelow, newSite,
        isGoingLeft: true);
      AddBreakpoint(newBreakpointLeft);


      var newBreakpointRight = new BreakpointTracker(
        _pointComparer,
        _sweepLine,
        newEdge, newSite, siteGeneratingArcBelow,
        isGoingLeft: false);
      AddBreakpoint(newBreakpointRight);

      CheckCircleEvent(newSite, originalBreakpointLeft, newBreakpointLeft);
      CheckCircleEvent(newSite, newBreakpointRight, originalBreakpointRight);

      DoAsserts();
    }

    Edge CreateBisectorOf(Point site1, Point site2)
    {
      Point leftSite;
      Point rightSite;
      if (_pointComparer.Compare(site2, site1) < 0)
      {
        leftSite = site2;
        rightSite = site1;
      }
      else
      {
        leftSite = site1;
        rightSite = site2;
      }
      Edge newEdge = Edge.BisectorOf(leftSite, rightSite);
      return newEdge;
    }

    void FireEdgeDiscovered(Edge edge)
    {
      Action<Edge> edgeDiscoveredHandler = EdgeDiscovered;
      if (edgeDiscoveredHandler != null)
      {
        edgeDiscoveredHandler(edge);
      }
    }

    void FireSiteEvent(Point newSite)
    {
      Action<Point> siteEventHandlers = SiteEvent;
      if (siteEventHandlers != null)
      {
        siteEventHandlers(newSite);
      }
    }

    void CheckCircleEvent(Point site, IBreakpointTracker breakpointLeft, IBreakpointTracker breakpointRight)
    {
      if (breakpointLeft == null || breakpointRight == null)
      {
        return;
      }

      Point pointLeftCircleEvent = BreakpointTracker.GetIntersectionPoint(breakpointLeft, breakpointRight);
      if (pointLeftCircleEvent == null)
      {
        return;
      }

      Circle circle = Circle.FindByCenterAndPointOnCircumference(pointLeftCircleEvent, site);
      var eventPoint = new Point(pointLeftCircleEvent.X, pointLeftCircleEvent.Y + circle.Radius);

      _eventScheduler.AddCircleEvent(this, breakpointLeft, eventPoint, site, circle);
      FireCircleEventScheduled(breakpointLeft.Edge, breakpointRight.Edge, circle, eventPoint);
    }

    void FireCircleEventScheduled(IEdge edge1, IEdge edge2, Circle circle, Point eventPoint)
    {
      Action<IEdge, IEdge, Circle, Point> circleEventScheduledHandler = CircleEventScheduled;
      if (circleEventScheduledHandler != null)
      {
        circleEventScheduledHandler(edge1, edge2, circle, eventPoint);
      }
    }

    internal void Handle(CircleEvent circleEvent)
    {
      DoAsserts();
      FireCircleEvent(circleEvent.Circle);

      IBreakpointTracker leftBreakpoint = circleEvent.BreakpointTracker;
      IBreakpointTracker secondLeftBreakpoint;
      IBreakpointTracker rightBreakpoint;
      IBreakpointTracker secondRightBreakpoint;
      _frontLine.PinpointBreakpoint(leftBreakpoint, out secondLeftBreakpoint, out rightBreakpoint,
        out secondRightBreakpoint);

      Point leftSite = leftBreakpoint.LeftArcSite;
      Point rightSite = rightBreakpoint.RightArcSite;

      Point edgeIntersectionPoint = leftBreakpoint.Breakpoint; // == rightBreakpoint.Breakpoint

      SetEndAndRemoveBreakpoint(leftBreakpoint, edgeIntersectionPoint);
      SetEndAndRemoveBreakpoint(rightBreakpoint, edgeIntersectionPoint);

      Edge newEdge = CreateBisectorOf(leftSite, rightSite);
      _edges.Add(newEdge);
      // todo: decide for vertical case

      Point middleSite = leftBreakpoint.RightArcSite; // assert: should be equal to rightBreakpoint.LeftArcSite

      // todo: test: left on top => breakpoint going right
      bool isNewRayGoingLeft = middleSite.X > Line.FootOfPerpendicularFrom(middleSite, leftSite, rightSite).X;
      var bisectorTracker =
        new BreakpointTracker(
          _pointComparer, _sweepLine, newEdge, leftSite, rightSite, isNewRayGoingLeft);
      AddBreakpoint(bisectorTracker);

      //test?
      newEdge.EndPoints[isNewRayGoingLeft ? EdgeSide.Right : EdgeSide.Left] = edgeIntersectionPoint;

      CheckCircleEvent(leftSite, secondLeftBreakpoint, bisectorTracker);
      CheckCircleEvent(leftSite, bisectorTracker, secondRightBreakpoint);

      DoAsserts();
    }

    void FireCircleEvent(Circle circle)
    {
      Action<Circle> circleEventHandler = CircleEvent;
      if (circleEventHandler != null)
      {
        circleEventHandler(circle);
      }
    }

    #region frontline operation

    void AddBreakpoint(BreakpointTracker newBreakpoint)
    {
      _frontLine.Add(newBreakpoint);
      FireBreakpointAdded(newBreakpoint);
    }

    public event Action<IBreakpointTracker> BreakpointAdded;

    void FireBreakpointAdded(BreakpointTracker breakpoint)
    {
      Action<IBreakpointTracker> breakpointAddedHandlers = BreakpointAdded;
      if (breakpointAddedHandlers != null)
      {
        breakpointAddedHandlers(breakpoint);
      }
    }

    void SetEndAndRemoveBreakpoint(IBreakpointTracker breakpoint, Point v)
    {
      breakpoint.SetEdgeEndpoint(v);

      _eventScheduler.TryDeleteCircleEventOf(breakpoint);

      _frontLine.Remove(breakpoint);

      FireBreakpointRemoved(breakpoint);
    }

    public event Action<IBreakpointTracker> BreakpointRemoved;

    void FireBreakpointRemoved(IBreakpointTracker breakpoint)
    {
      Action<IBreakpointTracker> breakpointRemovedHandlers = BreakpointRemoved;
      if (breakpointRemovedHandlers != null)
      {
        breakpointRemovedHandlers(breakpoint);
      }
    }

    #endregion
  }
}