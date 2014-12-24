using System.Linq;

namespace DelaunayTriangulationAlgorithm.EdgeList
{
  public class Triangle
  {
    readonly Edge[] _edges;

    public Triangle(Point point1, Point point2, Point point3)
      : this(point1, new Edge(point2, point3))
    {
    }

    public Triangle(Point point, Edge edge)
      : this(edge, new Edge(point, edge.Points[0]), new Edge(edge.Points[1], point))
    {
    }

    public Triangle(Edge edge1, Edge edge2, Edge edge3)
    {
      _edges = new[] {edge1, edge2, edge3};
    }

    internal PointLocation.Triangle PointLocationTriangle { get; set; }

    public Edge[] Edges
    {
      get { return _edges.ToArray(); }
    }

    internal GeometricElements.Point Center
    {
      get
      {
        GeometricElements.Point center = _edges.SelectMany(e => e.Points).Aggregate(new GeometricElements.Point(0, 0),
          (sum, p) => new GeometricElements.Point(sum.X + p.X, sum.Y + p.Y));
        center = new GeometricElements.Point(center.X/6, center.Y/6);
        return center;
      }
    }

    public bool IsSupporting
    {
      get { return Edges.SelectMany(e => e.Points).Any(p => p.SpecialIndex.HasValue); }
    }

    internal void BindEdges()
    {
      foreach (Edge edge in _edges)
      {
        edge.Add(this);
      }
    }

    internal Point PointOpposite(Edge edge)
    {
      return _edges.SelectMany(e => e.Points).First(p => !edge.Points.Contains(p));
    }

    public void UnbindEdges()
    {
      foreach (Edge edge in _edges)
      {
        edge.Remove(this);
      }
    }

    internal bool Contains(Point point)
    {
      GeometricElements.Point center = Center;

      return _edges.All(edge => edge.SideOfLine(point) == edge.SideOfLine(center))
             || _edges.Any(e => e.SegmentContains(point));
    }
  }
}