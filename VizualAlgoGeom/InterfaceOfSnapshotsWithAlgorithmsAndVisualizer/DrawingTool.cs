using System;
using GeometricElements;
using OpenTK.Graphics.OpenGL;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class DrawingTool : MarshalByRefObject, IDisposable
  {
    public virtual void Dispose()
    {
      GL.End();
    }

    internal DrawingTool(BeginMode mode)
    {
      Mode = mode;
    }

    BeginMode Mode { get; set; }

    public virtual DrawingTool Begin()
    {
      GL.Begin(Mode);
      return this;
    }

    public virtual DrawingTool Vertex(double x, double y)
    {
      GL.Vertex2(x, y);
      return this;
    }

    public virtual void Vertex(Point p)
    {
      GL.Vertex2(p.X, p.Y);
    }
  }
}