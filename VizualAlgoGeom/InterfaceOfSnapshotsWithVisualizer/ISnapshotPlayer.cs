namespace InterfaceOfSnapshotsWithVisualizer
{
  public delegate void SnapshotChangedEventHandler(object sender);

  public delegate void PlayerStatusChangedEventHandler(object sender);

  public enum PlayerStatus
  {
    Playing,
    Paused,
    Idle
  }

  public interface ISnapshotPlayer
  {
    ISnapshot[] SnapshotRecord { set; }
    ISnapshot CurrentSnapshot { get; }
    PlayerStatus Status { get; }
    int Index { get; set; }
    int FramesPerMinute { get; set; }
    string StatusString { get; }
    int PercentOfAlgorithmStepsCompleted { get; }
    int MaxIndex { get; }
    void Play();
    void Stop();
    void Pause();
    void Previous();
    void Next();
    void JumpToStart();
    void JumpToEnd();
    void JumpTo(int index);
    void SpeedUp();
    void SlowDown();
    event SnapshotChangedEventHandler OnSnapshotChange;
    event PlayerStatusChangedEventHandler OnStatusChange;
    void ClearRecord();
  }
}