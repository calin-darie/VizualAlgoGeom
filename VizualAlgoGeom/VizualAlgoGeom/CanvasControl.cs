using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ToolboxGeometricElements;
using VizualAlgoGeom.ThreadSafeComponentHandling;

namespace VizualAlgoGeom
{
  internal delegate void ClippingBoundsChangedHandler(object sender, ClippingBoundsEventArgs e);

  public partial class CanvasControl : UserControl, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    internal CanvasControl()
    {
      InitializeComponent();
      InitializeCanvas();
      InitializeData();
      InitializeMouseAdaptor();
      InitializeKeyboardAdaptor();
      Load += CanvasControl_Load;
      _drawAxes = true;
      _shouldDrawGrid = true;
      _pointSize = DefaultPointSize;
      _lineSize = DefaultLineSize;
      _fontSize = DefaultFontSize;
      _gridColor = Color.LightBlue;
      _axesColor = Color.Red;
      _backgroundColor = Color.Azure;
      _zoomOx = DefaultZoomFactor;
      _zoomOy = DefaultZoomFactor;
      _isDragging = false;
    }

    private void InitializeKeyboardAdaptor()
    {
       KeyboardAdapter = new KeyboardAdapter(Canvas);
    }

    void InitializeMouseAdaptor()
    {
      MouseAdapter = new MouseAdapter(Canvas);
      MouseAdapter.MouseScroll += MouseScroll;
      MouseAdapter.MouseEnter += _canvas_MouseEnter;
      MouseAdapter.MouseDrag += MouseDrag;
      MouseAdapter.MouseDragEnd += MouseDragEnd;
    }

    void _canvas_MouseEnter(object sender, EventArgs e)
    {
      Canvas.Focus();
    }

    internal void PropertyChangedAction(object sender, PropertyChangedEventArgs e)
    {
      Canvas.Invalidate();
    }

    void InitializeCanvas()
    {
      LastCursor = new Cursor(new MemoryStream(CursorsResource.PointingFinger));
      Canvas = new GLControl
      {
        TabIndex = 0,
        VSync = false,
        Cursor = LastCursor
      };
      Canvas.Paint += canvas_Paint;
      Controls.Add(Canvas);
    }

    internal void InitializeData()
    {
      Data = new Data();
      Data.AddDefaultGroup();
    }

    void SetCanvas()
    {
      Canvas.Dock = DockStyle.Fill;
      GL.ClearColor(_backgroundColor);
      GL.PointSize(_pointSize);
      GL.Enable(EnableCap.AlphaTest);
      GL.AlphaFunc(AlphaFunction.Notequal, 0);
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
      GL.Enable(EnableCap.PointSmooth);
      GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
      GL.Enable(EnableCap.LineSmooth);
      var top = 30;
      var bottom = 0;
      var left = 0;
      var right = Canvas.Width * PixelHeightInWorldUnits(top, bottom, Canvas.Height);
      SetBoundsWorldUnits(left, right, bottom, top);
      Canvas.Resize += canvas_Resize;
    }

