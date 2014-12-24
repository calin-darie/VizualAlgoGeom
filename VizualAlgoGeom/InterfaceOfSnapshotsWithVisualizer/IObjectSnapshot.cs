using System.Collections.Generic;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace InterfaceOfSnapshotsWithVisualizer
{
  public interface IObjectSnapshot
  {
    void CollectDrawableWrappers(CanvasViewRegistry drawableWrapperFactory, IList<IPendingDraw> wrappers);
    IObjectSnapshot CloneObjectForSnapshot();
    IObjectSnapshot ReplaceObject<TOther>(IEnumerable<TOther> newObject);
  }
}