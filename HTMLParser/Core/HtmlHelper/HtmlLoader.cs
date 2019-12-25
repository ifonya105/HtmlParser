using HTMLParser.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace HTMLParser.Core.HtmlHelper
{
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            client = new HttpClient();
            url = $"{settings.BaseUrl}/{settings.Prefix}";
        }

        /// <summary>
        /// Get HTML source as text from build url
        /// </summary>
        /// <param name="id">Page id</param>
        /// <returns>HTML text</returns>
        public async Task<string> GetSourceByPageId(int id)
        {
            var currentUrl = url.Replace("{CurrentId}", id.ToString());
            var response = await client.GetAsync(currentUrl);
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }

        /// <summary>
        /// Get HTML source as text from url
        /// </summary>
        /// <param name="id">Page id</param>
        /// <returns>HTML text</returns>
        public async Task<string> GetSource(string url)
        {
            var response = await client.GetAsync(url);
            string source = null;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
