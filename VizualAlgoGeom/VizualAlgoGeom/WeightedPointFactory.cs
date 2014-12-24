namespace VizualAlgoGeom
{
  internal class WeightedPointFactory : PointFactory
  {
    internal WeightedPointFactory()
    {
      _isWeighted = true;
    }

    protected override void GetGeometricElementsList()
    {
      _pointList = _group.WeightedPointList;
    }
  }
}