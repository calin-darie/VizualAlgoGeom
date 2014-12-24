using System;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public enum TextVerticalPosition
  {
    AbovePoint,
    BelowPoint
  }

  public enum TextHorizontalPosition
  {
    LeftOfPoint,
    RightOfPoint
  }

  public class TextPosition : MarshalByRefObject
  {
    public TextPosition()
      : this(TextHorizontalPosition.RightOfPoint, TextVerticalPosition.BelowPoint)
    {
    }

    public TextPosition(TextHorizontalPosition horizontalPosition,
      TextVerticalPosition verticalPosition)
    {
      HorizontalPosition = horizontalPosition;
      VerticalPosition = verticalPosition;
    }

    public TextHorizontalPosition HorizontalPosition { get; set; }
    public TextVerticalPosition VerticalPosition { get; set; }
  }
}