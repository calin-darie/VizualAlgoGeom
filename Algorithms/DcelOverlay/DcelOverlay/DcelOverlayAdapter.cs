using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AlgorithmResources;
using DcelOverlayAlgorithm;
using DefaultAuxiliariesImplementation;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DcelOverlayAlgorithmAdapter
{
    public class DcelOverlayAdapter : IAlgorithmAdapter
    {
        public void RunAlgorithm(IAlgorithmInput input, ISnapshotRecorder snapshotRecorder,
          CanvasViewRegistry canvasViewRegistry)
        {
            var algorithm = new DcelOverlay(input.DcelList);
            _snapshotRecorder = snapshotRecorder;
            _intersectionList = new List<DcelHalfEdge>();
            _intersectionPoints = new List<DcelVertex>();
            RegisterEvents(algorithm);

          using (_lineStatusDrawable = _snapshotRecorder.Show(DummyLineStatus, _visualStyles.LineStatus))
          using (_snapshotRecorder.Show(_intersectionPoints, _visualStyles.IntersectionPoints))
          using (_snapshotRecorder.Show(_intersectionList, _visualStyles.IntersectionList))
          using (_sweepLineDrawable = _snapshotRecorder.Show(DummySweepLine, _visualStyles.SweepLine))

          using( _splitLinesDrawable = _snapshotRecorder.Show(DummyLineStatusForSplitLines,_visualStyles.SplitLines))
          using( _facesDrawable = _snapshotRecorder.Show(DummyLineStatusForFaces,_visualStyles.SplitLines))
          using (_intersectedLinesDrawable = _snapshotRecorder.Show(DummyLineStatusForIntersectedLines, _visualStyles.IntersectedSegments))
            {
                algorithm.FindIntersections();
                _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.IntersectionsFound);
            }
        }

        List<DcelHalfEdge> _intersectionList;
        List<DcelVertex> _intersectionPoints;
        ISnapshotRecorder _snapshotRecorder;
        readonly string _explanation;
        readonly List<IPseudocodeLine> _pseudocode;
        readonly SnapshotDescriptions _snapshotDescriptions;
        readonly VisualStyles _visualStyles;

        static readonly Line DummySweepLine = new Line(1, 1, 1);
        static readonly List<LineSegment> DummyLineStatus = new List<LineSegment>();
        static readonly List<LineSegment> DummyLineStatusForIntersectedLines = new List<LineSegment>();
        static readonly List<LineSegment> DummyLineStatusForFaces = new List<LineSegment>();
        static readonly List<LineSegment> DummyLineStatusForSplitLines = new List<LineSegment>();
        IDrawableEntityTracker<List<LineSegment>> _lineStatusDrawable; 
        IDrawableEntityTracker<List<LineSegment>> _intersectedLinesDrawable; 
        IDrawableEntityTracker<List<LineSegment>> _splitLinesDrawable; 
        IDrawableEntityTracker<List<LineSegment>> _facesDrawable; 
        IDrawableEntityTracker<LineSegment[]> _segmentsTestedForIntersectionDrawable;
        IDrawableEntityTracker<Line> _sweepLineDrawable;
        IDrawableEntityTracker<Point> _eventPointDrawable;
        IDrawableEntityTracker<Point> _eventSplitPointDrawable;

        static Line GetDrawableSweepLine(double sweepLineY)
        {
            return Line.HorizontalWithY(sweepLineY);
        }

        void RegisterEvents(DcelOverlay dcelOverlayAlgorithm)
        {
            dcelOverlayAlgorithm.SweepLineInitialized += DcelOverlayAlgorithmOnSweepLineInitialized;
            dcelOverlayAlgorithm.SweepLineUpdated += DcelOverlayAlgorithmOnSweepLineUpdated;
            dcelOverlayAlgorithm.HandleEventPointBegan += DcelOverlayAlgorithmOnHandleEventPointBegan;
            dcelOverlayAlgorithm.HandleEventPointEnded += DcelOverlayAlgorithmOnHandleEventPointEnded;
            dcelOverlayAlgorithm.IntersectionPointFound += IntersectionPointFound;

            dcelOverlayAlgorithm.NewEventFound += NewEventFound;
            dcelOverlayAlgorithm.RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus +=
              RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus;
            dcelOverlayAlgorithm.RemovingSegmentsContainingEventPointFromLineStatus +=
              RemovingSegmentsContainingEventPointFromLineStatus;
            dcelOverlayAlgorithm.RestoringSegmentsContainingEventPoint += RestoringSegmentsContainingEventPoint;
            dcelOverlayAlgorithm.TestingSegmentIntersectionBegan += TestingSegmentIntersectionBegan;
            dcelOverlayAlgorithm.TestingSegmentIntersectionEnded += TestingSegmentIntersectionEnded;

            dcelOverlayAlgorithm.BeforeSplitingHalfEdges += BeforeSplitingHalfEdges;
            dcelOverlayAlgorithm.AfterSplitingHalfEdges += AfterSplitingHalfEdges;
            dcelOverlayAlgorithm.RemovingSplitHalfEdges += RemovingSplitHalfEdges;
            dcelOverlayAlgorithm.ColoringFace += ColoringFace;
            dcelOverlayAlgorithm.DecoloringFace += DecoloringFace;
        }

        void DcelOverlayAlgorithmOnSweepLineUpdated(double sweepLineY)
        {
            _sweepLineDrawable.Update(GetDrawableSweepLine(sweepLineY));
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.SweepLineUpdated);
        }

        void DcelOverlayAlgorithmOnSweepLineInitialized(double sweepLineY)
        {
            _sweepLineDrawable.Update(GetDrawableSweepLine(sweepLineY + 1));
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.SweepLineInitialized);
        }

        void DcelOverlayAlgorithmOnHandleEventPointBegan(Point point, String name)
        {
            _eventPointDrawable = _snapshotRecorder.Show(point, _visualStyles.EventPoint.WithFormattedName(name));
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.ExtractingEventPointFromQueue);
        }

        void DcelOverlayAlgorithmOnHandleEventPointEnded()
        {
            _eventPointDrawable.Dispose();
        }

        void RestoringSegmentsContainingEventPoint(List<LineSegment> lineStatus)
        {
            _lineStatusDrawable.Update(lineStatus);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RestoringSegmentsContainingEventPoint);
        }
        void ColoringFace(List<LineSegment> lineStatus)
        {
            _facesDrawable.Update(lineStatus);
        }
        void DecoloringFace(List<LineSegment> lineStatus)
        {
            _facesDrawable.Update(lineStatus);
        }
        void NewEventFound(Point intersection)
        {
            using (_snapshotRecorder.Show(intersection, _visualStyles.NewEventPoint))
            {
                _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.NewEventFound);
            }
        }

        void RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus(List<LineSegment> lineStatus)
        {
            _lineStatusDrawable.Update(lineStatus);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RemovingSegmentsHavingEventPointAsLowerEndFromLineStatus);
        }

        void BeforeSplitingHalfEdges(List<LineSegment> intersectedLines, Point eventPoint, String name )
        {
            _eventSplitPointDrawable = _snapshotRecorder.Show(eventPoint, _visualStyles.EventPoint.WithFormattedName(name));
            _intersectedLinesDrawable.Update(intersectedLines);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.BeforeSplitingHalfEdges);
        }
        void AfterSplitingHalfEdges(List<LineSegment> intersectedLines, List<LineSegment> splitLines, Point eventPoint)
        {
            _intersectedLinesDrawable.Update(intersectedLines);
            _splitLinesDrawable.Update(splitLines);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.AfterSplitingHalfEdges);
        }
        void RemovingSplitHalfEdges(List<LineSegment> intersectedLines, List<LineSegment> splitLines)
        {
            _eventSplitPointDrawable.Dispose();
            _intersectedLinesDrawable.Update(intersectedLines);
            _splitLinesDrawable.Update(splitLines);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RemovingSplitingHalfEdges);
        }
        void RemovingSegmentsContainingEventPointFromLineStatus(List<LineSegment> lineStatus)
        {
            _lineStatusDrawable.Update(lineStatus);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.RemovingSegmentsContainingEventPointFromLineStatus);
        }

        void TestingSegmentIntersectionBegan(LineSegment lTested1, LineSegment lTested2)
        {
            _segmentsTestedForIntersectionDrawable = _snapshotRecorder.Show(new[] { lTested1, lTested2 },
              _visualStyles.SegmentIntersectionTest);

            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.TestingSegmentIntersection);
        }

        void TestingSegmentIntersectionEnded()
        {
            _segmentsTestedForIntersectionDrawable.Dispose();
        }

       void SegmentIntersectionAlgorithmOnHandleEventPointBegan(Point point)
        {
            _eventPointDrawable = _snapshotRecorder.Show(point, _visualStyles.EventPoint);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.ExtractingEventPointFromQueue);
        }

        void SegmentIntersectionAlgorithmOnHandleEventPointEnded()
        {
            _eventPointDrawable.Dispose();
        }

        void IntersectionPointFound(Intersection intersection)
        {
            _intersectionList.AddRange(intersection.IntersectionSegments);
            _intersectionPoints.Add(intersection.IntersectionPoint);
            _snapshotRecorder.TakeSnapshot(_snapshotDescriptions.IntersectionPointFound);
        }
        public DcelOverlayAdapter()
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
            get { return _pseudocode; }
        }
    }
}