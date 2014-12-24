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
  }
}