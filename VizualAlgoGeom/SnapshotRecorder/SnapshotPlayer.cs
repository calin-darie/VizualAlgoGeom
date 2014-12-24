using System;
using System.Linq;
using System.Timers;
using InterfaceOfSnapshotsWithVisualizer;

namespace Snapshots
{
  public class SnapshotPlayer : ISnapshotPlayer
  {
    public ISnapshot CurrentSnapshot
    {
      get
      {
        return SnapshotRecord.Length == 0
          ? null
          : SnapshotRecord[_index];
      }
    }

    public ISnapshot[] SnapshotRecord
    {
      set
      {
        lock (_key)
        {
          if (value == null)
            throw new ArgumentException("Will not set the snapshotRecord to null");
          Stop();
          _snapshotRecord = value;
          NotifySnapshotChanged();
        }
      }
      private get { return _snapshotRecord; }
    }

    public PlayerStatus Status
    {
      get { return _status; }

      private set
      {
        lock (_key)
        {
          _status = value;
          CheckForPlaybackEnd();
          NotifyStatusChanged();
        }
      }
    }

    public int Index
    {
      get { return _index; }
      set
      {
        lock (_key)
        {
          if ((value >= 0) && (value <= SnapshotRecord.Length - 1))
          {
            _index = value;
            CheckForPlaybackEnd();

            NotifySnapshotChanged();
          }
        }
      }
    }

    public int FramesPerMinute
    {
      get { return 60000/(int) _timer.Interval; }
      set
      {
        lock (_key)
        {
          if (value > 0)
          {
            _timer.Interval = (double)60000/value;
            NotifyStatusChanged();
          }
        }
      }
    }

    public void Play()
    {
      lock (_key)
      {
        if (SnapshotRecord.Length > 0)
        {
          if (_status == PlayerStatus.Idle)
          {
            Index = 0;
          }
          Status = PlayerStatus.Playing;
          _timer.AutoReset = true;
          _timer.Elapsed += timer_Elapsed;
          _timer.Start();
        }
      }
    }

    public void Stop()
    {
      EndPlayback();
      Index = 0;
    }

    public void Pause()
    {
      lock (_key)
      {
        if (_status == PlayerStatus.Paused)
        {
          Status = PlayerStatus.Playing;
          _timer.Start();
        }
        else if (_status == PlayerStatus.Playing)
        {
          _timer.Stop();
          Status = PlayerStatus.Paused;
        }
      }
    }

    public void Previous()
    {
      Index--;
    }

    public void Next()
    {
      Index++;
    }

    public void JumpToStart()
    {
      Index = 0;
    }

    public void JumpTo(int index)
    {
      if (0 <= index && index <= MaxIndex)
      {
        Index = index;
      }
    }

    public void JumpToEnd()
    {
      EndPlayback();
      Index = SnapshotRecord.Length - 1;
    }

    public void SlowDown()
    {
      FramesPerMinute -= 10;
    }

    public void SpeedUp()
    {
      FramesPerMinute += 10;
    }

    public string StatusString
    {
      get
      {
        string playing = _status.ToString(); //XXX localizable resource
        int recordCount = SnapshotRecord.Length;
        string steps = (recordCount > 0)
          ? string.Format("Step {0} of {1}", _index + 1, recordCount)
          : "No step loaded";

        return string.Format("{0}; {1} frames per minute ; {2}",
          steps, FramesPerMinute, playing);
      }
    }

    public int PercentOfAlgorithmStepsCompleted
    {
      get
      {
        int count = SnapshotRecord.Count();
        return (count > 0)
          ? 100*(_index + 1)/SnapshotRecord.Count()
          : 0;
      }
    }

    public event SnapshotChangedEventHandler OnSnapshotChange;
    public event PlayerStatusChangedEventHandler OnStatusChange;

    public void ClearRecord()
    {
      SnapshotRecord = EmptyRecord;
      _index = 0;
      NotifySnapshotChanged();
    }

    public int MaxIndex
    {
      get { return SnapshotRecord.Length - 1; }
    }

    static readonly ISnapshot[] EmptyRecord = new ISnapshot[0];
    int _index;
    ISnapshot[] _snapshotRecord = EmptyRecord;
    PlayerStatus _status = PlayerStatus.Idle;
    readonly object _key = new object();
    readonly Timer _timer = new Timer(5000);

    void CheckForPlaybackEnd()
    {
      if (_index == SnapshotRecord.Length - 1)
        EndPlayback();
    }

    public void timer_Elapsed(object sender, ElapsedEventArgs args)
    {
      Next();
    }

    void EndPlayback()
    {
      lock (_key)
      {
        if (_status != PlayerStatus.Idle)
        {
          Status = PlayerStatus.Idle;
          _timer.Enabled = false;
          _timer.AutoReset = false;
          _timer.Stop();
        }
      }
      NotifyStatusChanged();
    }

    void NotifySnapshotChanged()
    {
      if (OnSnapshotChange != null)
        OnSnapshotChange(this);

      NotifyStatusChanged();
    }

    void NotifyStatusChanged()
    {
      if (OnStatusChange != null)
        OnStatusChange(this);
    }
  }
}