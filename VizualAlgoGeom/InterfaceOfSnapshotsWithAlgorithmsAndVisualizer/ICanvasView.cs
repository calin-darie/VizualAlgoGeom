namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public interface ICanvasView<TEntity>
  {
    void Draw(DrawCommand<TEntity> command, DrawingContext context);
  }
}