using System;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  internal class PendingDraw<T> : MarshalByRefObject, IPendingDraw
  {
    public void Execute(DrawingContext drawing)
    {
      _view.Draw(_drawCommand, drawing);
    }

    readonly DrawCommand<T> _drawCommand;
    readonly ICanvasView<T> _view;

    internal PendingDraw(DrawCommand<T> drawCommand, ICanvasView<T> view)
    {
      _drawCommand = drawCommand;
      _view = view;
    }
  }
}