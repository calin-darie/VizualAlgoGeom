using System;
using System.IO;

namespace AlgorithmResources
{
  public class AlgorithmResourcePaths
  {
    readonly string _dir;

    public AlgorithmResourcePaths(string pathToAlgorithmAdapterAssembly)
    {
      if (pathToAlgorithmAdapterAssembly == null) throw new ArgumentNullException("pathToAlgorithmAdapterAssembly");
      _dir = Path.Combine(
        Path.GetDirectoryName(pathToAlgorithmAdapterAssembly)
        ?? Path.GetPathRoot(pathToAlgorithmAdapterAssembly)
        , "Resources");
    }

    public string Pseudocode
    {
      get { return Path.Combine(_dir, "pseudocode.txt"); }
    }

    public string SnapshotDescriptions
    {
      get { return Path.Combine(_dir, "snapshotDescriptions.xml"); }
    }

    public string Explanation
    {
      get { return Path.Combine(_dir, "explanations.txt"); }
    }

    public string VisualStyles
    {
      get { return Path.Combine(_dir, "styles.xml"); }
    }
  }
}