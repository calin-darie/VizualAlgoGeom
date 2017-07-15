using System.Collections.Generic;
using System.Linq;
using GeometricElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolboxGeometricElements;
using static VizualAlgoGeom.DcelFactory;
using Point = ToolboxGeometricElements.Point;

namespace VizualAlgoGeom.Tests
{
  [TestClass]
  public class SerializationTests
  {
    [TestMethod]
    public void DcelHalfEdgeSerialization()
    {
      var points = new[]
      {
            new Point(-1, 0, "p1"),
            new Point(0, 1, "p2")
      };

      var vertices = points.Select(p => new DcelVertex(p, p.Name)).ToArray();

      DcelHalfEdge edge = new DcelHalfEdge(vertices[0], vertices[1]);
      edge.Twin = edge;

      var serializer = new Serializer();
      var serialized = serializer.Serialize(edge);
      var deserialized = serializer.Deserialize<DcelHalfEdge>(serialized);

      Assert.AreEqual(deserialized, deserialized.Twin);
    }

    [TestMethod]
    public void DcelSerialization()
    {
      var faces = new List<Polyline>
      {
        new Polyline
        {
          Points =
          {
            new Point(-1, 0, "p1"),
            new Point(0, 1, "p2"),
            new Point(2, 0, "p3"),
            new Point(-1, 0, "p1")
          }
        }
      };

      Dcel dcel = CreateDcel(faces);

      DcelHalfEdge outerComponentFirstEdge = dcel.DcelFaces[0].OuterComponent;
      Assert.AreNotEqual(outerComponentFirstEdge.Start.Point, outerComponentFirstEdge.End.Point);

      var serializer = new Serializer();
      var serialized = serializer.Serialize(dcel);
      var deserialized = serializer.Deserialize<Dcel>(serialized);

      foreach (var edgeDiff in deserialized.DcelFaces.SelectMany(f => f.HalfEdges())
        .Zip(dcel.DcelFaces.SelectMany(f => f.HalfEdges()), (deserializedEdge, edge) => new 
        {
          Original = edge,
          Deserialized = deserializedEdge
        }))
      {
        Assert.AreEqual(edgeDiff.Original.Start.Point, edgeDiff.Deserialized.Start.Point);
        Assert.AreEqual(edgeDiff.Original.End.Point, edgeDiff.Deserialized.End.Point);
        Assert.AreEqual(edgeDiff.Original.Next.End.Point, edgeDiff.Deserialized.Next.End.Point);
        Assert.AreEqual(edgeDiff.Original.Prev.Start.Point, edgeDiff.Deserialized.Prev.Start.Point);
        Assert.AreEqual(edgeDiff.Original.Twin.Start.Point, edgeDiff.Deserialized.Twin.Start.Point);
        Assert.AreEqual(edgeDiff.Original.IsOriented, edgeDiff.Deserialized.IsOriented);
      }
    }
    
  }

  
}
