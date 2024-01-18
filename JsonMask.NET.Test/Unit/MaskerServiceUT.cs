
namespace JsonMask.NET.Test.Unit
{
  internal class MaskerServiceUT
  {

    [Test]
    public void MaskTest()
    {
      IMaskerService maskerService = new MaskerService();
      Asserts.EqualsJToken(maskerService.Mask("{ a: 1, b: 1 }", "a"), "{ a: 1 }");
    }

  }
}
