using System;
using System.ComponentModel;
using Infrastructure;

namespace ToolboxGeometricElements
{
  public class Group : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    String _name;
    PointList _pointList;

    public Group()
      : this("DefaultGroup", System.Drawing.Color.Black)
    {
    }

    public Group(String groupName, Color color)
    {
      _name = groupName;
      Color = color;

      _pointList = new PointList();
      LineSegmentList = new LineSegmentList();
      LineList = new LineList();
      RayList = new RayList();
      PolylineList = new PolylineList();
      ClosedPolylineList = new ClosedPolylineList();
      WeightedPointList = new PointList();

      ResetAllIndex();
    }

    [Category("Miscelaneous"),
     DisplayName("Color"),
     Description(""),
     Show(true)]
    public Color Color { get; set; }

    [Category("Miscelaneous"),
     DisplayName("Name"),
     Description(""),
     Show(true)]
    public String Name
    {
      get { return _name; }
      set
      {
        _name = value;
        NotifyNameChanged(value);
      }
    }

    public PointList PointList
    {
      get { return _pointList; }
      set
      {
        if (value != null)
        {
          _pointList = value;
        }
      }
    }

    public PointList WeightedPointList { get; set; }
    public LineSegmentList LineSegmentList { get; set; }
    public PolylineList PolylineList { get; set; }
    public ClosedPolylineList ClosedPolylineList { get; set; }
    public RayList RayList { get; set; }
    public LineList LineList { get; set; }
    public int PointCurrentIndex { get; set; }
    public int WeightedPointCurrentIndex { get; set; }
    public int LineSegmentCurrentIndex { get; set; }
    public int LineCurrentIndex { get; set; }
    public int RayCurrentIndex { get; set; }
    public int PolylineCurrentIndex { get; set; }
    public int ClosedPolylineCurrentIndex { get; set; }

    void NotifyNameChanged(string value)
    {
      if (null != NameChanged)
        NameChanged(this, new NameChangedEventArgs(value));
    }

    public event NameChangedEventHandler NameChanged;

    public void ResetAllIndex()
    {
      PointCurrentIndex = 0;
      WeightedPointCurrentIndex = 0;
      LineSegmentCurrentIndex = 0;
      LineCurrentIndex = 0;
      RayCurrentIndex = 0;
      PolylineCurrentIndex = 0;
      ClosedPolylineCurrentIndex = 0;
    }
  }
}