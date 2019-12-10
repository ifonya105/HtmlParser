using AngleSharp.Html.Parser;
using HTMLParser.Core.HtmlHelper;
using HTMLParser.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser.Core
{
    class ParserWorker<T> where T : class
    {
        IParser<T> parser;
        IParserSettings parserSettings;
        HtmlLoader loader;
        bool isActive;

        #region Properties

        public IParser<T> Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }

        public IParserSettings Settings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        #endregion

        #region Constructors

        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings settings) : this(parser)
        {
            this.parserSettings = settings;
            loader = new HtmlLoader(settings);
        }

        #endregion

        #region Events

        public event Action<object, T> OnNewData;
        public event Action<object> OnCompleted;
        public event Action<object, int> OnPageCompleted;

        #endregion

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Abort()
        {
            isActive = false;
        }

        private async void Worker()
        {
            for (int i = parserSettings.StartPoint; i<=parserSettings.EndPoint; i++)
            {
                if (!isActive)
                {
                    OnCompleted?.Invoke(this);
                    return;
                }

                // get page source
                var source = await loader.GetSourceByPageId(i);
                var document = await new HtmlParser().ParseDocumentAsync(source);

                // get urls for this page
                var result = parser.CollectUrls(document);

                // parse each url
                await ParseEachUrl(result);

                OnPageCompleted?.Invoke(this, i);
            }

            OnCompleted?.Invoke(this);
            isActive = false;
        }

        private async Task ParseEachUrl(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                var source = await loader.GetSource(url);
                var document = await new HtmlParser().ParseDocumentAsync(source);

                var result = parser.Parse(document);

                OnNewData?.Invoke(this, result);
            }
        }
    }
}
