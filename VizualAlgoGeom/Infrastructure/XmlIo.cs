using System;
using System.IO;
using System.Security;
using System.Xml;
using System.Xml.Serialization;

namespace Infrastructure
{
  public class XmlIo<T>
    where T : class
  {
    public T LoadFrom(string fileName)
    {
      try
      {
        var serializer = new XmlSerializer(typeof (T));
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

    public void SaveTo(string destinationFileName, T item)
    {
      try
      {
        var serializer = new XmlSerializer(typeof(T));
        using (XmlWriter xmlWriter = XmlWriter.Create(destinationFileName))
        {
          serializer.Serialize(xmlWriter, item);
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
    }
  }
}