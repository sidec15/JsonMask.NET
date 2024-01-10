using Newtonsoft.Json.Linq;
using System.Collections;

namespace JsonMask.NET
{
  internal class Utils
  {

    public const string TYPE = "Type";
    public const string IS_WILDCARD = "IsWildcard";
    public const string PROPERTIES = "Properties";

    public static T Shift<T>(IList<T> list)
    {
      if (list == null || list.Count == 0)
      {
        return default;
      }

      var firstItem = list[0];
      list.RemoveAt(0);
      return firstItem;
    }

    public static bool IsEmpty(dynamic obj)
    {
      if (obj == null)
        return true;
      if (IsArray(obj))
      {
        return obj.Length == 0;
      }
      if (obj is string)
      {
        return obj.Length == 0;
      }

      IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
      foreach (var keyValuePair in dictionary)
      {
        if (HasKey(obj, keyValuePair.Key))
        {
          return false;
        }
      }

      return true;
    }

    public static bool IsArray(object obj)
    {
      return obj is Array || obj is IList;
    }

    public static bool IsObject(dynamic obj)
    {
      if (obj == null) return false;

      Type objType = obj.GetType();

      return objType.IsSubclassOf(typeof(Delegate)) || objType.IsClass;
    }

    //public static bool Has(dynamic obj, string key)
    //{
    //  if (obj == null) return false;

    //  Type objType = obj.GetType();
    //  return objType.GetProperty(key) != null;
    //}

    public static void Push(dynamic obj, string key, dynamic value)
    {
      var propsDictionary = obj as IDictionary<string, object>;
      propsDictionary[key] = value;
    }

    public static bool HasKey(dynamic obj, string key)
    {
      var expandoDict = obj as IDictionary<string, object>;
      if (expandoDict != null)
      {
        return expandoDict.ContainsKey(key);
      }
      return false;
    }

    public static dynamic GetOrDefault(dynamic obj, string key, dynamic defaultValue = null)
    {
      if(HasKey(obj, key))
      {
        return Get(obj, key);
      }

      return defaultValue;
    }

    public static dynamic Get(dynamic obj, string key)
    {
      var expandoDict = obj as IDictionary<string, object>;
      return expandoDict[key];
    }

  }
}
