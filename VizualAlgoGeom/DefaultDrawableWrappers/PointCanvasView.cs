using System.Drawing;
using System.Drawing.Text;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using Point = GeometricElements.Point;

namespace DefaultCanvasViews
{
  public class PointCanvasView : CanvasViewBase, ICanvasView<Point>
  {
    public void Draw(DrawCommand<Point> command, DrawingContext context)
    {
      DrawPoint(command, context);

      PrintName(command, context);
    }

    static void DrawPoint(DrawCommand<Point> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      using (DrawingTool pointTool = context.DrawingTools.GetPointTool().Begin())
      {
        pointTool.Vertex(command.Object);
      }
    }

    static void PrintName(DrawCommand<Point> command, DrawingContext context)
    {
      var font = new Font(new FontFamily(GenericFontFamilies.SansSerif), context.FontSize);
      //todo: move to context / hints
      TextTool textTool = context.DrawingTools.GetTextTool();

      textTool.PrintName(command.Object, command.Style, context.CanvasSizePx.Height, font);
    }
  }
}