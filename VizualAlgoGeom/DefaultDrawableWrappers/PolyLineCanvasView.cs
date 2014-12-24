using System.Collections.Generic;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class PolyLineCanvasView : CanvasViewBase, ICanvasView<PolyLine>
  {
    public void Draw(DrawCommand<PolyLine> command, DrawingContext context)
    {
      if (command.Style.ShouldShowPoints)
      {
        _pointsView.Draw(new DrawCommand<IEnumerable<Point>>(command.Object.Points, command.Style), context);
      }

      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);

      DrawingTool tool = command.Object.Closed
        ? context.DrawingTools.GetLineLoopTool()
        : context.DrawingTools.GetLineStripTool();

      using (tool.Begin())
      {
        foreach (Point p in command.Object.Points)
        {
          tool.Vertex(p);
        }
      }
    }

    readonly ICanvasView<IEnumerable<Point>> _pointsView;

    public PolyLineCanvasView(ICanvasView<IEnumerable<Point>> pointsView)
    {
      _pointsView = pointsView;
    }
  }
}