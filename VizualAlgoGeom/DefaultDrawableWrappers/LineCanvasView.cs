using System.Collections.Generic;
using DefaultCanvasViews.TextPositions;
using GeometricElements;
using Infrastructure;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;

namespace DefaultCanvasViews
{
  public class LineCanvasView : CanvasViewBase, ICanvasView<Line>
  {
    public void Draw(DrawCommand<Line> command, DrawingContext context)
    {
      context.DrawingTools.GetColorPalette().SetColor(command.Style.Color);
      using (DrawingTool lineTool = context.DrawingTools.GetLineTool().Begin())
      {
        DrawVisiblePartOfLine(command, context, lineTool);
      }
      DrawName(command.Object, command.Style, context);
    }

    protected readonly PointCanvasView _pointView;

    public LineCanvasView(PointCanvasView pointView)
    {
      _pointView = pointView;
    }

    internal virtual void DrawVisiblePartOfLine(DrawCommand<Line> command, DrawingContext context, DrawingTool lineTool)
    {
      Line line = command.Object;
      VisualStyle visualStyle = command.Style;

      IList<Point> intersections =
        line.IntersectionsWithRectangle(
          context.ViewWindowWorldUnits.Left,
          context.ViewWindowWorldUnits.Right,
          context.ViewWindowWorldUnits.Bottom,
          context.ViewWindowWorldUnits.Top);

      if (intersections.Count != 2) return;

      context.DrawingTools.GetColorPalette().SetColor(visualStyle.Color);
      lineTool.Vertex(intersections[0]);
      lineTool.Vertex(intersections[1]);
    }

    protected virtual void DrawName(Line line, VisualStyle visualStyle, DrawingContext drawingContext)
    {
      if (!string.IsNullOrWhiteSpace(visualStyle.Name))
      {
        IList<Point> intersections =
          line.IntersectionsWithRectangle(
            drawingContext.ViewWindowWorldUnits.Left,
            drawingContext.ViewWindowWorldUnits.Right,
            drawingContext.ViewWindowWorldUnits.Bottom,
            drawingContext.ViewWindowWorldUnits.Top);

        if (intersections.Count == 2)
        {
          TextPosition intersection0TextPosition;
          TextPosition intersection1TextPosition;
          CreateTextPositions(
            intersections[0], intersections[1],
            drawingContext.ViewWindowWorldUnits,
            out intersection0TextPosition, out intersection1TextPosition);

          _pointView.Draw(
            new DrawCommand<Point>(intersections[0],
              new VisualStyle(visualStyle.Color, visualStyle.Name, intersection0TextPosition ?? visualStyle.TextPosition)),
            drawingContext);
          _pointView.Draw(
            new DrawCommand<Point>(intersections[1],
              new VisualStyle(visualStyle.Color, visualStyle.Name, intersection0TextPosition ?? visualStyle.TextPosition)),
            drawingContext);
        }
      }
    }

    void CreateTextPositions(
      Point intersection0, Point intersection1,
      RectangleWorldUnits viewWindow,
      out TextPosition intersection0TextPosition, out TextPosition intersection1TextPosition)
    {
      var positionFactory = new UniquePositionFactory();

      Position intersection0SecantTo = GetIntersectionPosition(intersection0, viewWindow, positionFactory);
      Position intersection1SecantTo = GetIntersectionPosition(intersection1, viewWindow, positionFactory);

      Position[] availablePositions = positionFactory.CreatePositionsRemaining();
      if (availablePositions.Length != 2)
      {
        intersection0TextPosition = null;
        intersection1TextPosition = null;
        return;
      }

      Position intersection0RelativePosition;
      Position intersection1RelativePosition;
      DetermineRelativePositions(intersection0, intersection1,
        availablePositions[0], availablePositions[1],
        out intersection0RelativePosition, out intersection1RelativePosition);

      intersection0TextPosition = CreateTextPosition(
        intersection0SecantTo, intersection0RelativePosition);
      intersection1TextPosition = CreateTextPosition(
        intersection1SecantTo, intersection1RelativePosition);
    }

    static TextPosition CreateTextPosition(
      Position intersectionSecantTo, Position intersectionRelativePosition)
    {
      var builder = new TextPositionBuilder();
      intersectionRelativePosition.AcceptTextPositionBuilder(builder);
      intersectionSecantTo.AcceptTextPositionBuilder(builder);
      TextPosition intersection0TextPosition = builder.TextPosition;
      return intersection0TextPosition;
    }

    void DetermineRelativePositions(
      Point intersection0, Point intersection1,
      Position availablePositionA, Position availablePositionB,
      out Position intersection0RelativePosition, out Position intersection1RelativePosition)
    {
      if (availablePositionA.Compare(intersection0, intersection1) > 0)
      {
        intersection0RelativePosition = availablePositionA;
        intersection1RelativePosition = availablePositionB;
      }
      else
      {
        intersection0RelativePosition = availablePositionB;
        intersection1RelativePosition = availablePositionA;
      }
    }

    static Position GetIntersectionPosition(Point intersection0, RectangleWorldUnits viewWindow,
      UniquePositionFactory positionFactory)
    {

      if (Numbers.EqualTolerant(viewWindow.Left, intersection0.X)) return positionFactory.Create<Left>();
      if (Numbers.EqualTolerant(viewWindow.Top, intersection0.Y)) return positionFactory.Create<Top>();
      if (Numbers.EqualTolerant(viewWindow.Right, intersection0.X)) return positionFactory.Create<Right>();
      if (Numbers.EqualTolerant(viewWindow.Bottom, intersection0.Y)) return positionFactory.Create<Bottom>();
      return null;
    }
  }
}