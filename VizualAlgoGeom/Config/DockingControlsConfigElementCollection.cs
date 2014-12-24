using System.Configuration;

namespace VizualAlgoGeom.Config
{
  [ConfigurationCollection(
    typeof (DockingControlStateConfigElement),
    CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
  internal class DockingControlsConfigElementCollection : ConfigurationElementCollection
  {
    #region indexers

    public new DockingControlStateConfigElement this[string key]
    {
      get
      {
        var cfgElement =
          (DockingControlStateConfigElement) BaseGet(key);
        if (null == cfgElement)
        {
          cfgElement = CreateNewElement(key)
            as DockingControlStateConfigElement;
          BaseAdd(cfgElement, true);
        }
        return cfgElement;
      }
    }

    #endregion

    #region cached property collection pattern

    static readonly ConfigurationPropertyCollection s_cachedPropertyCollection;

    static DockingControlsConfigElementCollection()
    {
      s_cachedPropertyCollection = new ConfigurationPropertyCollection();
    }

    protected override ConfigurationPropertyCollection Properties
    {
      get { return s_cachedPropertyCollection; }
    }

    #endregion

    #region collection behavior override

    public override ConfigurationElementCollectionType CollectionType
    {
      get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return (element as DockingControlStateConfigElement).Id;
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return new DockingControlStateConfigElement();
    }

    protected override ConfigurationElement CreateNewElement(string key)
    {
      var newElement = new DockingControlStateConfigElement {Id = key};
      return newElement;
    }

    #endregion
  }
}