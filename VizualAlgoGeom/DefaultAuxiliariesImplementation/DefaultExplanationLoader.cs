using System.IO;

namespace DefaultAuxiliariesImplementation
{
  public class DefaultExplanationLoader
  {
    public string LoadFrom(string fileName)
    {
      try
      {
        var fi = new FileInfo(fileName);
        using (TextReader tr = fi.OpenText())
        {
          return tr.ReadToEnd();
        }
      }
      catch
      {
        return string.Empty;
      }
    }
  }
}