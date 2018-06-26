namespace Sitecore.MediaFramework.Indexing.Filters
{
  using System.Collections.Generic;

  using Lucene.Net.Analysis;
  using Lucene.Net.Analysis.Tokenattributes;

  public class MapCharFilter : TokenFilter
  {
    private readonly Dictionary<char, char> map;

    private readonly ITermAttribute termAtt;

    public MapCharFilter(Dictionary<char, char> map, TokenStream @in)
      : base(@in)
    {
      this.map = map;
      this.termAtt = this.AddAttribute<ITermAttribute>();
    }

    public override bool IncrementToken()
    {
      if (!this.input.IncrementToken())
      {
        return false;
      }

      char[] chars = this.termAtt.TermBuffer();
      int num = this.termAtt.TermLength();
      for (int i = 0; i < num; i++)
      {
        char key = chars[i];

        if (this.map.ContainsKey(key))
        {
          chars[i] = this.map[key];
        }

        chars[i] = char.ToLower(chars[i]);
      }

      return true;
    }
  }
}