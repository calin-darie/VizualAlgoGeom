using System;

namespace VizualAlgoGeom
{
  internal class EnableControlsEventArgs : EventArgs
  {
    internal EnableControlsEventArgs(bool flag)
    {
      Enable = flag;
    }

    internal bool Enable { get; set; }
  }
}