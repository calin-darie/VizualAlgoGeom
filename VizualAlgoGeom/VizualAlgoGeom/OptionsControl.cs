using System;
using System.Windows.Forms;

namespace VizualAlgoGeom
{
  internal delegate void CenterOriginHandler(object sender, EventArgs e);

  public partial class OptionsControl : UserControl
  {
    public OptionsControl()
    {
      InitializeComponent();
    }

    internal PropertyGrid OptionsGrid
    {
      get { return _optionsGrid; }
    }

    internal void PropertyChanged(object sender, EventArgs e)
    {
      _optionsGrid.Refresh();
    }
  }
}