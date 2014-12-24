using System;
using System.Collections.Generic;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public interface ISnapshotRecorder
  {
    void TakeSnapshot(SnapshotDescription description);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(Func<TGeometricElement> geometricElementGetter,
      VisualStyle visualStyle);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      Func<IEnumerable<TGeometricElement>> geometricElementGetter, VisualStyle visualStyle);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      Func<List<TGeometricElement>> geometricElementGetter, VisualStyle visualStyle);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(Func<TGeometricElement[]> geometricElementGetter,
      VisualStyle visualStyle);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(IList<TGeometricElement> geometricElements);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(IList<TGeometricElement> geometricElement,
      VisualStyle visualStyle);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(TGeometricElement geometricElement);

    IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(TGeometricElement geometricElement,
      VisualStyle visualStyle);
  }
}