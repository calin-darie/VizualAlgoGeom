using System;
using System.Collections.Generic;
using System.Linq;
using DefaultCanvasViews.TextPositions;

namespace DefaultCanvasViews
{
  internal class UniquePositionFactory
  {
    readonly HashSet<Type> _positionsRemaining;

    internal UniquePositionFactory()
    {
      _positionsRemaining = new HashSet<Type>
      {
        typeof (Left),
        typeof (Right),
        typeof (Top),
        typeof (Bottom)
      };
    }

    internal TPosition Create<TPosition>() where TPosition : Position, new()
    {
      if (_positionsRemaining.Remove(typeof (TPosition)))
      {
        return new TPosition();
      }
      throw new InvalidOperationException("this position was already created");
    }

    public Position[] CreatePositionsRemaining()
    {
      Position[] result = _positionsRemaining
        .Select(positionType => (Position) Activator.CreateInstance(positionType))
        .ToArray();

      _positionsRemaining.Clear();
      return result;
    }
  }
}