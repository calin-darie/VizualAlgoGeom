using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeometricElements;
using OpenTK;
using ToolboxGeometricElements;
using Point = ToolboxGeometricElements.Point;

namespace VizualAlgoGeom
{
    /* Rules of creating a DCEL:
     * 1) Build from inside out
     * 2) First point will be placed with a left click
     * 3) All faces will be closed with middle mouse click in the point where you began drawing the face
     * 4) New faces will begin in an already existing point, with middle mouse click
     * 5) If you create a new face, make sure to go over at least one edge of an existing closed face (Dcel property - all incident faces have at least one common edge)
     * 6) Do not overlap an edge twice! [Example: if you make a face with edge e1, e2 and e3 and create another face with edges e3, e4 and e5, 
     * do not create another face that goes through e3]     * 
     */


  class DcelFactory : PolylineFactory
  {
    Cursor _lastCursor;
    Point pointToSnapTo;
      readonly List<Polyline> _dcelFacesAsPolyLines;
      bool _faceClosed;

      public DcelFactory()
    {
      this._lastCursor = new Cursor(new MemoryStream(CursorsResource.Pen));
        _dcelFacesAsPolyLines = new List<Polyline>();
        _faceClosed = true;
    }

    internal override void canvas_MouseMove(object sender, MouseEventArgs e)
    {
      base.canvas_MouseMove(sender, e);
      Point pointToSnapTo = GetPointToSnapTo();
      if (pointToSnapTo != null)
      {
        _movingPoint.X = pointToSnapTo.X;
        _movingPoint.Y = pointToSnapTo.Y;
      }
    }

      internal override void canvas_MouseClick(object sender, MouseEventArgs e)
      {
          pointToSnapTo = GetPointToSnapTo();
          if (pointToSnapTo == null)
          {
              if (!e.Button.ToString().Equals("Middle"))
              {
              
              var canvas = (GLControl) sender;

              if (false == _inProgress)
              {
                  HandleFirstClick(e, canvas);
              }
              if (_faceClosed == false)
              {
                  //We add the new point behind the moving point 
                  var newPoint = new Point(_movingPoint.X, _movingPoint.Y,
                      _newPolyline.Name + "_p" + _newPolyline.Points.Count(), _group, _group.Color);
                  _newPolyline.Points.Insert(_newPolyline.Points.Count - 1, newPoint);
                  FireNewElementAdded(newPoint);
                  canvas.Invalidate();

                  RemoveMovingPoint();
                  _newPolyline.Points.Add(_movingPoint);
              }
          }
      }
          else
          {
              HandleClick(e, sender as GLControl);
          }
      
  }
    Point GetPointToSnapTo()
    {
      foreach (var polyLine in _dcelFacesAsPolyLines)
      foreach (var vertex in polyLine.Points)
      {
        //Point mouse_coord = e.Location.ToWorldCoordinates();
        if (vertex == _movingPoint)
        {
          continue;
        }
        if ( GeometricElements.Point.DistanceBetween(_movingPoint, vertex) < 1.0) // TODO zoom
        {
          return vertex;
        }
      }
      return null;
    }
    internal override void canvas_EnterPressed(object sender, KeyEventArgs  e)
    {
    }

    internal override void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        CloseDCEL();
    }

