using System;
using System.IO;
using System.Threading.Tasks;

namespace VizualAlgoGeom
{
  public class FileSystem : IFileSystem
  {
    public async Task WriteAllTextAsync(string filePath, string text)
    {
      await Task.Run(() =>
      {
        string directoryName = Path.GetDirectoryName(filePath);
        return Directory.CreateDirectory(directoryName);
      });
      using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
      using (StreamWriter sw = new StreamWriter(fs))
      {
        fs.SetLength(text.Length);
        await sw.WriteAsync(text);
      }
    }

    public async Task<string> ReadAllTextAsync(string filePath)
    {
      using (StreamReader stream = File.OpenText(filePath))
      {
        return await stream.ReadToEndAsync();
      }
    }
  }
}