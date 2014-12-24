using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometricElements;

namespace CGModels
{
    public class Edge
    {
        public double a, b, c; // line eq
        Point[] EndPoints;
        //Point[] reg; // Sites this edge bisects
        int edgenbr;

        public Edge()
        {
            a = 0;
            b = 0; 
            c = 0;
            EndPoints = new Point[2];
            //reg = new Site[2];
        }
    }
}
