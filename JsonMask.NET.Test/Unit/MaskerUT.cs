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
    public void MaskTest()
    {

      Assert.That(Masker.Mask(null, string.Empty), Is.Null);
    }

  }
}
