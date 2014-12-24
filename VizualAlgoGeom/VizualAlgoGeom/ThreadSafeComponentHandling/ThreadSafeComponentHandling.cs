using System.Windows.Forms;

namespace VizualAlgoGeom.ThreadSafeComponentHandling
{
  internal static class ThreadSafeComponentHandler
  {
    internal static void ThreadSafeInvalidate(this Control target)
    {
      if (target.InvokeRequired)
      {
        // We're not in the UI thread, so we need to call BeginInvoke
        target.BeginInvoke(new Calls(target.Invalidate));
      }
      else
      {
        // Must be on the UI thread if we've got here
        target.Invalidate();
      }
    }

    internal static void ThreadSafeMethodCalls(this Control target, Calls calls)
    {
      if (target.InvokeRequired)
      {
        // We're not in the UI thread, so we need to call BeginInvoke
        target.BeginInvoke(calls);
      }
      else
      {
        // Must be on the UI thread if we've got here
        calls();
      }
    }

    internal delegate void Calls();
  }
}