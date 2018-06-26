
namespace Sitecore.MediaFramework.Indexing.Analyzers
{
  using System.IO;
  using System.Collections.Generic;

  using Lucene.Net.Analysis;

  using Sitecore.MediaFramework.Indexing.Filters;

  public class MapCharLowerCaseAnalyzer : KeywordAnalyzer
  {
    public MapCharLowerCaseAnalyzer()
    {
      this.CharMap = new Dictionary<char, char>();
    }

    protected Dictionary<char, char> CharMap;

    public void AddCharMap(string value)
    {
      if (value.Length == 3)
      {
        this.CharMap.Add(value[0], value[2]);
      }
    }

    public override TokenStream TokenStream(string fieldName, TextReader reader)
    {
      TokenStream source = new KeywordTokenizer(reader);

      return new MapCharFilter(this.CharMap, new LowerCaseFilter(source));
    }
  }
}