using System.ComponentModel;
using System.Windows.Forms;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  public partial class PropertiesControl : UserControl
  {
    public PropertiesControl()
    {
      InitializeComponent();
    }

    internal void SelectionChanged(object sender, TreeViewEventArgs e)
    {
      pgProperties.SelectedObject = e.Node.Tag;
      pgProperties.BrowsableAttributes =
        new AttributeCollection(new ShowAttribute(true));
    }
  }
}