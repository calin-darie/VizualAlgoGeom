using System;
using GeometricElements;
using OpenTK.Graphics.OpenGL;
using Tao.OpenGl;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class CircleTool
  {
    public void Draw(Circle circle, SizeWorldUnits pixelSizeWorldUnits)
    {
      GL.PushMatrix();
      GL.Translate(circle.CenterX, circle.CenterY, 0);

      double diameterWorldUnits = 2d*circle.Radius;
      var widthPx = (int) (diameterWorldUnits/pixelSizeWorldUnits.Width);
      var heightPx = (int) (diameterWorldUnits/pixelSizeWorldUnits.Height);

      double thicknessWorldUnits = Math.Max(pixelSizeWorldUnits.Width, pixelSizeWorldUnits.Height);

      Glu.gluDisk(
        Glu.gluNewQuadric(), circle.Radius - thicknessWorldUnits, circle.Radius, widthPx + heightPx, 1);

      GL.PopMatrix();
    }
  }
}