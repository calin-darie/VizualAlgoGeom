using System.Configuration;
using System.Globalization;
using System.Threading;

namespace VizualAlgoGeom.Config
{
  public class CultureConfigElement : ConfigurationElement
  {
    static CultureConfigElement()
    {
      PrepareProperties();
    }

    #region cached config properties pattern

    static void PrepareProperties()
    {
      _nameConfigProperty = new ConfigurationProperty
        (
        "name",
        typeof (string),
        Thread.CurrentThread.CurrentUICulture.Name,
        null,
        null,
        ConfigurationPropertyOptions.None
        );
      _cachedProperties = new ConfigurationPropertyCollection {_nameConfigProperty};
    }

    protected override ConfigurationPropertyCollection Properties
    {
      get { return _cachedProperties; }
    }

    protected static ConfigurationPropertyCollection _cachedProperties;
    protected static ConfigurationProperty _nameConfigProperty;

    #endregion

    #region properties

    public CultureInfo Info
    {
      get
      {
        string cultureName;
        string defaultCultureName = CultureInfo.CurrentUICulture.Name;
        cultureName = Name;
        CultureInfo[] supportedCultures =
          CultureInfo.GetCultures(CultureTypes.FrameworkCultures);
        var isSupportedCulture = false;
        foreach (CultureInfo culture in supportedCultures)
          if (culture.Name.Equals(cultureName))
          {
            isSupportedCulture = true;
          }
        if (false == isSupportedCulture)
          cultureName = defaultCultureName;
        return CultureInfo.GetCultureInfo(cultureName);
      }
    }

    public string Name
    {
      get { return (string) base[_nameConfigProperty]; }

      set { base[_nameConfigProperty] = value; }
    }

    #endregion
  }
}