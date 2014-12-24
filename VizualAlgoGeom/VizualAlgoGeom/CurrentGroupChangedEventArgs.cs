using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  public class CurrentGroupChangedEventArgs
  {
    public CurrentGroupChangedEventArgs(Group g)
    {
      Group = g;
    }

    public Group Group { get; set; }
  }
}