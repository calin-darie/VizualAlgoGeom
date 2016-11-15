using System;
using System.Collections.Generic;
using GeometricElements;
using InterfaceOfAlgorithmAdaptersWithVisualizer;

namespace VizualAlgoGeom.DTO
{
    public class AlgorithmInput : MarshalByRefObject, IAlgorithmInput
    {
        public IList<Point> PointList { get; set; }
        public IList<LineSegment> LineSegmentList { get; set; }
        public IList<Ray> RayList { get; set; }
        public IList<Line> LineList { get; set; }
        public IList<PolyLine> ClosedPolylineList { get; set; }
        public IList<PolyLine> PolyLineList { get; set; }
        public IList<Weighted<Point>> WeightedPointList { get; set; }
        public IList<Dcel> DcelList { get; set; }

        public AlgorithmInput(
          IList<Point> pointList,
          IList<LineSegment> lineSegmentList,
          IList<Ray> rayList,
          IList<Line> lineList,
          IList<PolyLine> closedPolylineList,
          IList<PolyLine> polyLineList,
          IList<Weighted<Point>> weightedPointList,
          IList<Dcel> dcelList)
        {
            PointList = pointList;
            WeightedPointList = weightedPointList;
            LineSegmentList = lineSegmentList;
            ClosedPolylineList = closedPolylineList;
            PolyLineList = polyLineList;
            RayList = rayList;
            LineList = lineList;
            DcelList = dcelList;
        }

    }
}