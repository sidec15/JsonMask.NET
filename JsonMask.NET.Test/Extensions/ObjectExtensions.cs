using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonMask.NET.Test.Extensions
{
  public static class ObjectExtensions
  {

    public static string ToJsonResponseString<T>(this T obj)
    {
      if (obj == null) return "";

      var options = new JsonSerializerOptions();

      options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
      options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      options.ReferenceHandler = ReferenceHandler.IgnoreCycles;

      //options.Converters.Add(new DateTimeConverter());
      options.Converters.Add(new JsonStringEnumConverter());
      //options.Converters.Add(new TimeZoneInfoConverter());
      //options.Converters.Add(new DateOnlyJsonConverter());
      //options.Converters.Add(new DecimalConverter());
      string jsonString = JsonSerializer.Serialize(obj, options);

      return jsonString;
    }

    public static void NullifyVirtualProperties<T>(this T obj)
    {

      var properties = obj.GetType().GetProperties();
      foreach (var property in properties)
      {
        if (property.GetGetMethod().IsVirtual)
        {
          if (property.GetSetMethod() != null) // check if set method exists and is public
          {
            property.SetValue(obj, null);
          }
        }
      }

    }

    public static T DeepCopy<T>(this T obj)
    {
      //var serialized = JsonConvert.SerializeObject(obj);
      //T deserialized = JsonConvert.DeserializeObject<T>(serialized);
      JsonSerializerOptions options = new()
      {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
      };
      options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      var serialized = JsonSerializer.Serialize(obj, options);
      var deserialized = JsonSerializer.Deserialize<T>(serialized);
      return deserialized;
    }


  }
}