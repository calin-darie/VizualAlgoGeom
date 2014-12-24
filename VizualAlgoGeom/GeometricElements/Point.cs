using System;
using System.Diagnostics;

namespace GeometricElements
{
  [DebuggerDisplay("[{X}, {Y}]")]
  [Serializable]
  public class Point
  {
    protected bool Equals(Point other)
    {
      return _x.Equals(other._x) && _y.Equals(other._y);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (_x.GetHashCode()*397) ^ _y.GetHashCode();
      }
    }

    readonly double _x;
    readonly double _y;

    public Point()
      : this(0, 0)
    {
    }

    //copy constructor
    public Point(Point source)
      : this(source.X, source.Y)
    {
    }

    public Point(double x, double y)
    {
      AssertArgument.IsFiniteNumber(x, "x");
      AssertArgument.IsFiniteNumber(y, "y");
      _x = x;
      _y = y;
    }

    public double X
    {
      get { return _x; }
    }

    public double Y
    {
      get { return _y; }
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Point) obj);
    }

    public static double DistanceBetween(Point point1, Point point2)
    {
      double xDistance = point1.X - point2.X;
      double yDistance = point1.Y - point2.Y;
      return Math.Sqrt(xDistance*xDistance + yDistance*yDistance);
    }

    public override string ToString()
    {
      return string.Format("(x: {0}, y: {1})", X, Y);
    }
  }
}