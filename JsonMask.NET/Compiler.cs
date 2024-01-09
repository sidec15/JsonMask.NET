using System.Dynamic;

namespace JsonMask.NET
{


  public class Compiler
  {

    private static readonly IDictionary<char, int> TERMINALS = new Dictionary<char, int>()
    {
      { ',', 1 },{ '/', 2 },{ '(', 3 },{ ')', 4 }
    };
    private const char ESCAPE_CHAR = '\\';
    private const char WILDCARD_CHAR = '*';
    private const string _N = "_n";

    public object Compile(string text)
    {
      if (string.IsNullOrEmpty(text))
        return null;

      var scanRes = Scan(text);
      return Parse(scanRes);
    }

    private IList<dynamic> Scan(string text)
    {
      int len = text.Length;
      IList<dynamic> tokens = new List<dynamic>();
      var name = string.Empty;
      char ch;

      void MaybePushName()
      {
        if (string.IsNullOrEmpty(name)) return;
        dynamic token = new ExpandoObject();
        token.Tag = _N;
        token.Value = name;
        tokens.Add(token);
        name = string.Empty;
      }

      for (int i = 0; i < len; i++)
      {
        ch = text[i];
        if (ch == ESCAPE_CHAR)
        {
          i++;
          ch = text[i];
          name += ch == WILDCARD_CHAR ? ESCAPE_CHAR + WILDCARD_CHAR : ch;
        }
        else if (TERMINALS.ContainsKey(ch))
        {
          MaybePushName();
          dynamic token = new ExpandoObject();
          token.Tag = Convert.ToString(ch);
          tokens.Add(token);
        }
        else
        {
          name += ch;
        }
      }

      MaybePushName();

      return tokens;
    }

    private dynamic Parse(IList<dynamic> tokens)
    {
      return BuildTree(tokens, new ExpandoObject());
    }

    private dynamic BuildTree(IList<dynamic> tokens, dynamic parent)
    {
      dynamic props = new ExpandoObject();
      dynamic token;
      while (token = Utils.Shift(tokens) != null)
      {
        if (token.Tag == _N)
        {
          token.Type = "object";
          token.Properties = BuildTree(tokens, token);
          if (parent.HasChild)
          {
            AddToken(token, props);
            return props;
          }
        }
        else if (token.Tag == ",")
        {
          return props;
        }
        else if (token.Tag == "(")
        {
          parent.Type = "array";
          continue;
        }
        else if (token.Tag == ")")
        {
          return props;
        }
        else if (token.Tag == "/")
        {
          parent.HasChild = true;
          continue;
        }

        AddToken(token, props);
      }

      return null;
    }

    private void AddToken(dynamic token, dynamic props)
    {
      dynamic prop = new ExpandoObject();
      prop.Type = token.Type;

      if (token.Value == WILDCARD_CHAR)
        prop.IsWildcard = true;
      else if (token.Value == ESCAPE_CHAR + WILDCARD_CHAR)
        token.Value = WILDCARD_CHAR;

      if (!Utils.IsEmpty(token.Properties))
      {
        prop.Properties = token.Properties;
      }

      props[token.value] = prop;
    }


    //private class Token
    //{
    //  public string Tag { get; set; }
    //  public object Value { get; set; }
    //  public string Type { get; set; }
    //  public object Properties { get; set; }
    //}
  }

}