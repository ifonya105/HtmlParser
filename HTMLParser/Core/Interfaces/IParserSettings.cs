namespace HTMLParser.Core.Interfaces
{
    /// <summary>
    /// Parser settings
    /// </summary>
    interface IParserSettings
    {
        /// <summary>
        /// Base Url to parse
        /// </summary>
        string BaseUrl { get; set; }
        /// <summary>
        /// Additional prefix to add the base URL (for example: https://site.com/ + page)
        /// </summary>
        string Prefix { get; set; }
        /// <summary>
        /// Start point to parse (for example: page number - https://site.com/ + page + 1)
        /// </summary>
        int StartPoint { get; set; }
        /// <summary>
        /// End point to parse (for example: page number - https://site.com/ + page + 9)
        /// </summary>
        int EndPoint { get; set; }
        /// <summary>
        /// Number of used threads during parsing
        /// </summary>
        int ThreadCount { get; set; }
    }
}
