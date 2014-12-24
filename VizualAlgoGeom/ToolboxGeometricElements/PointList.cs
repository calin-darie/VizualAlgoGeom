using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Xml.Serialization;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using OpenTK.Graphics;
using BeginMode = OpenTK.Graphics.OpenGL.BeginMode;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace ToolboxGeometricElements
{
  public class PointList : MarshalByRefObject
  {
    List<Point> _points;
    protected TextPrinter _textPrinter;

    public PointList()
    {
      _points = new List<Point>();
      _textPrinter = new TextPrinter(TextQuality.Medium); //XXX !!!
    }

    [XmlArray("Points")]
    public List<Point> Points
    {
      get { return _points; }
      set
      {
        if (value != null)
        {
          _points = value;
        }
      }
    }

    public void Add(Point p)
    {
      _points.Add(p);
    }

    public void CanvasDraw(Size canvasSize, float fontSize)
    {
      foreach (Point p in _points)
      {
        System.Drawing.Point pointRelativeToWindowPx = CoordinateConverter.GetWinCoordinates(p.X, p.Y);
        GL.Color3(p.Color);
        _textPrinter.Begin();
        GL.Translate(pointRelativeToWindowPx.X, canvasSize.Height - pointRelativeToWindowPx.Y, 0);
        _textPrinter.Print(p.Name, new Font(new FontFamily(GenericFontFamilies.SansSerif), fontSize), p.Color);
        _textPrinter.End();
      }

      GL.Begin(BeginMode.Points);
      foreach (Point p in _points)
      {
        GL.Color3(p.Color);
        GL.Vertex2(p.X, p.Y);
      }
      GL.End();
    }
  }
}