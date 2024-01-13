using Newtonsoft.Json;

namespace JsonMask.NET
{
  public static class Masker
  {

    public static dynamic MaskObj(dynamic obj, string mask)
    {

      if (obj == null) return null;

      var compiledMask = Compiler.Compile(mask);
      var res = Filter.FilterObj(obj, compiledMask);
      if (res != null)
      {
        return res;
      }

      return null;

    }

    public static string Mask(string json, string mask)
    {
      if(json == null) return null;
      dynamic obj = JsonUtils.ConvertJsonToExpando(json);
      dynamic resObj = MaskObj(obj, mask);
      string response = JsonConvert.SerializeObject(resObj);
      return response;
    }

  }
}
