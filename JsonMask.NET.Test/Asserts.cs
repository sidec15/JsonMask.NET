using FluentAssertions.Json;
using JsonMask.NET.Test.Extensions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Data;
using System.Linq.Expressions;

namespace JsonMask.NET.Test
{
  public class Asserts
  {

    public static void EqualsJToken(JToken actual, JToken expected)
    {
      actual.Should().BeEquivalentTo(expected);

    }

    public static void EqualsJToken(object actual, object expected)
    {
      //JsonSerializerOptions options = new();
      //options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      //options.Converters.Add(new JsonStringEnumConverter());
      //string expectedJsonString = JsonSerializer.Serialize(expected, options);
      string expectedJsonString = expected.ToJsonResponseString();
      JToken expectedJToken = JToken.Parse(expectedJsonString);

      //string actualJsonString = JsonSerializer.Serialize(actual, options);
      string actualJsonString = actual.ToJsonResponseString();
      JToken actualJToken = JToken.Parse(actualJsonString);

      EqualsJToken(actualJToken, expectedJToken);
    }

    public static void EqualsJToken(string actual, object expected)
    {
      string expectedJsonString = expected.ToJsonResponseString();

      EqualsJToken(actual, expectedJsonString);
    }

    public static void EqualsJToken(string actual, string expected)
    {
      JToken expectedJToken = JToken.Parse(expected);

      JToken actualJToken = JToken.Parse(actual);

      EqualsJToken(actualJToken, expectedJToken);
    }

  }

}
