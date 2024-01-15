using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonMask.NET.Test.Unit
{
  internal class UtilsUT
  {

    class SimpleTests : UtilsUT
    {

      [Test]
      public void HasKeyTest()
      {
        Assert.That(Utils.HasKey(null, "k"), Is.False);

        dynamic obj = new ExpandoObject();
        var value = "value";
        obj.Key = value;
        Assert.That(Utils.HasKey(obj, "Key"), Is.True);
      }

      [Test]
      public void IsObjectTest()
      {

        dynamic obj = null;
        Assert.That(Utils.IsObject(obj), Is.False);

        obj = new ExpandoObject();
        Assert.That(Utils.IsObject(obj), Is.True);

        obj = (Delegate)(() => 1);
        Assert.That(Utils.IsObject(obj), Is.True);

      }

      [Test]
      public void IsEmptyTest()
      {
        dynamic obj = null;
        Assert.That(Utils.IsEmpty(obj), Is.True);

        obj = Array.Empty<dynamic>();
        Assert.That(Utils.IsEmpty(obj), Is.True);

        obj = string.Empty;
        Assert.That(Utils.IsEmpty(obj), Is.True);

        obj = new ExpandoObject();
        Assert.That(Utils.IsEmpty(obj), Is.True);

        obj.Key = "value";
        Assert.That(Utils.IsEmpty(obj), Is.False);

      }

    }

    class ShiftTests : UtilsUT
    {

      [Test]
      public void NullTest()
      {
        IList<int> list = null;
        Utils.Shift(list);
      }

      [Test]
      public void EmptyTest()
      {
        IList<int> list = Array.Empty<int>().ToList();
        Utils.Shift(list);
      }

    }

  }
}
