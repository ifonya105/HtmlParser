using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLParser.Core.Interfaces
{
    interface IParserModel
    {
        string Title { get; set; }
        string Content { get; set; }
    }
}
