using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews.TextPositions
{
  internal class Left : Position
  {
    public override void AcceptTextPositionBuilder(TextPositionBuilder builder)
    {
      builder.TextHorizontalPosition = TextHorizontalPosition.RightOfPoint;
    }

    public override int Compare(Point x, Point y)
    {
      return CompareLeftmostGreater(x, y);
    }
  }
}