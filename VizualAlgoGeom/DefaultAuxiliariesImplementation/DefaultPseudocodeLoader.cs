using System.Collections.Generic;
using System.IO;
using InterfaceOfAlgorithmAdaptersWithVisualizer;

namespace DefaultAuxiliariesImplementation
{
  public class DefaultPseudocodeLoader
  {
    public List<IPseudocodeLine> LoadFrom(string fileName)
    {
      var pseudocodeLines = new List<IPseudocodeLine>();
      try
      {
        var fi = new FileInfo(fileName);
        TextReader tr = fi.OpenText();
        string line;
        while ((line = tr.ReadLine()) != null)
        {
          string text = line.TrimStart('\t');
          if (text == string.Empty)
          {
            text = " ";
          }
          pseudocodeLines.Add(new DefaultPseudocodeLine(TabCount(line), text));
        }
        tr.Close();
      }
      catch
      {
      }
      return pseudocodeLines;
    }

    int TabCount(string str)
    {
      var j = 0;
      while (j < str.Length && str[j] == '\t')
      {
        ++j;
      }
      return j;
    }
  }
}