using System.Windows.Forms;

namespace VizualAlgoGeom
{
  internal class ClosedPolylineFactory : PolylineFactory
  {
    protected override void GetGeometricElementsList()
    {
      _polylineList = _group.ClosedPolylineList;
      _polylineType = Polylines.ClosedPolyline;
      NewPolyline(GetName());
    }

      internal override void canvas_EnterPressed(object sender, KeyEventArgs e)
      {
          throw new System.NotImplementedException();
      }
  }
}