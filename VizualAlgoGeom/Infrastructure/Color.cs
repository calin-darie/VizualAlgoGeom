using System;
using System.Drawing;

namespace Infrastructure
{
  public class Color : MarshalByRefObject
  {
    public Color()
    {
      Html = "Black";
    }

    public Color(System.Drawing.Color wrappedColor)
    {
      WrappedColor = wrappedColor;
    }

    public string Html
    {
      get { return ColorTranslator.ToHtml(WrappedColor); }
      set { WrappedColor = ColorTranslator.FromHtml(value); }
    }

    System.Drawing.Color WrappedColor { get; set; }

    public static implicit operator System.Drawing.Color(Color source)
    {
      return source.WrappedColor;
    }

    public static implicit operator Color(System.Drawing.Color source)
    {
      return new Color(source);
    }
  }
}