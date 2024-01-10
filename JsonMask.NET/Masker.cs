namespace JsonMask.NET
{
  internal class Masker
  {

    public static dynamic Mask(dynamic obj, string mask)
    {
      var compiledMask = Compiler.Compile(mask);

      var res = Filter.FilterObj(obj, compiledMask);

      if (res != null)
      {
        return res;
      }

      return null;

    }

  }
}
