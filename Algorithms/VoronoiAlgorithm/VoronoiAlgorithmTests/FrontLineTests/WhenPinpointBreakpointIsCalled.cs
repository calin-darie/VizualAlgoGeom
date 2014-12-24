using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VoronoiAlgorithm;

namespace VoronoiAlgorithmTests.FrontLineTests
{
  [TestClass]
  public class WhenPinpointBreakpointIsCalled
  {
    [TestMethod]
    public void GivenAFrontlineWithFourBreakpoints_WhenAskingForTheSecondLeftmostOne_TheOtherThreeAreReturned()
    {
      var breakpointTrackers = new[]
      {
        MockRepository.GenerateStub<IBreakpointTracker>(),
        MockRepository.GenerateStub<IBreakpointTracker>(),
        MockRepository.GenerateStub<IBreakpointTracker>(),
        MockRepository.GenerateStub<IBreakpointTracker>()
      };

      var comparer = MockRepository.GenerateStub<IComparer<IBreakpointTracker>>();
      foreach (var trackerPair in new[]
      {
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[0], breakpointTrackers[1]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[0], breakpointTrackers[2]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[0], breakpointTrackers[3]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[1], breakpointTrackers[2]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[1], breakpointTrackers[3]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[2], breakpointTrackers[3])
      })
      {
        Tuple<IBreakpointTracker, IBreakpointTracker> currentTrackerPair = trackerPair;
        comparer
          .Stub(c => c.Compare(currentTrackerPair.Item1, currentTrackerPair.Item2))
          .Return(-1);
      }
      foreach (var trackerPair in new[]
      {
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[1], breakpointTrackers[0]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[2], breakpointTrackers[0]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[3], breakpointTrackers[0]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[2], breakpointTrackers[1]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[3], breakpointTrackers[1]),
        new Tuple<IBreakpointTracker, IBreakpointTracker>(breakpointTrackers[3], breakpointTrackers[2])
      })
      {
        Tuple<IBreakpointTracker, IBreakpointTracker> currentTrackerPair = trackerPair;
        comparer
          .Stub(c => c.Compare(currentTrackerPair.Item1, currentTrackerPair.Item2))
          .Return(1);
      }
      // each tracker is equal to itself
      for (var i = 0; i < 4; ++i)
      {
        int currentIndex = i;
        comparer
          .Stub(c => c.Compare(breakpointTrackers[currentIndex], breakpointTrackers[currentIndex]))
          .Return(0);
      }
      var frontLine = new FrontLine(comparer);
      for (var i = 0; i < 4; ++i)
      {
        frontLine.Add(breakpointTrackers[i]);
      }

      IBreakpointTracker previous, next, secondNext;
      frontLine.PinpointBreakpoint(breakpointTrackers[1],
        out previous, out next, out secondNext);

      Assert.IsTrue(
        new[] {previous, next, secondNext}.SequenceEqual(new[]
        {breakpointTrackers[0], breakpointTrackers[2], breakpointTrackers[3]})
        , "expected: <0, 2, 3>; actual: <{0}>", string.Join(", ", new[]
        {
          Array.IndexOf(breakpointTrackers, previous),
          Array.IndexOf(breakpointTrackers, next),
          Array.IndexOf(breakpointTrackers, secondNext)
        })
        );
    }
  }
}