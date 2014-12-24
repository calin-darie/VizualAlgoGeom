using OpenTK.Graphics.OpenGL;

namespace ToolboxGeometricElements
{
  public class LineSegmentList : LineList
  {
    public override void CanvasDraw(double left, double right, double bottom, double top, int height, float fontSize)
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
      GL.Begin(BeginMode.Lines);
      foreach (Line l in _lines)
      {
        GL.Color3(l.Color);
        GL.Vertex2(l.FirstPoint.X, l.FirstPoint.Y);
        GL.Vertex2(l.SecondPoint.X, l.SecondPoint.Y);
      }
      GL.End();
    }
  }
}