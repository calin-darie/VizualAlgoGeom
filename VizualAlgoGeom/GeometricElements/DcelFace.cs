using System;
using System.Collections.Generic;

namespace GeometricElements
{
  [Serializable]
  public class DcelFace
  {
    DcelHalfEdge _outerComponent;
    DcelHalfEdge _innerComponent;
    readonly string _name;
    public int DcelParent { get; set; }

    public DcelFace(int faceCount)
    {
      _name = "f" + faceCount;
    }

    public void SetOuterComponent(DcelHalfEdge e)
    {
      _outerComponent = e;
    }

    public void SetInnerComponent(DcelHalfEdge e)
    {
      _innerComponent = e;
    }

    public DcelHalfEdge GetOuterComponent()
    {
      return _outerComponent;
    }
    public DcelHalfEdge GetInnerComponent()
    {
      return _innerComponent;
    }

    public string GetName() { return _name; }

    public bool IsOriented()
    {
      bool allOriented = true;
      DcelHalfEdge e = _outerComponent;
      DcelHalfEdge travelPerimeter = e;
      do
      {
        if (!travelPerimeter.IsOriented())
          allOriented = false;
        travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
      }
      while (!travelPerimeter.Equals(e));

      return allOriented;
    }

    public void LockEdgesPoints()
    {
      DcelHalfEdge e = _outerComponent;
      DcelHalfEdge travelPerimeter = e;
      do
      {
        travelPerimeter.LockOrientation();
        travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
      }
      while (!travelPerimeter.Equals(e));
    }

    public void SwapEdgesOrientation()
    {
      DcelHalfEdge e = _outerComponent;
      DcelHalfEdge travelPerimeter = e;
      do
      {
        travelPerimeter.SwapEndPoints();
        travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
      }
      while (!travelPerimeter.Equals(e));
    }

    public IEnumerable<DcelHalfEdge> HalfEdges()
    {
      if (GetOuterComponent() != null)
      {
        DcelHalfEdge start = GetOuterComponent();
        DcelHalfEdge travelPerimeter = start;
        do
        {
          yield return travelPerimeter;
          travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
        } while (!travelPerimeter.Equals(start));
      }
      if (GetInnerComponent() != null)
      {
        DcelHalfEdge start = GetInnerComponent();
        DcelHalfEdge travelPerimeter = start;
        do
        {
          yield return travelPerimeter;
          travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
        } while (!travelPerimeter.Equals(start));
      }
    }
  }
}
