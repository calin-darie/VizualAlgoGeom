using System;
using System.Collections.Generic;
using System.Linq;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class CanvasViewRegistry : MarshalByRefObject
  {
    readonly Dictionary<Type, object> _registry = new Dictionary<Type, object>();

    public void RegisterView<TEntity, TView>(TView view)
      where TView : ICanvasView<TEntity>
    {
      _registry[typeof (TEntity)] = view;
    }

    public IPendingDraw GetPendingDraw<TypeToDraw>(TypeToDraw objectToDraw, VisualStyle visualStyle)
    {
      ICanvasView<TypeToDraw> view = GetView<TypeToDraw>();

      if (view == null) return null;

      return new PendingDraw<TypeToDraw>(new DrawCommand<TypeToDraw>(objectToDraw, visualStyle), view);
    }

    public ICanvasView<TypeToDraw> GetView<TypeToDraw>()
    {
      object canvasViewAsObject;
      _registry.TryGetValue(typeof (TypeToDraw), out canvasViewAsObject);

      var view = canvasViewAsObject as ICanvasView<TypeToDraw> 
        ?? _registry
        .Where(kvp => kvp.Key.IsAssignableFrom(typeof (TypeToDraw)))
        .Select(kvp => kvp.Value).FirstOrDefault()
        as ICanvasView<TypeToDraw>;

      return view;
    }
  }
}