using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace JsonMask.NET
{
  internal class JsonUtils
  {

    internal static dynamic ConvertJsonToExpando(string jsonString)
    {
      var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);
      return ConvertJObjectToExpando(jsonObject);
    }

    private static dynamic ConvertJObjectToExpando(JObject jObject)
    {
      dynamic expando = new ExpandoObject();
      var expandoDict = (IDictionary<string, object>)expando;

      foreach (var pair in jObject)
      {
        if (pair.Value is JObject innerObject)
        {
          expandoDict[pair.Key] = ConvertJObjectToExpando(innerObject);
        }
        else if (pair.Value is JArray array)
        {
          expandoDict[pair.Key] = ConvertJArrayToExpandoList(array);
        }
        else
        {
          expandoDict[pair.Key] = pair.Value.ToObject<object>();
        }
      }

      return expando;
    }

    private static List<dynamic> ConvertJArrayToExpandoList(JArray array)
    {
      var expandoList = new List<dynamic>();
      foreach (var item in array)
      {
        if (item is JObject)
        {
          expandoList.Add(ConvertJObjectToExpando((JObject)item));
        }
        else
        {
          expandoList.Add(item.ToObject<object>());
        }
      }
      return expandoList;
    }
  }


}
