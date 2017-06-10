using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GeometricElements
{
    [Serializable]
   public class Dcel
   {
        public List<DcelFace> DcelFaces { get; private set; }
        List<DcelVertex> _dcelVertex;
        static int _currentDcelCount=0;

       public Dcel(List<DcelFace> faceList,  List<DcelVertex> dcelVertex)
       {
           _dcelVertex = dcelVertex;
           DcelFaces= faceList;
           _currentDcelCount++;
           finalHalfEdgeModifications();
       }

        void finalHalfEdgeModifications()
        {
            foreach (DcelFace face in DcelFaces)
            {
                face.DcelParent = _currentDcelCount;
                foreach (DcelHalfEdge halfEdge in face.HalfEdges())
                {
                    halfEdge.SetUpperLowerEndPoints();
                    halfEdge.DcelParent = _currentDcelCount;
                    halfEdge.GetStart().DcelParent = _currentDcelCount;
                    halfEdge.GetEnd().DcelParent = _currentDcelCount;
                    halfEdge.GetUpper().AddIncidentEdgeOnUpperPoint(halfEdge);
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
                   Debug.Print(halfEdge.GetName() + " : " + halfEdge.GetStart().Name + " -> " +
                               halfEdge.GetEnd().Name + " in face " + face.GetName());
                   Debug.Print("Twin : " + halfEdge.GetTwin().GetName() + " : " +
                               halfEdge.GetTwin().GetStart().Name + " -> " +
                               halfEdge.GetTwin().GetEnd().Name + " in face " +
                               halfEdge.GetTwin().GetFace().GetName() + "\n\n");
               }
           }
       }

        //Prints all the vertexes in this dcel
        public void DebugVertexPrint()
        {
            foreach (DcelVertex vertex in _dcelVertex)
            {
                Debug.Print(vertex.Name+" incident with + ");
                foreach (DcelHalfEdge hedge in vertex.IncidentHalfEdges)
                {
                    Debug.Print(hedge.GetName() + ", ");
                }
                Debug.Print("\n");
            }
        }
   }
}
