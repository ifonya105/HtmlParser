using AngleSharp.Html.Dom;
using HTMLParser.Core.Interfaces;
using HTMLParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HTMLParser.Core.VisualCron
{
    class VisualCronParser : IParser<VisualCronModel>
    {
        public IEnumerable<string> CollectUrls(IHtmlDocument document)
        {
            var results = new List<string>();
            var items = document.QuerySelectorAll("h2").Where(i => i.ClassName != null && i.ClassName.Contains("post-title")).Select(i=>i?.QuerySelector("a"));
            foreach (var item in items)
            {
                string url = item.GetAttribute("href");

                if (!string.IsNullOrEmpty(url))
                {
                    results.Add("https://www.visualcron.com" + url);
                }
            }
            return results;
        }

        public VisualCronModel Parse(IHtmlDocument document)
        {
            var post = document.GetElementsByClassName("post").FirstOrDefault();

            if (post == null)
            {
                return null;
            }

            // get title
            var title = post.GetElementsByClassName("post-title").FirstOrDefault()?.TextContent?.Trim();

            // get post date
            var dateStr = post.GetElementsByClassName("post-date").FirstOrDefault()?.TextContent?.Trim();
            var date = DateTime.MinValue;
            if (!string.IsNullOrEmpty(dateStr))
            {
                DateTime.TryParse(dateStr, out date);
            }

            // get post author
            var author = post.GetElementsByClassName("post-author").FirstOrDefault()?.TextContent?.Trim();

            // get post category
            var category = post.GetElementsByClassName("post-category").FirstOrDefault()?.TextContent?.Trim();

            // get post content
            var content = post.GetElementsByClassName("post-body text").FirstOrDefault()?.TextContent?.Trim();

            // get post tags
            var tags = post.GetElementsByClassName("post-tags").FirstOrDefault()?.QuerySelectorAll("a").Select(t=>t.TextContent?.Trim()).ToArray();

            VisualCronModel model = new VisualCronModel()
            {
                Author = author,
                Content = content,
                PostDate = date,
                Title = title,
                Category = category,
                Tags = tags
            };

            return model;
        }
    }
}
