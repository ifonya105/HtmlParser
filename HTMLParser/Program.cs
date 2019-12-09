using HTMLParser.Core;
using HTMLParser.Core.Habra;
using System;
using System.Collections.Generic;
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
        static ParserWorker<List<string>> worker = null;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = new UTF8Encoding();
            Console.WriteLine("HTML parser by ifonya." + Environment.NewLine);
            Console.WriteLine(new string('-', 10) + Environment.NewLine);
            do
            {
                Console.WriteLine("Which parser you do want to use?");
                Console.WriteLine("1. Habr");
                Console.Write("Enter number: ");

                Parsers parserNumber = (Parsers)Convert.ToInt32(Console.ReadLine());

                switch (parserNumber)
                {
                    case Parsers.Habr:
                        Console.Clear();
                        Console.Write("Enter start page id: ");
                        var startPoint = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter end page id: ");
                        var endPoint = Convert.ToInt32(Console.ReadLine());

                        worker = new ParserWorker<List<string>>(new HabraParser(), new HabraParserSettings(startPoint, endPoint));
                        worker.OnCompleted += Worker_OnCompleted;
                        worker.OnNewData += Worker_OnNewData;
                        worker.Start();
                        Console.Clear();
                        Console.WriteLine("Parser is started");
                        Console.WriteLine(new string('-', 10) + Environment.NewLine);
                        break;
                    default:
                        Console.WriteLine("Wrong number. Please try again.");
                        Console.WriteLine("Press 'Escapse' for exit or any key for try again");
                        break;
                }

                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.Escape)
                {
                    worker.Abort();
                    Console.WriteLine("PParser is stopped.");
                    break;
                }
                else
                {
                    Console.Clear();
                }
            }
            while (true);

            Console.WriteLine("Console close after 3s..");
            await Task.Delay(3000);
        }

        private static void Worker_OnNewData(object obj, List<string> items)
        {
            items.ForEach(i => Console.WriteLine(i));
        }

        private static void Worker_OnCompleted(object obj)
        {
            Console.WriteLine(new string('-', 10) + Environment.NewLine);
            Console.WriteLine("Parser is completed");
            Console.WriteLine("Press 'Escapse' for exit or any key for try again");
        }
    }
}
