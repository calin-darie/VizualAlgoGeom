using System;

namespace GeometricElements
{
  [Serializable]
  public class DcelHalfEdge
  {
    public DcelVertex Start { get; private set; }
    public DcelVertex End { get; private set; }
    public DcelVertex Upper { get; private set; }
    public DcelVertex Lower { get; private set; }
    public DcelHalfEdge Twin { get; set; }
    public DcelHalfEdge Next { get; set; }
    public DcelHalfEdge Prev { get; set; }
    public DcelFace IncidentFace { get; set; }
    public string Name { get; set; }
    public bool IsOriented { get; private set; }

    //for serialization only
    public DcelHalfEdge() { }
    public DcelHalfEdge(DcelVertex start, DcelVertex end)
    {
      Start = start;
      End = end;
      SetUpperLowerEndPoints();
    }
    
    public void SetUpperLowerEndPoints()
    {
      if (Start.Point.Y > End.Point.Y)
      {
        Upper = Start;
        Lower = End;
      }
      else
      {
        Upper = End;
        Lower = Start;
      }
    }

    public int IsPossibleTwin(DcelHalfEdge e)
    {
      if (e.Start.Name.Equals(Start.Name) && e.End.Name.Equals(End.Name))
        return 2;
      if (e.Start.Name.Equals(End.Name) && e.End.Name.Equals(Start.Name))
        return 1;
      return 0;
    }

    public void LockOrientation()
    {
      IsOriented = true;
    }

    public void UnlockOrientation()
    {
      IsOriented = false;
    }

    public void SwapEndPoints()
    {
      DcelVertex aux = Start;
      Start = End;
      End = aux;
      IsOriented = true;
    }
  }
}
