using System;
using System.Collections.Generic;
using System.Linq;
using GeometricElements;
using VoronoiAlgorithm;

namespace VoronoiAlgorithmAdapter
{
  public class Diagram : MarshalByRefObject
  {
    readonly List<Line> _lines = new List<Line>();
    readonly List<Ray> _rays = new List<Ray>();
    readonly List<LineSegment> _segments = new List<LineSegment>();

    public IEnumerable<Line> Lines
    {
      get { return _lines; }
    }

    public IEnumerable<Ray> Rays
    {
      get { return _rays; }
    }

    public IEnumerable<LineSegment> Segments
    {
      get { return _segments; }
    }

    internal void Add(IEdge edge)
    {
      Line line = edge.Line;
      KeyValuePair<EdgeSide, Point>[] endPoints = edge.EndPoints.Where(e => e.Value != null).ToArray();

      if (!endPoints.Any())
      {
        _lines.Add(line);
      }
      else if (endPoints.Count() == 1)
      {
        KeyValuePair<EdgeSide, Point> endPoint = endPoints.Single();
        Point pointLeft = GetOtherPointOnLine(line, endPoint.Value, -1);
        Point pointRight = GetOtherPointOnLine(line, endPoint.Value, +1);

        _rays.Add(new Ray(endPoint.Value, endPoint.Key == EdgeSide.Left ? pointRight : pointLeft));
      }
      else // two endpoints
      {
        _segments.Add(new LineSegment(endPoints.First().Value, endPoints.Skip(1).First().Value));
      }
    }

    static Point GetOtherPointOnLine(Line line, Point endPoint, double deltaX)
    {
      double x = endPoint.X + deltaX;
      double y;
      line.IntersectVertical(x, out y);
      return new Point(x, y);
    }
  }
}