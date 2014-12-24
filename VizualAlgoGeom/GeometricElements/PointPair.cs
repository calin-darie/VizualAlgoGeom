using System;

namespace GeometricElements
{
  [Serializable]
  public class PointPair : IEquatable<PointPair>
  {
    public bool Equals(PointPair otherPointPair)
    {
      return (FirstPoint.Equals(otherPointPair.FirstPoint) && SecondPoint.Equals(otherPointPair.SecondPoint)) ||
             (SecondPoint.Equals(otherPointPair.FirstPoint) && FirstPoint.Equals(otherPointPair.SecondPoint));
    }

    public PointPair(Point firstPoint, Point secondPoint)
    {
      FirstPoint = firstPoint;
      SecondPoint = secondPoint;
    }

    public PointPair()
    {
      FirstPoint = new Point();
      SecondPoint = new Point();
    }

    public Point FirstPoint { get; set; }
    public Point SecondPoint { get; set; }

    public Line Line
    {
      get
      {
        double a, b, c;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        // avoiding division by zero
        if (FirstPoint.X != SecondPoint.X)
        {
          a = (FirstPoint.Y - SecondPoint.Y)/(FirstPoint.X - SecondPoint.X);
          b = -1;
          c = FirstPoint.Y - a*FirstPoint.X;
        }
        else
        {
          a = 1;
          b = 0;
          c = -FirstPoint.X;
        }
        return new Line(a, b, c);
      }
    }

    public override bool Equals(object obj)
    {
      var otherPointPair = obj as PointPair;
      if (otherPointPair == null)
        return false;
      return Equals(otherPointPair);
    }

    public bool SegmentContains(Point p)
    {
      double xStart = FirstPoint.X;
      double yStart = FirstPoint.Y;
      double xEnd = SecondPoint.X;
      double yEnd = SecondPoint.Y;
      double x = p.X;
      double y = p.Y;

      return (
        ((x >= xStart && x <= xEnd) || (x <= xStart && x >= xEnd)) &&
        ((y >= yStart && y <= yEnd) || (y <= yStart && y >= yEnd))) && Line.Contains(p);
    }

    public double SideOfLine(Point p)
    {
      Point p0 = FirstPoint;
      Point p1 = SecondPoint;
      return Line.SideOfLine(p0, p1, p);
    }
  }
}