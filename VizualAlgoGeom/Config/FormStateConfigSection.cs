using System.Configuration;
using System.Windows.Forms;

namespace VizualAlgoGeom.Config
{
  /// <summary>
  ///   Maintain size and position information for a windows form
  ///   between application executions
  /// </summary>
  public class FormStateConfigSection : ConfigurationSection
  {
    /// <summary>
    ///   Overridden so that configuration properties can be altered.
    ///   Default implementation returns true.
    /// </summary>
    /// <returns>Always returns false.</returns>
    public override bool IsReadOnly()
    {
      return false;
    }

    #region static fields' initialization

    /// <summary>
    ///   - Gets a per-user-configuration object to load the particular sections with
    ///   - Predefines the properties and initializes the property collection
    /// </summary>
    static FormStateConfigSection()
    {
      PredefineProperties();
    }

    protected static void PredefineProperties()
    {
      ///perdefine properties
      s_cfgpropWidth = new ConfigurationProperty
        (
        "width",
        typeof (int),
        DefaultWidth,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );
      s_cfgpropHeight = new ConfigurationProperty
        (
        "height",
        typeof (int),
        DefaultHeight,
        null,
        new IntegerValidator(0, int.MaxValue),
        ConfigurationPropertyOptions.None
        );
      s_cfgpropLeft = new ConfigurationProperty
        (
        "pixelsFromLeft",
        typeof (int),
        DefaultLeft,
        null,
        new IntegerValidator(int.MinValue, int.MaxValue),
        /*XXX to do - custom validator: min = -Width, max = Screen width*/
        ConfigurationPropertyOptions.None
        );
      s_cfgpropTop = new ConfigurationProperty
        (
        "pixelsFromTop",
        typeof (int),
        DefaultTop,
        null,
        new IntegerValidator(int.MinValue, int.MaxValue),
        ConfigurationPropertyOptions.None
        );

      s_cfgpropWindowState = new ConfigurationProperty
        (
        "windowState",
        typeof (FormWindowState),
        DefaultWindowState,
        new GenericEnumConverter(typeof (FormWindowState)),
        null,
        ConfigurationPropertyOptions.None
        );

      /// build and cache property collection
      s_CachedProperties = new ConfigurationPropertyCollection
      {
        s_cfgpropWidth,
        s_cfgpropHeight,
        s_cfgpropLeft,
        s_cfgpropTop,
        s_cfgpropWindowState
      };
    }

    #endregion

    #region config properties getters & setters

    protected override ConfigurationPropertyCollection Properties
    {
      get { return s_CachedProperties; }
    }

    public int Width
    {
      get { return (int) base[s_cfgpropWidth]; }

      set { base[s_cfgpropWidth] = value; }
    }

    public int Height
    {
      get { return (int) base[s_cfgpropHeight]; }

      set { base[s_cfgpropHeight] = value; }
    }

    public int Left
    {
      get { return (int) base[s_cfgpropLeft]; }

      set { base[s_cfgpropLeft] = value; }
    }

    public int Top
    {
      get { return (int) base[s_cfgpropTop]; }

      set { base[s_cfgpropTop] = value; }
    }

    public FormWindowState WindowState
    {
      get { return (FormWindowState) base[s_cfgpropWindowState]; }

      set { base[s_cfgpropWindowState] = value; }
    }

    #endregion

    #region cached config properties pattern

    protected static ConfigurationPropertyCollection s_CachedProperties;
    protected static ConfigurationProperty s_cfgpropWidth;
    protected static ConfigurationProperty s_cfgpropHeight;
    protected static ConfigurationProperty s_cfgpropLeft;
    protected static ConfigurationProperty s_cfgpropTop;
    protected static ConfigurationProperty s_cfgpropWindowState;

    #endregion

    #region default values for the properties

    internal const int DefaultWidth = 1000;
    internal const int DefaultHeight = 700;
    internal const int DefaultLeft = 0;
    internal const int DefaultTop = 50;
    internal const FormWindowState DefaultWindowState = FormWindowState.Normal;

    #endregion
  }
}