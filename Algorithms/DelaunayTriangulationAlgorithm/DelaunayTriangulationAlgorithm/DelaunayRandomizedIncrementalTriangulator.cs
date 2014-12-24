using System;
using System.Collections.Generic;
using System.Linq;
using DelaunayTriangulationAlgorithm.EdgeList;
using DelaunayTriangulationAlgorithm.PointLocation;
using GeometricElements;
using Triangle = DelaunayTriangulationAlgorithm.EdgeList.Triangle;

namespace DelaunayTriangulationAlgorithm
{
  public class DelaunayRandomizedIncrementalTriangulator
  {
    BoundingTriangle _pointLocationRoot;
    List<Triangle> _triangles = new List<Triangle>();
    public event Action<Triangle, Circle, GeometricElements.Point> TestingCircle;

    public Triangle[] Triangles
    {
      get { return _triangles.ToArray(); }
    }

    public void Run(IEnumerable<Point> planarPointSet)
    {
      List<Point> pointSet = planarPointSet.ToList();
      Triangle boundingTriangle = ComputeBoundingTriangle(pointSet);
      _triangles = new List<Triangle> {boundingTriangle};
      _pointLocationRoot = new BoundingTriangle(boundingTriangle);
      Point[] randomPermutationOfGivenPointSet = GetRandomPermutation(pointSet);
      foreach (Point point in randomPermutationOfGivenPointSet)
      {
        Insert(point);
      }
      foreach (Triangle triangle in _triangles.ToArray())
      {
        if (triangle.Edges.SelectMany(e => e.Points).Any(p => p.SpecialIndex < 0))
        {
          Remove(triangle);
        }
      }
    }

    static Triangle ComputeBoundingTriangle(IEnumerable<Point> planarPointSet)
    {
      double m = planarPointSet.SelectMany(p => new[] {p.X, p.Y}).Select(Math.Abs).Max();
      var boundingTriangle = new Triangle(
        new Point(new GeometricElements.Point(3*m, 0), -1),
        new Point(new GeometricElements.Point(0, 3*m), -2),
        new Point(new GeometricElements.Point(-3*m, -3*m), -3)
        );
      return boundingTriangle;
    }

    internal void Insert(Point pointToInsert)
    {
      FireInsertingPoint(pointToInsert);
      Triangle[] triangles = FindTrianglesContaining(pointToInsert);

      var edgesToLegalize = new List<Edge>();
      var addedEdges = new List<Edge>();
      foreach (Triangle triangle in triangles)
      {
        SplitTriangleByConnectingPointToItsVertices(triangle, pointToInsert, addedEdges, edgesToLegalize);
      }

      foreach (Edge edge in edgesToLegalize)
      {
        LegalizeEdge(edge, pointToInsert);
      }
    }

    Triangle[] FindTrianglesContaining(Point pointToInsert)
    {
      PointLocation.Triangle pointLocation = _pointLocationRoot.FindTriangleContaining(pointToInsert);
      Triangle triangle = pointLocation.EdgeListTriangle;
      Edge edge = triangle == _pointLocationRoot.EdgeListTriangle
        ? null
        : triangle.Edges.FirstOrDefault(e => e.SegmentContains(pointToInsert));
      return edge != null
        ? edge.Triangles
        : new[] {pointLocation.EdgeListTriangle};
    }

    void SplitTriangleByConnectingPointToItsVertices(Triangle triangle, Point pointToInsert, List<Edge> addedEdges,
      List<Edge> edgesToLegalize)
    {
      Remove(triangle);
      var addedTriangles = new List<InternalTriangle>();

      foreach (Edge edge in triangle.Edges.Where(e => !e.SegmentContains(pointToInsert)))
      {
        var edges = new Edge[2];
        for (var i = 0; i <= 1; i++)
        {
          edges[i] = addedEdges.FirstOrDefault(e => e.Points[1] == edge.Points[i]);
          if (edges[i] == null)
          {
            edges[i] = new Edge(pointToInsert, edge.Points[i]);
            addedEdges.Add(edges[i]);
          }
        }
        var edgeListTriangle = new Triangle(edges[0], edges[1], edge);
        var pointLocationTriangle = new InternalTriangle(edgeListTriangle);
        Add(edgeListTriangle);
        edgesToLegalize.Add(edge);
        addedTriangles.Add(pointLocationTriangle);
      }

      triangle.PointLocationTriangle.Add(addedTriangles);
    }

    void LegalizeEdge(Edge edge, Point pointToInsert)
    {
      if (!IsLegal(edge))
      {
        Flip(edge, pointToInsert);
      }
    }

