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
      public string? Name { get; set; }
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
      var actualObj = Masker.Mask(obj, mask);
      var actual = JsonConvert.SerializeObject(actualObj);
      var expected = new Person() { Name = person.Name }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);


      mask = "(name)";
      actualObj = Masker.Mask(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
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

      var actualObj = Masker.Mask(obj, mask);

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
      var actualObj = Masker.Mask(obj, mask);
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
      actualObj = Masker.Mask(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      Asserts.EqualsJToken(actual, expected);

      mask = "name,age,pet(name,age),children(name,age)";
      actualObj = Masker.Mask(obj, mask);
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

      var mask = "children/*";
      var actualObj = Masker.Mask(obj, mask);
      var actual = JsonConvert.SerializeObject(actualObj);
      var expected = new Person()
      {
        Children = new Person[]
        {
          person.Children[0],
          person.Children[1]
        }
      }.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);

      mask = "children(*)";
      actualObj = Masker.Mask(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      Asserts.EqualsJToken(actual, expected);

      mask = "name,age,pet(name,age),children/*";
      actualObj = Masker.Mask(obj, mask);
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
      actualObj = Masker.Mask(obj, mask);
      actual = JsonConvert.SerializeObject(actualObj);
      expected = person.ToJsonResponseString();
      Asserts.EqualsJToken(actual, expected);


    }


  }
}