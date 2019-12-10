using HTMLParser.Core;
using HTMLParser.Core.Habra;
using HTMLParser.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser
{
    enum Parsers
    {
        Habr = 1,
        VisualCron = 2
    }
    class Program
    {
        static readonly object locker = new object();
        static ParserWorker<HabraModel> worker = null;
        static string filePath = "D:\\Downloads\\habra_result.txt";

        static void Main(string[] args)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
                
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine("HTML parser by ifonya." + Environment.NewLine);
            Console.WriteLine(new string('-', 10) + Environment.NewLine);

            Console.Write("Enter start page id: ");
            var startPoint = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter end page id: ");
            var endPoint = Convert.ToInt32(Console.ReadLine());

            worker = new ParserWorker<HabraModel>(new HabraParser(), new HabraParserSettings(startPoint, endPoint));
            worker.OnCompleted += Worker_OnCompleted;
            worker.OnNewData += Worker_OnNewData;
            worker.OnPageCompleted += Worker_OnPageCompleted;
            worker.Start();
            Console.WriteLine("Parser is started");
            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
        }

        private static void Worker_OnPageCompleted(object obj, int pageNumber)
        {
            Console.WriteLine($"Page {pageNumber} is completed");
        }

        private static void Worker_OnNewData(object obj, HabraModel model)
        {
            lock (locker)
            {
                Console.WriteLine(model.Title);
                File.AppendAllText(filePath, model.ToString());
            }
        }

        private static void Worker_OnCompleted(object obj)
        {
            Console.WriteLine("Parser is completed");
        }
    }
}
