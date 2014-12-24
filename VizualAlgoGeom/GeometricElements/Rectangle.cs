using System;

namespace GeometricElements
{
  [Serializable]
  public class Rectangle
  {
    readonly double _xLeft;
    readonly double _xRight;
    readonly double _yBottom;
    readonly double _yTop;

    public Rectangle(double xLeft, double xRight, double yBottom, double yTop)
    {
      AssertArgument.IsFiniteNumber(xLeft, "xLeft");
      AssertArgument.IsFiniteNumber(xRight, "xRight");
      AssertArgument.IsFiniteNumber(yBottom, "yBottom");
      AssertArgument.IsFiniteNumber(yTop, "yTop");
      AssertArgument.AscendingOrderStrict(xLeft, xRight, "x coordinates");
      AssertArgument.AscendingOrderStrict(yBottom, yTop, "y coordinates");

      _xLeft = xLeft;
      _xRight = xRight;
      _yBottom = yBottom;
      _yTop = yTop;
    }

    public double XLeft
    {
      get { return _xLeft; }
    }

    public double XRight
    {
      get { return _xRight; }
    }

    public double YBottom
    {
      get { return _yBottom; }
    }

    public double YTop
    {
      get { return _yTop; }
    }
  }
}