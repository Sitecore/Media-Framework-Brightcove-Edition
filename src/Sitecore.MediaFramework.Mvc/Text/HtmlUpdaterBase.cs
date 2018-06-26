namespace Sitecore.MediaFramework.Mvc.Text
{
  using System.Text;

  public abstract class HtmlUpdaterBase : IHtmlUpdater
  {
    public abstract bool UpdateHtml(StringBuilder html);

    protected int IndexOfHeadEnd(StringBuilder html)
    {
      return this.IndexOf(html, "</head>", 0, true);
    }

    protected int IndexOfBodyEnd(StringBuilder html)
    {
      return this.IndexOf(html, "</body>", 0, true);
    }

    protected int IndexOf(StringBuilder html, string value, int startIndex, bool ignoreCase)
    {
      int num3;
      int length = value.Length;
      int num2 = (html.Length - length) + 1;

      if (ignoreCase == false)
      {
        for (int i = startIndex; i < num2; i++)
        {
          if (html[i] == value[0])
          {
            num3 = 1;
            while ((num3 < length) && (html[i + num3] == value[num3]))
            {
              num3++;
            }
            if (num3 == length)
            {
              return i;
            }
          }
        }
      }
      else
      {
        for (int j = startIndex; j < num2; j++)
        {
          if (char.ToLower(html[j]) == char.ToLower(value[0]))
          {
            num3 = 1;
            while ((num3 < length) && (char.ToLower(html[j + num3]) == char.ToLower(value[num3])))
            {
              num3++;
            }
            if (num3 == length)
            {
              return j;
            }
          }
        }
      }
      return -1;
    }
  };
}