using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace ToolboxGeometricElements
{
  public class RayList : LineList
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
        GL.Color3(l.Color);
        GL.Vertex2(l.FirstPoint.X, l.FirstPoint.Y);
        GL.Vertex2(l.SecondPoint.X, l.SecondPoint.Y);
      }
      GL.End();

      //Drawing part of the ray
      GL.Begin(BeginMode.Lines);
      foreach (Line l in _lines)
      {
        var pointsToDraw = l.IntersectionsWithRectangle(left, right, bottom, top).Where(l.RayContains).ToList();
        if (pointsToDraw.Count == 1)
        {
          pointsToDraw.Add(l.FirstPoint);
        }

        if (pointsToDraw.Count != 2) continue;

        GL.Color3(l.Color);
        foreach (var point in pointsToDraw)
        {
          GL.Vertex2(point.X, point.Y);
        }
      }
      GL.End();
    }
  }
}