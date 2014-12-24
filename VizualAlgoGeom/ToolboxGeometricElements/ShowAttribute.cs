using System;

namespace ToolboxGeometricElements
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class ShowAttribute : Attribute
  {
    public ShowAttribute(bool show)
    {
      Show = show;
    }

    public bool Show { get; private set; }
  }
}