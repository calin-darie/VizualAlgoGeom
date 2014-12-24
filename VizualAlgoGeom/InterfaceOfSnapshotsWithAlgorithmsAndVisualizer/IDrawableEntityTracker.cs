using System;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public interface IDrawableEntityTracker : IDisposable
  {
    void Update();
  }
}