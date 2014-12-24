using GeometricElements;

namespace VoronoiAlgorithm
{
  public interface IEvent
  {
    Point Point { get; }
    void Fire();
  }
}