using System;
using System.Collections.Generic;
using Infrastructure;

namespace ToolboxGeometricElements
{
  public class Polyline : GeometricElement
  {
    public Polyline(String newName, Group newGroup, Color newColor)
      : base(newName, newGroup, newColor)
    {
      Points = new List<Point>();
    }

    /// <summary>
    ///   needed for serialization only
    /// </summary>
    Polyline() : base(string.Empty, new Group(), System.Drawing.Color.Black)
    {
    }

    public List<Point> Points { get; set; }
  }
}