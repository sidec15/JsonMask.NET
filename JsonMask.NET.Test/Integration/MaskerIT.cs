using Newtonsoft.Json;

namespace JsonMask.NET.Test.Integration
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void MaskTest()
    {

      string jsonString = @"
      {
          ""Name"": ""John Doe"",
          ""Age"": 30,
          ""IsEmployee"": true
      }";

      dynamic obj = JsonUtils.ConvertJsonToExpando(jsonString);

      string mask = "Name";

      var actualObj = Masker.Mask(obj, mask);

      string actual = JsonConvert.SerializeObject(actualObj, Formatting.Indented);
      string expected = @"
      {
          ""Name"": ""John Doe"",
      }";

      Asserts.EqualsJToken(actual, expected);

    }
  }
}