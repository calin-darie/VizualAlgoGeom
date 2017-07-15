using System;
using System.ComponentModel;
using Infrastructure;

namespace ToolboxGeometricElements
{
  public class Point : GeometricElement
  {
    double _x;
    double _y;

    /// <summary>
    ///   needed for serialization only
    /// </summary>
    public Point():base("", null, null) { }

    public Point(double x, double y, string name = "")
      : this(x, y, name, new Group(), System.Drawing.Color.Black)
    {
    }

    public Point(double x, double y, String newName, Group newGroup, Color newColor)
      : base(newName, newGroup, newColor)
    {
      _x = x;
      _y = y;
    }

    [Category("Miscelaneous"),
     DisplayName("X"),
     Description("X coordonate of the point"),
     Show(true)]
    public double X
    {
      get { return _x; }
      set
      {
        _x = value;
        NotifyPropertyChanged("X");
      }
    }

    [Category("Miscelaneous"),
     DisplayName("Y"),
     Description("Y coordonate of the point"),
     Show(true)]
    public double Y
    {
      get { return _y; }
      set
      {
        _y = value;
        NotifyPropertyChanged("Y");
      }
    }

    public static implicit operator GeometricElements.Point(Point p)
    {
      return new GeometricElements.Point(p.X, p.Y);
    }
  }
}