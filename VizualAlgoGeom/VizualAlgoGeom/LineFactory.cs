using System.Windows.Forms;
using OpenTK;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  public enum Lines
  {
    Segment,
    Line,
    Ray
  }

  internal class LineFactory : Factory
  {
    protected bool _inProgress;
    protected LineList _lineList;
    protected Lines _lineType;
    protected Line _newLine;

    internal LineFactory()
    {
      _inProgress = false;
    }

    protected override void GetGeometricElementsList()
    {
      _lineList = _group.LineList;
      _lineType = Lines.Line;
    }

    internal override void canvas_MouseClick(object sender, MouseEventArgs e)
    {
      var canvas = (GLControl) sender;
      double x;
      double y;
      double z;

      GetWorldCoordinates(e.X, canvas.Height - e.Y, 0, out x, out y, out z);

      if (false == _inProgress)
      {
        string name = GetName();
        var newPoint = new Point(x, y, name + "_p1", _group, _group.Color);
        FireNewElementAdded(newPoint);
        _newLine = new Line(newPoint, newPoint, name, _group, _group.Color);
        _lineList.Lines.Add(_newLine);
        _inProgress = true;
        FireEnableControls(false);
        canvas.Invalidate();
      }
      else
      {
        _newLine.SecondPoint.X = x;
        _newLine.SecondPoint.Y = y;
        _newLine.SecondPoint.Name = _newLine.Name + "_p2";
        _inProgress = false;
        FireNewElementAdded(_newLine);
        FireEnableControls(true);
        canvas.Invalidate();
      }
    }

    string GetName()
    {
      string name = _group.Name;
      switch (_lineType)
      {
        case Lines.Line:
          _group.LineCurrentIndex++;
          name += "_l" + _group.LineCurrentIndex;
          break;
        case Lines.Segment:
          _group.LineSegmentCurrentIndex++;
          name += "_s" + _group.LineSegmentCurrentIndex;
          break;
        case Lines.Ray:
          _group.RayCurrentIndex++;
          name += "_r" + _group.RayCurrentIndex;
          break;
      }
      return name;
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
        var newRightPoint = new Point(x, y, _newLine.Name + "_p2", _group, _group.Color);
        _newLine.SecondPoint = newRightPoint;
        canvas.Invalidate();
      }
    }

    internal override void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

      internal override void canvas_EnterPressed(object sender, KeyEventArgs e)
      {          
      }
  }
}