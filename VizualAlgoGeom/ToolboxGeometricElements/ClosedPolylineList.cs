using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace ToolboxGeometricElements
{
  public class ClosedPolylineList : PolylineList
  {
    public override void CanvasDraw(Size canvasSize, float fontSize)
    {
      Draw(BeginMode.LineLoop, canvasSize, fontSize);
    }
  }
}