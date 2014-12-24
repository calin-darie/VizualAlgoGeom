using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Darwen.Windows.Forms.Controls.Docking;

namespace VizualAlgoGeom.Config
{
  public interface IPreservableForm
  {
    int Width { get; set; }
    int Height { get; set; }
    int Top { get; set; }
    int Left { get; set; }
    bool Visible { get; set; }
    FormWindowState WindowState { get; set; }
    Rectangle RestoreBounds { get; }
    event EventHandler Load;
    event FormClosingEventHandler FormClosing;
  }

  public interface IPreservableFormWithDockingChildren
    : IPreservableForm
  {
    Dictionary<string, IDockingControl> PreservableDockingControls { get; }
  }
}