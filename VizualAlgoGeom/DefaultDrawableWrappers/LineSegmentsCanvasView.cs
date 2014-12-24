using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class LineSegmentsCanvasView : CanvasViewBase, ICanvasView<IEnumerable<LineSegment>>,
    ICanvasView<List<LineSegment>>, ICanvasView<LineSegment[]>
  {
    public void Draw(DrawCommand<IEnumerable<LineSegment>> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      using (DrawingTool lineTool = context.DrawingTools.GetLineTool().Begin())
      {
        foreach (LineSegment seg in command.Object)
        {
          lineTool.Vertex(seg.FirstPoint);
          lineTool.Vertex(seg.SecondPoint);
        }
      }

      if (command.Style.ShouldShowPoints)
      {
        using (DrawingTool pointTool = context.DrawingTools.GetPointTool().Begin())
        {
          foreach (LineSegment seg in command.Object)
          {
            pointTool.Vertex(seg.FirstPoint);
            pointTool.Vertex(seg.SecondPoint);
          }
        }
        PrintPointNames(command, context);
      }
    }

    public void Draw(DrawCommand<LineSegment[]> command, DrawingContext context)
    {
      Draw(new DrawCommand<IEnumerable<LineSegment>>(command.Object, command.Style), context);
    }

    public void Draw(DrawCommand<List<LineSegment>> command, DrawingContext context)
    {
      Draw(new DrawCommand<IEnumerable<LineSegment>>(command.Object, command.Style), context);
    }

    void PrintPointNames(DrawCommand<IEnumerable<LineSegment>> command, DrawingContext context)
    {
      var font = new Font(new FontFamily(GenericFontFamilies.SansSerif), context.FontSize);
      TextTool textTool = context.DrawingTools.GetTextTool();
      var index = 1;
      foreach (LineSegment seg in command.Object)
      {
        PrintPointName(seg,
          new VisualStyle(command.Style.Color,
            string.Format("{0}_{1}", command.Style.Name, index), command.Style.TextPosition),
          font,
          textTool,
          context.CanvasSizePx.Height);
        ++index;
      }
    }

    void PrintPointName(LineSegment seg, VisualStyle visualStyle, Font font, TextTool textTool, int canvasHeightPx)
    {
      textTool.PrintName(seg.FirstPoint, visualStyle, canvasHeightPx, font);
      textTool.PrintName(seg.SecondPoint, visualStyle, canvasHeightPx, font);
    }
  }
}