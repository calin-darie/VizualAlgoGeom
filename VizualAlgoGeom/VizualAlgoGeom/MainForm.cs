using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Darwen.Windows.Forms.Controls.Docking;
using DefaultCanvasViews;
using GeometricElements;
using Infrastructure;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using InterfaceOfSnapshotsWithVisualizer;
using Snapshots;
using ToolboxGeometricElements;
using VizualAlgoGeom.AssemblyLoading;
using VizualAlgoGeom.Config;
using VizualAlgoGeom.DTO;
using VizualAlgoGeom.ThreadSafeComponentHandling;
using Color = System.Drawing.Color;
using Line = GeometricElements.Line;
using Point = GeometricElements.Point;

namespace VizualAlgoGeom
{
  public partial class MainForm : Form, IPreservableFormWithDockingChildren
  {
    public Dictionary<string, IDockingControl> PreservableDockingControls
    {
      get { return dockableControl.PreservableDockingControls; }
    }

    AlgorithmLoader _algorithmLoader;
    OpenFileDialog _openDialog;
    AlgorithmSandbox _sandbox;
    readonly ISnapshotPlayer _snapshotPlayer = new SnapshotPlayer();
    private static readonly string AutosavePath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      "VizualAlgoGeom",
      "autosave.current.xml");
    private XmlIo<List<Group>> _xmlIo = new XmlIo<List<Group>>();

    public MainForm()
    {
      InitializeComponent();
      _InitializeComponent();
      new FormWithDockingChildrenStatePreserver(this, "mainForm");
      Load += (s, e) => OnGotSynchronizationContext();
    }

    void OnGotSynchronizationContext()
    {
      dockableControl.ElementsChanged
        .Throttle(TimeSpan.FromSeconds(3))
        .ObserveOn(SynchronizationContext.Current)
        .Subscribe(_ => Autosave());
    }

    async Task Autosave()
    {
      Debug.WriteLine("autosave");
      toolStripStatusLabel.Text = Translations.MainForm_Autosaving;
      
      var success = await Save(AutosavePath);
      if (!success)
      {
        //todo: log. send bug report.
        toolStripStatusLabel.Text = Translations.MainForm_AutosaveFailed;
      }
      else
      {
        toolStripStatusLabel.Text = Translations.MainForm_AutosaveSucceeded;
      }
    }

    void _InitializeComponent()
    {
      _resources = new ComponentResourceManager(typeof(MainForm));

      _openDialog = new OpenFileDialog
      {
        InitialDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? Path.GetPathRoot(Application.ExecutablePath), "Algorithms"),
        // ReSharper disable once LocalizableElement
        Filter = "Algorithms|*AlgorithmAdapter.dll|All Files|*.*"
      };
      _resources.ApplyResources(_openDialog, "OpenDialog");

      _InitializeAlgorithmToolStrip();
      _snapshotPlayer.OnSnapshotChange += _snapshotPlayer_OnSnapshotChange;
      _snapshotPlayer.OnStatusChange += _snapshotPlayer_OnStatusChange;

      closeAlgorithmToolStripMenuItem.Click += closeAlgorithmToolStripMenuItem_Click;

      _dockingWindowListMenuItem.Initialise(dockableControl);

