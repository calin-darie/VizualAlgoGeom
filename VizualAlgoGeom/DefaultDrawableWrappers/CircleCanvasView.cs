using System;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class CircleCanvasView : MarshalByRefObject, ICanvasView<Circle>
  {
    public void Draw(DrawCommand<Circle> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      Circle circle = command.Object;
      context.DrawingTools.GetCircleTool().Draw(circle, context.PixelSizeInWorldUnits);
    }
  }
}