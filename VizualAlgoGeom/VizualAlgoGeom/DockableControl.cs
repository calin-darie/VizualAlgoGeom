using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Darwen.Windows.Forms.Controls.Docking;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using ToolboxGeometricElements;
using VizualAlgoGeom.ThreadSafeComponentHandling;

namespace VizualAlgoGeom
{
  public partial class DockableControl : DockingManagerControl
  {
    AlgorithmControl _algorithmControl;
    IDockingControl _algorithmDockingControl;
    ExplanationsControl _explanationsControl;
    IDockingControl _explanationsDockingControl;
    Factory _lastFactory;
    OptionsControl _optionsControl;
    IDockingControl _optionsDockingControl;
    PropertiesControl _propertiesControl;
    IDockingControl _propertiesDockingControl;
    RemarksControl _remarksControl;
    IDockingControl _remarksDockingControl;
    SelectionControl _selectionControl;
    IDockingControl _selectionDockingControl;
    ToolboxControl _toolboxControl;
    IDockingControl _toolboxDockingControl;

    public DockableControl()
    {
      PreservableDockingControls = new Dictionary<string, IDockingControl>();

      InitializeComponent();
      InitializeControls();
      _toolboxControl.SetCurrentGroupForFactories(CanvasControl.Data.CurrentGroup);
      InitializeToolChangeEvent();
      OptionsBinding();
      PopulateSelectionTreeview();
      BindEvents();
      GeometricElementFactoriesBindNewElementEvent();
    }

    internal Dictionary<string, IDockingControl> PreservableDockingControls { get; set; }
    public CanvasControl CanvasControl { get; set; }

    void BindEvents()
    {
      _toolboxControl.BtCenter.Click += (s, e) => CanvasControl.CenterOrigin();
      CanvasControl.PropertyChanged += _optionsControl.PropertyChanged;
      _selectionControl.TvSelection.AfterSelect += _propertiesControl.SelectionChanged;
      _selectionControl.TvSelection.AfterSelect += tvSelection_AfterSelect;
      CanvasControl.Data.CurrentGroup.NameChanged += _selectionControl.NameChanged; //default group
      _selectionControl.ElementDeletedEvent += ElementDeletedEventHandler;
      _selectionControl.GroupAddedEvent += GroupAddedEvent;
    }

    void GroupAddedEvent(object sender, GroupAddedEventArgs e)
    {
      if (e.IsDefault)
      {
        CanvasControl.Data.AddDefaultGroup();
      }
      else
      {
        int groupIndex = ++CanvasControl.Data.GroupCurrentIndex;
        string groupName = "NewGroup" + groupIndex;
        Group newGroup = AddGroup(groupName);
        CanvasControl.Data.CurrentGroup = newGroup;
      }
      _toolboxControl.SetCurrentGroupForFactories(CanvasControl.Data.CurrentGroup);
      PopulateSelectionTreeview();
    }

    internal Group AddGroup(string groupName)
    {
      var newGroup = new Group(groupName, Color.Black);
      return AddGroup(newGroup);
    }

    internal Group AddGroup(Group newGroup)
    {
      newGroup.NameChanged += _selectionControl.NameChanged;
      CanvasControl.Data.Groups.Add(newGroup);
      return newGroup;
    }

    void tvSelection_AfterSelect(object sender, TreeViewEventArgs e)
    {
      object selectedObject = e.Node.Tag;
      var group = selectedObject as Group;
      if (group == null) return;

      CanvasControl.Data.CurrentGroup = group;
      _toolboxControl.SetCurrentGroupForFactories(group);
    }

    void ElementDeletedEventHandler(object sender, EventArgs e)
    {
      CanvasControl.Canvas.Invalidate();
    }

    void OptionsBinding()
    {
      _optionsControl.OptionsGrid.SelectedObject = CanvasControl;
      _optionsControl.OptionsGrid.BrowsableAttributes =
        new AttributeCollection(new ShowAttribute(true));
    }

    void GeometricElementFactoriesBindNewElementEvent()
    {
      _toolboxControl.PointFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.LineSegmentFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.RayFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.LineFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.PolylineFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.ClosedPolylineFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.WeightedPointFactory.NewElementAdded += NewElementAdded;
      _toolboxControl.PolylineFactory.ElementDeleted += ElementDeleted;
      _toolboxControl.ClosedPolylineFactory.ElementDeleted += ElementDeleted;
    }

    void ElementDeleted(object sender, EventArgs e)
    {
      PopulateSelectionTreeview();
    }

    void NewElementAdded(object sender, NewGeometricElmentEventArgs e)
    {
      GeometricElement geometricElement = e.GeometricElement;
      OnElementAdded(geometricElement);
    }

    internal void OnElementAdded(GeometricElement geometricElement)
    {
      geometricElement.NameChanged += _selectionControl.NameChanged;
      geometricElement.PropertyChanged += CanvasControl.PropertyChangedAction;
      PopulateSelectionTreeview();
    }

    void InitializeToolChangeEvent()
    {
      _toolboxControl.ToolChanged += ToolChangedEventListener;
      _lastFactory = null;
    }

