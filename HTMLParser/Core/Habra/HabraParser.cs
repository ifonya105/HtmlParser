using AngleSharp.Html.Dom;
using HTMLParser.Core.Interfaces;
using HTMLParser.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HTMLParser.Core.Habra
{
    class HabraParser : IParser<HabraModel>
    {
        public IEnumerable<string> CollectUrls(IHtmlDocument document)
        {
            var results = new List<string>();
            var items = document.QuerySelectorAll("a").Where(i => i.ClassName != null && i.ClassName.Contains("post__title_link"));
            foreach (var item in items)
            {
                string url = item.GetAttribute("href");

                if (!string.IsNullOrEmpty(url))
                {
                    results.Add(url);
                }
            }
            return results;
        }

        public HabraModel Parse(IHtmlDocument document)
        {
            var post = document.GetElementsByClassName("post__wrapper").FirstOrDefault();

            if (post==null)
            {
                return null;
            }

            var author = post.GetElementsByClassName("user-info__nickname user-info__nickname_small").FirstOrDefault()?.TextContent;
            var dateStr = post.GetElementsByClassName("post__time").FirstOrDefault()?.GetAttribute("data-time_published");
            var date = DateTime.MinValue;
            if (!string.IsNullOrEmpty(dateStr))
            {
                DateTime.TryParse(dateStr, out date);
            }
            var title = post.GetElementsByClassName("post__title-text").FirstOrDefault()?.TextContent;

            var content = post.QuerySelectorAll("div").Where(m => m.LocalName == "div" &&
                                     m.HasAttribute("id") &&
                                     m.GetAttribute("id").StartsWith("post-content-body")).FirstOrDefault()?.TextContent;

            HabraModel model = new HabraModel()
            {
                Author = author,
                Content = content,
                PostDate = date,
                Title = title
            };

            return model;
        }
    }
}
