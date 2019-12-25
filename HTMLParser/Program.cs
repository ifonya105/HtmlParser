using HTMLParser.Core;
using HTMLParser.Core.Habra;
using HTMLParser.Core.Interfaces;
using HTMLParser.Core.Models;
using HTMLParser.Core.VisualCron;
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
        static string filePath = "D:\\Downloads\\parse-result.txt";

        static void Main(string[] args)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
                
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine("HTML parser by ifonya." + Environment.NewLine);
            Console.WriteLine(new string('-', 10) + Environment.NewLine);

            Console.WriteLine("Parser:");
            foreach (Parsers p in (Parsers[])Enum.GetValues(typeof(Parsers)))
            {
                Console.WriteLine((int)p + " - " + p.ToString());
            }
            Console.Write("Select parser id: ");
            var selectedParser = (Parsers)Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter start page id: ");
            var startPoint = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter end page id: ");
            var endPoint = Convert.ToInt32(Console.ReadLine());
            Console.Write("Thread count: ");
            var threadCount = Convert.ToInt32(Console.ReadLine());

            switch (selectedParser)
            {
                case Parsers.Habr:
                    StartHabra(startPoint, endPoint, threadCount);
                    break;
                case Parsers.VisualCron:
                    StartVC(startPoint, endPoint, threadCount);
                    break;
                default:
                    throw new NotImplementedException($"Parser '{selectedParser}' not implemented");
            }
            
            Console.WriteLine("Parser is started");
            Console.WriteLine("Press enter to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Start habra parser
        /// </summary>
        /// <param name="startPoint">Start point. For example: page id</param>
        /// <param name="endPoint">End point. For example: page end id</param>
        /// <param name="threadCount">Parse thread count</param>
        private static void StartHabra(int startPoint, int endPoint, int threadCount = 1)
        {
            ParserWorker<HabraModel> worker = new ParserWorker<HabraModel>(new HabraParser(), new HabraParserSettings(startPoint, endPoint, threadCount));
            worker.OnCompleted += Worker_OnCompleted;
            worker.OnNewData += Worker_OnNewData;
            worker.OnPageCompleted += Worker_OnPageCompleted;
            worker.Start();
        }

        /// <summary>
        /// Start VisualCron parser
        /// </summary>
        /// <param name="startPoint">Start point. For example: page id</param>
        /// <param name="endPoint">End point. For example: page end id</param>
        /// <param name="threadCount">Parse thread count</param>
        private static void StartVC(int startPoint, int endPoint, int threadCount = 1)
        {
            ParserWorker<VisualCronModel> worker = new ParserWorker<VisualCronModel>(new VisualCronParser(), new VisualCronParserSettings(startPoint, endPoint, threadCount));
            worker.OnCompleted += Worker_OnCompleted;
            worker.OnNewData += Worker_OnNewData;
            worker.OnPageCompleted += Worker_OnPageCompleted;
            worker.Start();
        }

        /// <summary>
        /// Page completed event handler
        /// </summary>
        /// <param name="obj">Parser worker</param>
        /// <param name="pageNumber">Completed page number</param>
        private static void Worker_OnPageCompleted(object obj, int pageNumber)
        {
            Console.WriteLine($"Page {pageNumber} is completed");
        }

        /// <summary>
        /// New parsed data event handler
        /// </summary>
        /// <param name="obj">Parser worker</param>
        /// <param name="model">Parsed model</param>
        private static void Worker_OnNewData(object obj, IParserModel model)
        {
            lock (locker)
            {
                Console.WriteLine(model.Title);
                File.AppendAllText(filePath, model.ToString());
            }
        }

        /// <summary>
        /// Parser completed event handler
        /// </summary>
        /// <param name="obj">Parser worker</param>
        private static void Worker_OnCompleted(object obj)
        {
            Console.WriteLine("Parser is completed");
        }
    }
}
