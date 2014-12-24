using System;
using System.Collections.Generic;
using System.Linq;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using OX.Copyable;

namespace InterfaceOfSnapshotsWithVisualizer
{
  public class ObjectSnapshot<T> : IObjectSnapshot
  {
    public void CollectDrawableWrappers(CanvasViewRegistry drawableWrapperFactory, IList<IPendingDraw> wrappers)
    {
      foreach (IPendingDraw itemWrapper in _values
        .Select(item => drawableWrapperFactory.GetPendingDraw(item, _visualStyle))
        .Where(itemWrapper => itemWrapper != null))
      {
        wrappers.Add(itemWrapper);
      }
    }

    public IObjectSnapshot CloneObjectForSnapshot()
    {
      T[] clone = _values.Select(TryClone)
        .Where(c => c != null)
        .ToArray();
      return new ObjectSnapshot<T>(clone, _visualStyle);
    }

    public IObjectSnapshot ReplaceObject<TOther>(IEnumerable<TOther> newObjects)
    {
      return new ObjectSnapshot<TOther>(newObjects, _visualStyle);
    }

    readonly IEnumerable<T> _values;
    readonly VisualStyle _visualStyle;

    public ObjectSnapshot(T value, VisualStyle visualStyle)
      : this(new[] {value}, visualStyle)
    {
    }

    public ObjectSnapshot(IEnumerable<T> values, VisualStyle visualStyle)
    {
      if (values == null)
      {
        throw new ArgumentNullException("values");
      }
      _values = values;
      _visualStyle = visualStyle;
    }

    T TryClone(T value)
    {
      try
      {
        return (T) value.Copy();
      }
      catch
      {
        var cloneable = value as ICloneable;
        if (cloneable == null)
        {
          return default(T);
        }
        return (T) cloneable.Clone();
      }
    }
  }
}