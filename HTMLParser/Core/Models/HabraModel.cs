using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLParser.Core.Models
{
    class HabraModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string Author { get; set; }

        public override string ToString()
        {
            return
                Environment.NewLine +
                new string('-',10) +
                Environment.NewLine +
                "Title: " + Title + Environment.NewLine + 
                "Content: " + Content + Environment.NewLine + 
                "PostData: " + PostDate.ToString("dd.MM.yyyy HH:mm") + Environment.NewLine + 
                "Author: " + Author +
                Environment.NewLine +
                new string('-', 10) +
                Environment.NewLine;
        }
    }
}