      Load += MainForm_Load;
    }

    void _InitializeAlgorithmToolStrip()
    {
      tsBtStart.Click += tsBtStart_Click;
      tsBtPause.Click += tsBtPause_Click;
      tsBtPreviousStep.Click += tsBtPreviousStep_Click;
      tsBtNextStep.Click += tsBtNextStep_Click;
      tsBtStop.Click += tsBtStop_Click;
      tsBtBegining.Click += tsBtBegining_Click;
      tsBtEnding.Click += tsBtEnding_Click;
      tsBtSpeedDown.Click += tsBtSpeedDown_Click;
      tsBtSpeedUp.Click += tsBtSpeedUp_Click;
    }

    void MainForm_Load(object sender, EventArgs args)
    {
      UpdatePlayerStatus();
    }

    public void _snapshotPlayer_OnSnapshotChange(object sender)
    {
      ISnapshot snapshot = _snapshotPlayer.CurrentSnapshot;

      if (snapshot != null && _algorithmLoader != null)
      {
        dockableControl.CanvasControl.Data.DrawableObjects = _algorithmLoader.GetDrawableObjects(snapshot);
        dockableControl.SetRemark(_snapshotPlayer.CurrentSnapshot.Remarks);
        dockableControl.HighlightLineNumber(_snapshotPlayer.CurrentSnapshot.PseudocodeLine);
      }
      else
      {
        dockableControl.CanvasControl.Data.DrawableObjects =
          new List<IPendingDraw>();
      }

      dockableControl.CanvasControl.InvalidateCanvas();
    }

    public void _snapshotPlayer_OnStatusChange(object sender)
    {
      UpdatePlayerStatus();
    }

    void UpdatePlayerStatus()
    {
      this.UiThreadExecute(
        () =>
        {
          toolStripStatusLabel.Text = _snapshotPlayer.StatusString;
          stPbAction.Value = _snapshotPlayer.PercentOfAlgorithmStepsCompleted;
          toolStripStepTextBox.Text = (_snapshotPlayer.Index + 1).ToString();
          snapshotTrackBar.Maximum = Math.Max(0, _snapshotPlayer.MaxIndex);
          snapshotTrackBar.Value = _snapshotPlayer.Index;

          toolStrip1.Invalidate();
          statusStrip.Invalidate();
        });
    }

    void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogResult result = _openDialog.ShowDialog();
      string fileName = _openDialog.FileName;

      if (result == DialogResult.OK &&
          (!string.IsNullOrEmpty(fileName)))
      {
        try
        {
          Group selectedGroup = dockableControl.CanvasControl.Data.CurrentGroup;

          CloseAlgorithm();
          _sandbox = new AlgorithmSandbox();
          _algorithmLoader = _sandbox.GetAlgorithmLoaderWithDependencies();
          Seed(_algorithmLoader.CanvasViewRegistry);
          _algorithmLoader.Input = ConvertToAlgorithmInputData(selectedGroup);
          var progressDialog = new AlgorithmLoadProgressDialog(_algorithmLoader, fileName);

          result = progressDialog.ShowDialog();
          if (result == DialogResult.Cancel)
          {
            ClearAlgorithmSandbox();
            return;
          }
          _snapshotPlayer.SnapshotRecord = _algorithmLoader.SnapshotRecorder.SnapshotRecord;
          toolStripStepTextBox.Visible = true;
          dockableControl.PopulateControls(_algorithmLoader.PseudocodeLines, _algorithmLoader.Explanation);
        }
        catch (Exception exception)
        {
          CloseAlgorithm();
          MessageBox.Show(
            _resources.GetString(exception.GetType().ToString()),
            _resources.GetString("file open problem"),
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        }
      }
    }

    AlgorithmInput ConvertToAlgorithmInputData(Group group)
    {
      return new AlgorithmInput(
        group.PointList.Points.Select(ConvertToGeometricElements).ToList(),
        group.LineSegmentList.Lines.Select(
          s => new LineSegment(ConvertToGeometricElements(s.FirstPoint), ConvertToGeometricElements(s.SecondPoint)))
          .ToList(),
        group.RayList.Lines.Select(
          r => new Ray(ConvertToGeometricElements(r.FirstPoint), ConvertToGeometricElements(r.SecondPoint))).ToList(),
        group.LineList.Lines.Select(l => new Line(l.A, l.B, l.C)).ToList(),
        group.ClosedPolylineList.Polylines.Select(
          cpl => new PolyLine(cpl.Points.Select(ConvertToGeometricElements).ToList(), closed: true)).ToList(),
        group.PolylineList.Polylines.Select(
          cpl => new PolyLine(cpl.Points.Select(ConvertToGeometricElements).ToList(), closed: false)).ToList(),
        group.WeightedPointList.Points.GetWeightedPoints()
          .Select(wp => new Weighted<Point>(new Point(wp.X, wp.Y), wp.Weight))
          .ToList(),
         group.DcelList);
    }

    static Point ConvertToGeometricElements(ToolboxGeometricElements.Point p)
    {
      return new Point(p.X, p.Y);
    }

    void Seed(CanvasViewRegistry registry)
    {
      var pointView = new PointCanvasView();
      var pointsView = new PointsCanvasView();
      var lineView = new LineCanvasView(pointView);
      var lineSegmentView = new LineSegmentCanvasView(pointView);
      var rayView = new RayCanvasView(pointView);
      registry.RegisterView<Circle, CircleCanvasView>(new CircleCanvasView());
      registry.RegisterView<Point, PointCanvasView>(pointView);
      registry.RegisterView<Line, LineCanvasView>(lineView);
      registry.RegisterView<LineSegment, LineSegmentCanvasView>(lineSegmentView);
      registry.RegisterView<Ray, RayCanvasView>(rayView);
      registry.RegisterView<PolyLine, PolyLineCanvasView>(new PolyLineCanvasView(pointsView));
      registry.RegisterView<IEnumerable<Point>, PointsCanvasView>(pointsView);
      registry.RegisterView<List<Point>, PointsCanvasView>(pointsView);
      registry.RegisterView<Point[], PointsCanvasView>(pointsView);
      registry.RegisterView<IEnumerable<LineSegment>, LineSegmentsCanvasView>(new LineSegmentsCanvasView());
      registry.RegisterView<List<LineSegment>, LineSegmentsCanvasView>(new LineSegmentsCanvasView());
      registry.RegisterView<LineSegment[], LineSegmentsCanvasView>(new LineSegmentsCanvasView());
      registry.RegisterView<IEnumerable<Ray>, RaysCanvasView>(new RaysCanvasView(rayView));
      registry.RegisterView<List<Ray>, RaysCanvasView>(new RaysCanvasView(rayView));
      registry.RegisterView<Ray[], RaysCanvasView>(new RaysCanvasView(rayView));
    }

    void ClearAlgorithmSandbox()
    {
      if (_sandbox != null)
      {
        _sandbox.Dispose();
        _sandbox = null;
      }
    }

    void closeAlgorithmToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CloseAlgorithm();
    }

    void CloseAlgorithm()
    {
      _algorithmLoader = null;

      ClearAlgorithmSandbox();
      _snapshotPlayer.ClearRecord();
      toolStripStepTextBox.Visible = false;
      dockableControl.RemoveAlgorithmTextFromControls();
    }

    void tsBtStart_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.Play();
    }

    void tsBtPause_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.Stop();
      UpdatePlayerStatus();
    }

    void tsBtStop_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.Pause();
      UpdatePlayerStatus();
    }

    void tsBtPreviousStep_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.Previous();
    }

    void tsBtNextStep_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.Next();
    }

    void tsBtBegining_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.JumpToStart();
    }

    void tsBtEnding_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.JumpToEnd();
    }

    void tsBtSpeedDown_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.SlowDown();
    }

    void tsBtSpeedUp_Click(object sender, EventArgs e)
    {
      _snapshotPlayer.SpeedUp();
    }

    void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var saveDialog = new SaveFileDialog();
      DialogResult result = saveDialog.ShowDialog();
      if (result != DialogResult.OK)
      {
        return;
      }

      string destinationFileName = saveDialog.FileName;
      if (string.IsNullOrEmpty(destinationFileName))
      {
        return;
      }

      Save(destinationFileName).Wait();
    }

    async Task<bool> Save(string destinationFileName)
    {
      return await _xmlIo.SaveTo(destinationFileName, dockableControl.CanvasControl.Data.Groups);
    }

    void LoadPointsAndLinesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var openDialog =
        new OpenFileDialog
        {
          // ReSharper disable once LocalizableElement
          Filter = ".xml Files|*.xml|All Files|*.*"
        };
      DialogResult result = openDialog.ShowDialog();
      if (result != DialogResult.OK)
      {
        return;
      }

      string sourceFileName = openDialog.FileName;
      if (string.IsNullOrEmpty(sourceFileName))
      {
        return;
      }

      var readGroups = _xmlIo.LoadFrom(sourceFileName);
      if (readGroups == null)
      {
        return;
      }
      MergeGroups(readGroups);
    }

    void MergeGroups(List<Group> groupsToAdd)
    {
      foreach (Group group in groupsToAdd)
      {
        Group existingGroup =
          dockableControl.CanvasControl.Data.Groups.SingleOrDefault(g => g.Name == group.Name);
        if (existingGroup != null)
        {
          MergeGroups(group, existingGroup);
        }
        else
        {
          AddGroup(group);
        }
      }

      dockableControl.CanvasControl.Canvas.Invalidate();
    }

    void AddGroup(Group group)
    {
      group.Color = Color.Black; //TODO: make serialization work for color
      dockableControl.AddGroup(group);
    }

    void MergeGroups(Group group, Group existingGroup)
    {
      MergeElementLists(group, existingGroup, g => g.PointList.Points);
      MergeElementLists(group, existingGroup, g => g.ClosedPolylineList.Polylines);
      MergeElementLists(group, existingGroup, g => g.LineList.Lines);
      MergeElementLists(group, existingGroup, g => g.LineSegmentList.Lines);
      MergeElementLists(group, existingGroup, g => g.RayList.Lines);
      MergeElementLists(group, existingGroup, g => g.WeightedPointList.Points);
    }

    void MergeElementLists<TElement>(
      Group group,
      Group existingGroup,
      Func<Group, IList<TElement>> elementListGetter)
      where TElement : GeometricElement
    {
      IList<TElement> existingList = elementListGetter(existingGroup);
      foreach (TElement element in elementListGetter(group))
      {
        if (!existingList.Contains(element))
        {
          element.Color = existingGroup.Color;
          element.Group = existingGroup;

          existingList.Add(element);
          dockableControl.OnElementAdded(element);
        }
      }
    }

    void snapshotTrackBar_ValueChanged(object sender, EventArgs e)
    {
      _snapshotPlayer.JumpTo(snapshotTrackBar.Value);
    }

    private void MainForm_Shown(object sender, EventArgs e)
    {
      dockableControl.CanvasControl.CenterOrigin();
    }

  }
  
}