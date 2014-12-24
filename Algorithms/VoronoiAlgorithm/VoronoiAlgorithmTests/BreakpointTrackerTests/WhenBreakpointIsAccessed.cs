using System.Collections.Generic;
using GeometricElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VoronoiAlgorithm;

namespace VoronoiAlgorithmTests.BreakpointTrackerTests
{
  [TestClass]
  public class WhenBreakpointIsAccessed
  {
    [TestMethod]
    public void WhenLeftSiteIsNotGeneratingRightArc_BreakpointTrackerProgressesToTheLeft()
    {
      var sweepLineY = 100;
      var sweepLine = MockRepository.GenerateStub<ISweepLine>();
      sweepLine.Stub(sl => sl.Y).Return(sweepLineY);
      var edge = MockRepository.GenerateStub<IEdge>();
      var comparer = MockRepository.GenerateStub<IComparer<Point>>();

      var breakpointTracker =
        new BreakpointTracker(comparer, sweepLine, edge, new Point(0, 5), new Point(0, -5), isGoingLeft: true);

      //act
      Point result = breakpointTracker.Breakpoint;

      // assert

      Assert.IsTrue(result.X < 0, "expected: <negative number>; actual: <{0}>", result.X);
    }

    [TestMethod]
    public void WhenLeftSiteIsGeneratingRightArc_BreakpointProgressesToTheRight()
    {
      var sweepLineY = 100;
      var sweepLine = MockRepository.GenerateStub<ISweepLine>();
      sweepLine.Stub(sl => sl.Y).Return(sweepLineY);
      var edge = MockRepository.GenerateStub<IEdge>();
      var comparer = MockRepository.GenerateStub<IComparer<Point>>();

      var breakpointTracker =
        new BreakpointTracker(comparer, sweepLine, edge, new Point(0, 5), new Point(0, -5), isGoingLeft: false);

      //act
      Point result = breakpointTracker.Breakpoint;

      // assert
      Assert.IsTrue(result.X > 0, "expected: <positive number>; actual: <{0}>", result.X);
    }
  }
}