using System;

namespace GeometricElements
{
  [Serializable]
  public class DcelHalfEdge
  {
    DcelVertex _start, _end, _upper, _lower;
    DcelHalfEdge _twin;
    DcelHalfEdge _next, _prev;
    DcelFace _incidentFace;
    string _name;
    bool _correctOrientation;
    public int DcelParent { get; set; }
    public LineSegment ToLineSegment { get; private set; }


    public DcelHalfEdge(DcelVertex dcelVertex, DcelVertex nextVertex)
    {
      _start = dcelVertex;
      _end = nextVertex;
      _twin = null;
      ResetLineToSegment();
      _correctOrientation = false;
      SetUpperLowerEndPoints();
    }

    public void ResetLineToSegment()
    {

      LineSegment temp = new LineSegment(_start.Point, _end.Point);
      ToLineSegment = temp;
    }
    public void SetUpperLowerEndPoints()
    {
      if (_start.Point.Y > _end.Point.Y)
      {
        _upper = _start;
        _lower = _end;
      }
      else
      {
        _upper = _end;
        _lower = _start;
      }
    }
    public static DcelHalfEdge Twin(DcelHalfEdge e)
    {
      return e._twin;
    }

    public static DcelHalfEdge Next(DcelHalfEdge e)
    {
      return e._next;
    }

    public static DcelHalfEdge Prev(DcelHalfEdge e)
    {
      return e._prev;
    }

    public static DcelVertex Origin(DcelHalfEdge e)
    {
      return e._start;
    }

    public void SetStart(DcelVertex vertex)
    {
      _start = vertex;
    }
    public void SetEnd(DcelVertex vertex)
    {
      _end = vertex;
    }
    public void SetTwin(DcelHalfEdge e)
    {
      _twin = e;
    }

    public void SetNext(DcelHalfEdge e)
    {
      _next = e;
    }

    public DcelHalfEdge GetNext()
    {
      return _next;
    }

    public DcelHalfEdge GetPrev()
    {
      return _prev;
    }
    public void SetFace(DcelFace face)
    {
      _incidentFace = face;
    }

    public void SetPrev(DcelHalfEdge e)
    {
      _prev = e;
    }

    public void SetName(string name)
    {
      _name = name;
    }


    public string GetName()
    {
      return _name;
    }

    public int IsPossibleTwin(DcelHalfEdge e)
    {
      if (e._start.Name.Equals(_start.Name) && e._end.Name.Equals(_end.Name))
        return 2;
      if (e._start.Name.Equals(_end.Name) && e._end.Name.Equals(_start.Name))
        return 1;
      return 0;
    }

    public bool IsOriented()
    {
      return _correctOrientation;
    }

    public void LockOrientation()
    {
      _correctOrientation = true;
    }

    public void UnlockOrientation()
    {
      _correctOrientation = false;
    }

    public void SwapEndPoints()
    {
      DcelVertex aux = _start;
      _start = _end;
      _end = aux;
      _correctOrientation = true;
    }


    public DcelVertex GetStart()
    {
      return _start;
    }

    public DcelVertex GetEnd()
    {
      return _end;
    }

    public DcelHalfEdge GetTwin()
    {
      return _twin;
    }

    public DcelFace GetFace()
    {
      return _incidentFace;
    }


    public DcelVertex GetUpper()
    {
      return _upper;
    }

    public DcelVertex GetLower()
    {
      return _lower;
    }
  }
}
