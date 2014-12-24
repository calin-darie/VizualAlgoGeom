using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using OpenTK.Graphics;
using BeginMode = OpenTK.Graphics.OpenGL.BeginMode;
using GetPName = OpenTK.Graphics.OpenGL.GetPName;
using Glu = Tao.OpenGl.Glu;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace ToolboxGeometricElements
{
  public class LineList : MarshalByRefObject
  {
    protected List<Line> _lines;
    protected TextPrinter _textPrinter;

    public LineList()
    {
      _lines = new List<Line>();
      _textPrinter = new TextPrinter(TextQuality.Medium); //XXX !!!
    }

    public List<Line> Lines
    {
      get { return _lines; }
    }

    public virtual void CanvasDraw(double left, double right, double bottom, double top, int height, float fontSize)
    {
      foreach (Line l in _lines)
      {
        GL.Color3(l.Color);
        PrintPointName(l.FirstPoint, height, fontSize);
        PrintPointName(l.SecondPoint, height, fontSize);
      }

      GL.Begin(BeginMode.Points);
      foreach (Line l in _lines)
      {
        GL.Color3(l.FirstPoint.Color);
        GL.Vertex2(l.FirstPoint.X, l.FirstPoint.Y);
        GL.Color3(l.SecondPoint.Color);
        GL.Vertex2(l.SecondPoint.X, l.SecondPoint.Y);
      }
      GL.End();

      //Drawing part of the line

      GL.Begin(BeginMode.Lines);
      foreach (Line l in _lines)
      {
        GL.Color3(l.Color);
        IList<GeometricElements.Point> pointsToDraw = l.IntersectionsWithRectangle(left, right, bottom, top);
        if (pointsToDraw.Count != 2) continue;
        foreach (var point in pointsToDraw)
        {
          GL.Vertex2(point.X, point.Y);
        }
      }
      GL.End();
    }

    protected void PrintPointName(Point p, int height, float fontSize)
    {
      System.Drawing.Point pointRelativeToWindow = CoordinateConverter.GetWinCoordinates(p.X, p.Y);
      _textPrinter.Begin();
      GL.Translate(pointRelativeToWindow.X, height - pointRelativeToWindow.Y, 0);
      _textPrinter.Print(p.Name, new Font(new FontFamily(GenericFontFamilies.SansSerif), fontSize), p.Color);
      _textPrinter.End();
    }
  }
}