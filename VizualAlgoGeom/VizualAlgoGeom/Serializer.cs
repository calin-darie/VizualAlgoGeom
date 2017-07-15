using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VizualAlgoGeom
{
  public class Serializer : ISerializer
  {
    public T Deserialize<T>(string serializedObject)
    {
      return JsonConvert.DeserializeObject<T>(serializedObject, _serializerSettings);
    }

    public string Serialize(object objectToSerialize)
    {
      return JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented, _serializerSettings);
    }

    public Serializer()
    {
      _serializerSettings = new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        ContractResolver = new NonPublicPropertiesResolver()
      };
    }

    readonly JsonSerializerSettings _serializerSettings;
  }

  public class NonPublicPropertiesResolver : DefaultContractResolver
  {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      JsonProperty prop = base.CreateProperty(member, memberSerialization);
      prop.Writable = (member as PropertyInfo)?.SetMethod != null;
      return prop;
    }
  }
}