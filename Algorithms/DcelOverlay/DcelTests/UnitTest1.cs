using System;
using System.Runtime.CompilerServices;
using DcelOverlayAlgorithm;
using GeometricElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DcelTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dummyNext = new DcelHalfEdge(new DcelVertex(new Point(100, 100), "dummy"), new DcelVertex(new Point(5,5),"dummy2" )) ;
            var north = new DcelVertex(new Point(0, 1), "north");
            var south = new DcelVertex(new Point(0,-1 ), "SOUTH");
            var east =  new DcelVertex(new Point(1, 0 ), "east");
            var west =  new DcelVertex(new Point(-1, 0), "west");
            
            var intersection = new DcelVertex(new Point(0, 0), "intersection");
            var eNS = new DcelHalfEdge(north, south);
            eNS.SetNext(dummyNext);
            var eSN = new DcelHalfEdge(south, north);
            eSN.SetNext(dummyNext);
            eSN.SetTwin(eNS);
            eNS.SetTwin(eSN);
            var eWE = new DcelHalfEdge(west, east);
            var eEW = new DcelHalfEdge(east, west);
            eEW.SetNext(dummyNext);
            eWE.SetNext(dummyNext);
            eEW.SetTwin(eWE);
            eWE.SetTwin(eEW);
            intersection.AddIncidentEdge(eSN);
            intersection.AddIncidentEdge(eEW);

            var split = DcelOverlay.SplitHalfEdges(intersection);

            var expectedEdge = split[2].GetTwin();
            var edgeNextAfterFirst = split[0].GetNext();
            Assert.AreEqual(expectedEdge, edgeNextAfterFirst, "After {1}, {0} should follow. {2} follows instead.", expectedEdge.GetName(), split[0].GetName(), edgeNextAfterFirst.GetName());


            //Testeaza logica de la SPLIT edges.
        }
    }
}
