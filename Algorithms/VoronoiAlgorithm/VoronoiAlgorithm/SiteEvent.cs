using System;
using GeometricElements;

namespace VoronoiAlgorithm
{
  internal class SiteEvent : IEvent
  {
    public Point Point
    {
      get { return _site; }
    }

    public void Fire()
    {
      _algorithm.Handle(this);
    }

    readonly VoronoiAlgorithm _algorithm;
    readonly Point _site;

    public SiteEvent(VoronoiAlgorithm algorithm, Point site)
    {
      if (site == null)
      {
        throw new ArgumentNullException("site");
      }
      if (algorithm == null)
      {
        throw new ArgumentNullException("algorithm");
      }

      _algorithm = algorithm;
      _site = site;
    }
  }
}