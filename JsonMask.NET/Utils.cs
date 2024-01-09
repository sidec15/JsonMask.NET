namespace JsonMask.NET
{
  public class Utils
  {

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
        if (Has(obj, keyValuePair.Key))
        {
          return false;
        }
      }

      return true;
    }

    public static bool IsArray(object obj)
    {
      return obj is Array;
    }

    public static bool IsObject(dynamic obj)
    {
      if (obj == null) return false;

      Type objType = obj.GetType();

      return objType.IsSubclassOf(typeof(Delegate)) || objType.IsClass;
    }

    public static bool Has(dynamic obj, string key)
    {
      if (obj == null) return false;

      Type objType = obj.GetType();
      return objType.GetProperty(key) != null;
    }

  }
}
