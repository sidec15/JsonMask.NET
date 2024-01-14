namespace JsonMask.NET.Test.Unit
{
  internal class JsonUtilsUT
  {

    class ConvertJsonToExpandoTests : JsonUtilsUT
    {

      [Test]
      public void UnsupportedTest()
      {

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(delegate
        {
          JsonUtils.ConvertJsonToExpando("\"Hello, World!\"");
        });

        Assert.That(ex.Message, Is.EqualTo("Unsupported JSON token type."));

      }

    }

  }
}
