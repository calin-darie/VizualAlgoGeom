namespace DelaunayTriangulationAlgorithm.PointLocation
{
  internal class InternalTriangle : Triangle
  {
    internal InternalTriangle(EdgeList.Triangle t)
      : base(t)
    {
    }

    internal bool Contains(Point point)
    {
      return _edgeListTriangle.Contains(point);
    }
  }
}