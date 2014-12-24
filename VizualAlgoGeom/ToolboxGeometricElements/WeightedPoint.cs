using System;
using System.ComponentModel;
using System.Drawing;

namespace ToolboxGeometricElements
{
  public class WeightedPoint : Point
  {
    double _weight;

    public WeightedPoint(double x, double y, String newName, Group newGroup, Color newColor)
      : base(x, y, newName, newGroup, newColor)
    {
      Weight = 1;
    }

    [Category("Miscelaneous"),
     DisplayName("Weight"),
     Description("The point's weight. Only pozitive values."),
     Show(true)]
    public double Weight
    {
      get { return _weight; }
      set
      {
        if (value > 0)
        {
          _weight = value;
          NotifyPropertyChanged("Weight");
        }
      }
    }
  }
}