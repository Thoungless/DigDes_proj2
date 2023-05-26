using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace DigDes_2_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            string book = (@"http://az.lib.ru/t/tolstoj_lew_nikolaewich/text_0040.shtml");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\book.txt";
            Dictionary<string, int> res = new Dictionary<string, int>();

            WebRequest req = WebRequest.Create(book);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
            string text = sr.ReadToEnd();


            Stopwatch privateMethodStopwatch = new Stopwatch();
            Stopwatch publicMethodStopwatch = new Stopwatch();

            Type type = typeof(WordCounter.WordCounter);

            MethodInfo methodInfo = type.GetMethod("CountWords", BindingFlags.NonPublic | BindingFlags.Instance);
            WordCounter.WordCounter wc2 = new WordCounter.WordCounter();

            if (methodInfo != null)
            {
                WordCounter.WordCounter wc = new WordCounter.WordCounter();
                object[] parametrs = { text };

                privateMethodStopwatch.Start();
                res = methodInfo.Invoke(wc, parametrs) as Dictionary<string, int>;
                privateMethodStopwatch.Stop();

                res = null;


                publicMethodStopwatch.Start();
                res = wc2.CountWordsMultiThreaded(text);
                publicMethodStopwatch.Stop();

                Console.WriteLine($"Приватный метод: Обработка текста завершена.");
                Console.WriteLine($"Приветный метод обработкой: Время выполнения: {privateMethodStopwatch.ElapsedMilliseconds} мс");


                Console.WriteLine($"Публичный метод с многопоточной обработкой: Обработка текста завершена.");
                Console.WriteLine($"Публичный метод с многопоточной обработкой: Время выполнения: {publicMethodStopwatch.ElapsedMilliseconds} мс");
            }
            else
            {
                Console.WriteLine("Метод не найден");
            }

            foreach (var item in res)
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(item);
                }
            }
        }

    }
}