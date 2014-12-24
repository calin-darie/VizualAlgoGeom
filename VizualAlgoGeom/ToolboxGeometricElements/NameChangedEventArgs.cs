using System;

namespace ToolboxGeometricElements
{
  public class NameChangedEventArgs : EventArgs
  {
    public NameChangedEventArgs(string newName)
    {
      NewName = newName;
    }

    public string NewName { get; set; }
  }
}