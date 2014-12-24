using System;
using InterfaceOfAlgorithmAdaptersWithVisualizer;

namespace DefaultAuxiliariesImplementation
{
  public class DefaultPseudocodeLine : MarshalByRefObject, IPseudocodeLine
  {
    public int Depth { get; private set; }
    public string Text { get; private set; }

    public DefaultPseudocodeLine(int depth, string line)
    {
      Depth = depth;
      Text = line;
    }
  }
}