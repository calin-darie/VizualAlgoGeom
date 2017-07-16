using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using SegmentIntersectionAlgorithm;

namespace SegmentIntersectionAlgorithmAdapter
{
  public class SegmentIntersectionAdapter : IAlgorithmAdapter
  {
    public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
      CanvasViewRegistry canvasViewRegistry)
    {
      _snapshotRecorder = snapshotRecorder;
      var segmentIntersectionAlgorithm = new SegmentIntersection(input.LineSegmentList.ToArray());

      _intersectionList = new List<LineSegment>();
      _intersectionPoints = new List<Point>();

      RegisterEvents(segmentIntersectionAlgorithm);

      using (_lineStatusDrawable = _snapshotRecorder.Show(DummyLineStatus, _visualStyles.LineStatus))
      using (_snapshotRecorder.Show(_intersectionPoints, _visualStyles.IntersectionPoints))
      using (_snapshotRecorder.Show(_intersectionList, _visualStyles.IntersectionList))
      using (_sweepLineDrawable = _snapshotRecorder.Show(DummySweepLine, _visualStyles.SweepLine))
      {
        segmentIntersectionAlgorithm.FindIntersections();
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.IntersectionsFound);
      }
    }

    static readonly Line DummySweepLine = new Line(1, 1, 1);
    static readonly List<LineSegment> DummyLineStatus = new List<LineSegment>();

    IDrawableEntityTracker<Point> _eventPointDrawable;
    List<LineSegment> _intersectionList;
    List<Point> _intersectionPoints;
    IDrawableEntityTracker<List<LineSegment>> _lineStatusDrawable;
    IDrawableEntityTracker<LineSegment[]> _segmentsTestedForIntersectionDrawable;
    ISnapshotRecorder _snapshotRecorder;
    IDrawableEntityTracker<Line> _sweepLineDrawable;

    readonly string _explanation;
    readonly List<IPseudocodeLine> _pseudocode;
    readonly SnapshotDescriptions _snapshotDescriptions;
    readonly VisualStyles _visualStyles;

    public SegmentIntersectionAdapter()
    {
      var paths = new AlgorithmResourcePaths(Assembly.GetExecutingAssembly().Location);

      _visualStyles = new XmlIo<VisualStyles>().LoadFrom(paths.VisualStyles) ?? new VisualStyles();
      _snapshotDescriptions = new XmlIo<SnapshotDescriptions>().LoadFrom(paths.SnapshotDescriptions) ?? new SnapshotDescriptions();
      _explanation = new DefaultExplanationLoader().LoadFrom(paths.Explanation) ?? string.Empty;
      _pseudocode = new DefaultPseudocodeLoader().LoadFrom(paths.Pseudocode) ?? new List<IPseudocodeLine>();
    }

    public string Explanation
    {
      get { return _explanation; }
    }

    public List<IPseudocodeLine> Pseudocode
    {
      get { return _pseudocode;}
    }

    void RegisterEvents(SegmentIntersection segmentIntersectionAlgorithm)
    {
      segmentIntersectionAlgorithm.SweepLineInitialized += SegmentIntersectionAlgorithmOnSweepLineInitialized;
      segmentIntersectionAlgorithm.SweepLineUpdated += SegmentIntersectionAlgorithmOnSweepLineUpdated;
      segmentIntersectionAlgorithm.HandleEventPointBegan += SegmentIntersectionAlgorithmOnHandleEventPointBegan;
      segmentIntersectionAlgorithm.HandleEventPointEnded += SegmentIntersectionAlgorithmOnHandleEventPointEnded;
      segmentIntersectionAlgorithm.IntersectionPointFound += IntersectionPointFound;
      segmentIntersectionAlgorithm.NewEventFound += NewEventFound;
      segmentIntersectionAlgorithm.RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus +=
        RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus;
      segmentIntersectionAlgorithm.RemovingSegmentsContainingEventPointFromLineStatus +=
        RemovingSegmentsContainingEventPointFromLineStatus;
      segmentIntersectionAlgorithm.RestoringSegmentsContainingEventPoint += RestoringSegmentsContainingEventPoint;
      segmentIntersectionAlgorithm.TestingSegmentIntersectionBegan += TestingSegmentIntersectionBegan;
      segmentIntersectionAlgorithm.TestingSegmentIntersectionEnded += TestingSegmentIntersectionEnded;
    }

    void SegmentIntersectionAlgorithmOnSweepLineUpdated(double sweepLineY)
    {
      _sweepLineDrawable.Update(GetDrawableSweepLine(sweepLineY));
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.SweepLineUpdated);
    }

    void SegmentIntersectionAlgorithmOnSweepLineInitialized(double sweepLineY)
    {
      _sweepLineDrawable.Update(GetDrawableSweepLine(sweepLineY + 1));
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.SweepLineInitialized);
    }

    static Line GetDrawableSweepLine(double sweepLineY)
    {
      return Line.HorizontalWithY(sweepLineY);
    }

    void RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus(List<LineSegment> lineStatus)
    {
      _lineStatusDrawable.Update(lineStatus);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus);
    }

    void RemovingSegmentsContainingEventPointFromLineStatus(List<LineSegment> lineStatus)
    {
      _lineStatusDrawable.Update(lineStatus);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RemovingSegmentsContainingEventPointFromLineStatus);
    }

    void IntersectionPointFound(Intersection intersection)
    {
      _intersectionList.AddRange(intersection.IntersectionSegments);
      _intersectionPoints.Add(intersection.IntersectionPoint);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.IntersectionPointFound);
    }

    void RestoringSegmentsContainingEventPoint(List<LineSegment> lineStatus)
    {
      _lineStatusDrawable.Update(lineStatus);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RestoringSegmentsContainingEventPoint);
    }

    void NewEventFound(Point intersection)
    {
      using (_snapshotRecorder.Show(intersection, _visualStyles.NewEventPoint))
      {
        _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.NewEventFound);
      }
    }

    void TestingSegmentIntersectionBegan(LineSegment lTested1, LineSegment lTested2)
    {
      _segmentsTestedForIntersectionDrawable = _snapshotRecorder.Show(new[] {lTested1, lTested2},
        _visualStyles.SegmentIntersectionTest);

      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.TestingSegmentIntersection);
    }

    void TestingSegmentIntersectionEnded()
    {
      _segmentsTestedForIntersectionDrawable.Dispose();
    }

    #region event point highlighting

    void SegmentIntersectionAlgorithmOnHandleEventPointBegan(Point point)
    {
      _eventPointDrawable = _snapshotRecorder.Show(point, _visualStyles.EventPoint);
      _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.ExtractingEventPointFromQueue);
    }

    void SegmentIntersectionAlgorithmOnHandleEventPointEnded()
    {
      _eventPointDrawable.Dispose();
    }

    #endregion
  }
}