using GeometricElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VoronoiAlgorithm;

namespace VoronoiAlgorithmTests.FrontLinePointComparerTests
{
  [TestClass]
  public class WhenCompareIHasBreakpointOverloadIsCalled
  {
    [TestMethod]
    public void
      GivenTwoBreakpointTrackersWithSameBreakPointAndSamePreviouslyKnownPoint_GoingLeftIsLesserThanNotGoingLeft()
    {
      var equalBreakPoint = new Point(1, 0);
      var equalPreviouslyKnownPoint = new Point(0, 1);
      var breakpointTrackerGoingRight = MockRepository.GenerateStub<IBreakpointTracker>();
      breakpointTrackerGoingRight.Stub(tracker => tracker.Breakpoint)
        .Return(equalBreakPoint);
      breakpointTrackerGoingRight.Stub(tracker => tracker.PreviouslyKnownPoint)
        .Return(equalPreviouslyKnownPoint);
      breakpointTrackerGoingRight.Stub(tracker => tracker.IsGoingLeft)
        .Return(false);
      var breakpointTrackerGoingLeft = MockRepository.GenerateStub<IBreakpointTracker>();
      breakpointTrackerGoingLeft.Stub(tracker => tracker.Breakpoint)
        .Return(equalBreakPoint);
      breakpointTrackerGoingLeft.Stub(tracker => tracker.PreviouslyKnownPoint)
        .Return(equalPreviouslyKnownPoint);
      breakpointTrackerGoingLeft.Stub(tracker => tracker.IsGoingLeft)
        .Return(true);
      var comparer = new FrontLinePointComparer();

      int comparisonResult = comparer.Compare(breakpointTrackerGoingLeft, breakpointTrackerGoingRight);

      Assert.IsTrue(comparisonResult < 0);
    }

    [TestMethod]
    public void
      GivenTwoBreakpointsAtSameXYStartingAtSameXFirstGoingLeftStartingAtY1AndSecondGoingRightStartingAtY0_Breakpoint1IsLesser
      ()
    {
      var equalBreakPoint = new Point(1, 0);
      var sameStartingX = 1;

      var breakpointTrackerGoingLeft = MockRepository.GenerateStub<IBreakpointTracker>();
      breakpointTrackerGoingLeft.Stub(tracker => tracker.Breakpoint)
        .Return(equalBreakPoint);
      breakpointTrackerGoingLeft.Stub(tracker => tracker.PreviouslyKnownPoint)
        .Return(new Point(sameStartingX, 1));
      breakpointTrackerGoingLeft.Stub(tracker => tracker.IsGoingLeft)
        .Return(true);

      var breakpointTrackerGoingRight = MockRepository.GenerateStub<IBreakpointTracker>();
      breakpointTrackerGoingRight.Stub(tracker => tracker.Breakpoint)
        .Return(equalBreakPoint);
      breakpointTrackerGoingRight.Stub(tracker => tracker.PreviouslyKnownPoint)
        .Return(new Point(sameStartingX, 0));
      breakpointTrackerGoingRight.Stub(tracker => tracker.IsGoingLeft)
        .Return(false);
      var comparer = new FrontLinePointComparer();


      //act
      int comparisonResult = comparer.Compare(breakpointTrackerGoingLeft, breakpointTrackerGoingRight);

      Assert.IsTrue(comparisonResult < 0);
    }

    /// <summary>
    ///   make sure the starting order is kept
    /// </summary>
    [TestMethod]
    public void
      GivenTwoBreakpointsAtSameXYFirstWithPreviouslyKnownPointX0Y0GoingRightSecondWithPreviouslyKnownPointX1Y1GoingLeft_BreakpointWithLesserPreviouslyKnownPointIsLesser
      ()
    {
      var equalBreakPoint = new Point(2, 2);
      var breakpoint1StartingLeft = MockRepository.GenerateStub<IBreakpointTracker>();
      breakpoint1StartingLeft.Stub(tracker => tracker.Breakpoint)
        .Return(equalBreakPoint);
      breakpoint1StartingLeft.Stub(tracker => tracker.PreviouslyKnownPoint)
        .Return(new Point(0, 0));
      breakpoint1StartingLeft.Stub(tracker => tracker.IsGoingLeft)
        .Return(false);

      var breakpoint2StartingRight = MockRepository.GenerateStub<IBreakpointTracker>();
      breakpoint2StartingRight.Stub(tracker => tracker.Breakpoint)
        .Return(equalBreakPoint);
      breakpoint2StartingRight.Stub(tracker => tracker.PreviouslyKnownPoint)
        .Return(new Point(1, 1));
      breakpoint2StartingRight.Stub(tracker => tracker.IsGoingLeft)
        .Return(true);

      var comparer = new FrontLinePointComparer();

      int comparisonResult = comparer.Compare(breakpoint1StartingLeft, breakpoint2StartingRight);

      Assert.IsTrue(comparisonResult < 0);
    }
  }
}