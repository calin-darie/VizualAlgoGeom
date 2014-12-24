using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceOfCanvasWrappersWithVisualizer
{
    public interface IDrawableOnCanvas
    {
        void CanvasDraw(
            double left, double right,
            double bottom, double top,
            float fontSize,
            int canvasWidth, int canvasHeight);
    }
}
