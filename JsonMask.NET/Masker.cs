using Newtonsoft.Json;

namespace JsonMask.NET
{
  public class Masker
  {

    public static dynamic MaskObj(dynamic obj, string mask)
    {
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
      dynamic obj = JsonUtils.ConvertJsonToExpando(json);
      dynamic resObj = MaskObj(obj, mask);
      string response = JsonConvert.SerializeObject(resObj);
      return response;
    }

  }
}
