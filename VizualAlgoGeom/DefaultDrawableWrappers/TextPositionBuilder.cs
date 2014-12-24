using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  internal class TextPositionBuilder
  {
    public TextHorizontalPosition TextHorizontalPosition { get; set; }
    public TextVerticalPosition TextVerticalPosition { get; set; }

    public TextPosition TextPosition
    {
      get { return new TextPosition(TextHorizontalPosition, TextVerticalPosition); }
    }
  }
}