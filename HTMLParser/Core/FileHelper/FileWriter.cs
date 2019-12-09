using HTMLParser.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HTMLParser.Core.FileHelper
{
    class FileWriter : IWriter<List<string>>
    {
        /// <summary>
        /// Base directory (default MyDocuments directory)
        /// </summary>
        public string BaseDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        /// <summary>
        /// Filename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Get full file name
        /// </summary>
        public string FullPath
        {
            get
            {
                return Path.Combine(BaseDirectory, FileName);
            }
        }

        public FileWriter(string fileName)
        {
            this.FileName = fileName;
        }

        public FileWriter(string baseDirectory, string fileName) : this(fileName)
        {
            this.BaseDirectory = baseDirectory;
        }

        /// <summary>
        /// Write items to file
        /// </summary>
        /// <param name="items">Items to write</param>
        public async void WriteAsync(List<string> items)
        {
            await File.WriteAllLinesAsync(FullPath, items);
        }

        /// <summary>
        /// Write item to file
        /// </summary>
        /// <param name="item"></param>
        public async void WriteAsync(string item)
        {
            await File.WriteAllTextAsync(FullPath, item);
        }
    }
}
