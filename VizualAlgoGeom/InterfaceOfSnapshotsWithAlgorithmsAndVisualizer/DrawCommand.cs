using System;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class DrawCommand<TypeToDraw> : MarshalByRefObject
  {
    public DrawCommand(TypeToDraw objectToDraw, VisualStyle visualStyle)
    {
      Object = objectToDraw;
      Style = visualStyle ?? new VisualStyle();
    }

    public TypeToDraw Object { get; private set; }
    public VisualStyle Style { get; private set; }
  }
}