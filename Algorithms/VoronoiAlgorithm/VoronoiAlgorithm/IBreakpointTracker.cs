using GeometricElements;

namespace VoronoiAlgorithm
{
  public interface IBreakpointTracker
  {
    Point Breakpoint { get; }
    bool IsGoingLeft { get; }
    IEdge Edge { get; }

    /// <summary>
    ///   this is the site generating the lead arc - expanding more quickly than the base
    /// </summary>
    Point TopArcSite { get; }

    /// <summary>
    ///   this is the site generating the base arc - expanding more slowly than the top
    /// </summary>
    Point BottomArcSite { get; }

    Point PreviouslyKnownPoint { get; }
    Point LeftArcSite { get; }
    Point RightArcSite { get; }
    void SetEdgeEndpoint(Point v);
  }
}