    void UpdateGlOrtho()
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(_left, _right, _bottom, _top, 0, 1);
      GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
      Canvas.Invalidate();
    }

    internal void InvalidateCanvas()
    {
      Canvas.UiThreadInvalidate();
    }

    void SetBoundsWorldUnits(double newLeft, double newRight, double newBottom, double newTop)
    {
      if (newTop <= newBottom) return;
      if (newRight <= newLeft) return;
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (_left != newLeft)
      {
        _left = newLeft;
        NotifyPropertyChanged("Left");
      }
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (_right != newRight)
      {
        _right = newRight;
        NotifyPropertyChanged("Right");
      }
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (_bottom != newBottom)
      {
        _bottom = newBottom;
        NotifyPropertyChanged("Bottom");
      }
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      if (_top != newTop)
      {
        _top = newTop;
        NotifyPropertyChanged("Top");
      }
      UpdateGlOrtho();
      _canvasSizeOnLastBoundsChange = Canvas.Size;
    }

    void canvas_Paint(object sender, PaintEventArgs e)
    {
      if (!_glControlLoaded)
        return;
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      if (_shouldDrawGrid)
        DrawGrid();
      if (_drawAxes)
        DrawAxis();

      var canvasSize = new Size(Canvas.Width, Canvas.Height);

      foreach (Group group in Data.Groups)
      {
        group.PointList.CanvasDraw(canvasSize, _fontSize);
        group.WeightedPointList.CanvasDraw(canvasSize, _fontSize);
        group.LineSegmentList.CanvasDraw(_left, _right, _bottom, _top, canvasSize.Height, _fontSize);
        group.LineList.CanvasDraw(_left, _right, _bottom, _top, canvasSize.Height, _fontSize);
        group.RayList.CanvasDraw(_left, _right, _bottom, _top, canvasSize.Height, _fontSize);
        group.PolylineList.CanvasDraw(canvasSize, _fontSize);
        group.ClosedPolylineList.CanvasDraw(canvasSize, _fontSize);
      }

      IEnumerable<IPendingDraw> drawableObjects = Data.DrawableObjects;
      if (drawableObjects != null)
        foreach (IPendingDraw pendingDraw in drawableObjects)
        {
          try
          {
            pendingDraw.Execute(
              new DrawingContext(_canvasDrawingToolFactory,
                new RectangleWorldUnits
                {
                  Left = _left,
                  Right = _right,
                  Bottom = _bottom,
                  Top = _top
                },
                new SizePx
                {
                  Width = Canvas.Size.Width,
                  Height = Canvas.Size.Height
                },
                _fontSize));
          }
          catch (Exception)
          {
            //todo: log & report
          }
        }

      Canvas.SwapBuffers();
    }

    /// <summary>
    /// </summary>
    /// <returns> World value for one pixel on the horizontal</returns>
    double PixelWidthInWorldUnits()
    {
      return (_right - _left)/Canvas.Width;
    }

    /// <summary>
    /// </summary>
    /// <returns>World value for one pixel on the vertical</returns>
    double PixelHeightInWorldUnits()
    {
      return PixelHeightInWorldUnits(_top, _bottom, Canvas.Height);
    }

    static double PixelHeightInWorldUnits(double top, double bottom, int canvasHeightPx)
    {
      return (top - bottom)/canvasHeightPx;
    }

    void DrawAxis()
    {
      GL.LineWidth(GridAndAxesLineWidth);
      //Draw the lines for the axes
      GL.Color3(_axesColor);
      GL.Begin(BeginMode.Lines);
      GL.Vertex2(_left, 0);
      GL.Vertex2(_right, 0);
      GL.Vertex2(0, _bottom);
      GL.Vertex2(0, _top);
      GL.End();
      //Draw the horizontal arrow
      double xP = PixelWidthInWorldUnits();
      double yP = PixelHeightInWorldUnits();
      GL.Begin(BeginMode.LineStrip);
      GL.Vertex2(_right - xP*10, 0 - yP*5);
      GL.Vertex2(_right, 0);
      GL.Vertex2(_right - xP*10, 0 + yP*5);
      GL.End();
      //Draw the vertical arrow
      GL.Begin(BeginMode.LineStrip);
      GL.Vertex2(0 - xP*5, _top - yP*10);
      GL.Vertex2(0, _top);
      GL.Vertex2(0 + xP*5, _top - yP*10);
      GL.End();
      GL.LineWidth(LineSize);
    }

    /// <summary>
    ///   computes the horizontal and the vertical span
    ///   between horizontal and vertical grid lines respectively;
    ///   then draws the lines within the clipping bounds
    /// </summary>
    void DrawGrid()
    {
      /*
      /// On each side of a grid cell, 
      /// the user should be able to distinguish between
      /// corner points and middle points
      /// *-*-*
      /// |---|
      /// *-*-*
      /// |---|
      /// *-*-*
      /// The span between the grid lines should then be at least
      /// five times the size of the point representation
      const int POINTS_PER_GRIDCELL_SIDE = 5;
      double minHorizontalSpan = pixelWidthInWorldUnits() * _pointSize * POINTS_PER_GRIDCELL_SIDE;
      double minVerticalSpan = pixelHeightInWorldUnits() * _pointSize * POINTS_PER_GRIDCELL_SIDE;
      */
      double minHorizontalSpan = PixelWidthInWorldUnits()*50;
      double minVerticalSpan = PixelWidthInWorldUnits()*50;


      // The actual span should be a power of ten, so choose 
      // the smallest power of ten greater than or equal to the min span
      double horizontalSpan = Math.Pow(10, Math.Ceiling(Math.Log10(minHorizontalSpan)));
      double verticalSpan = Math.Pow(10, Math.Ceiling(Math.Log10(minVerticalSpan)));

      // Actual drawing
      GL.LineWidth(GridAndAxesLineWidth);
      GL.Color3(_gridColor);
      GL.Begin(BeginMode.Lines);
      for (double x = horizontalSpan*Math.Ceiling(_left/horizontalSpan);
        x <= _right;
        x += horizontalSpan)
      {
        GL.Vertex2(x, _bottom);
        GL.Vertex2(x, _top);
      }
      for (double y = verticalSpan*Math.Ceiling(_bottom/verticalSpan);
        y <= _top;
        y += verticalSpan)
      {
        GL.Vertex2(_left, y);
        GL.Vertex2(_right, y);
      }
      GL.End();
      GL.LineWidth(LineSize);
    }

    void canvas_Resize(object sender, EventArgs e)
    {
      double oldPixelWidthWorldUnits = (_right - _left) / _canvasSizeOnLastBoundsChange.Width;
      double oldPixelHeightWorldUnits = PixelHeightInWorldUnits(_top, _bottom, _canvasSizeOnLastBoundsChange.Height);
      int deltaWidthPx = Canvas.Size.Width - _canvasSizeOnLastBoundsChange.Width; 
      int deltaHeightPx = Canvas.Size.Height - _canvasSizeOnLastBoundsChange.Height;
      double deltaWidthWorldUnits = deltaWidthPx*oldPixelWidthWorldUnits;
      double deltaHeightWorldUnits = deltaHeightPx*oldPixelHeightWorldUnits;

      SetBoundsWorldUnits(_left, _right + deltaWidthWorldUnits, _bottom - deltaHeightWorldUnits, _top);
    }

    internal void CanvasControl_Load(object sender, EventArgs e)
    {
      _glControlLoaded = true;
      SetCanvas();
    }

    internal void CenterOrigin()
    {
      double newRight = (_right - _left)/2;
      double newLeft = -newRight;
      double newTop = (_top - _bottom)/2;
      double newBottom = -newTop;
      SetBoundsWorldUnits(newLeft, newRight, newBottom, newTop);
    }

    void NotifyPropertyChanged(String info)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(info));
      }
    }

    void MouseDrag(object sender, MouseEventArgs e)
    {
      if (_isDragging == false)
      {
        LastCursor = Canvas.Cursor;
        Canvas.Cursor = new Cursor(new MemoryStream(CursorsResource.DraggingHand));
        _isDragging = true;
      }

      var ev = (MouseMoveEventArgs) e;
      double xStart;
      double yStart;

      CoordinateConverter.GetWorldCoordinates(ev.XStart, Canvas.Height - ev.YStart, out xStart, out yStart);
      double xEnd;
      double yEnd;

      CoordinateConverter.GetWorldCoordinates(ev.X, Canvas.Height - ev.Y, out xEnd, out yEnd);


      double xDifference = xStart - xEnd;
      double yDifference = yStart - yEnd;


      SetBoundsWorldUnits(
        _left + xDifference,
        _right + xDifference, 
        _bottom + yDifference,
        _top + yDifference);
    }

    #region Consts

    const double ZoomPerScroll = 0.1;
    const double MaxZoom = 10;
    const double MinZoom = 0.3;

    const float DefaultFontSize = 10;
    const float DefaultPointSize = 8;
    const float DefaultLineSize = 3;
    const float DefaultZoomFactor = 1;

    const float FontIncrement = 0.5f;
    const float PointIncrement = 1;
    const float LineSizeIncrement = 1;

    const int MaxFontZize = 20;
    const int MinFontZize = 10;

    const float MaxPointSize = 30;
    const float MinPointSize = 6;

    const float MaxLineSize = 25;
    const float MinLineSize = 1;

    #endregion

    #region Private members

    bool _isDragging;
    bool _glControlLoaded;

    bool _drawAxes;
    bool _shouldDrawGrid;
    bool _preserveAspectRatio;

    double _left;
    double _right;
    double _top;
    double _bottom;

    double _zoomOx;
    double _zoomOy;

    float _pointSize;
    Color _gridColor;
    Color _axesColor;
    Color _backgroundColor;
    float _fontSize;
    readonly DrawingToolFactory _canvasDrawingToolFactory = new DrawingToolFactory();
    private Size _canvasSizeOnLastBoundsChange;
    private float _lineSize;
    const int GridAndAxesLineWidth = 1;

    #endregion

    #region Properties

    internal MouseAdapter MouseAdapter { get; set; }

    internal KeyboardAdapter KeyboardAdapter { get; set; }

    internal Data Data { get; set; }

    internal GLControl Canvas { get; set; }

    internal Cursor LastCursor { get; set; }

    [Category("Accesories"), Description(""), Show(true)]
    public bool DrawAxes
    {
      get { return _drawAxes; }
      set
      {
        _drawAxes = value;
        Canvas.Invalidate();
      }
    }

    [Category("Accesories"), Description(""), Show(true)]
    public bool ShouldDrawGrid
    {
      get { return _shouldDrawGrid; }
      set
      {
        _shouldDrawGrid = value;
        Canvas.Invalidate();
      }
    }

    [Category("Scale"), Description(""), Show(true)]
    public bool PreserveAspectRatio
    {
      get { return _preserveAspectRatio; }
      set
      {
        _preserveAspectRatio = value;
        if (value)
        {
          ZoomOy = _zoomOx;
        }
      }
    }

    [Category("Scale"), Description(""), Show(true)]
    public double ZoomOx
    {
      get { return _zoomOx; }
      set
      {
        double zoomFraction = _zoomOx/value;
        if (false == _preserveAspectRatio)
          Zoom((_right + _left)/2, (_top + _bottom)/2, zoomFraction, 1);
        else
        {
          Zoom((_right + _left)/2, (_top + _bottom)/2, zoomFraction, zoomFraction);
          _zoomOy = value;
          NotifyPropertyChanged("ZoomOY");
        }
        _zoomOx = value;
        NotifyPropertyChanged("ZoomOX");
      }
    }

    [Category("Scale"), Description(""), Show(true)]
    public double ZoomOy
    {
      get { return _zoomOy; }
      set
      {
        double zoomFraction = _zoomOy/value;
        if (false == _preserveAspectRatio)
          Zoom((_right + _left)/2, (_top + _bottom)/2, 1, zoomFraction);
        else
        {
          Zoom((_right + _left)/2, (_top + _bottom)/2, zoomFraction, zoomFraction);
          _zoomOx = value;
          NotifyPropertyChanged("ZoomOX");
        }
        _zoomOy = value;
        NotifyPropertyChanged("ZoomOY");
      }
    }

    [Category("Clipping rectangle"), Description(""), Show(true)]
    public new double Left
    {
      get { return _left; }
      set { SetBoundsWorldUnits(value, Right, Bottom, Top); }
    }

    [Category("Clipping rectangle"), Description(""), Show(true)]
    public new double Right
    {
      get { return _right; }
      set { SetBoundsWorldUnits(Left, value, Bottom, Top); }
    }

    [Category("Clipping rectangle"), Description(""), Show(true)]
    public new double Bottom
    {
      get { return _bottom; }
      set { SetBoundsWorldUnits(Left, Right, value, Top); }
    }

    [Category("Clipping rectangle"), Description(""), Show(true)]
    public new double Top
    {
      get { return _top; }
      set { SetBoundsWorldUnits(Left, Right, Bottom, value); }
    }

    [Category("Colors"),
     DisplayName("Background color"),
     Show(true)]
    public Color BackgroundColor
    {
      get { return _backgroundColor; }

      set
      {
        _backgroundColor = value;
        GL.ClearColor(value);
        Canvas.Invalidate();
      }
    }

    [Category("Colors"),
     DisplayName("Grid color"),
     Description("Color used when rendering the grid"),
     Show(true)]
    public Color GridColor
    {
      get { return _gridColor; }

      set
      {
        _gridColor = value;
        Canvas.Invalidate();
      }
    }

    [Category("Colors"),
     DisplayName("Axes color"),
     Description("Color used when rendering the axes"),
     Show(true)]
    public Color AxesColor
    {
      get { return _axesColor; }

      set
      {
        _axesColor = value;
        Canvas.Invalidate();
      }
    }

    [Category("Miscelaneous"),
     DisplayName("Point size"),
     Description(""),
     Show(true)]
    public float PointSize
    {
      get { return _pointSize; }

      set
      {
        if (!(MinPointSize < value && value <= MaxPointSize)) return;
        _pointSize = value;
        NotifyPropertyChanged("PointSize");
        GL.PointSize(_pointSize);
        Canvas.Invalidate();
      }
    }

    [Category("Miscelaneous"),
     DisplayName("Line size"),
     Description(""),
     Show(true)]
    public float LineSize
    {
      get { return _lineSize; }

      set
      {
        if (!(MinLineSize < value && value <= MaxLineSize)) return;
        _lineSize = value;
        NotifyPropertyChanged("LineSize");
        GL.LineWidth(_lineSize);
        Canvas.Invalidate();
      }
    }


    [Category("Miscelaneous"),
     DisplayName("Font size"),
     Description(""),
     Show(true)]
    public float FontSize
    {
      get { return _fontSize; }

      set
      {
        if (!(MinFontZize <= value) || !(value <= MaxFontZize)) return;
        _fontSize = value;
        NotifyPropertyChanged("FontSize");
        Canvas.Invalidate();
      }
    }

    #endregion

    #region Zoom

    void ZoomText(int delta)
    {
      if (delta > 0)
      {
        FontSize += FontIncrement;
        PointSize += PointIncrement;
        LineSize += LineSizeIncrement;
      }
      if (delta < 0)
      {
        FontSize -= FontIncrement;
        PointSize -= PointIncrement;
        LineSize -= LineSizeIncrement;
      }
    }

    void ZoomWithDelta(int mouseX, int mouseY, int delta)
    {
      double x, y;
      CoordinateConverter.GetWorldCoordinates(mouseX, Canvas.Height - mouseY, out x, out y);
      double oldZoomOx = _zoomOx;
      double oldZoomOy = _zoomOy;
      if (delta > 0)
      {
        if (_zoomOx + ZoomPerScroll <= MaxZoom)
        {
          _zoomOx += ZoomPerScroll;
          NotifyPropertyChanged("ZoomOX");
        }
        if (_zoomOy + ZoomPerScroll <= MaxZoom)
        {
          _zoomOy += ZoomPerScroll;
          NotifyPropertyChanged("ZoomOY");
        }
      }
      else if (delta < 0)
      {
        if (_zoomOx - ZoomPerScroll >= MinZoom)
        {
          _zoomOx -= ZoomPerScroll;
          NotifyPropertyChanged("ZoomOX");
        }
        if (_zoomOy - ZoomPerScroll >= MinZoom)
        {
          _zoomOy -= ZoomPerScroll;
          NotifyPropertyChanged("ZoomOY");
        }
      }
      double zoomFractionOx = oldZoomOx/_zoomOx;
      double zoomFractionOy = oldZoomOy/_zoomOy;

      Zoom(x, y, zoomFractionOx, zoomFractionOy);
    }

    void Zoom(double worldX, double worldY, double zoomFractionOx, double zoomFractionOy)
    {
      _left = worldX - ((worldX - _left)*zoomFractionOx);
      _right = worldX + ((_right - worldX)*zoomFractionOx);
      _top = worldY + ((_top - worldY)*zoomFractionOy);
      _bottom = worldY - ((worldY - _bottom)*zoomFractionOy);
      SetBoundsWorldUnits(_left, _right, _bottom, _top);
    }

    #endregion

    #region Mouse actions

    void MouseDragEnd(object sender, MouseEventArgs e)
    {
      Canvas.Cursor = LastCursor;
      _isDragging = false;
    }

    void MouseScroll(object sender, MouseEventArgs e)
    {
      if ((ModifierKeys & Keys.Control) != 0)
        ZoomText(e.Delta);
      else
      {
        ZoomWithDelta(e.X, e.Y, e.Delta);
      }
    }

    #endregion
  }
}