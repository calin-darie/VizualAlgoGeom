using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ToolboxGeometricElements;

namespace VizualAlgoGeom
{
  public delegate void ElementDeletedEventHandler(object sender, EventArgs e);

  public delegate void GroupAddedEventHandler(object sender, GroupAddedEventArgs e);

  public partial class SelectionControl : UserControl
  {
    readonly Data _data;

    public SelectionControl(Data data)
    {
      InitializeComponent();
      _data = data;
    }

    public TreeView TvSelection { get; set; }
    public event ElementDeletedEventHandler ElementDeletedEvent;
    public event GroupAddedEventHandler GroupAddedEvent;

    public void NameChanged(object sender, NameChangedEventArgs e)
    {
      TvSelection.SelectedNode.Text = e.NewName;
    }

    public void PopulateSelectionTreeview()
    {
      TvSelection.Nodes.Clear();
      var newRootNode = new TreeNode("Groups");
      TvSelection.Nodes.Add(newRootNode);
      newRootNode.Nodes.AddRange(_data.Groups.Select(group =>
        new TreeNode(group.Name, GetGroupsNodeList(group)) {Tag = group})
        .ToArray());
      TvSelection.ExpandAll();
    }

    TreeNode[] GetGroupsNodeList(Group group)
    {
      TreeNode[] pointsNodeList = GetPointsNodeList(group.PointList.Points);
      var pointsTreeNode = new TreeNode("Point list", pointsNodeList);
      TreeNode[] weightedPointsNodeList = GetPointsNodeList(group.WeightedPointList.Points);
      var weightedPointsTreeNode = new TreeNode("Weighted point list", weightedPointsNodeList);
      TreeNode[] segmentsNodeList = GetLineNodeList(group.LineSegmentList.Lines);
      var segmentsTreeNode = new TreeNode("Segment list", segmentsNodeList);
      TreeNode[] linesNodeList = GetLineNodeList(group.LineList.Lines);
      var linesTreeNode = new TreeNode("Line list", linesNodeList);
      TreeNode[] raysNodeList = GetLineNodeList(group.RayList.Lines);
      var raysTreeNode = new TreeNode("Ray list", raysNodeList);
      TreeNode[] polylinesNodeList = GetPolylineNodeList(group.PolylineList.Polylines);
      var polylinesTreeNode = new TreeNode("Polyline list", polylinesNodeList);
      TreeNode[] closedPolylinesNodeList = GetPolylineNodeList(group.ClosedPolylineList.Polylines);
      var closedPolylinesTreeNode = new TreeNode("Closed polyline list", closedPolylinesNodeList);

      var treeNodeList = new[]
      {
        pointsTreeNode,
        weightedPointsTreeNode,
        segmentsTreeNode,
        linesTreeNode,
        raysTreeNode,
        polylinesTreeNode,
        closedPolylinesTreeNode
      };
      return treeNodeList;
    }

    TreeNode[] GetPolylineNodeList(List<Polyline> polylineList)
    {
      var polylineNodeList = new TreeNode[polylineList.Count];
      var index = 0;
      foreach (Polyline polyline in polylineList)
      {
        TreeNode[] pointsNodeList = GetPointsNodeList(polyline.Points);
        polylineNodeList[index] = new TreeNode(polyline.Name, pointsNodeList) {Tag = polyline};
        index++;
      }
      return polylineNodeList;
    }

    TreeNode[] GetLineNodeList(List<Line> lineList)
    {
      var lineNodeList = new TreeNode[lineList.Count];
      var index = 0;
      foreach (Line line in lineList)
      {
        var pointsNodeList = new TreeNode[2];
        pointsNodeList[0] = new TreeNode(line.FirstPoint.Name) {Tag = line.FirstPoint};
        pointsNodeList[1] = new TreeNode(line.SecondPoint.Name) {Tag = line.SecondPoint};
        lineNodeList[index] = new TreeNode(line.Name, pointsNodeList) {Tag = line};
        index++;
      }
      return lineNodeList;
    }

    TreeNode[] GetPointsNodeList(List<Point> pointList)
    {
      TreeNode[] result = pointList.Select(point => new TreeNode(point.Name) {Tag = point}).ToArray();
      return result;
    }

    void _tvSelection_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        // Select the clicked node
        TvSelection.SelectedNode = TvSelection.GetNodeAt(e.X, e.Y);

