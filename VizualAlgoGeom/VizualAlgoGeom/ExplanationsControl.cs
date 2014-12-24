using System.Windows.Forms;

namespace VizualAlgoGeom
{
  public partial class ExplanationsControl : UserControl
  {
    public ExplanationsControl()
    {
      InitializeComponent();
    }

    internal void SetExplanation(string explanation)
    {
      textBoxExplanation.Text = explanation;
    }
  }
}