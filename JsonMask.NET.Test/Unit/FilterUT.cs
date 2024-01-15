using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonMask.NET.Test.Unit
{
  internal class FilterUT
  {

    [Test]
    public void FilterTest()
    {

      string compiledMaskString = @"

{
  a: { Type: 'object' },
  b: {
    Type: 'array',
    Properties: {
      d: {
        Type: 'object',
        Properties: {
          '*': {
            Type: 'object',
            IsWildcard: true,
            Properties: {
              z: { Type: 'object' }
            }
          }
        }
      },
      b: {
        Type: 'array',
        Properties: {
          g: { Type: 'object' }
        }
      }
    }
  },
  c: { Type: 'object' },
  'd/e': { Type: 'object' },
  '*': { Type: 'object' }
}
";

      string objectString = @"
{
  a: 11,
  n: 0,
  b: [{
    d: { g: { z: 22 }, b: 34, c: { a: 32 } },
    b: [{ z: 33 }],
    k: 99
  }],
  c: 44,
  g: 99,
  'd/e': 101,
  '*': 110
}
";

      string expectedString = @"
{
  a: 11,
  b: [{
    d: {
      g: {
        z: 22
      },
      c: {}
    },
    b: [{}]
  }],
  c: 44,
  'd/e': 101,
  '*': 110
}
";

      dynamic obj = JsonUtils.ConvertJsonToExpando(objectString);
      dynamic compiledMask = JsonUtils.ConvertJsonToExpando(compiledMaskString);

      var actual = Filter.FilterObj(obj, compiledMask);
      dynamic expected = JsonUtils.ConvertJsonToExpando(expectedString);

      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void ObjUndefinedTest()
    {
      Assert.That(Filter.FilterObj(Filter.undefined, null), Is.EqualTo(Filter.undefined));
    }

  }
}
