using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeometricElements;

namespace DelaunayTriangulationAlgorithm.EdgeList
{
  [DebuggerDisplay("[{_points[0]}, {_points[1]}]")]
  public class Edge
  {
    readonly LineSegment _lineSegment;
    readonly Point[] _points;
    readonly List<Triangle> _triangles = new List<Triangle>();

    internal Edge(Point firstPoint, Point secondPoint)
    {
      _points = new[] {firstPoint, secondPoint};
      _lineSegment = new LineSegment(Points[0], Points[1]);
    }

    public Triangle[] Triangles
    {
      get { return _triangles.ToArray(); }
    }

    public Point[] Points
    {
      get { return _points.ToArray(); }
    }

    internal Line Line
    {
      get { return _lineSegment.Line; }
    }

    public void Add(Triangle triangle)
    {
      if (_triangles.Any(t => t.Center.Equals(triangle.Center)))
      {
      }
      _triangles.Add(triangle);
    }

    public void Remove(Triangle triangle)
    {
      _triangles.Remove(triangle);
    }

    internal int SideOfLine(GeometricElements.Point point)
    {
      return Math.Sign(
        (Points[1].X - Points[0].X)*(point.Y - Points[1].Y) -
        (Points[1].Y - Points[0].Y)*(point.X - Points[1].X));
    }

    public static implicit operator LineSegment(Edge e)
    {
      return e.ToLineSegment();
    }

    public LineSegment ToLineSegment()
    {
      return _lineSegment;
    }

    internal bool SegmentContains(Point point)
    {
      return _lineSegment.SegmentContains(point);
    }
  }
}