    private Dcel CreateDcel(List<Polyline> polylineList)
    {
       //var Faces;
       //var Point;
        var vertexList = new List<DcelVertex>();
        var faceList = new List<DcelFace>();

        var faceCount = 1;
        foreach (var polyline in polylineList)
        {
            //In adapter use input.DcelList..

            //Take each point and the next one, and make an Edge
            var face = new DcelFace(faceCount);
            bool setOuterComp = false;
            faceCount++;
            var lastIndex = polyline.Points.Count - 2;
            List<DcelHalfEdge> halfEdges = new List<DcelHalfEdge>();
            for (int i=0;i<= lastIndex;i++)
            {
                //i and i+1
                var dcelVertex = new DcelVertex(polyline.Points.ElementAt(i), polyline.Points.ElementAt(i).Name);
                int vertexIndexA, vertexIndexB;
                var nextIndex = i == lastIndex? 0 : i+1;
                var nextVertex = new DcelVertex(polyline.Points.ElementAt(nextIndex),polyline.Points.ElementAt(nextIndex).Name);

                //Add the incident Edge pointer of the vertexes as you find them
                if (!vertexList.Exists(x => x.Name.Equals(dcelVertex.Name)))
                    vertexList.Add(dcelVertex);

                if (!vertexList.Exists(x => x.Name.Equals(nextVertex.Name)))
                    vertexList.Add(nextVertex);
                vertexIndexA = vertexList.FindIndex(x => x.Name.Equals(dcelVertex.Name));
                vertexIndexB = vertexList.FindIndex(x => x.Name.Equals(nextVertex.Name));

                var halfEdge = new DcelHalfEdge(vertexList.ElementAt(vertexIndexA), vertexList.ElementAt(vertexIndexB));
                halfEdges.Add(halfEdge);

                
                vertexList.ElementAt(vertexIndexA).AddIncidentEdge(halfEdge);
                vertexList.ElementAt(vertexIndexB).AddIncidentEdge(halfEdge);
                    
                if (setOuterComp == false)
                {
                    face.SetOuterComponent(halfEdges.ElementAt(0));
                    setOuterComp = true;
                }
                
            }
            
            lastIndex = halfEdges.Count - 1;

            for (int i = 0; i <= lastIndex; i++)
            {
                var nextIndex = i == lastIndex ? 0 : i + 1;
                var prevIndex = i == 0 ? lastIndex : i - 1;
                halfEdges.ElementAt(i).SetNext(halfEdges.ElementAt(nextIndex));
                halfEdges.ElementAt(i).SetPrev(halfEdges.ElementAt(prevIndex));
                halfEdges.ElementAt(i).SetName(face.GetName() + "_" + (i+1));
                halfEdges.ElementAt(i).SetFace(face);
            }

            faceList.Add(face);
        }

        //At this point we have a list of faces, with their perimeter edge list, but no outer face. twins are not set

        //Set the twins

        foreach (DcelFace df in faceList)
        {
            if (!df.IsOriented())
            {
                df.LockEdgesPoints();
            }

            DcelHalfEdge start = df.GetOuterComponent();
            DcelHalfEdge travelPerimeter = start;
            do
            {
                //Debug.Print(travelPerimeter.GetName());
                bool hasTwin =  DcelHalfEdge.Twin(travelPerimeter) == null;
                if(hasTwin)
                {
                    foreach (DcelFace df2 in faceList)
                    {
                        if (!df2.Equals(df))
                        {
                            DcelHalfEdge start2 = df2.GetOuterComponent();
                            DcelHalfEdge travelPerimeter2 = start2;
                            do
                            {
                                int twinState = travelPerimeter.IsPossibleTwin(travelPerimeter2);
                                  if(twinState>0)
                                  {
                                      if (twinState == 1)
                                      {
                                          
                                          travelPerimeter.SetTwin(travelPerimeter2);
                                          travelPerimeter2.SetTwin(travelPerimeter);
                                          if (!df2.IsOriented())
                                          {
                                              df2.LockEdgesPoints();
                                          }
                                      }
                                      if (twinState == 2)
                                      {
                                          if (!df2.IsOriented())
                                          {
                                              df2.SwapEdgesOrientation();
                                              
                                              df2.LockEdgesPoints();
                                          }
                                          else { Debug.Print("Locking Error. Wrong orientation for edges in face" + df2.GetName());}
                                      }
                                      break;
                                  }
                                 travelPerimeter2 = DcelHalfEdge.Next(travelPerimeter2);
                                }
                                 while (!travelPerimeter2.Equals(start2));
                        }
                    }
                }

                travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
            }
            while (!travelPerimeter.Equals(start));
        }

        //At this point, we have set most of the twin pointers. But we still do not have the outer face / edge computed. So we
        //take each point without a twin in all the faces, and add them to the outer face.
        DcelFace outerFace = new DcelFace(faceCount);
        var outerFaceList = new List<DcelHalfEdge>();
        bool hasOuterComp = false;
        int numberOfEdges = 0;
        foreach (DcelFace df in faceList)
        {
            DcelHalfEdge start = df.GetOuterComponent();
            DcelHalfEdge travelPerimeter = start;
            do
            { 
                bool hasTwin =  DcelHalfEdge.Twin(travelPerimeter) == null;
                if (hasTwin)
                {
                    if (!hasOuterComp)
                    {
                        hasOuterComp = true;
                        DcelHalfEdge newTwin = new DcelHalfEdge(travelPerimeter.GetEnd(),travelPerimeter.GetStart());
                        numberOfEdges++;
                        newTwin.SetName(outerFace.GetName()+numberOfEdges);
                        outerFaceList.Add(newTwin);
                        outerFace.SetInnerComponent(newTwin);

                        travelPerimeter.SetTwin(newTwin);
                        newTwin.SetTwin(travelPerimeter);
                    }
                    else
                    {
                        DcelHalfEdge newTwin = new DcelHalfEdge(travelPerimeter.GetEnd(), travelPerimeter.GetStart());
                        numberOfEdges++;
                        newTwin.SetName(outerFace.GetName() + numberOfEdges);
                        outerFaceList.Add(newTwin);
                        
                        travelPerimeter.SetTwin(newTwin);
                        newTwin.SetTwin(travelPerimeter);
                    }
                }
                travelPerimeter = DcelHalfEdge.Next(travelPerimeter);
            }
            while (!travelPerimeter.Equals(start));
        }
        
        //We created a list of outer edges, we now need to link them in the proper order to become the ordered set of edges which is the outer face perimeter
        int lastIndexOuter = outerFaceList.Count-1;
        for (int i = 0; i <= lastIndexOuter; i++)
        {
            DcelHalfEdge prev = outerFaceList.Find(x => x.GetEnd().Name.Equals(outerFaceList.ElementAt(i).GetStart().Name));
            DcelHalfEdge next = outerFaceList.Find(x => x.GetStart().Name.Equals(outerFaceList.ElementAt(i).GetEnd().Name));
            outerFaceList.ElementAt(i).SetNext(next);
            outerFaceList.ElementAt(i).SetPrev(prev);
            outerFaceList.ElementAt(i).SetFace(outerFace);
        }

        outerFace.SetInnerComponent(null);
        faceList.Add(outerFace);
        var newDcel = new Dcel(faceList, vertexList);

        //At this point, the dcel is complete. We have linked all edges with their twins.
        //All that is left is to compute the holes in a face. TODO holes at a later stage
        
        //Cout the full dcel:
        //newDcel.DebugPrint();
        
        Debug.Print("Succesfully created new Dcel");
        return newDcel;
    }

