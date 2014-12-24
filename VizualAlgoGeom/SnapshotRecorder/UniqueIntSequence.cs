namespace Snapshots
{
  public class UniqueIntSequence
  {
    int _current = int.MinValue;

    public int Generate()
    {
      return ++_current;
    }
  }
}