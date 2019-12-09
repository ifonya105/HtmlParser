using AngleSharp.Html.Dom;
using HTMLParser.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HTMLParser.Core.Habra
{
    class HabraParser : IParser<List<string>>
    {
        public List<string> Parse(IHtmlDocument document)
        {
            var results = new List<string>();
            var items = document.QuerySelectorAll("a").Where(i=> i.ClassName != null && i.ClassName.Contains("post__title_link"));
            foreach (var item in items)
            {
                results.Add(item.TextContent);
            }
            return results;
        }
    }
}
