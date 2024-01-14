
namespace JsonMask.NET.Test.Unit
{
  internal class CompilerUT
  {

    [Test]
    public void CompileTest()
    {

      string text;
      dynamic expected;
      dynamic actual;

      text = "a";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{ a: { Type: 'object' } }");
      Asserts.EqualsJToken(actual, expected);

      text = "a/*/c";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    a: {
      Type: 'object',
      Properties: {
        '*': {
          Type: 'object',
          IsWildcard: true,
          Properties: {
            c: { Type: 'object' }
          }
        }
      }
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a,b(d/*/g,b),c";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
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
                g: { Type: 'object' }
              }
            }
          }
        },
        b: { Type: 'object' }
      }
    },
    c: { Type: 'object' }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a(b/c,e)";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    a: {
      Type: 'array',
      Properties: {
        b: {
          Type: 'object',
          Properties: {
            c: { Type: 'object' }
          }
        },
        e: { Type: 'object' }
      }
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a(b/c),e";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    a: {
      Type: 'array',
      Properties: {
        b: {
          Type: 'object',
          Properties: {
            c: { Type: 'object' }
          }
        }
      }
    },
    e: { Type: 'object' }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a(b/c/d),e";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    a: {
      Type: 'array',
      Properties: {
        b: {
          Type: 'object',
          Properties: {
            c: {
              Type: 'object',
              Properties: {
                d: { Type: 'object' }
              }
            }
          }
        }
      }
    },
    e: { Type: 'object' }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a\\/b\\/c";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    'a/b/c': {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a\\(b\\)c";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    'a(b)c': {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "a\\bc";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    abc: {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "\\*";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    '*': {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "*";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    '*': {
      Type: 'object',
      IsWildcard: true
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "*(a,b,\\*,\\(,\\),\\,)";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    '*': {
      Type: 'array',
      IsWildcard: true,
      Properties: {
        a: { Type: 'object' },
        b: { Type: 'object' },
        '*': { Type: 'object' },
        '(': { Type: 'object' },
        ')': { Type: 'object' },
        ',': { Type: 'object' }
      }
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "\\\\";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    '\\': {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "foo*bar";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    'foo*bar': {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "\\n";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    n: {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);

      text = "multi\nline";
      actual = Compiler.Compile(text);
      expected = JsonUtils.ConvertJsonToExpando(@"{
    'multi\nline': {
      Type: 'object'
    }
  }");
      Asserts.EqualsJToken(actual, expected);


    }

  }
}