    protected void HandleFirstClick(MouseEventArgs e, GLControl canvas)
    {
        double x;
        double y;
        double z;

        //The first click opens a new face.
        _faceClosed = false;

        GetWorldCoordinates(e.X, canvas.Height - e.Y, 0, out x, out y, out z);

        _inProgress = true;
        FireEnableControls(false);
        NewPolyline(GetName());
        _dcelFacesAsPolyLines.Add(_newPolyline);
        _group.PolylineList.Polylines.Add(_newPolyline);
        //The moving point will be used to show the line that follows the mouse.
        //It is the last point in line strip's point list
        _newPolyline.Points.Add(new Point(x, y, _newPolyline.Name + "_p" + (_newPolyline.Points.Count() + 1), _group, _group.Color));
        _movingPoint = _newPolyline.Points.Last();
        _movingPoint.Name = _newPolyline.Name + "_p";
        FireNewElementAdded(_newPolyline);
    }

      protected void HandleClick(MouseEventArgs e, GLControl canvas)
      {
          //Will add a new point to the current face only if left mouse button is pressed.
          if (e.Button == MouseButtons.Left)
          {
              if (_faceClosed == false)
              {
                  _newPolyline.Points.Remove(_movingPoint);
                  //If the user click on the same point twice, it should not be added to the face twice, 
                  //unless it's the closing of a face, which is handled on Right Click bellow.
                  if (!_newPolyline.Points.Contains(pointToSnapTo))
                      _newPolyline.Points.Add(pointToSnapTo);

                  //TODO add rules for common edges, prevent intersections
                  _newPolyline.Points.Add(_movingPoint);
              }
          }

            else
          {
              //Middle click will close of a face
              if (e.Button == MouseButtons.Middle)
              {
                  
                  if (_faceClosed == false)
                  {
                      if (_newPolyline.Points.Count < 4)
                      {
                          Debug.Print("Cannot create face without a minimum of three points!");
                          /* TODO add a label on wrong click
                          Label lb = new Label();
                          lb.Location = new System.Drawing.Point(e.X,e.Y);
                          lb.Text = "You need atleast three points to create a face!";
                          canvas. .Add(lb);*/
                      }
                      else
                      {
                          if (_newPolyline.Points.ElementAt(0).Equals(pointToSnapTo))
                          {

                              _faceClosed = true;
                              _newPolyline.Points.Remove(_movingPoint);

                              _newPolyline.Points.Add(pointToSnapTo);
                              // Right now, all faces will close in the starting point. TODO a better job

                              NewPolyline(GetName());
                              _dcelFacesAsPolyLines.Add(_newPolyline);
                              _group.PolylineList.Polylines.Add(_newPolyline);
                              //The moving point will be used to show the line that follows the mouse.
                              //It is the last point in line strip's point list

                              _movingPoint.Name = _newPolyline.Name + "_p";
                              _newPolyline.Points.Add(_movingPoint);
                              FireNewElementAdded(_newPolyline);
                          }
                      }
                  }
                  else
                  {                     
                      _faceClosed = false;
                      _newPolyline.Points.Remove(_movingPoint);
                      _newPolyline.Points.Add(pointToSnapTo);

                      _newPolyline.Points.Add(_movingPoint);
                  }
              }

          }
         
    }

      protected void CloseDCEL()
      {
          if (_faceClosed && _dcelFacesAsPolyLines.Count > 0)
          {
              RemoveMovingPoint();
              if (_newPolyline.Points.Count == 0)
                  _dcelFacesAsPolyLines.Remove(_newPolyline);
              //The polygon is complete
              _inProgress = false;

              //The controls (toolbox) are enabled
              FireEnableControls(true);
              _group.DcelList.Add(CreateDcel(_dcelFacesAsPolyLines));
              _dcelFacesAsPolyLines.Clear();
              //TODO add holes creation
          }
      }

      protected void CleanPoints()
      {
          Polyline pl, temp = null;
          int i = 0;
          while (i < _dcelFacesAsPolyLines.Count)
          {
              if (temp == null)
              {
                  pl = _dcelFacesAsPolyLines.ElementAt(i);
                  if (pl.Points.Count < 3)
                      temp = pl;
                  else i++;
              }
              else
              {
                  _dcelFacesAsPolyLines.Remove(temp);
                  temp = null;
              }
          }

          if (temp != null)
          {
              _dcelFacesAsPolyLines.Remove(temp);
              temp = null;
          }

      }

  }
}
