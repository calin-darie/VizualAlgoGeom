using System.Collections.Generic;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using ToolboxGeometricElements;
using Color = System.Drawing.Color;

namespace VizualAlgoGeom
{
  public class Data
  {
    public Data()
    {
      Groups = new List<Group>();
    }

    public int GroupCurrentIndex { get; set; }
    public Group CurrentGroup { get; set; }
    public List<Group> Groups { get; set; }
    public IEnumerable<IPendingDraw> DrawableObjects { get; set; }

    internal void AddDefaultGroup()
    {
      var g = new Group("Default", Color.Black);
      Groups.Add(g);
      CurrentGroup = g;
      GroupCurrentIndex = 0;
    }
  }
}