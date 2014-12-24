using System;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  internal class NewGeometricElmentEventArgs : EventArgs
  {
    internal NewGeometricElmentEventArgs(GeometricElement geometricElement)
    {
      GeometricElement = geometricElement;
    }

    internal GeometricElement GeometricElement { get; private set; }
  }
}