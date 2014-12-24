using System.Collections.Generic;

namespace InterfaceOfSnapshotsWithVisualizer
{
  public interface ISnapshot : IEnumerable<KeyValuePair<int, IObjectSnapshot>>
  {
    Dictionary<int, IObjectSnapshot>.KeyCollection Keys { get; }
    Dictionary<int, IObjectSnapshot>.ValueCollection Values { get; }
    int Count { get; }
    int PseudocodeLine { get; }
    string Remarks { get; }
    void Add(int hash, IObjectSnapshot obj);
  }
}