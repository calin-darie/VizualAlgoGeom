using System;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  public enum Polylines
  {
    ClosedPolyline,
    Polyline
  }

  public delegate void ElementDeleted(object sender, EventArgs e);

  internal class PolylineFactory : Factory
  {
    protected bool _inProgress;
    protected Point _movingPoint;
    protected Polyline _newPolyline;
    protected PolylineList _polylineList;
    protected Polylines _polylineType;

    internal PolylineFactory()
    {
      _inProgress = false;
    }

    public event ElementDeleted ElementDeleted;

    protected override void GetGeometricElementsList()
    {
      _polylineList = _group.PolylineList;
      _polylineType = Polylines.Polyline;
     
    }

    protected string GetName()
    {
      string name = _group.Name;
      if (_polylineType == Polylines.Polyline)
      {
        _group.PolylineCurrentIndex++;
        name += "_pl" + _group.PolylineCurrentIndex;
      }
      else
      {
        _group.ClosedPolylineCurrentIndex++;
        name += "_cpl" + _group.ClosedPolylineCurrentIndex;
      }
      return name;
    }

    protected void NewPolyline(string name)
    {
      _newPolyline = new Polyline(name, _group, _group.Color);
    }

    internal override void canvas_MouseClick(object sender, MouseEventArgs e)
    {
      var canvas = (GLControl) sender;
     
      if (false == _inProgress)
      {
        HandleFirstClick(e, canvas);
      }
      //We add the new point behind the moving point 
      var newPoint = new Point(_movingPoint.X,_movingPoint.Y, _newPolyline.Name + "_p" + _newPolyline.Points.Count(), _group, _group.Color);
      _newPolyline.Points.Insert(_newPolyline.Points.Count - 1, newPoint);
      FireNewElementAdded(newPoint);
      canvas.Invalidate();
    }

    protected void HandleFirstClick(MouseEventArgs e, GLControl canvas)
    {
      double x;
      double y;
      double z;

      GetWorldCoordinates(e.X, canvas.Height - e.Y, 0, out x, out y, out z);

      _inProgress = true;
      FireEnableControls(false);
      NewPolyline(GetName());
      _polylineList.Polylines.Add(_newPolyline);
      //The moving point will be used to show the line that follows the mouse.
      //It is the last point in line strip's point list
      _newPolyline.Points.Add(new Point(x, y, _newPolyline.Name + "_p" + (_newPolyline.Points.Count() + 1), _group,_group.Color));
      _movingPoint = _newPolyline.Points.Last();
      _movingPoint.Name = _newPolyline.Name + "_p";
      FireNewElementAdded(_newPolyline);
    }

    internal override void canvas_MouseMove(object sender, MouseEventArgs e)
    {
      if (_inProgress)
      {
        var canvas = (GLControl) sender;
        double x;
        double y;
        double z;

        GetWorldCoordinates(e.X, canvas.Height - e.Y, 0, out x, out y, out z);
        var newPoint = new Point(x, y, _group.Name, _group, _group.Color);
        SetMovingPoint(newPoint);
        canvas.Invalidate();
      }
    }

    protected void SetMovingPoint(Point newPoint)
    {
      _movingPoint.X = newPoint.X;
      _movingPoint.Y = newPoint.Y;
    }

    internal override void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      RemoveMovingPoint();
      //The line strip is complete
      _inProgress = false;

      //The controls (toolbox) are enabled
      FireEnableControls(true);

      //A new line strip is ready to be filled with points
      NewPolyline(GetName());

      ElementComplete.OnNext(_newPolyline);
    }

      internal override void canvas_EnterPressed(object sender, KeyEventArgs e)
      {
      }

      protected void RemoveMovingPoint()
    {
      _newPolyline.Points.Remove(_movingPoint);
      FireElementDeleted();
    }

    protected void FireElementDeleted()
    {
      if (ElementDeleted != null)
        ElementDeleted(this, new EventArgs());
    }
  }
}