    void InitializeControls()
    {
      _algorithmControl = new AlgorithmControl();
      _explanationsControl = new ExplanationsControl();
      _optionsControl = new OptionsControl();
      _propertiesControl = new PropertiesControl();
      _remarksControl = new RemarksControl();
      _selectionControl = new SelectionControl(CanvasControl.Data);
      _toolboxControl = new ToolboxControl();

      IDockingPanel rightPanel = Panels[DockingType.Right].InsertPanel(0);
      IDockingPanel leftPanel = Panels[DockingType.Left].InsertPanel(0);
      IDockingPanel topPanel = Panels[DockingType.Top].InsertPanel(0);
      IDockingPanel bottomPanel = Panels[DockingType.Bottom].InsertPanel(0);

      rightPanel.Dimension = _selectionControl.Width;
      topPanel.Dimension = 2*_toolboxControl.Height;

      _selectionDockingControl = rightPanel.DockedControls.Add("Selection", _selectionControl);
      _algorithmDockingControl = bottomPanel.DockedControls.Add("Algorithm", _algorithmControl);

      _propertiesDockingControl = leftPanel.DockedControls.Add("Properties", _propertiesControl);
      _optionsDockingControl = leftPanel.DockedControls.Add("Options", _optionsControl);

      _explanationsDockingControl = bottomPanel.DockedControls.Add("Explanations", _explanationsControl);

      _toolboxDockingControl = topPanel.DockedControls.Add("Toolbox", _toolboxControl);
      _remarksDockingControl = bottomPanel.DockedControls.Add("Remarks", _remarksControl);

      leftPanel.LayoutControls();
      rightPanel.LayoutControls();
      topPanel.LayoutControls();
      bottomPanel.LayoutControls();

      MakeControlsPreservable();

      ApplyResources();
    }

    void PopulateSelectionTreeview()
    {
      _selectionControl.PopulateSelectionTreeview();
    }

    void ApplyResources()
    {
      var resources = new ComponentResourceManager(typeof (DockableControl));

      resources.ApplyResources(_algorithmDockingControl, "algorithmControl");
      resources.ApplyResources(_optionsDockingControl, "optionsControl");
      resources.ApplyResources(_remarksDockingControl, "remarksControl");
      resources.ApplyResources(_explanationsDockingControl, "explanationsControl");
      resources.ApplyResources(_propertiesDockingControl, "propertiesControl");
      resources.ApplyResources(_selectionDockingControl, "selectionControl");
      resources.ApplyResources(_toolboxDockingControl, "toolboxControl");
    }

    void MakeControlsPreservable()
    {
      PreservableDockingControls.Add("algorithm", _algorithmDockingControl);
      PreservableDockingControls.Add("options", _optionsDockingControl);
      PreservableDockingControls.Add("remarks", _remarksDockingControl);
      PreservableDockingControls.Add("explanations", _explanationsDockingControl);
      PreservableDockingControls.Add("properties", _propertiesDockingControl);
      PreservableDockingControls.Add("selection", _selectionDockingControl);
      PreservableDockingControls.Add("toolbox", _toolboxDockingControl);
    }

    void ToolChangedEventListener(object sender, ToolChangedEventArgs e)
    {
      if (null != _lastFactory)
      {
        // remove all previous event handlers
        CanvasControl.MouseAdapter.MouseLeftClick -= _lastFactory.canvas_MouseClick;
        CanvasControl.MouseAdapter.MouseMiddleClick -= _lastFactory.canvas_MouseClick;
        CanvasControl.MouseAdapter.MouseMove -= _lastFactory.canvas_MouseMove;
        CanvasControl.MouseAdapter.MouseLeftDoubleClick -= _lastFactory.canvas_MouseDoubleClick;
          CanvasControl.KeyboardAdapter.KeyEnter -= _lastFactory.canvas_EnterPressed;
      }
      // Canvas event will be listened by the element factory that was send through event args 
      CanvasControl.MouseAdapter.MouseLeftClick += e._elementFactory.canvas_MouseClick; 
      CanvasControl.MouseAdapter.MouseMiddleClick += e._elementFactory.canvas_MouseClick;
      CanvasControl.MouseAdapter.MouseMove += e._elementFactory.canvas_MouseMove;
      CanvasControl.MouseAdapter.MouseLeftDoubleClick += e._elementFactory.canvas_MouseDoubleClick;
        CanvasControl.KeyboardAdapter.KeyEnter += e._elementFactory.canvas_EnterPressed;
      // Last factory will be the current listener factory
      _lastFactory = e._elementFactory;
      CanvasControl.LastCursor = new Cursor(new MemoryStream(CursorsResource.Pen));
      CanvasControl.Canvas.Cursor = CanvasControl.LastCursor;
    }

    internal void SetRemark(string p)
    {
      _remarksControl.TextBoxRemark.Text = p;
      _remarksControl.UiThreadInvalidate();
    }

    internal void PopulateControls(List<IPseudocodeLine> pseudocodeLines, string explanation)
    {
      _algorithmControl.ArrangeInTreeView(pseudocodeLines);
      _explanationsControl.SetExplanation(explanation);
    }

    internal void HighlightLineNumber(int lineNumber)
    {
      _algorithmControl.SelectLine(lineNumber);
    }

    internal void RemoveAlgorithmTextFromControls()
    {
      _algorithmControl.EmptyTreeView();
      _explanationsControl.SetExplanation("");
      _remarksControl.TextBoxRemark.Text = "";
    }
  }
}