using System;
using System.Collections.Generic;

namespace GeometricElements
{
  [Serializable]
  public class PolyLine : MarshalByRefObject
  {
    public PolyLine()
      : this(new List<Point>())
    {
    }

    public PolyLine(List<Point> list)
      : this(list, false)
    {
    }

    public PolyLine(List<Point> list, bool closed)
    {
      Points = list;
      Closed = closed;
    }

    public bool Closed { get; private set; }
    public IList<Point> Points { get; private set; }
  }
}