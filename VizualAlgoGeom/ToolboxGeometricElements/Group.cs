using System;
using System.Collections.Generic;
using System.ComponentModel;
using GeometricElements;
using Infrastructure;
using System.Runtime.CompilerServices;

namespace ToolboxGeometricElements
{
  public class Group : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    string _name;
    PointList _pointList;
    Color _color;

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
      DcelList = new List<Dcel>();

      ResetAllIndex();
    }


      [Category("Miscelaneous"),
     DisplayName("Color"),
     Description(""),
     Show(true)]
    public Color Color
    {
      get { return _color; }
      set
      {
        if (_color == value) return;
        _color = value;
        this.OnPropertyChanged();
      }
    }

    [Category("Miscelaneous"),
     DisplayName("Name"),
     Description(""),
     Show(true)]
    public string Name
    {
      get { return _name; }
      set
      {
        if (_name == value) return;
        _name = value;
        NotifyNameChanged(value);
        OnPropertyChanged();
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
    public List<Dcel> DcelList { get; set; }
    public int PointCurrentIndex { get; set; }
    public int WeightedPointCurrentIndex { get; set; }
    public int LineSegmentCurrentIndex { get; set; }
    public int LineCurrentIndex { get; set; }
    public int RayCurrentIndex { get; set; }
    public int PolylineCurrentIndex { get; set; }
    public int ClosedPolylineCurrentIndex { get; set; }

    void NotifyNameChanged(string value)
    {
      NameChanged?.Invoke(this, new NameChangedEventArgs(value));
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


    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}