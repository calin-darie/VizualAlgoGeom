using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Tao.OpenGl;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public static class CoordinateConverter
  {
    public static Point RelativeToWindowPx(this GeometricElements.Point pointWorldUnits)
    {
      return GetWinCoordinates(pointWorldUnits.X, pointWorldUnits.Y);
    }

    public static Point GetWinCoordinates(double x, double y)
    {
      var viewPort = new int[4];
      var modelMatrix = new double[16];
      var projMatrix = new double[16];

      GL.GetInteger(GetPName.Viewport, viewPort);
      GL.GetDouble(GetPName.ModelviewMatrix, modelMatrix);
      GL.GetDouble(GetPName.ProjectionMatrix, projMatrix);

      double xWin, yWin, zWin;
      Glu.gluProject(x, y, 0, modelMatrix, projMatrix, viewPort, out xWin, out yWin, out zWin);
      return new Point((int) xWin, (int) yWin);
    }

    public static GeometricElements.Point ToWorldCoordinates(this Point p)
    {
      double x, y;
      GetWorldCoordinates(p.X, p.Y, out x, out y);
      return new GeometricElements.Point(x, y);
    }
    
    public static void GetWorldCoordinates(double xWin, double yWin, out double x, out double y)
    {
      var viewPort = new int[4];
      var modelMatrix = new double[16];
      var projMatrix = new double[16];

      GL.GetInteger(GetPName.Viewport, viewPort);
      GL.GetDouble(GetPName.ModelviewMatrix, modelMatrix);
      GL.GetDouble(GetPName.ProjectionMatrix, projMatrix);

      double z;
      Glu.gluUnProject(xWin, yWin, 0, modelMatrix, projMatrix, viewPort, out x, out y, out z);
    }

  }
}