    bool IsLegal(Edge edge)
    {
      if (edge.Triangles.Length < 2)
      {
        return true;
      }

      Point oppositePoint0 = edge.Triangles[0].PointOpposite(edge);
      Point oppositePoint1 = edge.Triangles[1].PointOpposite(edge);

      var flippedEdge = new Edge(oppositePoint0, oppositePoint1);
      bool theFourPointsDoNotDefineAConvexQuadrilater =
        flippedEdge.SideOfLine(edge.Points[0]) == flippedEdge.SideOfLine(edge.Points[1]) ||
        flippedEdge.Line.Contains(edge.Points[0]) ||
        flippedEdge.Line.Contains(edge.Points[1]);

      if (theFourPointsDoNotDefineAConvexQuadrilater)
      {
        return true;
      }

      if (edge.Points[0].SpecialIndex.HasValue &&
          edge.Points[1].SpecialIndex.HasValue)
      {
        return true; // because we must preserve edges of bounding triangle
      }
      if (!edge.Points[0].SpecialIndex.HasValue &&
          !edge.Points[1].SpecialIndex.HasValue &&
          !oppositePoint0.SpecialIndex.HasValue &&
          !oppositePoint1.SpecialIndex.HasValue /*todo: find out if needed*/)
      {
        if (edge.Line.Contains(oppositePoint0))
        {
          return false;
        }

        Circle circumcircle = Circle.FindCircumscribed(
          edge.Points[0], edge.Points[1], oppositePoint0);

        FireTestingCircle(new Triangle(edge.Points[0], edge.Points[1], oppositePoint0), circumcircle, oppositePoint1);
        return !circumcircle.InteriorContains(oppositePoint1);
      }
      if ((edge.Points[0].SpecialIndex.HasValue ^ edge.Points[1].SpecialIndex.HasValue) &&
          !oppositePoint0.SpecialIndex.HasValue &&
          !oppositePoint1.SpecialIndex.HasValue)
      {
        return false;
      }
      if (!edge.Points[0].SpecialIndex.HasValue &&
          !edge.Points[1].SpecialIndex.HasValue &&
          (oppositePoint0.SpecialIndex.HasValue ^ oppositePoint1.SpecialIndex.HasValue))
      {
        return true;
      }
      int edgeSpecialPointIndex = (edge.Points[0].SpecialIndex ?? edge.Points[1].SpecialIndex).Value;
      int oppositePointsSpecialIndex = (oppositePoint0.SpecialIndex ?? oppositePoint1.SpecialIndex).Value;
      return oppositePointsSpecialIndex < edgeSpecialPointIndex;
    }

    void FireTestingCircle(Triangle triangle, Circle circumcircle, Point pointToTestForInclusion)
    {
      Action<Triangle, Circle, GeometricElements.Point> handler = TestingCircle;
      if (handler != null)
      {
        handler(triangle, circumcircle, pointToTestForInclusion);
      }
    }

    bool Add(Triangle triangle)
    {
      bool shouldAdd = !_triangles.Any(t => Equals(t.Center, triangle.Center));
      if (shouldAdd)
      {
        triangle.BindEdges();
        _triangles.Add(triangle);
        FireTriangleAdded(triangle);
      }
      return shouldAdd;
    }

    void Remove(Triangle triangle)
    {
      _triangles.Remove(triangle);
      triangle.UnbindEdges();
      FireTriangleRemoved(triangle);
    }

    void Flip(Edge edge, Point pointToInsert)
    {
      Triangle thisTriangle = edge.Triangles.Single(t => t.Edges.SelectMany(e => e.Points).Contains(pointToInsert));
      Triangle otherTriangle = edge.Triangles.Single(t => t != thisTriangle);

      Point oppositePoint = otherTriangle.PointOpposite(edge);

      var flippedEdge = new Edge(pointToInsert, oppositePoint);

      FireFlippingEdge(edge, pointToInsert);

      Triangle[] removedTriangles = edge.Triangles;
      foreach (Triangle triangle in removedTriangles)
      {
        Remove(triangle);
      }

      var edgesToLegalize = new List<Edge>();
      var addedTriangles = new List<InternalTriangle>();

      foreach (Point point in edge.Points)
      {
        Edge edgeConnectedToPointToInsert = thisTriangle.Edges.Single(e =>
          e != edge && e.Points.Contains(point));
        Edge otherEdge = otherTriangle.Edges.Single(e =>
          e != edge && e.Points.Contains(point));

        var edgeListTriangle = new Triangle(edgeConnectedToPointToInsert, otherEdge, flippedEdge);
        var pointLocationTriangle = new InternalTriangle(edgeListTriangle);
        bool added = Add(edgeListTriangle);
        if (added)
        {
          addedTriangles.Add(pointLocationTriangle);
          edgesToLegalize.Add(otherEdge);
        }
      }

      foreach (Edge edgeToLegalize in edgesToLegalize)
      {
        LegalizeEdge(edgeToLegalize, pointToInsert);
      }

      foreach (Triangle removedTriangle in removedTriangles)
      {
        removedTriangle.PointLocationTriangle.Add(addedTriangles);
      }
    }

    #region published algorithm step events

    public event Action<Point> InsertingPoint;
    public event Action<Triangle> TriangleAdded;
    public event Action<Triangle> TriangleRemoved;
    public event Action<Edge, Point> FlippingEdge;

    void FireTriangleAdded(Triangle triangle)
    {
      Action<Triangle> handler = TriangleAdded;
      if (handler != null)
      {
        handler(triangle);
      }
    }

    void FireTriangleRemoved(Triangle triangle)
    {
      Action<Triangle> handler = TriangleRemoved;
      if (handler != null)
      {
        handler(triangle);
      }
    }

    void FireFlippingEdge(Edge edge, Point pointToInsert)
    {
      Action<Edge, Point> handler = FlippingEdge;
      if (handler != null)
      {
        handler(edge, pointToInsert);
      }
    }

    void FireInsertingPoint(Point pointToInsert)
    {
      Action<Point> handler = InsertingPoint;
      if (handler != null)
      {
        handler(pointToInsert);
      }
    }

    #endregion

    #region todo: move to generic algorithms library

    TElement[] GetRandomPermutation<TElement>(IEnumerable<TElement> planarPointSet)
    {
      var randomNumberGenerator = new Random();
      TElement[] randomizedSet = planarPointSet.ToArray();
      for (int i = randomizedSet.Length - 1; i >= 0; i--)
      {
        int positionToSwapWith = randomNumberGenerator.Next(i + 1);
        Swap(randomizedSet, i, positionToSwapWith);
      }
      return randomizedSet;
    }

    void Swap<TElement>(TElement[] array, int i, int positionToSwapWith)
    {
      TElement temp = array[i];
      array[i] = array[positionToSwapWith];
      array[positionToSwapWith] = temp;
    }

    #endregion
  }
}