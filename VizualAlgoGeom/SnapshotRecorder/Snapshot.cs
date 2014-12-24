using System;
using System.Collections;
using System.Collections.Generic;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using InterfaceOfSnapshotsWithVisualizer;

namespace Snapshots
{
  internal class Snapshot : MarshalByRefObject, ISnapshot
  {
    public int PseudocodeLine
    {
      get { return _description.PseudocodeLine; }
    }

    public string Remarks
    {
      get { return _description.Remark; }
    }

    public Dictionary<int, IObjectSnapshot>.KeyCollection Keys
    {
      get { return _objectSnapshots.Keys; }
    }

    public Dictionary<int, IObjectSnapshot>.ValueCollection Values
    {
      get { return _objectSnapshots.Values; }
    }

    public int Count
    {
      get { return _objectSnapshots.Count; }
    }

    public void Add(int hash, IObjectSnapshot obj)
    {
      _objectSnapshots.Add(hash, obj);
    }

    public IEnumerator<KeyValuePair<int, IObjectSnapshot>> GetEnumerator()
    {
      return _objectSnapshots.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _objectSnapshots.GetEnumerator();
    }

    readonly SnapshotDescription _description;
    readonly Dictionary<int, IObjectSnapshot> _objectSnapshots = new Dictionary<int, IObjectSnapshot>();

    public Snapshot(SnapshotDescription description)
    {
      _description = description ?? new SnapshotDescription();
    }
  }
}