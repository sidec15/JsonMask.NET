using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonMask.NET.Test.Unit
{
  internal class MaskerUT
  {

    [Test]
    public void MaskObjTest()
    {

      Assert.That(Masker.MaskObj(null, string.Empty), Is.Null);
    }

  }
}
