using AngleSharp.Html.Parser;
using HTMLParser.Core.HtmlHelper;
using HTMLParser.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
                // raise completed event and exit if parser is deactivate
                if (!isActive)
                {
                    OnCompleted?.Invoke(this);
                    return;
                }

                // get page source by page id
                var source = await loader.GetSourceByPageId(i);

                // create html document from source
                var document = await new HtmlParser().ParseDocumentAsync(source);

                // get urls for this page
                var result = parser.CollectUrls(document);

                // if urls is empty then get next page
                if (result.Count() == 0)
                {
                    continue;
                }

                // create urls queue
                var urlQueue = new ConcurrentQueue<string>(result);

                // parse each url in multithreaded mode
                Task[] tasks = new Task[parserSettings.ThreadCount];
                for (int j = 0; j < parserSettings.ThreadCount; j++)
                {
                    tasks[j] = ParseEachUrl(urlQueue);
                }

                // wait for all tasks to complete
                Task.WaitAll(tasks);

                // raise page completed event
                OnPageCompleted?.Invoke(this, i);
            }

            // raise completed event
            OnCompleted?.Invoke(this);

            // parser is deactivate
            isActive = false;
        }

        private async Task ParseEachUrl(ConcurrentQueue<string> urls)
        {
            while (urls.TryDequeue(out string url))
            {
                // get url htmls source code
                var source = await loader.GetSource(url);

                // create html document from html source code
                var document = await new HtmlParser().ParseDocumentAsync(source);

                // parse html document 
                var result = parser.Parse(document);

                // raise new data event
                OnNewData?.Invoke(this, result);
            }
        }
    }
}
