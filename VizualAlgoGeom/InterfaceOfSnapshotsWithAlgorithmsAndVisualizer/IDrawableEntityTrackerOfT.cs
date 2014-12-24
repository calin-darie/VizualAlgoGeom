using System.Collections.Generic;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public interface IDrawableEntityTracker<in T> : IDrawableEntityTracker
  {
    void Update(T newElement);
    void Update(IEnumerable<T> enumerable);
  }
}