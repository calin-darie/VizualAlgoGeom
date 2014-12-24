namespace InterfaceOfSnapshotsWithVisualizer
{
  public interface ISnapshotRecorder :
    InterfaceOfSnapshotsWithAlgorithmsAndVisualizer.ISnapshotRecorder
  {
    ISnapshot[] SnapshotRecord { get; }
    void RemoveAllObjects();
  }
}