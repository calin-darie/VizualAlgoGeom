using System;
using OpenTK.Graphics.OpenGL;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class DrawingToolFactory : MarshalByRefObject
  {
    readonly CircleTool _circleTool = new CircleTool();
    readonly ColorPalette _colorPalette = new ColorPalette();
    readonly TextTool _textTool = new TextTool();

    public DrawingTool GetLineTool()
    {
      return new DrawingTool(BeginMode.Lines);
    }

    public ColorPalette GetColorPalette()
    {
      return _colorPalette;
    }

    public CircleTool GetCircleTool()
    {
      return _circleTool;
    }

    public DrawingTool GetPointTool()
    {
      return new DrawingTool(BeginMode.Points);
    }

    public TextTool GetTextTool()
    {
      return _textTool;
    }

    public DrawingTool GetLineLoopTool()
    {
      return new DrawingTool(BeginMode.LineLoop);
    }

    public DrawingTool GetLineStripTool()
    {
      return new DrawingTool(BeginMode.LineStrip);
    }
  }
}