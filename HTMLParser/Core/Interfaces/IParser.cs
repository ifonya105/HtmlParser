using AngleSharp.Html.Dom;
using System.Collections.Generic;

namespace HTMLParser.Core.Interfaces
{
    interface IParser<T> where T : class
    {
        /// <summary>
        /// Parse document
        /// </summary>
        /// <param name="document">Html document</param>
        /// <returns>Parse result</returns>
        T Parse(IHtmlDocument document);

        IEnumerable<string> CollectUrls(IHtmlDocument document);
    }
}
