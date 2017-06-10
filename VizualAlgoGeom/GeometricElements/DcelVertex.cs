using System;
using System.Collections.Generic;

namespace GeometricElements
{
  [Serializable]
  public class DcelVertex
  {
    public Point Point { get; }
    public List<DcelHalfEdge> IncidentHalfEdges { get; private set; }
    public List<DcelHalfEdge> IncidentHalfEdgesUpperEndpoint { get; private set; }
    public string Name { get; private set; }
    public int DcelParent { get; set; }

    public DcelVertex(Point point, string name)
    {
      Point = point;
      Name = name;
      IncidentHalfEdges = new List<DcelHalfEdge>();
      IncidentHalfEdgesUpperEndpoint = new List<DcelHalfEdge>();
    }

    public void ResetIncidentHalfEdges()
    {
      IncidentHalfEdges = new List<DcelHalfEdge>();
      IncidentHalfEdgesUpperEndpoint = new List<DcelHalfEdge>();
    }

    public void AddIncidentEdge(DcelHalfEdge halfEdge)
    {
      IncidentHalfEdges.Add(halfEdge);
    }

    public void AddIncidentEdgeOnUpperPoint(DcelHalfEdge halfEdge)
    {
      IncidentHalfEdgesUpperEndpoint.Add(halfEdge);
    }

    public void RemoveIncidentEdgeOnUpperPoint(DcelHalfEdge halfEdge)
    {
      IncidentHalfEdgesUpperEndpoint.Remove(halfEdge);
    }
    public void RemoveIncidentEdge(DcelHalfEdge halfEdge)
    {
      IncidentHalfEdges.Remove(halfEdge);
    }
  }
}
