using System;
using System.ComponentModel;
using Infrastructure;

namespace ToolboxGeometricElements
{
  public delegate void NameChangedEventHandler(object sender, NameChangedEventArgs e);

  public class GeometricElement : MarshalByRefObject, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    Color _color;
    String _name;

    public GeometricElement(String newName, Group newGroup, Color newColor)
    {
      _name = newName;
      Group = newGroup;
      _color = newColor;
    }

    [Category("Miscelaneous"),
     DisplayName("Name"),
     Description("The name of the geometric element."),
     Show(true)]
    public String Name
    {
      get { return _name; }
      set
      {
        _name = value;
        NotifyNameChanged(value);
        NotifyPropertyChanged("Name");
      }
    }
    
    public Group Group { get; set; }

    [Category("Miscelaneous"),
     DisplayName("Color"),
     Description("The color of the geometric element."),
     Show(true)]
    public Color Color
    {
      get { return _color; }
      set
      {
        _color = value;
        NotifyPropertyChanged("Color");
      }
    }

    [Category("Miscelaneous"),
     DisplayName("Group name"),
     Description(""),
     Show(true),
     ReadOnly(true)]
    public String GroupName
    {
      get { return Group.Name; }
    }

    protected void NotifyNameChanged(string newName)
    {
      if (null != NameChanged)
        NameChanged(this, new NameChangedEventArgs(newName));
    }

    protected void NotifyPropertyChanged(string value)
    {
      if (null != PropertyChanged)
        PropertyChanged(this, new PropertyChangedEventArgs(value));
    }

    public event NameChangedEventHandler NameChanged;
  }
}