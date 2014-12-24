using System;

namespace VizualAlgoGeom
{
  internal class ToolChangedEventArgs : EventArgs
  {
    internal Factory _elementFactory;

    internal ToolChangedEventArgs(Factory factory)
    {
      _elementFactory = factory;
    }
  }
}