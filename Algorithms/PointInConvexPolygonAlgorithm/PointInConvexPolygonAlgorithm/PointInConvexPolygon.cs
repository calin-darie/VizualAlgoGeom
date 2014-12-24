using System;
using System.Linq;
using GeometricElements;

namespace PointInConvexPolygonAlgorithm
{
  public class PointInConvexPolygon
  {
    Point _interiorPoint;
    readonly Point[] _polygonVertices;
    readonly Point _searchPoint;

    public PointInConvexPolygon(Point[] polygonVertices, Point searchPoint)
    {
      _polygonVertices = polygonVertices;
      _searchPoint = searchPoint;
    }

    public event Action<Point> InteriorPointFound;
    public event Action<Ray[]> SearchingInZone;
    public event Action<LineSegment> EdgeToCompareFound;

    public bool IsInterior()
    {
      _interiorPoint = FindInteriorPoint();
      bool isNotPolygon = _interiorPoint == null;
      if (isNotPolygon) return false;

      OnInteriorPointFound(_interiorPoint);

      int index = FindZoneIndex();
      var segment = new LineSegment(_polygonVertices[index], _polygonVertices[(index + 1)%_polygonVertices.Length]);
      OnEdgeToCompareFound(segment);
      return
        Line.SideOfLine(_searchPoint, _polygonVertices[(index + 1)%_polygonVertices.Length], _polygonVertices[index]) <
        (-double.Epsilon);
    }

    Point FindInteriorPoint()
    {
      for (var i = 2; i < _polygonVertices.Length; i++)
        if (Math.Abs(Line.SideOfLine(_polygonVertices[i], _polygonVertices[1], _polygonVertices[0])) > Double.Epsilon)
        {
          return Center(_polygonVertices[0], _polygonVertices[1], _polygonVertices[i]);
        }
      return null;
    }

    Point Center(Point p1, Point p2, Point p3)
    {
      double x = (p1.X + p2.X + p3.X)/3;
      double y = (p1.Y + p2.Y + p3.Y)/3;

      var center = new Point(x, y);
      return center;
    }

    int FindZoneIndex()
    {
      for (var i = 0; i < _polygonVertices.Length - 1; i++)
      {
        var rayGoingThroughNextPoint = new Ray(_interiorPoint, _polygonVertices[i + 1]);
        var rayGoingThroughCurrentPoint = new Ray(_interiorPoint, _polygonVertices[i]);

        OnSearchingInZone(new[] {rayGoingThroughCurrentPoint, rayGoingThroughNextPoint});

        if (rayGoingThroughNextPoint.SideOfLine(_searchPoint) < (-double.Epsilon) &&
            rayGoingThroughCurrentPoint.SideOfLine(_searchPoint) > double.Epsilon)
          return i;
      }
      OnSearchingInZone(new[]
      {
        new Ray(_interiorPoint, _polygonVertices.Last()), 
        new Ray(_interiorPoint, _polygonVertices.First()),
      });
      return _polygonVertices.Length - 1;
    }

    protected virtual void OnInteriorPointFound(Point obj)
    {
      Action<Point> handler = InteriorPointFound;
      if (handler != null) handler(obj);
    }

    protected virtual void OnSearchingInZone(Ray[] obj)
    {
      Action<Ray[]> handler = SearchingInZone;
      if (handler != null) handler(obj);
    }

    protected virtual void OnEdgeToCompareFound(LineSegment obj)
    {
      Action<LineSegment> handler = EdgeToCompareFound;
      if (handler != null) handler(obj);
    }
  }
}