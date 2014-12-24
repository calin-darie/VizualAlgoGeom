using System;
using Infrastructure;

namespace GeometricElements
{
  [Serializable]
  public class Circle
  {
    readonly Point _center;
    readonly double _radius;

    public Circle(Point center, double radius)
    {
      _center = center;
      _radius = radius;
    }

    public double Radius
    {
      get { return _radius; }
    }

    public double CenterX
    {
      get { return _center.X; }
    }

    public double CenterY
    {
      get { return _center.Y; }
    }

    public Point Center
    {
      get { return new Point(_center); }
    }

    public bool InteriorContains(Point p)
    {
      return Numbers.EqualTolerant(Point.DistanceBetween(p, Center),  Radius);
    }

    public static Circle FindByCenterAndPointOnCircumference(Point center, Point onCircumference)
    {
      return new Circle(center, Point.DistanceBetween(center, onCircumference));
    }

    public static Circle FindCircumscribed(Point point1, Point point2, Point point3)
    {
      Point center = Line.BisectorOf(point1, point2).IntersectionPointWith(
        Line.BisectorOf(point2, point3));
      return FindByCenterAndPointOnCircumference(center, point1);
    }
  }
}