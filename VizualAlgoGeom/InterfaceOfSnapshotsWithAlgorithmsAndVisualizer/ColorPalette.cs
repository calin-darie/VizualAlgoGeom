using System;
using OpenTK.Graphics.OpenGL;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class ColorPalette : MarshalByRefObject
  {
    public void SetColor(Infrastructure.Color color)
    {
      GL.Color3(color);
    }
  }
}