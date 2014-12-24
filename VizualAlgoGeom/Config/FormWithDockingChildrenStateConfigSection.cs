using System.Configuration;

namespace VizualAlgoGeom.Config
{
  public class FormWithDockingChildrenStateConfigSection : FormStateConfigSection
  {
    static readonly ConfigurationProperty s_cfgpropDockingControlsCollection;

    static FormWithDockingChildrenStateConfigSection()
    {
      PredefineProperties();
      s_cfgpropDockingControlsCollection = new ConfigurationProperty
        (
        "dockingControls",
        typeof (DockingControlsConfigElementCollection),
        null,
        ConfigurationPropertyOptions.IsRequired
        );
      s_CachedProperties.Add(s_cfgpropDockingControlsCollection);
    }

    internal DockingControlsConfigElementCollection DockingControlConfigElementCollection
    {
      get { return (DockingControlsConfigElementCollection) base[s_cfgpropDockingControlsCollection]; }
    }
  }
}