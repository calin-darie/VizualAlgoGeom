using System;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using VoronoiAlgorithmAdapter.Geometry;

namespace VoronoiAlgorithmAdapter.CanvasViews
{
  public class FrontLineView : ICanvasView<ParabolicArc[]>
  {
    public void Draw(DrawCommand<ParabolicArc[]> command, DrawingContext context)
    {
      ParabolicArc[] arcs = command.Object;
      var shouldStartNewSegment = false;
      using (DrawingTool lineTool = context.DrawingTools.GetLineTool().Begin())
      {
        foreach (ParabolicArc arc in arcs)
        {
          for (double x = Math.Max(arc.XLeft, context.ViewWindowWorldUnits.Left);
            x <= Math.Min(arc.XRight, context.ViewWindowWorldUnits.Right);
            x += context.PixelSizeInWorldUnits.Width)
          {
            double y = arc.GetY(x);
            if (context.ViewWindowWorldUnits.Bottom < y && y < context.ViewWindowWorldUnits.Top)
            {
              lineTool.Vertex(x, y);
              if (shouldStartNewSegment)
              {
                lineTool.Vertex(x, y);
              }
              shouldStartNewSegment = true;
            }
          }
        }
      }
    }
  }
}