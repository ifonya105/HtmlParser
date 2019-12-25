using HTMLParser.Core.Interfaces;

namespace HTMLParser.Core.VisualCron
{
    class VisualCronParserSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://www.visualcron.com/blog";
        public string Prefix { get; set; } = "?page={CurrentId}";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public int ThreadCount { get; set; } = 1;

        public VisualCronParserSettings(int startPoint, int endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public VisualCronParserSettings(int startPoint, int endPoint, int threadCount)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            ThreadCount = threadCount;
        }
    }
}