        if (TvSelection.SelectedNode != null)
        {
          treeviewContextMenuStrip.Show(TvSelection, e.Location);

          //I keep the selected node to perform the delete action
          deleteToolStripMenuItem.Tag = TvSelection.SelectedNode;
          SetVisibleDeleteOption(TvSelection.SelectedNode);
        }
      }
    }

    void SetVisibleDeleteOption(TreeNode treeNode)
    {
      deleteGroupToolStripMenuItem.Visible = false;
      deleteToolStripMenuItem.Enabled = true;
      if (treeNode.Level >= 0 && treeNode.Level <= 2)
      {
        deleteToolStripMenuItem.Text = "Delete all";
        if (treeNode.Level == 1)
        {
          deleteGroupToolStripMenuItem.Visible = true;
          deleteGroupToolStripMenuItem.Tag = treeNode;
        }
      }
      else
      {
        deleteToolStripMenuItem.Text = "Delete this";
        if (treeNode.Nodes.Count == 0 && (treeNode.Parent.Tag != null))
          deleteToolStripMenuItem.Enabled = false;
      }
    }

    void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var deleteItem = (ToolStripMenuItem) sender;
      var node = (TreeNode) deleteItem.Tag;
      if (deleteItem.Text.CompareTo("Delete this") == 0) 
      {
        //The node is a geometric element and we have to remove it from its certain list

        var element = (GeometricElement) node.Tag;
        Group elementGroup = element.Group;
        switch (node.Parent.Text)
        {
          case "Point list":
            elementGroup.PointList.Points.Remove((Point) element);
            break;
          case "Weighted point list":
            elementGroup.WeightedPointList.Points.Remove((WeightedPoint) element);
            break;
          case "Segment list":
            elementGroup.LineSegmentList.Lines.Remove((Line) element);
            break;
          case "Line list":
            elementGroup.LineList.Lines.Remove((Line) element);
            break;
          case "Ray list":
            elementGroup.RayList.Lines.Remove((Line) element);
            break;
          case "Polyline list":
            elementGroup.PolylineList.Polylines.Remove((Polyline) element);
            break;
          case "Closed polyline list":
            elementGroup.ClosedPolylineList.Polylines.Remove((Polyline) element);
            break;
        }
      }
      else
      {
        //Delete all elements from all groups and create a default group
        if (node.Parent == null)
        {
          _data.Groups.Clear();
          FireNewGroupAdded(true);
        }
        //The node is a group. Delete all elements from it
        else if (node.Parent.Parent == null)
        {
          var group = (Group) node.Tag;
          group.LineList.Lines.Clear();
          group.LineSegmentList.Lines.Clear();
          group.PointList.Points.Clear();
          group.PolylineList.Polylines.Clear();
          group.RayList.Lines.Clear();
          group.WeightedPointList.Points.Clear();
          group.ResetAllIndex();
        }
        else //The node is a list. We need to check which one and delete all elements from it
        {
          var group = (Group) node.Parent.Tag;
          switch (node.Text)
          {
            case "Point list":
              group.PointList.Points.Clear();
              group.PointCurrentIndex = 0;
              break;
            case "Weighted point list":
              group.WeightedPointList.Points.Clear();
              group.WeightedPointCurrentIndex = 0;
              break;
            case "Segment list":
              group.LineSegmentList.Lines.Clear();
              group.LineSegmentCurrentIndex = 0;
              break;
            case "Line list":
              group.LineList.Lines.Clear();
              group.LineCurrentIndex = 0;
              break;
            case "Ray list":
              group.RayList.Lines.Clear();
              group.RayCurrentIndex = 0;
              break;
            case "Polyline list":
              group.PolylineList.Polylines.Clear();
              group.PolylineCurrentIndex = 0;
              break;
            case "Closed polyline list":
              group.ClosedPolylineList.Polylines.Clear();
              group.ClosedPolylineCurrentIndex = 0;
              break;
          }
        }
      }
      PopulateSelectionTreeview();
      TvSelection.SelectedNode = TvSelection.TopNode;
      FireDeletedElement(node);
    }

    void FireNewGroupAdded(bool isDefault)
    {
      if (GroupAddedEvent != null)
        GroupAddedEvent(this, new GroupAddedEventArgs(isDefault));
    }

    void FireDeletedElement(object sender)
    {
      if (ElementDeletedEvent != null)
        ElementDeletedEvent(sender, new EventArgs());
    }

    void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var deleteItem = (ToolStripMenuItem) sender;
      var node = (TreeNode) deleteItem.Tag;
      var group = (Group) node.Tag;
      _data.Groups.Remove(group);
      if (_data.Groups.Count == 0)
        _data.AddDefaultGroup();
      PopulateSelectionTreeview();
      FireDeletedElement(node);
    }

    void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FireNewGroupAdded(false);
    }
  }
}