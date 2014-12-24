using System;
using System.ComponentModel;
using GeometricElements;
using Infrastructure;

namespace ToolboxGeometricElements
{
  public class Line : GeometricElement
  {
    Point _firstPoint;
    Point _secondPoint;
    GeometricElements.Line _line;

    /// <summary>
    ///   needed for serialization only
    /// </summary>
    Line()
      : base(string.Empty, new Group(), System.Drawing.Color.Black)
    {
    }

    public Line(Point p1, Point p2, String newName, Group newGroup, Color newColor)
      : base(newName, newGroup, newColor)
    {
      FirstPoint = p1;
      SecondPoint = p2;
    }

    void UpdateLineEquation()
    {
      if (_secondPoint == null) return;
      _line = new PointPair(_firstPoint, _secondPoint).Line;
    }

    // From the two points we get the line equation: aX + bY + c = 0; 

    [Category("Line equation"),
     DisplayName("a"),
     Description("aX + bY + c = 0"),
     Show(true),
     ReadOnly(true)]
    public double A { get { return _line.A; } }

    [Category("Line equation"),
     DisplayName("b"),
     Description("aX + bY + c = 0"),
     Show(true),
     ReadOnly(true)]
    public double B { get { return _line.B; } }

    [Category("Line equation"),
     DisplayName("c"),
     Description("aX + bY + c = 0"),
     Show(true),
     ReadOnly(true)]
    public double C { get { return _line.C; } }

    public Point FirstPoint
    {
      get { return _firstPoint; }
      set
      {
        _firstPoint = value;
        _firstPoint.PropertyChanged += delegate { UpdateLineEquation(); };
        UpdateLineEquation();
        NotifyPropertyChanged("FirstPoint");
      }
    }

    public Point SecondPoint
    {
      get { return _secondPoint; }
      set
      {
        _secondPoint = value;
        _secondPoint.PropertyChanged += delegate { UpdateLineEquation(); };
        UpdateLineEquation();
        NotifyPropertyChanged("SecondPoint");
      }
    }

    public bool IntersectVertical(double x, out double y)
    {
      return _line.IntersectVertical(x, out y);
    }

    public bool IntersectHorizontal(double y, out double x)
    {
      return _line.IntersectHorizontal(y, out x);
    }

    public bool RayContains(GeometricElements.Point p)
    {
      return new LineSegment(FirstPoint, SecondPoint).SegmentContains(p) ||
             new LineSegment(FirstPoint, p).SegmentContains(SecondPoint);
    }

    internal System.Collections.Generic.IList<GeometricElements.Point> IntersectionsWithRectangle(double left, double right, double bottom, double top)
    {
      return _line.IntersectionsWithRectangle(left, right, bottom, top);
    }
  }
}