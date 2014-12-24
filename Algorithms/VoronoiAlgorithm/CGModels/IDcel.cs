using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiAlgorithmInterfaces.DataStructures
{
    public interface IDcel
    {
        global::CGModels.HalfEdge HalfEdgeLeftOf(global::GeometricElements.Point newSite);

        CGModels.HalfEdge HalfEdgeRightOf(CGModels.HalfEdge lbnd);
    }
}
