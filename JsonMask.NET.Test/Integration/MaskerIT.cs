using JsonMask.NET.Test.Extensions;
using Newtonsoft.Json;

namespace JsonMask.NET.Test.Integration
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    class Person
    {
      public string Name { get; set; }
      public int? Age { get; set; }
      public bool? HasLicense { get; set; }
      public Person[] Children { get; set; }
      public Pet Pet { get; set; }

      public override string ToString()
      {
        return this.ToJsonResponseString();
      }

    }

    class Pet
    {
      public string Name { get; set; }
      public int? Age { get; set; }
      public string Species { get; set; }
    }

    [Test]
    public void MaskSimpleTest()
    {

      Person person = new()
      {
        Name = "foo",
        Age = 40,
        HasLicense = true
      };

      string jsonString = person.ToString();

      dynamic obj = JsonUtils.ConvertJsonToExpando(jsonString);

      string mask = "name";
      var actualObj = Masker.MaskObj(obj, mask);
      var actual = JsonConvert.SerializeObject(actualObj);
      var expected = new Person() { Name = person.Name }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);


      mask = "(name)";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      expected = new Person() { Name = person.Name }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void MaskSimpleStringTest()
    {

      Person person = new()
      {
        Name = "foo",
        Age = 40,
        HasLicense = true
      };

      string jsonString = person.ToString();

      string mask = "name";
      var actual = Masker.Mask(jsonString, mask);
      //var actual = JsonConvert.SerializeObject(actualObj);
      var expected = new Person() { Name = person.Name }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);


      mask = "(name)";
      actual = Masker.Mask(jsonString, mask);
      //actual = JsonConvert.SerializeObject(actualObj);
      expected = new Person() { Name = person.Name }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void MaskSimpleArrayTest()
    {

      Person p1 = new()
      {
        Name = "foo",
        Age = 40,
        HasLicense = true
      };
      Person p2 = new()
      {
        Name = "bar",
        Age = 18,
        HasLicense = false
      };
      Person[] persons = { p1, p2 };

      string jsonString = persons.ToJsonResponseString();

      dynamic obj = JsonUtils.ConvertJsonToExpando(jsonString);

      string mask = "name,age";

      var actualObj = Masker.MaskObj(obj, mask);

      string actual = JsonConvert.SerializeObject(actualObj);
      string expected = new Person[]
      {
        new(){ Name = p1.Name, Age = p1.Age},
        new(){ Name = p2.Name, Age = p2.Age}
      }.ToJsonResponseString();

      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void MaskNestedObjectTest()
    {

      Person person = new()
      {
        Name = "foo",
        Age = 40,
        HasLicense = true,
        Pet = new()
        {
          Name = "book",
          Age = 10,
          Species = "dog"
        },
        Children = new Person[] {
          new()
          {
            Name = "bar1",
            Age = 18,
            HasLicense = false
          },
          new()
          {
            Name = "bar2",
            Age = 12,
            HasLicense = true
          }
        }
      };

      string jsonString = person.ToString();

      dynamic obj = JsonUtils.ConvertJsonToExpando(jsonString);

      string mask = "pet(name)";
      var actualObj = Masker.MaskObj(obj, mask);
      var actual = JsonConvert.SerializeObject(actualObj);
      var expected = new Person()
      {
        Pet = new()
        {
          Name = person.Pet.Name
        },
      }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

      mask = "pet/name";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      Asserts.EqualsJToken(actual, expected);

      mask = "name,age,pet(name,age),children(name,age)";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      expected = new Person()
      {
        Name = person.Name,
        Age = person.Age,
        Pet = new()
        {
          Name = person.Pet.Name,
          Age = person.Pet.Age
        },
        Children = new Person[]
        {
          new()
          {
            Name = person.Children[0].Name,
            Age = person.Children[0].Age
          },
          new()
          {
            Name = person.Children[1].Name,
            Age = person.Children[1].Age
          }
        }
      }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void WildCardTest()
    {

      Person person = new()
      {
        Name = "foo",
        Age = 40,
        HasLicense = true,
        Pet = new()
        {
          Name = "book",
          Age = 10,
          Species = "dog"
        },
        Children = new Person[] {
          new()
          {
            Name = "bar1",
            Age = 18,
            HasLicense = false,
            Pet = new()
            {
              Age = 5,
              Name = "marlin",
              Species = "Black Moor"
            }
          },
          new()
          {
            Name = "bar2",
            Age = 12,
            HasLicense = true
          }
        }
      };

      string jsonString = person.ToString();

      dynamic obj = JsonUtils.ConvertJsonToExpando(jsonString);

      string mask;
      dynamic actualObj;
      string actual;
      string expected;

      mask = "children/*";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      expected = new Person()
      {
        Children = new Person[]
        {
          person.Children[0],
          person.Children[1]
        }
      }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

      mask = "children(*)";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      Asserts.EqualsJToken(actual, expected);

      mask = "name,age,pet(name,age),children/*";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      expected = new Person()
      {
        Name = person.Name,
        Age = person.Age,
        Pet = new()
        {
          Name = person.Pet.Name,
          Age = person.Pet.Age
        },
        Children = new Person[]
        {
          person.Children[0],
          person.Children[1]
        }
      }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

      mask = "*";
      actualObj = Masker.MaskObj(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      expected = person.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void EscapeTest()
    {
      string jsonString = "{\"name\": \"1\", \",\": \"2\", \"*\": \"3\"}";

      string mask = "name";
      var actual = Masker.Mask(jsonString, mask);
      var expected = "{\"name\": \"1\"}";
      Asserts.EqualsJToken(actual, expected);

      mask = "\\,,name";
      actual = Masker.Mask(jsonString, mask);
      expected = "{\",\": \"2\", \"name\": \"1\"}";
      Asserts.EqualsJToken(actual, expected);

      mask = "*";
      actual = Masker.Mask(jsonString, mask);
      expected = "{\"name\": \"1\", \",\": \"2\", \"*\": \"3\"}";
      Asserts.EqualsJToken(actual, expected);

      mask = "\\,,\\*";
      actual = Masker.Mask(jsonString, mask);
      expected = "{\",\": \"2\", \"*\": \"3\"}";
      Asserts.EqualsJToken(actual, expected);

    }

    [Test]
    public void BatteryTests()
    {
      string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fixture", "activities.json");
      string fixture = File.ReadAllText(filePath);

      Assert.That(Masker.Mask(null, "a"), Is.Null);
      Asserts.EqualsJToken(Masker.Mask("{ b: 1 }", "a"), "{}");
      Asserts.EqualsJToken(Masker.Mask("{ a: null, b: 1 }", "a"), "{ a: null }");
      Asserts.EqualsJToken(Masker.Mask("[{ b: 1 }]", "a"), "[{}]");
      Asserts.EqualsJToken(Masker.Mask("{ a: 1 }", null), "{ a: 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ a: 1 }", ""), "{ a: 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ a: 1, b: 1 }", "a"), "{ a: 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ notEmptyStr: '' }", "notEmptyStr"), "{ notEmptyStr: '' }");
      Asserts.EqualsJToken(Masker.Mask("{ notEmptyNum: 0 }", "notEmptyNum"), "{ notEmptyNum: 0 }");
      Asserts.EqualsJToken(Masker.Mask("{ a: 1, b: 1, c: 1 }", "a,b"), "{ a: 1, b: 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ obj: { s: 1, t: 2 }, b: 1 }", "obj/s"), "{ obj: { s: 1 } }");
      Asserts.EqualsJToken(Masker.Mask("{ arr: [{ s: 1, t: 2 }, { s: 2, t: 3 }], b: 1 }", "arr/s"), "{ arr: [{ s: 1 }, { s: 2 }] }");
      Asserts.EqualsJToken(Masker.Mask("{ a: { s: { g: 1, z: 1 } }, t: 2, b: 1 }", "a/s/g,b"), "{ a: { s: { g: 1 } }, b: 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ a: 2, b: null, c: 0, d: 3 }", "*"), "{ a: 2, b: null, c: 0, d: 3 }");
      Asserts.EqualsJToken(Masker.Mask("{ a: { s: { g: 3 }, t: { g: 4 }, u: { z: 1 } }, b: 1 }", "a/*/g"), "{ a: { s: { g: 3 }, t: { g: 4 }, u: {} } }");
      Asserts.EqualsJToken(Masker.Mask("{ a: { s: { g: 3 }, t: { g: 4 }, u: { z: 1 } }, b: 3 }", "a/*"), "{ a: { s: { g: 3 }, t: { g: 4 }, u: { z: 1 } } }");
      Asserts.EqualsJToken(Masker.Mask("{ a: [{ g: 1, d: 2 }, { g: 2, d: 3 }] }", "a(g)"), "{ a: [{ g: 1 }, { g: 2 }] }");
      Asserts.EqualsJToken(Masker.Mask("{ a: [], c: {} }", "a,c"), "{ a: [], c: {} }");
      Asserts.EqualsJToken(Masker.Mask("{ b: [{ d: { g: { z: 22 }, b: 34 } }] }", "b(d/*/z)"), "{ b: [{ d: { g: { z: 22 } } }] }");
      Asserts.EqualsJToken(Masker.Mask("{ url: 1, id: '1', obj: { url: 'h', a: [{ url: 1, z: 2 }], c: 3 } }", "url,obj(url,a/url)"), "{ url: 1, obj: { url: 'h', a: [{ url: 1 }] } }");
      Asserts.EqualsJToken(Masker.Mask("{ p1: { a: 1, b: 1, c: 1 }, p2: { a: 2, b: 2, c: 2 } }", "*(a,b)"), "{ p1: { a: 1, b: 1 }, p2: { a: 2, b: 2 } }");
      Asserts.EqualsJToken(Masker.Mask(fixture, "kind"), "{ kind: 'plus#activity' }");
      Asserts.EqualsJToken(Masker.Mask(fixture, "object(objectType)"), "{ object: { objectType: 'note' } }");
      Asserts.EqualsJToken(Masker.Mask(fixture, "url,object(content,attachments/url)"), "{url:'https://plus.google.com/102817283354809142195/posts/F97fqZwJESL',object:{content:'Congratulations! You have successfully fetched an explicit public activity. The attached video is your reward. :)',attachments:[{url:'http://www.youtube.com/watch?v=dQw4w9WgXcQ'}]}}");
      Asserts.EqualsJToken(Masker.Mask("[{ i: 1, o: 2 }, { i: 2, o: 2 }]", "i"), "[{ i: 1 }, { i: 2 }]");
      Asserts.EqualsJToken(Masker.Mask("{ foo: { biz: 'bar' } }", "foo(bar)"), "{ foo: {} }");
      Asserts.EqualsJToken(Masker.Mask("{ foo: { biz: 'baz' } }", "foo(bar)"), "{ foo: {} }");
      Asserts.EqualsJToken(Masker.Mask("{ foobar: { foo: 'bar' }, foobiz: 1 }", "foobar,foobiz"), "{ foobar: { foo: 'bar' }, foobiz: 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ foo: 'bar' }", "foobar"), "{}");
      Asserts.EqualsJToken(Masker.Mask("[{ biz: 'baz' }]", "foobar"), "[{}]");
      Asserts.EqualsJToken(Masker.Mask("{ a: [0, 0] }", "a"), "{ a: [0, 0] }");
      Asserts.EqualsJToken(Masker.Mask("{ a: [1, 0, 1] }", "a"), "{ a: [1, 0, 1] }");
      Asserts.EqualsJToken(Masker.Mask("{ a: [{ b: { c: 1 } }, { d: 2 }], e: 3, f: 4, g: 5 }", "a(b/c),e"), "{ a: [{ b: { c: 1 } }, {}], e: 3 }");
      Asserts.EqualsJToken(Masker.Mask("{ a: [{ b: { c: { d: 1 } } }, { d: 2 }], e: 3, f: 4, g: 5 }", "a(b/c/d),e"), "{ a: [{ b: { c: { d: 1 } } }, {}], e: 3 }");
      Asserts.EqualsJToken(Masker.Mask("{alpha:3,beta:{first:'fv',second:{third:'tv',fourth:'fv'}},cappa:{first:'fv',second:{third:'tv',fourth:'fv'}}}", "beta(first,second/third),cappa(first,second/third)"), "{beta:{first:'fv',second:{third:'tv'}},cappa:{first:'fv',second:{third:'tv'}}}");
      Asserts.EqualsJToken(Masker.Mask("{ 'a/b': 1, c: 2 }", "a\\/b"), "{ 'a/b': 1 }");
      Asserts.EqualsJToken(Masker.Mask("{alpha:3,beta:{first:'fv','second/third':'tv',third:{fourth:'fv'}},cappa:{first:'fv','second/third':'tv',third:{fourth:'fv'}}}", "beta(first,second\\/third),cappa(first,second\\/third)"), "{beta:{first:'fv','second/third':'tv'},cappa:{first:'fv','second/third':'tv'}}");
      Asserts.EqualsJToken(Masker.Mask("{ '*': 101, beta: 'hidden' }", "\\*"), "{ '*': 101 }");
      Asserts.EqualsJToken(Masker.Mask("{ first: { '*': 101, beta: 'hidden' } }", "first(\\*)"), "{ first: { '*': 101 } }");
      Asserts.EqualsJToken(Masker.Mask("{ '*': 101, beta: 'hidden', some: 'visible' }", "some,\\*"), "{ '*': 101, some: 'visible' }");
      Asserts.EqualsJToken(Masker.Mask("{ '\\\\': 120, beta: 'hidden', some: 'visible' }", "some,\\\\"), "{ '\\\\': 120, some: 'visible' }");
      Asserts.EqualsJToken(Masker.Mask("{ multi: 130, line: 131, 'multi\nline': { a: 135, b: 134 } }", "multi\nline(a)"), "{ 'multi\nline': { a: 135 } }");
      Asserts.EqualsJToken(Masker.Mask("{ 'a*': 1, b: 2 }", "a*"), "{ 'a*': 1 }");
      Asserts.EqualsJToken(Masker.Mask("{ '*a': 1, b: 2 }", "*a"), "{ '*a': 1 }");

    }

  }
}