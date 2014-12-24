using System;
using System.Collections.Generic;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace Snapshots
{
  public class DrawableEntityTracker<TGeometricElement> : IDrawableEntityTracker<TGeometricElement>
  {
    public void Update()
    {
      Update(_geometricElementGetter());
    }

    public void Update(TGeometricElement newElement)
    {
      Update(new[] {newElement});
    }

    public void Update(IEnumerable<TGeometricElement> enumerable)
    {
      _snapshotRecorder.Replace(_snapshotObjectId, enumerable);
    }

    public void Dispose()
    {
      _snapshotRecorder.Remove(_snapshotObjectId);
    }

    int _snapshotObjectId;
    readonly Func<IEnumerable<TGeometricElement>> _geometricElementGetter;
    readonly SnapshotRecorder _snapshotRecorder;
    readonly VisualStyle _visualStyle;

    internal DrawableEntityTracker(
      Func<TGeometricElement> geometricElementGetter, SnapshotRecorder snapshotRecorder, VisualStyle visualStyle)
      : this(() => new[] {geometricElementGetter()}, snapshotRecorder, visualStyle)
    {
    }

    internal DrawableEntityTracker(
      Func<IEnumerable<TGeometricElement>> geometricElementGetter,
      SnapshotRecorder snapshotRecorder,
      VisualStyle visualStyle)
    {
      if (geometricElementGetter == null
          || snapshotRecorder == null)
      {
        throw new ArgumentNullException();
      }
      _geometricElementGetter = geometricElementGetter;
      _snapshotRecorder = snapshotRecorder;
      _visualStyle = visualStyle;
    }

    internal void Show()
    {
      _snapshotObjectId = _snapshotRecorder.Add(_geometricElementGetter(), _visualStyle);
    }
  }
}