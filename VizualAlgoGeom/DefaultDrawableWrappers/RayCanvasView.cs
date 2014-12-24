using System.Collections.Generic;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class RayCanvasView : LineCanvasView, ICanvasView<Ray>
  {
    public void Draw(DrawCommand<Ray> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      using (DrawingTool lineTool = context.DrawingTools.GetLineTool().Begin())
      {
        DrawVisiblePartOfLine(command.Object, context, lineTool);
      }
    }

    public RayCanvasView(PointCanvasView pointView)
      : base(pointView)
    {
    }

    public void DrawVisiblePartOfLine(Ray ray, DrawingContext context, DrawingTool lineTool)
    {
      //Drawing part of the ray
      IList<Point> pointsToDraw = ray.IntersectionsWithRectangle(
        new Rectangle(context.ViewWindowWorldUnits.Left, context.ViewWindowWorldUnits.Right,
          context.ViewWindowWorldUnits.Bottom, context.ViewWindowWorldUnits.Top));
      int numPoints = pointsToDraw.Count;
      switch (numPoints)
      {
        default:
          return;
        case 1:
          pointsToDraw.Add(ray.FirstPoint);
          break;
        case 2:
          // nothing to add
          break;
      }
      foreach (Point p in pointsToDraw)
      {
        lineTool.Vertex(p);
      }
    }
  }
}