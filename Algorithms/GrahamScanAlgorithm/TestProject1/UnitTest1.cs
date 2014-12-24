using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometricElements2;
using GrahamScanAlgorithm;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace TestProject1
{
    class SnapRecorder : ISnapshotRecorder
    {

        public int AddObject(object obj, string name, System.Drawing.Color color)
        {
            return 0;
        }

        public int AddObject(object obj)
        {
            return 0;
        }

        public void RemoveObject(int hash)
        {
        }

        public void RemoveObject(object obj)
        {
        }

        public void ReplaceObject(int hash, object newObject)
        {
        }

        public void TakeSnapshot(int pseudocodeLine)
        {
        }

        public int AddObject(object obj, IVisualHints visualHints)
        {
            return 0;
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Point[] Points = {
                new Point(1, 1),
                new Point(-1, 1),
                new Point(0, 0),
                new Point(1, -1),
                new Point(-1, -1),
            };
          //  IList<Point> ch = new GrahamScan(Points, (ISnapshotRecorder) new SnapRecorder()).Graham();
            GrahamScan gs = new GrahamScan(Points, (ISnapshotRecorder)new SnapRecorder());
            double left = gs.isLeft(Points[0], Points[2], Points[1]);
            Assert.IsTrue(left < 0);
          //  gs.sorting(ref Points);
          //  Assert.AreEqual(Points[4].X, 1);
          //  Assert.AreEqual(Points[4].Y, 1);
        }
    }
}
