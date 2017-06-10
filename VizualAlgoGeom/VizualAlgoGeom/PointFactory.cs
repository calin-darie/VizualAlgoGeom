using System.Windows.Forms;
using OpenTK;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  internal class PointFactory : Factory
  {
    protected bool _isWeighted;
    protected PointList _pointList;

    internal PointFactory()
    {
      _isWeighted = false;
    }

    protected override void GetGeometricElementsList()
    {
      _pointList = _group.PointList;
    }

    internal override void canvas_MouseClick(object sender, MouseEventArgs e)
    {
      var canvas = (GLControl) sender;
      double x;
      double y;
      double z;

      GetWorldCoordinates(e.X, canvas.Height - e.Y, 0, out x, out y, out z);
      Point newPoint;

      if (_isWeighted)
      {
        _group.WeightedPointCurrentIndex++;
        newPoint = new WeightedPoint(x, y, _group.Name + "_wp" + _group.WeightedPointCurrentIndex, _group, _group.Color);
      }
      else
      {
        _group.PointCurrentIndex++;
        newPoint = new Point(x, y, _group.Name + "_p" + _group.PointCurrentIndex, _group, _group.Color);
      }
      _pointList.Add(newPoint);
      FireNewElementAdded(newPoint);
      canvas.Invalidate();
    }

    internal override void canvas_MouseMove(object sender, MouseEventArgs e)
    {
    }

    internal override void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

      internal override void canvas_EnterPressed(object sender, KeyEventArgs e)
      {       
      }
  }
}