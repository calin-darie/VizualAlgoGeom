using System.Configuration;
using Darwen.Windows.Forms.Controls.Docking;

namespace VizualAlgoGeom.Config
{
  internal class DockingControlStateConfigElement : ConfigurationElement
  {
    #region static fields' initialization

    /// <summary>
    ///   Predefines the properties for this config element
    ///   and initializes the property collection
    /// </summary>
    static DockingControlStateConfigElement()
    {
      ///perdefine properties

      IdConfigPropety = new ConfigurationProperty
        (
        "id",
        typeof (string),
        null,
        ConfigurationPropertyOptions.IsKey |
        ConfigurationPropertyOptions.IsRequired
        );

      WidthIfFloatingConfigProperty = new ConfigurationProperty
        (
        "widthIfFloating",
        typeof (int),
        DefaultDim,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );
      HeightIfFloatingConfigProperty = new ConfigurationProperty
        (
        "heightIfFloating",
        typeof (int),
        DefaultDim,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );
      LeftIfFloatingConfigProperty = new ConfigurationProperty
        (
        "pixelsFromLeftIfFloating",
        typeof (int),
        DefaultDim,
        null,
        new IntegerValidator(int.MinValue, int.MaxValue),
        /*XXX to do - custom validator: min = -Width, max = Screen width*/
        ConfigurationPropertyOptions.None
        );
      TopIfFloatingConfigProperty = new ConfigurationProperty
        (
        "pixelsFromTopIfFloating",
        typeof (int),
        DefaultDim,
        null,
        new IntegerValidator(int.MinValue, int.MaxValue),
        ConfigurationPropertyOptions.None
        );

      DockingTypeConfigProperty = new ConfigurationProperty
        (
        "dockingType",
        typeof (DockingType),
        DefaultDockingType,
        new GenericEnumConverter(typeof (DockingType)),
        null,
        ConfigurationPropertyOptions.IsRequired
        );

      DockDimensionConfigProperty = new ConfigurationProperty
        (
        "dockDimension",
        typeof (int),
        DefaultDim,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );

      DockIndexConfigProperty = new ConfigurationProperty
        (
        "dockIndex",
        typeof (int),
        DefaultDockIndex,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );

      PanelIndexConfigProperty = new ConfigurationProperty
        (
        "panelIndex",
        typeof (int),
        DefaultPanelIndex,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );

      AutoHideConfigProperty = new ConfigurationProperty
        (
        "autoHide",
        typeof (bool),
        DefaultAutohide,
        ConfigurationPropertyOptions.None
        );

      CancelledConfigProperty = new ConfigurationProperty
        (
        "cancelled",
        typeof (bool),
        DefaultCancelled,
        ConfigurationPropertyOptions.None
        );

      /// build and cache property collection
      CachedProperties = new ConfigurationPropertyCollection
      {
        IdConfigPropety,
        WidthIfFloatingConfigProperty,
        HeightIfFloatingConfigProperty,
        LeftIfFloatingConfigProperty,
        TopIfFloatingConfigProperty,
        DockDimensionConfigProperty,
        DockIndexConfigProperty,
        PanelIndexConfigProperty,
        DockingTypeConfigProperty,
        AutoHideConfigProperty,
        CancelledConfigProperty
      };
    }

    #endregion

    #region property getters and setters

    public string Id
    {
      get { return (string) base[IdConfigPropety]; }
      set { base[IdConfigPropety] = value; }
    }

    public int? TopIfFloating
    {
      get { return (int?) base[TopIfFloatingConfigProperty]; }

      set { base[TopIfFloatingConfigProperty] = value; }
    }

    public int? LeftIfFloating
    {
      get { return (int?) base[LeftIfFloatingConfigProperty]; }

      set { base[LeftIfFloatingConfigProperty] = value; }
    }

    public int? WidthIfFloating
    {
      get { return (int?) base[WidthIfFloatingConfigProperty]; }

      set { base[WidthIfFloatingConfigProperty] = value; }
    }

    public int? HeightIfFloating
    {
      get { return (int?) base[HeightIfFloatingConfigProperty]; }

      set { base[HeightIfFloatingConfigProperty] = value; }
    }

    public int DockDimension
    {
      get { return (int) base[DockDimensionConfigProperty]; }

      set { base[DockDimensionConfigProperty] = value; }
    }

    public int DockIndex
    {
      get { return (int) base[DockIndexConfigProperty]; }

      set { base[DockIndexConfigProperty] = value; }
    }

    public int PanelIndex
    {
      get { return (int) base[PanelIndexConfigProperty]; }

      set { base[PanelIndexConfigProperty] = value; }
    }

    public bool AutoHide
    {
      get { return (bool) base[AutoHideConfigProperty]; }
      set { base[AutoHideConfigProperty] = value; }
    }

    public bool Cancelled
    {
      get { return (bool) base[CancelledConfigProperty]; }
      set { base[CancelledConfigProperty] = value; }
    }

    public DockingType DockingType
    {
      get { return (DockingType) base[DockingTypeConfigProperty]; }
      set { base[DockingTypeConfigProperty] = value; }
    }

    protected override ConfigurationPropertyCollection Properties
    {
      get { return CachedProperties; }
    }

    #endregion

    #region static fields (config properties)

    static readonly ConfigurationPropertyCollection CachedProperties;
    static readonly ConfigurationProperty IdConfigPropety;
    static readonly ConfigurationProperty WidthIfFloatingConfigProperty;
    static readonly ConfigurationProperty HeightIfFloatingConfigProperty;
    static readonly ConfigurationProperty LeftIfFloatingConfigProperty;
    static readonly ConfigurationProperty TopIfFloatingConfigProperty;
    static readonly ConfigurationProperty DockDimensionConfigProperty;
    static readonly ConfigurationProperty DockIndexConfigProperty;
    static readonly ConfigurationProperty PanelIndexConfigProperty;
    static readonly ConfigurationProperty DockingTypeConfigProperty;
    static readonly ConfigurationProperty AutoHideConfigProperty;
    static readonly ConfigurationProperty CancelledConfigProperty;
    const DockingType DefaultDockingType = DockingType.Right;
    const int DefaultDockIndex = 0;
    const int DefaultPanelIndex = 0;
    const bool DefaultCancelled = false;
    const bool DefaultAutohide = true;

    const int DefaultDim = 200;

    #endregion
  }
}