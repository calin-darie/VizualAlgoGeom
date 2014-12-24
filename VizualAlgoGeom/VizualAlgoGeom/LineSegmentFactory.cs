//using OpenTK.Input;

namespace VizualAlgoGeom
{
  internal delegate void EnableControlsEventHandler(object sender, EnableControlsEventArgs e);

  internal class LineSegmentFactory : LineFactory
  {
    protected override void GetGeometricElementsList()
    {
      _lineList = _group.LineSegmentList;
      _lineType = Lines.Segment;
    }
  }
}