﻿using HTMLParser.Core.Interfaces;

namespace HTMLParser.Core.Habra
{
    class HabraParserSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://habr.com/ru/";
        public string Prefix { get; set; } = "page{CurrentId}";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
        public int ThreadCount { get; set; } = 1;

        public HabraParserSettings(int startPoint, int endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public HabraParserSettings(int startPoint, int endPoint, int threadCount)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            ThreadCount = threadCount;
        }
    }
}
