using System;
using System.Collections.Generic;
using Infrastructure;

namespace GeometricElements
{
  [Serializable]
  public class Line
  {
    protected const double Epsilon = 0.001;
    protected double _a;
    protected double _b;
    protected double _c;

    public Line(Line source)
      : this(source._a, source._b, source._c)
    {
    }

    public Line(double a, double b, double c)
    {
      _a = a;
      _b = b;
      _c = c;
    }

    public bool IntersectVertical(double x, out double y)
    {
      if (Numbers.EqualTolerant(_b, 0))
      {
        if (Numbers.EqualTolerant(_a * x + _c, 0))
        {
          y = x;
          return true;
        }
        y = 0;
        return false;
      }
      y = -(_a * x + _c) / _b;
      return true;
    }

    public bool IntersectHorizontal(double y, out double x)
    {
      if (Numbers.EqualTolerant(_a, 0))
      {
        if (Numbers.EqualTolerant(_b * y + _c, 0))
        {
          x = y;
          return true;
        }
        x = 0;
        return false;
      }
      x = -(_b * y + _c) / _a;
      return true;
    }

    public IList<Point> IntersectionsWithRectangle(Rectangle r)
    {
      return IntersectionsWithRectangle(r.XLeft, r.XRight, r.YBottom, r.YTop);
    }

    // XXX vezi cursuri
    public IList<Point> IntersectionsWithRectangle(
      double left, double right, double bottom, double top)
    {
      double x;
      double y;
      var response = new List<Point>();

      if (IntersectVertical(left, out y) && (y >= bottom && y <= top))
      {
        response.Add(new Point(left, y));
      }

      if (IntersectVertical(right, out y) && (y >= bottom && y <= top))
      {
        response.Add(new Point(right, y));
      }

      if (IntersectHorizontal(bottom, out x) && (x >= left && x <= right))
      {
        response.Add(new Point(x, bottom));
      }

      if (IntersectHorizontal(top, out x) && (x >= left && x <= right))
      {
        response.Add(new Point(x, top));
      }
      return response;
    }

    public bool Contains(Point point)
    {
      return Math.Abs(_a * point.X + _b * point.Y + _c) <= Epsilon;
    }

    public Point IntersectionPointWith(Line other)
    {
      Line e1 = this;
      Line e2 = other;

      if (e2 == null)
      {
        return null;
      }

      double d, xint, yint;

      d = e1._a * e2._b - e1._b * e2._a;
      if (-1.0e-10 < d && d < 1.0e-10)
      {
        return null;
      }

      xint = (e1._c * e2._b - e2._c * e1._b) / d;
      yint = (e2._c * e1._a - e1._c * e2._a) / d;

      var v = new Point(-xint, -yint);
      return v;
    }

    public static Line BisectorOf(Point leftPoint, Point rightPoint)
    {
      var midPoint = new Point((leftPoint.X + rightPoint.X) / 2, (leftPoint.Y + rightPoint.Y) / 2);

      Line perpendicularBisector = PerpendicularContaining(midPoint, leftPoint, rightPoint);

      return perpendicularBisector;
    }

    static Line PerpendicularContaining(
      Point containedPoint,
      Point leftPoint,
      Point rightPoint)
    {
      double a, b, c;

      double dy = rightPoint.Y - leftPoint.Y;
      if (Numbers.EqualTolerant(dy, 0))
      {
        a = 1;
        b = 0;
      }
      else
      {
        double dx = rightPoint.X - leftPoint.X;

        a = -(dx / dy);
        b = -1;
      }
      c = -(a * containedPoint.X + b * containedPoint.Y);

      return new Line(a, b, c);
    }

    public static Point FootOfPerpendicularFrom(Point p, Point leftPoint, Point rightPoint)
    {
      Line perpendicular = PerpendicularContaining(p, leftPoint, rightPoint);
      return perpendicular.IntersectionPointWith(new PointPair(leftPoint, rightPoint).Line);
    }

    public static Line HorizontalWithY(double y)
    {
      return new Line(0, 1, -y);
    }

    /* Input:  three points P0, P1, and P2
     * Return: positive for P2 left of the line through P0 and P1
     *         0 for P2 on the line
     *         negative for P2 right of the line*/

    public static double SideOfLine(Point p0, Point p1, Point p2)
    {
      return p1.X * p2.Y + p0.X * p1.Y + p2.X * p0.Y
             - p1.X * p0.Y - p2.X * p1.Y - p0.X * p2.Y;
    }

    public double A { get { return _a; } }
    public double B { get { return _b; } }
    public double C { get { return _c; } }
  }
}