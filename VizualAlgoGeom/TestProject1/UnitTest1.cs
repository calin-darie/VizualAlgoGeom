using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnapshotRecorder;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<int> list = new List<int>();
            SnapRecorder s = new SnapRecorder();
            s.AddObject(list);
            s.TakeSnapshot(0);
            list.Add(1);
            list.Add(1);
            list.Add(1);
            s.TakeSnapshot(0);
            Assert.Equals(s.SnapshotRecord.Count, 2);
            Assert.Equals(s.SnapshotRecord[1].Count, 3);
            list.RemoveAt(0);
            s.TakeSnapshot(0);
            Assert.Equals(s.SnapshotRecord.Count, 3);
            Assert.Equals(s.SnapshotRecord[2].Count, 2);
        }
    }
}
