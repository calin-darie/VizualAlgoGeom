using System;
using GeometricElements;

namespace VoronoiAlgorithmAdapter.Geometry
{
  public class ParabolicArc : MarshalByRefObject
  {
    public ParabolicArc()
    {
      XRight = double.MaxValue;
      XLeft = double.MinValue;
    }

    public double XRight { get; set; }
    public double XLeft { get; set; }
    public Point Focus { get; set; }
    public double DirectrixY { get; set; }

    public double GetY(double x)
    {
      return (Focus.Y + DirectrixY)/2
             + (Focus.X - x)*(Focus.X - x)/(2*(Focus.Y - DirectrixY));
    }
  }
}