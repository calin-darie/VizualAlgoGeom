using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VizualAlgoGeom
{
  public class XmlIo<T>
    where T : class
  {
    public T LoadFrom(string fileName)
    {
      try
      {
        var serializer = new XmlSerializer(typeof(T));
        using (XmlReader xmlReader = XmlReader.Create(fileName))
        {
          var result = serializer.Deserialize(xmlReader) as T;
          return result;
        }
      }
      catch (InvalidOperationException)
      {
      }
      catch (DirectoryNotFoundException) //path is invalid (for example, it is on an unmapped drive).
      {
      }
      catch (IOException) //     An I/O error occurred while opening the file.
      {
      }
      catch (NotSupportedException) //     path is in an invalid format.
      {
      }
      catch (SecurityException)
      {
      }
      catch (UnauthorizedAccessException)
      {
      }
      return default(T);
    }

    public async Task<bool> SaveTo(string destinationFileName, T item)
    {
      var serializer = new XmlSerializer(typeof(T));
      string serializedItem;
      using (StringWriter stringWriter = new StringWriter())
      {
        serializer.Serialize(stringWriter, item);
        serializedItem = stringWriter.ToString();
      }

      return await WriteAllTextAsync(destinationFileName, serializedItem);
    }

    static async Task<bool> WriteAllTextAsync(string filePath, string serializedItem)
    {
      await Task.Run(() =>
      {
        string directoryName = Path.GetDirectoryName(filePath);
        return Directory.CreateDirectory(directoryName);
      });
      byte[] encodedText = Encoding.Unicode.GetBytes(serializedItem);
      try
      {
        using (FileStream sourceStream = File.Open(filePath, FileMode.OpenOrCreate))
        {
          await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
          return true;
        }
      }
      catch (Exception e)
      {
        //todo: log e
        return false;
      }
    }
  }
}