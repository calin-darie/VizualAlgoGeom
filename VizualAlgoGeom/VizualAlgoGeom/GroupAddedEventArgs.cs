namespace VizualAlgoGeom
{
  public class GroupAddedEventArgs
  {
    public GroupAddedEventArgs(bool isDefault)
    {
      IsDefault = isDefault;
    }

    public bool IsDefault { get; set; }
  }
}