using System.Collections.Generic;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace VoronoiAlgorithmAdapter.CanvasViews
{
  internal class DiagramView : ICanvasView<Diagram>
  {
    public void Draw(DrawCommand<Diagram> command, DrawingContext context)
    {
      Diagram diagram = command.Object;
      foreach (Line line in diagram.Lines)
      {
        _lineView.Draw(new DrawCommand<Line>(line, command.Style), context);
      }
      _raysView.Draw(new DrawCommand<IEnumerable<Ray>>(diagram.Rays, command.Style), context);
      _segmentsView.Draw(new DrawCommand<IEnumerable<LineSegment>>(diagram.Segments, command.Style), context);
    }

    readonly ICanvasView<Line> _lineView;
    readonly ICanvasView<IEnumerable<Ray>> _raysView;
    readonly ICanvasView<IEnumerable<LineSegment>> _segmentsView;

    public DiagramView(ICanvasView<Line> lineView, ICanvasView<IEnumerable<Ray>> raysView,
      ICanvasView<IEnumerable<LineSegment>> segmentsView)
    {
      _lineView = lineView;
      _raysView = raysView;
      _segmentsView = segmentsView;
    }
  }
}