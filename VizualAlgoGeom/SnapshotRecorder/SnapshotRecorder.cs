using System;
using System.Collections.Generic;
using System.Linq;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using InterfaceOfSnapshotsWithVisualizer;
using ISnapshotRecorder = InterfaceOfSnapshotsWithVisualizer.ISnapshotRecorder;

namespace Snapshots
{
  public class SnapshotRecorder : MarshalByRefObject, ISnapshotRecorder
  {
    public void RemoveAllObjects()
    {
      _objectsToTrack.Clear();
    }

    public void TakeSnapshot(int pseudocodeLine)
    {
      TakeSnapshot(pseudocodeLine, string.Empty);
    }

    public void TakeSnapshot(int pseudocodeLine, string remarks)
    {
      TakeSnapshot(new SnapshotDescription
      {
        PseudocodeLine = pseudocodeLine,
        Remark = remarks
      });
    }

    public void TakeSnapshot(SnapshotDescription description)
    {
      ISnapshot snapshot = new Snapshot(description);
      foreach (KeyValuePair<int, IObjectSnapshot> element in _objectsToTrack)
      {
        IObjectSnapshot clone = element.Value.CloneObjectForSnapshot();
        if (clone != null)
        {
          snapshot.Add(element.Key, clone);
        }
      }
      _snapshots.Add(snapshot);
    }

    public ISnapshot[] SnapshotRecord
    {
      get { return _snapshots.ToArray(); }
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      TGeometricElement geometricElement)
    {
      return Show(geometricElement, new VisualStyle());
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      TGeometricElement geometricElement, VisualStyle visualStyle)
    {
      return Show(() => geometricElement, visualStyle);
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      Func<TGeometricElement> geometricElementGetter, VisualStyle visualStyle)
    {
      var drawableEntity = new DrawableEntityTracker<TGeometricElement>(
        geometricElementGetter,
        this,
        visualStyle);

      drawableEntity.Show();

      return drawableEntity;
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      Func<IEnumerable<TGeometricElement>> geometricElementGetter,
      VisualStyle visualStyle)
    {
      return ShowCollection(geometricElementGetter, visualStyle);
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      Func<List<TGeometricElement>> geometricElementGetter,
      VisualStyle visualStyle)
    {
      return ShowCollection(geometricElementGetter, visualStyle);
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(
      Func<TGeometricElement[]> geometricElementGetter,
      VisualStyle visualStyle)
    {
      return ShowCollection(geometricElementGetter, visualStyle);
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(IList<TGeometricElement> geometricElements)
    {
      return Show(geometricElements, new VisualStyle());
    }

    public IDrawableEntityTracker<TGeometricElement> Show<TGeometricElement>(IList<TGeometricElement> geometricElement,
      VisualStyle visualStyle)
    {
      return ShowCollection(() => geometricElement, visualStyle);
    }

    readonly UniqueIntSequence _idGenerator = new UniqueIntSequence();
    readonly Dictionary<int, IObjectSnapshot> _objectsToTrack = new Dictionary<int, IObjectSnapshot>();
    readonly IList<ISnapshot> _snapshots = new List<ISnapshot>();

    internal int Add<T>(IEnumerable<T> obj)
    {
      return Add(obj, new VisualStyle());
    }

    internal int Add<T>(IEnumerable<T> obj, VisualStyle visualStyle)
    {
      int id = _idGenerator.Generate();
      _objectsToTrack[id] = new ObjectSnapshot<T>(obj, visualStyle);
      return id;
    }

    internal void Remove(int id)
    {
      _objectsToTrack.Remove(id);
    }

    internal void Replace<T>(int id, IEnumerable<T> newObject)
    {
      _objectsToTrack[id] = _objectsToTrack[id].ReplaceObject(newObject);
    }

    IDrawableEntityTracker<TGeometricElement> ShowCollection<TGeometricElement>(
      Func<IEnumerable<TGeometricElement>> geometricElementGetter,
      VisualStyle visualStyle)
    {
      var drawableEntity = new DrawableEntityTracker<TGeometricElement>(
        geometricElementGetter,
        this,
        visualStyle);

      drawableEntity.Show();

      return drawableEntity;
    }
  }
}