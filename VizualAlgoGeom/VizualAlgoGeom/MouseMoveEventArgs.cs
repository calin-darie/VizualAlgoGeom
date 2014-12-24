using System.Windows.Forms;

namespace VizualAlgoGeom
{
  internal class MouseMoveEventArgs : MouseEventArgs
  {
    internal MouseMoveEventArgs(MouseButtons mouseButton, int clicks, int x, int y, int delta, int newXStart,
      int newYStart)
      : base(mouseButton, clicks, x, y, delta)
    {
      XStart = newXStart;
      YStart = newYStart;
    }

    public int XStart { get; set; }
    public int YStart { get; set; }
  }
}