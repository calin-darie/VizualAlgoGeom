using System.ComponentModel;
using System.Reactive.Subjects;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Tao.OpenGl;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  internal delegate void NewGeometricElmentEventHandler(object sender, NewGeometricElmentEventArgs e);

  public delegate void PropertyChangedHandler(object sender, PropertyChangedEventArgs e);

  public abstract class Factory
  {
    protected Group _group;
    protected PropertyChangedHandler _propertyChangedHandler;

    internal Group Group
    {
      set
      {
        _group = value;
        GetGeometricElementsList();
      }
    }

    internal event EnableControlsEventHandler EnableControls;
    internal event NewGeometricElmentEventHandler NewElementAdded;
    protected abstract void GetGeometricElementsList();
    internal abstract void canvas_MouseClick(object sender, MouseEventArgs e);
    internal abstract void canvas_MouseMove(object sender, MouseEventArgs e);
    internal abstract void canvas_MouseDoubleClick(object sender, MouseEventArgs e);
    public Subject<GeometricElement> ElementComplete = new Subject<GeometricElement>();


    protected void GetWorldCoordinates(double xWin, double yWin, double zWin, out double x, out double y, out double z)
    {
      var viewPort = new int[4];
      var modelMatrix = new double[16];
      var projMatrix = new double[16];

      GL.GetInteger(GetPName.Viewport, viewPort);
      GL.GetDouble(GetPName.ModelviewMatrix, modelMatrix);
      GL.GetDouble(GetPName.ProjectionMatrix, projMatrix);

      Glu.gluUnProject(xWin, yWin, zWin, modelMatrix, projMatrix, viewPort, out x, out y, out z);
    }

    protected void FireNewElementAdded(GeometricElement geometricElement)
    {
      if (null != NewElementAdded)
        NewElementAdded(this, new NewGeometricElmentEventArgs(geometricElement));
    }

    protected void FireEnableControls(bool enable)
    {
      if (null != EnableControls)
        EnableControls(this, new EnableControlsEventArgs(enable));
    }
  }
}