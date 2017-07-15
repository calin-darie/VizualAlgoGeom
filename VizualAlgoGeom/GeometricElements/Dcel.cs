using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GeometricElements
{
  [Serializable]
  public class Dcel
  {
    public List<DcelFace> DcelFaces { get; }
    readonly List<DcelVertex> _dcelVertex;

    public Dcel() { DcelFaces = new List<DcelFace>(); }
    public Dcel(List<DcelFace> dcelFaces, List<DcelVertex> dcelVertex)
    {
      _dcelVertex = dcelVertex;
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

    //Debug printing:
    //----------------------------------------------------------
    //Prints all face names, half-edges and vertexes in this dcel
    public void DebugPrint()
    {
      foreach (DcelFace face in DcelFaces)
      {
        foreach (DcelHalfEdge halfEdge in face.HalfEdges())
        {
          Debug.Print(halfEdge.Name + " : " + halfEdge.Start.Name + " -> " +
                      halfEdge.End.Name + " in face " + face.GetName());
          Debug.Print("Twin : " + halfEdge.Twin.Name + " : " +
                      halfEdge.Twin.Start.Name + " -> " +
                      halfEdge.Twin.End.Name + " in face " +
                      halfEdge.Twin.IncidentFace.GetName() + "\n\n");
        }
      }
    }

    //Prints all the vertexes in this dcel
    public void DebugVertexPrint()
    {
      foreach (DcelVertex vertex in _dcelVertex)
      {
        Debug.Print(vertex.Name + " incident with + ");
        foreach (DcelHalfEdge hedge in vertex.IncidentHalfEdges)
        {
          Debug.Print(hedge.Name + ", ");
        }
        Debug.Print("\n");
      }
    }
  }
}
