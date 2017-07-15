using System;
using System.Collections.Generic;

namespace GeometricElements
{
  [Serializable]
  public class DcelFace
  {
    public DcelHalfEdge OuterComponent { get; set; }
    public DcelHalfEdge InnerComponent { get; set; }
    readonly string _name;
    
    public DcelFace() { }
    public DcelFace(int faceCount)
    {
      _name = "f" + faceCount;
    }

    public string GetName() { return _name; }

    public bool IsOriented()
    {
      bool allOriented = true;
      DcelHalfEdge e = OuterComponent;
      DcelHalfEdge travelPerimeter = e;
      do
      {
        if (!travelPerimeter.IsOriented)
          allOriented = false;
        travelPerimeter = travelPerimeter.Next;
      }
      while (!travelPerimeter.Equals(e));

      return allOriented;
    }

    public void LockEdgesPoints()
    {
      DcelHalfEdge e = OuterComponent;
      DcelHalfEdge travelPerimeter = e;
      do
      {
        travelPerimeter.LockOrientation();
        travelPerimeter = travelPerimeter.Next;
      }
      while (!travelPerimeter.Equals(e));
    }

    public void SwapEdgesOrientation()
    {
      DcelHalfEdge e = OuterComponent;
      DcelHalfEdge travelPerimeter = e;
      do
      {
        travelPerimeter.SwapEndPoints();
        travelPerimeter = travelPerimeter.Next;
      }
      while (!travelPerimeter.Equals(e));
    }

    public IEnumerable<DcelHalfEdge> HalfEdges()
    {
      if (OuterComponent != null)
      {
        DcelHalfEdge start = OuterComponent;
        DcelHalfEdge travelPerimeter = start;
        do
        {
          yield return travelPerimeter;
          travelPerimeter = travelPerimeter.Next;
        } while (!travelPerimeter.Equals(start));
      }
      if (InnerComponent != null)
      {
        DcelHalfEdge start = InnerComponent;
        DcelHalfEdge travelPerimeter = start;
        do
        {
          yield return travelPerimeter;
          travelPerimeter = travelPerimeter.Next;
        } while (!travelPerimeter.Equals(start));
      }
    }
  }
}
