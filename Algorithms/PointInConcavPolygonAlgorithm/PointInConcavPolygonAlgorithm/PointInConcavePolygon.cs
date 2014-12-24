using System;
using System.Collections.Generic;
using GeometricElements;

namespace PointInConcavePolygonAlgorithm
{
  public class PointInConcavePolygon
  {
    readonly Point[] _polygonVertices;
    readonly double _searchLineY;
    readonly Point _searchPoint;

    public PointInConcavePolygon(Point[] polygonVertices, Point searchPoint)
    {
      _polygonVertices = polygonVertices;
      _searchPoint = searchPoint;
      _searchLineY = searchPoint.Y;
    }

    public bool IsInterior()
    {
      var intersectionPoints = new List<Point>();
      for (var i = 0; i < _polygonVertices.Length; i++)
      {
        var currentSegment = new LineSegment(
          _polygonVertices[i],
          _polygonVertices[NextIndexWrapAround(i, _polygonVertices)]);

        OnTestingEdge(currentSegment);

        if (IsHorizontal(currentSegment) || !IsIntersectingSearchLine(currentSegment)) continue;

        double x;
        currentSegment.Line.IntersectHorizontal(_searchLineY, out x);
        bool isIntersectingSearchLineToTheLeft = x < _searchPoint.X;
        if (isIntersectingSearchLineToTheLeft)
        {
          var intersectionPoint = new Point(x, _searchLineY);
          intersectionPoints.Add(intersectionPoint);
          OnIntersectionFound(intersectionPoint);
        }
      }

      return intersectionPoints.Count%2 == 1;
    }

    static int NextIndexWrapAround(int i, ICollection<Point> p)
    {
      return (i + 1)%p.Count;
    }

    public event Action<LineSegment> TestingEdge;
    public event Action<Point> IntersectionFound;

    bool IsIntersectingSearchLine(LineSegment crtSegment)
    {
      double minY;
      double maxY;
      if (crtSegment.FirstPoint.Y < crtSegment.SecondPoint.Y)
      {
        minY = crtSegment.FirstPoint.Y;
        maxY = crtSegment.SecondPoint.Y;
      }
      else
      {
        minY = crtSegment.SecondPoint.Y;
        maxY = crtSegment.FirstPoint.Y;
      }

      return minY < _searchLineY && _searchLineY <= maxY;
    }

    bool IsHorizontal(LineSegment crtSegment)
    {
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      return crtSegment.FirstPoint.Y == crtSegment.SecondPoint.Y;
    }

    protected virtual void OnTestingEdge(LineSegment obj)
    {
      Action<LineSegment> handler = TestingEdge;
      if (handler != null) handler(obj);
    }

    protected virtual void OnIntersectionFound(Point obj)
    {
      Action<Point> handler = IntersectionFound;
      if (handler != null) handler(obj);
    }
  }
}