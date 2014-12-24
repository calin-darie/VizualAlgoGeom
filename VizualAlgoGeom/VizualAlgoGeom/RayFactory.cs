namespace VizualAlgoGeom
{
  internal class RayFactory : LineFactory
  {
    protected override void GetGeometricElementsList()
    {
      _lineList = _group.RayList;
      _lineType = Lines.Ray;
    }
  }
}