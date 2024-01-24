
using NUnit.Framework;

namespace JsonMask.NET.Test.Unit
{
  internal class MaskerServiceUT
  {

    [Test]
    public void MaskTest()
    {
      IMaskerService maskerService = new MaskerService();
      var original = "{ a: 1, b: 1 }";
      var mask = "a";
      var result = maskerService.Mask(original, mask);
      Console.WriteLine(result); // result is: "{ a: 1 }"
      Asserts.EqualsJToken(result, "{ a: 1 }");
    }

  }
}
