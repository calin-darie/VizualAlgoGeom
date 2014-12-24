using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGModels
{
    public enum EdgeListPm
    {
        left,
        right
    }

    public class HalfEdge
    {
        HalfEdge Previous, Next;
        Edge Edge;
        EdgeListPm ELpm;
        //Site vertex;
    }
}
