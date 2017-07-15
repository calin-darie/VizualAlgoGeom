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
      return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X.GetHashCode()*397) ^ Y.GetHashCode();
      }
    }

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
      X = x;
      Y = y;
    }
    
    public double X { get; private set; }
    
    public double Y { get; private set; }

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