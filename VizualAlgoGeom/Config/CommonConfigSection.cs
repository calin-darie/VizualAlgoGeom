using System.Configuration;

namespace VizualAlgoGeom.Config
{
  public class CommonConfigSection : ConfigurationSection
  {
    static CommonConfigSection()
    {
      PrepareProperties();
    }

    #region cached config properties pattern

    static void PrepareProperties()
    {
      _cultureConfigProperty = new ConfigurationProperty(
        "culture",
        typeof (CultureConfigElement),
        null,
        ConfigurationPropertyOptions.None
        );
      _cachedProperties = new ConfigurationPropertyCollection {_cultureConfigProperty};
    }

    protected override ConfigurationPropertyCollection Properties
    {
      get { return _cachedProperties; }
    }

    protected static ConfigurationPropertyCollection _cachedProperties;
    protected static ConfigurationProperty _cultureConfigProperty;

    #endregion

    #region properties getters & setters

    public CultureConfigElement Culture
    {
      get { return (CultureConfigElement) base[_cultureConfigProperty]; }
    }

    public static CommonConfigSection DefaultSection
    {
      get { return CommonConfig.GetSection("common") as CommonConfigSection; }
    }

    #endregion
  }
}