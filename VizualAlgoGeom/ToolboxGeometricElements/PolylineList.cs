using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using OpenTK.Graphics;
using BeginMode = OpenTK.Graphics.OpenGL.BeginMode;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace ToolboxGeometricElements
{
  public class PolylineList : MarshalByRefObject
  {
    protected TextPrinter _textPrinter;

    public PolylineList()
    {
      Polylines = new List<Polyline>();
      _textPrinter = new TextPrinter(TextQuality.Medium); //XXX !!!
    }

    public List<Polyline> Polylines { get; set; }

    public virtual void CanvasDraw(Size canvasSize, float fontSize)
    {
      Draw(BeginMode.LineStrip, canvasSize, fontSize);
    }

    protected void Draw(BeginMode drawingMode, Size canvasSize, float fontSize)
    {
      foreach (Polyline l in Polylines)
      {
        GL.Color3(l.Color);
        foreach (Point p in l.Points)
        { 
          System.Drawing.Point pointRelativeToWindowPx = CoordinateConverter.GetWinCoordinates(p.X, p.Y);
          _textPrinter.Begin();
          GL.Translate(pointRelativeToWindowPx.X, canvasSize.Height - pointRelativeToWindowPx.Y, 0);
          _textPrinter.Print(p.Name, new Font(new FontFamily(GenericFontFamilies.SansSerif), fontSize), l.Color);
          _textPrinter.End();
        }
      }

      GL.Begin(BeginMode.Points);
      foreach (Polyline l in Polylines)
      {
        foreach (Point p in l.Points)
        {
          GL.Color3(p.Color);
          GL.Vertex2(p.X, p.Y);
        }
      }
      GL.End();


      foreach (Polyline l in Polylines)
      {
        GL.Begin(drawingMode);
        GL.Color3(l.Color);
        foreach (Point p in l.Points)
        {
          GL.Vertex2(p.X, p.Y);
        }
        GL.End();
      }
    }
  }
}