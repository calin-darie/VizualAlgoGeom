using System;
using System.Collections.Generic;

namespace GeometricElements
{
  [Serializable]
  public class Dcel
  {
    public List<DcelFace> DcelFaces { get; }

    public Dcel() { DcelFaces = new List<DcelFace>(); }
    public Dcel(List<DcelFace> dcelFaces)
    {
      DcelFaces = dcelFaces;
      FinalHalfEdgeModifications();
    }

    void FinalHalfEdgeModifications()
    {
      foreach (DcelFace face in DcelFaces)
      {
        foreach (DcelHalfEdge halfEdge in face.HalfEdges())
        {
          halfEdge.SetUpperLowerEndPoints();
          halfEdge.Upper.AddIncidentEdgeOnUpperPoint(halfEdge);
        }
      }
    }
  }
}
