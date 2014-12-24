using System.Collections.Generic;
using GeometricElements;

namespace DefaultCanvasViews
{
  internal abstract class Position : IComparer<Point>
  {
    public abstract int Compare(Point x, Point y);
    public abstract void AcceptTextPositionBuilder(TextPositionBuilder builder);

    protected int CompareLeftmostGreater(
      Point pointA, Point pointB)
    {
      return pointA.X > pointB.X ? 1 : -1;
    }

    protected int CompareTopmostGreater(
      Point pointA, Point pointB)
    {
      return pointA.Y > pointB.Y ? 1 : -1;
    }
  }
}