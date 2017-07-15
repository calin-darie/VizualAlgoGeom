using System.Threading.Tasks;

namespace VizualAlgoGeom
{
  public class Persister<T>
    where T : class
  {
    readonly ISerializer _serializer;
    readonly IFileSystem _fileSystem;

    public Persister(IFileSystem fileSystem, ISerializer serializer)
    {
      _fileSystem = fileSystem;
      _serializer = serializer;
    }

    public async Task<T> LoadFrom(string fileName)
    {
      string serializedObject = await _fileSystem.ReadAllTextAsync(fileName);
      if (serializedObject == null) return default(T);
      return _serializer.Deserialize<T>(serializedObject);
    }

    public async Task SaveTo(string destinationFileName, T @object)
    {
      var serializedObject = _serializer.Serialize(@object);
      await _fileSystem.WriteAllTextAsync(destinationFileName, serializedObject);
    }
  }
}