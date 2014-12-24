using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews.TextPositions
{
  internal class Top : Position
  {
    public override void AcceptTextPositionBuilder(TextPositionBuilder builder)
    {
      builder.TextVerticalPosition = TextVerticalPosition.BelowPoint;
    }

    public override int Compare(Point x, Point y)
    {
      return CompareTopmostGreater(x, y);
    }
  }
}