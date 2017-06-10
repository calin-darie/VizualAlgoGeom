using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;

namespace VizualAlgoGeom
{
  internal class MouseAdapter
  {
    MouseButtons _buttonDragging = MouseButtons.None;
    int _clickcount;
    Point _dragStart;
    ClickInfo _lastClickInfo;
    readonly GLControl _control;
    readonly int _doubleClickMaxDeltaX = SystemInformation.DoubleClickSize.Width/2;
    readonly int _doubleClickMaxDeltaY = SystemInformation.DoubleClickSize.Height/2;
    readonly int _doubleClickTime = SystemInformation.DoubleClickTime;

    readonly Dictionary<MouseButtons, bool> _isPressed =
      new Dictionary<MouseButtons, bool>
      {
        {MouseButtons.Left, false},
        {MouseButtons.Right, false},
        {MouseButtons.Middle, false},
        {MouseButtons.XButton1, false},
        {MouseButtons.XButton2, false}
      };

    internal MouseAdapter(GLControl control)
    {
      _control = control;
      control.MouseDown += control_MouseButtonPressed;
      control.MouseUp += control_MouseButtonReleased;
      control.MouseMove += control_MouseMove;
      control.Click += control_Click;
    }

    internal event EventHandler MouseEnter
    {
      add { _control.MouseEnter += value; }
      remove { _control.MouseEnter -= value; }
    }

    internal event MouseEventHandler MouseLeftClick;
    internal event MouseEventHandler MouseRightClick;
    internal event MouseEventHandler MouseMiddleClick;
    internal event MouseEventHandler MouseLeftDoubleClick;
    internal event MouseEventHandler MouseMove;
    internal event MouseEventHandler MouseDrag;
    internal event MouseEventHandler MouseDragStart;
    internal event MouseEventHandler MouseDragEnd;

    internal event MouseEventHandler MouseScroll
    {
      add { _control.MouseWheel += value; }
      remove { _control.MouseWheel -= value; }
    }

    void control_MouseButtonPressed(object sender, MouseEventArgs e)
    {
      _isPressed[e.Button] = true;
      if (_buttonDragging != MouseButtons.None)
      {
        FireMouseDragEnd(sender, e);
        _buttonDragging = MouseButtons.None;
      }
    }

    void control_MouseButtonReleased(object sender, MouseEventArgs e)
    {
      MouseButtons button = e.Button;
      if (_buttonDragging == button)
      {
        FireMouseDragEnd(sender, e);
        _buttonDragging = MouseButtons.None;
      }
      else if (_isPressed[button])
      {
        var info = new ClickInfo {_time = DateTime.Now, _position = {_x = e.X, _y = e.Y}};

        if ((info._time.Subtract(_lastClickInfo._time).Milliseconds > _doubleClickTime ||
             Math.Abs(info._position._x - _lastClickInfo._position._x) > _doubleClickMaxDeltaX ||
             Math.Abs(info._position._y - _lastClickInfo._position._y) > _doubleClickMaxDeltaY))
        {
          _clickcount = 0;
        }

        _clickcount++;
        _lastClickInfo = info;

        if (_clickcount == 1)
        {
          if (button == MouseButtons.Left)
            FireMouseLeftClick(sender, e);
          else if (button == MouseButtons.Right)
            FireMouseRightClick(sender, e);
          else if (button == MouseButtons.Middle)
              FireMouseMiddleClick(sender, e);
        }
        else if (_clickcount == 2)
        {
          if (button == MouseButtons.Left)
            FireMouseLeftDoubleClick(sender, e);
        }
      }

      _isPressed[button] = false;
    }
      
      void control_Click(object sender, EventArgs e)
    {      
    }

    void control_MouseMove(object sender, MouseEventArgs e)
    {
      foreach (MouseButtons button in Enum.GetValues(typeof (MouseButtons)))
      {
        if (button != MouseButtons.None && _isPressed[button])
        {
          if (_buttonDragging == MouseButtons.None)
          {
            _buttonDragging = button;

            _dragStart._x = e.X;
            _dragStart._y = e.Y;
            FireDragStart(sender, e);
          }
          else
          {
            var ev = new MouseMoveEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta,
              _dragStart._x, _dragStart._y);
            _dragStart._x = e.X;
            _dragStart._y = e.Y;
            FireMouseDrag(sender, ev);
          }
        }
      }
      if (_buttonDragging == MouseButtons.None)
      {
        FireMouseMove(sender, e);
      }
    }

    struct Point // todo: reduce nr. of point structs. use system.Drawing?
    {
      internal int _x, _y;
    }

    struct ClickInfo
    {
      internal Point _position;
      internal DateTime _time;
    }

    #region fire events

    #region fire drag events

    void FireDragStart(object sender, MouseEventArgs e)
    {
      if (null != MouseDragStart)
        MouseDragStart(sender, e);
    }

    void FireMouseDrag(object sender, MouseMoveEventArgs e)
    {
      if (MouseDrag != null)
        MouseDrag(sender, e);
    }

    void FireMouseDragEnd(object sender, MouseEventArgs e)
    {
      if (null != MouseDragEnd)
        MouseDragEnd(sender, e);
    }

    #endregion fire drag events

    void FireMouseRightClick(object sender, MouseEventArgs e)
    {
      if (MouseRightClick != null)
        MouseRightClick(sender, e);
    }

    void FireMouseLeftClick(object sender, MouseEventArgs e)
    {
      if (MouseLeftClick != null)
        MouseLeftClick(sender, e);
    }
    void FireMouseMiddleClick(object sender, MouseEventArgs e)
    {
        if (MouseMiddleClick != null)
            MouseMiddleClick(sender, e);
    }

    void FireMouseLeftDoubleClick(object sender, MouseEventArgs e)
    {
      if (MouseLeftDoubleClick != null)
        MouseLeftDoubleClick(sender, e);
    }

    void FireMouseMove(object sender, MouseEventArgs e)
    {
      if (MouseMove != null)
        MouseMove(sender, e);
    }

    #endregion fire events
  }
}