using System;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class DrawingContext : MarshalByRefObject
  {
    public DrawingContext(
      DrawingToolFactory drawingToolFactory,
      RectangleWorldUnits viewWindowWorldUnitsWorldUnits,
      SizePx canvasSize,
      float fontSize)
    {
      CanvasSizePx = canvasSize;
      ViewWindowWorldUnits = viewWindowWorldUnitsWorldUnits;
      DrawingTools = drawingToolFactory;
      FontSize = fontSize;
      ViewWindowWorldUnits = viewWindowWorldUnitsWorldUnits;
    }

    public SizeWorldUnits PixelSizeInWorldUnits
    {
      get
      {
        return new SizeWorldUnits
        {
          Width = ViewWindowWorldUnits.Width/CanvasSizePx.Width,
          Height = ViewWindowWorldUnits.Height/CanvasSizePx.Height
        };
      }
    }

    public DrawingToolFactory DrawingTools { get; private set; }
    public float FontSize { get; private set; }
    public RectangleWorldUnits ViewWindowWorldUnits { get; private set; }
    public SizePx CanvasSizePx { get; private set; }
  }

  public class SizeWorldUnits : MarshalByRefObject
  {
    public double Width { get; set; }
    public double Height { get; set; }
  }

  public class RectangleWorldUnits : MarshalByRefObject
  {
    public double Top { get; set; }
    public double Left { get; set; }
    public double Bottom { get; set; }
    public double Right { get; set; }

    public double Width
    {
      get { return Right - Left; }
    }

    public double Height
    {
      get { return Top - Bottom; }
    }
  }

  public class SizePx : MarshalByRefObject
  {
    public int Width { get; set; }
    public int Height { get; set; }
  }
}