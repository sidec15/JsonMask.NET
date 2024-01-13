using System.Dynamic;

namespace JsonMask.NET
{
  internal static class Filter
  {
    public static dynamic FilterObj(dynamic obj, dynamic compiledMask)
    {

      bool isArray = Utils.IsArray(obj);

      if (isArray)
      {
        return _ArrayProperties((dynamic[])obj, compiledMask);
      }

      return _Properties(obj, compiledMask);

    }

    private static dynamic _ArrayProperties(dynamic[] arr, dynamic mask)
    {
      dynamic maskInt = new ExpandoObject();
      maskInt._ = new ExpandoObject();
      maskInt._.Type = "array";
      maskInt._.Properties = mask;

      dynamic objInt = new ExpandoObject();
      objInt._ = arr;

      dynamic obj = _Properties(objInt, maskInt);

      if (obj == null) return null;

      return obj._;
    }

    private static dynamic _Object(dynamic obj, string key, dynamic mask)
    {
      var value = Utils.GetOrDefault(obj, key);
      if (value != null)
      {
        if (Utils.IsArray(value))
        {
          return _Array(obj, key, mask);
        }

        if (mask != null)
        {
          return _Properties(value, mask);
        }
      }
      return value;
    }

    private static dynamic _Array(dynamic obj, string key, dynamic mask)
    {
      IList<dynamic> ret = new List<dynamic>();
      dynamic arr = Utils.Get(obj, key);
      if (arr != null)
      {

        if (!Utils.IsArray(arr))
        {
          return _Properties(arr, mask);
        }

        if (Utils.IsEmpty(arr))
        {
          return arr;
        }

        dynamic _object, maskedObj;
        var l = arr.Length;

        for (int i = 0; i < l; i++)
        {
          _object = arr[i];
          maskedObj = _Properties(_object, mask);
          if (maskedObj != null)
          {
            ret.Add(maskedObj);
          }
        }

        if (ret.Any())
        {
          return ret.ToArray();
        }

      }

      return null;
    }

    private static dynamic _Properties(dynamic obj, dynamic mask)
    {
      if (obj == null || mask == null) // original: if (!obj || !mask)
        return obj;

      bool isArray = Utils.IsArray(obj);
      bool isObject = Utils.IsObject(obj);
      dynamic maskedObj = null;

      if (isArray)
        maskedObj = new List<dynamic>();
      else if (isObject)
        maskedObj = new ExpandoObject();

      IDictionary<string, object> maskDict = mask as IDictionary<string, object>;

      foreach (var maskKvp in maskDict)
      {
        string key = maskKvp.Key;
        dynamic value = maskKvp.Value;

        if (!Utils.HasKey(mask, key))
        {
          continue;
        }

        dynamic ret = null;
        string type = Utils.Get(value, Utils.TYPE);
        bool isObjectType = type == "object";
        if (Utils.HasKey(value, Utils.IS_WILDCARD))
        {
          var properties = Utils.GetOrDefault(value, Utils.PROPERTIES);
          ret = _ForAll(obj, properties, isObjectType);
          IDictionary<string, object> retDict = ret as IDictionary<string, object>;
          foreach (var kvp in retDict)
          {
            //if (!Utils.HasKey(ret, kvp.Key))
            //{
            //  continue;
            //}
            Utils.Push(maskedObj, kvp.Key, kvp.Value);
          }
        }
        else
        {
          dynamic maskInt = Utils.GetOrDefault(value, Utils.PROPERTIES);
          if (isObjectType)
          {
            ret = _Object(obj, key, maskInt);
          }
          else
          {
            ret = _Array(obj, key, maskInt);
          }
          if (ret != null)
          {
            Utils.Push(maskedObj, key, ret);
          }
        }

      }

      if (isArray)
        return ((IList<dynamic>)maskedObj).ToArray();

      return maskedObj;
    }

    private static object _ForAll(dynamic obj, dynamic mask, bool isObjectType = false)
    {
      dynamic ret = new ExpandoObject();

      IDictionary<string, object> dictionary = obj as IDictionary<string, object>;

      foreach (var keyValuePair in dictionary)
      {
        string key = keyValuePair.Key;
        //object value = keyValuePair.Value;

        //if (!Utils.HasKey(obj, key))
        //{
        //  continue;
        //}

        dynamic value;
        if (isObjectType)
        {
          value = _Object(obj, key, mask);
        }
        else
        {
          value = _Array(obj, key, mask);
        }

        if (value != null)
        {
          Utils.Push(ret, key, value);
        }
      }

      return ret;
    }


  }
}
