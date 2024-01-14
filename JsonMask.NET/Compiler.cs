using System.Dynamic;

namespace JsonMask.NET
{
  internal static class Compiler
  {

    private static readonly IDictionary<string, int> TERMINALS = new Dictionary<string, int>()
    {
      { ",", 1 },{ "/", 2 },{ "(", 3 },{ ")", 4 }
    };
    private const string ESCAPE_CHAR = "\\";
    private const string WILDCARD_CHAR = "*";
    private const string _N = "_n";
    private const string HAS_CHILD = "HasChild";

    public static object Compile(string text)
    {
      if (string.IsNullOrEmpty(text))
        return null;

      var scanRes = Scan(text);
      var parseRes = Parse(scanRes);

      return parseRes;
    }

    private static IList<dynamic> Scan(string text)
    {
      int len = text.Length;
      IList<dynamic> tokens = new List<dynamic>();
      var name = string.Empty;
      string ch;

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
        ch = text[i].ToString();
        if (ch == ESCAPE_CHAR)
        {
          i++;
          ch = text[i].ToString();
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

    private static dynamic Parse(IList<dynamic> tokens)
    {
      var res = BuildTree(tokens, new ExpandoObject());
      return res;
    }

    private static dynamic BuildTree(IList<dynamic> tokens, dynamic parent)
    {
      dynamic props = new ExpandoObject();
      dynamic token;
      while ((token = Utils.Shift(tokens)) != null)
      {
        if (token.Tag == _N)
        {
          token.Type = "object";
          token.Properties = BuildTree(tokens, token);
          var parentDict = parent as IDictionary<string, object>;
          if (parentDict.ContainsKey(HAS_CHILD))
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
          Utils.Push(parent, HAS_CHILD, true);
          //parent[HAS_CHILD] = true;
          continue;
        }

        AddToken(token, props);
      }

      return props;
    }

    private static void AddToken(dynamic token, dynamic props)
    {
      dynamic prop = new ExpandoObject();
      prop.Type = token.Type;

      if (token.Value == WILDCARD_CHAR)
        Utils.Push(prop, Utils.IS_WILDCARD, true);
      else if (token.Value == ESCAPE_CHAR + WILDCARD_CHAR)
        token.Value = WILDCARD_CHAR;

      if (!Utils.IsEmpty(token.Properties))
      {
        prop.Properties = token.Properties;
      }

      Utils.Push(props, token.Value, prop);
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