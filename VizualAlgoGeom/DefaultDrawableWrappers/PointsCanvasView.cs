using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using Point = GeometricElements.Point;

namespace DefaultCanvasViews
{
  public class PointsCanvasView : CanvasViewBase,
    ICanvasView<IEnumerable<Point>>,
    ICanvasView<List<Point>>,
    ICanvasView<Point[]>
  {
    public void Draw(DrawCommand<IEnumerable<Point>> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);

      DrawPoints(command, context);

      PrintNames(command, context);
    }

    public void Draw(DrawCommand<List<Point>> command, DrawingContext context)
    {
      Draw(new DrawCommand<IEnumerable<Point>>(command.Object, command.Style), context);
    }

    public void Draw(DrawCommand<Point[]> command, DrawingContext context)
    {
      Draw(new DrawCommand<IEnumerable<Point>>(command.Object, command.Style), context);
    }

    static void DrawPoints(DrawCommand<IEnumerable<Point>> command, DrawingContext context)
    {
      using (DrawingTool pointTool = context.DrawingTools.GetPointTool().Begin())
      {
        foreach (Point p in command.Object)
        {
          pointTool.Vertex(p);
        }
      }
    }

    static void PrintNames(DrawCommand<IEnumerable<Point>> command, DrawingContext context)
    {
      var font = new Font(new FontFamily(GenericFontFamilies.SansSerif), context.FontSize);
      //todo: move to context / hints
      TextTool textTool = context.DrawingTools.GetTextTool();

      var index = 1;
      foreach (Point p in command.Object)
      {
        textTool.PrintName(p,
          new VisualStyle(command.Style.Color,
            string.Format("{0}_{1}", command.Style.Name, index), command.Style.TextPosition),
          context.CanvasSizePx.Height,
          font);
        index++;
      }
    }
  }
}