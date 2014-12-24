using System;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class LineSegmentCanvasView :MarshalByRefObject, ICanvasView<LineSegment>
  {
    readonly PointCanvasView _pointView;

    public void Draw(DrawCommand<LineSegment> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      _segment = command.Object;
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      if (command.Style.ShouldShowPoints)
      {
        _pointView.Draw(
          new DrawCommand<Point>(command.Object.FirstPoint,
            new VisualStyle(command.Style.Color, string.Format("{0}_p1", command.Style.Name), command.Style.TextPosition)),
          context);
        _pointView.Draw(
          new DrawCommand<Point>(command.Object.SecondPoint,
            new VisualStyle(command.Style.Color, string.Format("{0}_p2", command.Style.Name), command.Style.TextPosition)),
          context);
      }

      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      using (DrawingTool lineTool = context.DrawingTools.GetLineTool().Begin())
      {
        lineTool.Vertex(_segment.FirstPoint);
        lineTool.Vertex(_segment.SecondPoint);
      }
    }

    LineSegment _segment;

    public LineSegmentCanvasView(PointCanvasView pointView)
    {
      _pointView = pointView;
    }
  }
}