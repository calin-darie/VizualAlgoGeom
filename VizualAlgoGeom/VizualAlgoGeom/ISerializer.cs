namespace VizualAlgoGeom
{
  public interface ISerializer
  {
    T Deserialize<T>(string serializedObject);
    string Serialize(object objectToSerialize);
  }
}