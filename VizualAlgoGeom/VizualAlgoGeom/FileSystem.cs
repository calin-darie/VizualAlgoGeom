using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace VizualAlgoGeom
{
  public class FileSystem : IFileSystem
  {
    public async Task WriteAllTextAsync(string filePath, string text)
    {
      await EnsureDirectory(filePath);
      using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
      using (StreamWriter sw = new StreamWriter(fs))
      {
        fs.SetLength(text.Length);
        await sw.WriteAsync(text);
      }
    }

    static Task EnsureDirectory(string filePath)
    {
      return Task.Run(() =>
      {
        string directoryName = Path.GetDirectoryName(filePath);
        return Directory.CreateDirectory(directoryName);
      });
    }

    public async Task<string> ReadAllTextAsync(string filePath)
    {
      using (StreamReader stream = File.OpenText(filePath))
      {
        return await stream.ReadToEndAsync();
      }
    }
    
    public async Task ZipDirectory(string sourceDirectory, string destinationArchive)
    {
      await EnsureDirectory(destinationArchive);
      await Task.Run(() =>
          ZipFile.CreateFromDirectory(sourceDirectory, destinationArchive));
    }
  }
}