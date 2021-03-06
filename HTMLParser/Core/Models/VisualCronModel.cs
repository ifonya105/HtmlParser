﻿using HTMLParser.Core.Interfaces;
using System;

namespace HTMLParser.Core.Models
{
    class VisualCronModel : IParserModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string[] Tags { get; set; }

        public override string ToString()
        {
            return
                Environment.NewLine +
                new string('-', 10) +
                Environment.NewLine +
                "Title: " + Title + Environment.NewLine +
                "Content: " + Content + Environment.NewLine +
                "PostData: " + PostDate.ToString("dd.MM.yyyy") + Environment.NewLine +
                "Author: " + Author + Environment.NewLine +
                "Category: " + Category + Environment.NewLine +
                "Tags: " + string.Join(",", Tags) +
                Environment.NewLine +
                new string('-', 10) +
                Environment.NewLine;
        }
    }
}
