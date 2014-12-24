using System.Collections.Generic;
using GeometricElements;

namespace VoronoiAlgorithm
{
  public class Edge : Line, IEdge
  {
    public Dictionary<EdgeSide, Point> EndPoints { get; set; }

    public Point IntersectionPointWith(IEdge other)
    {
      return base.IntersectionPointWith(other.Line);
    }

    public Line Line
    {
      get { return new Line(_a, _b, _c); }
    }

    public Edge(
      Dictionary<EdgeSide, Point> endPoints,
      double a, double b, double c
      )
      : this(endPoints, new Line(a, b, c))
    {
    }

    public Edge(
      Dictionary<EdgeSide, Point> endPoints,
      Line line
      )
      : base(line)
    {
      EndPoints = endPoints;
    }

    public new static Edge BisectorOf(Point leftPoint, Point rightPoint)
    {
      // to begin with, there are no endpoints on the bisector - it goes to infinity
      var endPoints =
        new Dictionary<EdgeSide, Point>
        {
          {EdgeSide.Left, null},
          {EdgeSide.Right, null}
        };

      Line perpendicular = Line.BisectorOf(leftPoint, rightPoint);

      var newEdge = new Edge(
        endPoints,
        perpendicular);


      return newEdge;
    }
  }
}