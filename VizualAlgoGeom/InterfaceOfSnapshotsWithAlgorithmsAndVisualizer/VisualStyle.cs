using Infrastructure;
using System;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class VisualStyle : MarshalByRefObject
  {
    static readonly Color DefaultColor = System.Drawing.Color.Black;

    public VisualStyle()
      : this(DefaultColor, string.Empty, shouldShowPoints: true)
    {
    }

    public VisualStyle(Color color, string name, bool shouldShowPoints = false)
      : this(color, name, CreateDefaultTextPosition(), shouldShowPoints)
    {
    }

    public VisualStyle(Color color, bool shouldShowPoints = false)
      : this(color, string.Empty, CreateDefaultTextPosition(), shouldShowPoints)
    {
    }

    public VisualStyle(Color color, string name, TextPosition textPosition, bool shouldShowPoints = true)
    {
      Name = name;
      Color = color;
      TextPosition = textPosition;
      ShouldShowPoints = shouldShowPoints;
    }

    public string Name { get; set; }
    public Color Color { get; set; }
    public TextPosition TextPosition { get; set; }
    public bool ShouldShowPoints { get; set; }

    static TextPosition CreateDefaultTextPosition()
    {
      return new TextPosition();
    }

    public VisualStyle WithFormattedName(params object[] args)
    {
      return new VisualStyle(Color, string.Format(Name, args), ShouldShowPoints);
    }
  }
}