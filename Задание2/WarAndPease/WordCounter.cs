using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WordCounter
{
    public class WordCounter
    {
        char[] delimiters = { ' ', '.', ',', ';', '!', '?' };

        string[] words;

        private Dictionary<string, int> CountWords(string text)
        {
            string l = "<dd>&nbsp;&nbsp;";
            text = text.Substring(text.IndexOf(l) + l.Length);
            text = Regex.Replace(text, "<.*?>", String.Empty);
            text = Regex.Replace(text, @"\r", String.Empty);
            text = Regex.Replace(text, @"\n", String.Empty);
            text = Regex.Replace(text, @"&nbsp;", String.Empty);

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();


            words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);


            foreach (string word in words)
            {
                string lowercaseWord = word.ToLower();

                if (wordCounts.ContainsKey(lowercaseWord))
                {
                    wordCounts[lowercaseWord]++;
                }
                else
                {
                    wordCounts[lowercaseWord] = 1;
                }
            }

            var wordCountsDesc = wordCounts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return wordCountsDesc;
       
        }

        public Dictionary<string, int> CountWordsMultiThreaded(string text)
        {
            string l = "<dd>&nbsp;&nbsp;";
            text = text.Substring(text.IndexOf(l) + l.Length);
            text = Regex.Replace(text, "<.*?>", String.Empty);
            text = Regex.Replace(text, @"\r", String.Empty);
            text = Regex.Replace(text, @"\n", String.Empty);
            text = Regex.Replace(text, @"&nbsp;", String.Empty);

            char[] delimiters = { ' ', '.', ',', ';', '!', '?' };

            words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            ConcurrentDictionary<string, int> parallelWordCounts = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(words, word =>
            {
                string lowercaseWord = word.ToLower();
                parallelWordCounts.AddOrUpdate(lowercaseWord, 1, (_, count) => count + 1);
            });

            Dictionary<string, int> wordCountsDesc = parallelWordCounts.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            return wordCountsDesc;
        }

    }
}
