using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeometricElements;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using OpenTK;

namespace VizualAlgoGeom
{
  class DcelFactory : PolylineFactory
  {
    Cursor _lastCursor;


    public DcelFactory()
    {
      this._lastCursor = new Cursor(new MemoryStream(CursorsResource.Pen));
    }

    protected override void GetGeometricElementsList()
    {
      _polylineList = _group.ClosedPolylineList;
      _polylineType = Polylines.ClosedPolyline;
      NewPolyline(GetName());
    }


    internal override void canvas_MouseMove(object sender, MouseEventArgs e)
    {
      base.canvas_MouseMove(sender, e);
      Point pointToSnapTo = GetPointToSnapTo();
      if (pointToSnapTo != null)
      {
        _movingPoint.X = pointToSnapTo.X;
        _movingPoint.Y = pointToSnapTo.Y;
      }
    }

    internal override void canvas_MouseClick(object sender, MouseEventArgs e)
    {
      Point pointToSnapTo = GetPointToSnapTo();
      if (pointToSnapTo == null)
      {
        base.canvas_MouseClick(sender, e);
        return;
      }
      RemoveMovingPoint();
      HandleFirstClick(e, sender as GLControl); // TODO verificare
      //A new line strip is ready to be filled with points
      //TODO add movingPoint to the new polyline + connect the previous polyline.
    }
    Point GetPointToSnapTo()
    {
      foreach (var vertex in _newPolyline.Points)
      {
        //Point mouse_coord = e.Location.ToWorldCoordinates();
        if (vertex == _movingPoint)
        {
          continue;
        }
        if (Point.DistanceBetween(_movingPoint, vertex) < 1.0) // TODO zoom
        {
          return vertex;
        }
      }
      return null;
    }

  }
}
