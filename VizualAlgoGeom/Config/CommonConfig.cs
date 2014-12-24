using System.Collections.Generic;
using System.Configuration;

namespace VizualAlgoGeom.Config
{
  public static class CommonConfig
  {
    static CommonConfig()
    {
      ///cache local user config
      LocalUserConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

      ///initialize cached sections dictionary
      CachedSections = new Dictionary<string, ConfigurationSection>();
    }

    #region cached local user configuration object

    public static void SaveConfig()
    {
      LocalUserConfiguration.Save();
    }

    internal static Configuration LocalUserConfiguration { get; set; }

    #endregion

    #region cached config sections pattern

    /// <summary>
    ///   Gets the configuration section using the specified element name.
    /// </summary>
    public static ConfigurationSection GetSection(string name)
    {
      ConfigurationSection section;
      if (false == CachedSections.TryGetValue(name, out section))
      {
        section = LocalUserConfiguration.GetSection(name);
        if (section != null)
          CachedSections.Add(name, section);
        else
          throw new ConfigurationErrorsException("The <" + name +
                                                 "> section is not defined in your .config file!");
      }
      return section;
    }

    static readonly Dictionary<string, ConfigurationSection> CachedSections;

    #endregion
  }
}