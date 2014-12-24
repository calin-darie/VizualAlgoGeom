using System;

namespace VizualAlgoGeom
{
  internal class ClippingBoundsEventArgs : EventArgs
  {
    internal ClippingBoundsEventArgs(double newLeft, double newRight, double newBottom, double newTop)
    {
      Left = newLeft;
      Right = newRight;
      Bottom = newBottom;
      Top = newTop;
    }

    internal double Left { get; set; }
    internal double Right { get; set; }
    internal double Bottom { get; set; }
    internal double Top { get; set; }
  }
}