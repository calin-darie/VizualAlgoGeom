using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews.TextPositions
{
  internal class Right : Position
  {
    public override void AcceptTextPositionBuilder(TextPositionBuilder builder)
    {
      builder.TextHorizontalPosition = TextHorizontalPosition.LeftOfPoint;
    }

    public override int Compare(Point x, Point y)
    {
      return -CompareLeftmostGreater(x, y);
    }
  }
}