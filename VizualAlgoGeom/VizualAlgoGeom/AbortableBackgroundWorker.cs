using System.ComponentModel;
using System.Threading;

namespace VizualAlgoGeom
{
  public class AbortableBackgroundWorker : BackgroundWorker
  {
    Thread _workerThread;

    protected override void OnDoWork(DoWorkEventArgs e)
    {
      _workerThread = Thread.CurrentThread;
      try
      {
        base.OnDoWork(e);
      }
      catch (ThreadAbortException)
      {
        e.Cancel = true; //We must set Cancel property to true!
        Thread.ResetAbort(); //Prevents ThreadAbortException propagation
      }
    }

    public void Abort()
    {
      if (_workerThread != null)
      {
        _workerThread.Abort();
        _workerThread = null;
      }
    }
  }
}