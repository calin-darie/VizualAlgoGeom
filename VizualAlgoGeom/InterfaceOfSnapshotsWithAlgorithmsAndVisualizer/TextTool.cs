//using System.Drawing;
//using OpenTK.Graphics;
//using GL = OpenTK.Graphics.OpenGL.GL;
//using Point = GeometricElements.Point;

//namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
//{
//  public class TextTool
//  {
//    protected readonly TextPrinter _textPrinter = new TextPrinter(TextQuality.Medium);

//    public void PrintName(Point point, VisualStyle visualStyle, int canvasHeight, Font font)
//    {
//      SizeF textSize = _textPrinter.Measure(visualStyle.Name, font).BoundingBox.Size;

//      float horizontalTranslation = 0;
//      float verticalTranslation = 0;
//      if (visualStyle.TextPosition.HorizontalPosition == TextHorizontalPosition.LeftOfPoint)
//        horizontalTranslation = -textSize.Width;
//      if (visualStyle.TextPosition.VerticalPosition == TextVerticalPosition.AbovePoint)
//        verticalTranslation = textSize.Height;

//      _textPrinter.Begin();
//      System.Drawing.Point pointPx = point.RelativeToWindowPx();
//      GL.Translate(pointPx.X + horizontalTranslation, canvasHeight - (pointPx.Y + verticalTranslation), 0);
//      _textPrinter.Print(visualStyle.Name, font, visualStyle.Color);
//      _textPrinter.End();
//    }
//  }
//}

using System.Drawing;
using OpenTK.Graphics;
using GetPName = OpenTK.Graphics.OpenGL.GetPName;
using Glu = Tao.OpenGl.Glu;
using GL = OpenTK.Graphics.OpenGL.GL;
using Point = GeometricElements.Point;

namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class TextTool
  {
    protected readonly TextPrinter _textPrinter = new TextPrinter(TextQuality.Medium);

    public void PrintName(Point point, VisualStyle visualStyle, int canvasHeight, Font font)
    {
      SizeF textSize = _textPrinter.Measure(visualStyle.Name, font).BoundingBox.Size;

      float horizontalTranslation = visualStyle.TextPosition.HorizontalPosition == TextHorizontalPosition.LeftOfPoint
        ? -textSize.Width
        : 0;

      float verticalTranslation = visualStyle.TextPosition.VerticalPosition == TextVerticalPosition.AbovePoint
        ? textSize.Height
        : 0;
      
      System.Drawing.Point pointPx = point.RelativeToWindowPx();

      _textPrinter.Begin();
      GL.Translate(pointPx.X + horizontalTranslation, canvasHeight - (pointPx.Y + verticalTranslation), 0);
      _textPrinter.Print(visualStyle.Name, font, visualStyle.Color);
      _textPrinter.End();
    }
  }
}