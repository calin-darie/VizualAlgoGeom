using System.Threading.Tasks;

namespace VizualAlgoGeom
{
  public interface IFileSystem
  {
    Task WriteAllTextAsync(string filePath, string text);
    Task<string> ReadAllTextAsync(string filePath);
    Task ZipDirectory(string sourceDirectory, string destinationArchive);
  }
}