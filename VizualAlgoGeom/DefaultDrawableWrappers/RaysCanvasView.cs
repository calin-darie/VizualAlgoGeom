using System;
using System.Collections.Generic;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class RaysCanvasView : CanvasViewBase, ICanvasView<IEnumerable<Ray>>, ICanvasView<List<Ray>>,
    ICanvasView<Ray[]>
  {
    public void Draw(DrawCommand<IEnumerable<Ray>> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      using (DrawingTool lineTool = context.DrawingTools.GetLineTool().Begin())
      {
        foreach (Ray r in command.Object)
        {
          _rayCanvasView.DrawVisiblePartOfLine(r, context, lineTool);
        }
      }
    }

    public void Draw(DrawCommand<List<Ray>> command, DrawingContext context)
    {
      Draw(new DrawCommand<IEnumerable<Ray>>(command.Object, command.Style), context);
    }

    public void Draw(DrawCommand<Ray[]> command, DrawingContext context)
    {
      Draw(new DrawCommand<IEnumerable<Ray>>(command.Object, command.Style), context);
    }

    readonly RayCanvasView _rayCanvasView;

    public RaysCanvasView(RayCanvasView rayCanvasView)
    {
      if (rayCanvasView == null)
      {
        throw new ArgumentNullException("rayCanvasView");
      }
      _rayCanvasView = rayCanvasView;
    }
  }
}