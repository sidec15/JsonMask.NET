
namespace JsonMask.NET
{
  public interface IMaskerService
  {
    string Mask(string json, string mask);
  }

  public class MaskerService : IMaskerService
  {

    public string Mask(string json, string mask)
    {
      return Masker.Mask(json, mask);
    }

  }
}
