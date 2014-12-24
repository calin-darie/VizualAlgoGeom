using System.Collections.Generic;
using System.Linq;

namespace DelaunayTriangulationAlgorithm.PointLocation
{
  internal abstract class Triangle
  {
    protected EdgeList.Triangle _edgeListTriangle;
    readonly List<InternalTriangle> _triangles = new List<InternalTriangle>();

    public Triangle(EdgeList.Triangle edgeListTriangle)
    {
      _edgeListTriangle = edgeListTriangle;
      _edgeListTriangle.PointLocationTriangle = this;
    }

    public EdgeList.Triangle EdgeListTriangle
    {
      get { return _edgeListTriangle; }
    }

    internal void Add(List<InternalTriangle> addedTriangles)
    {
      _triangles.AddRange(addedTriangles);
    }

    internal Triangle FindTriangleContaining(Point point)
    {
      if (!_triangles.Any())
      {
        return this;
      }
      foreach (InternalTriangle childTriangle in _triangles)
      {
        if (childTriangle.Contains(point))
        {
          return childTriangle.FindTriangleContaining(point);
        }
      }
      return this;
    }
  }
}