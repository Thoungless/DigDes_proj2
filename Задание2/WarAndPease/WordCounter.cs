using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WordCounter
{
    public class WordCounter
    {
        private Dictionary<string, int> CountWords(string text)
        {
            string l = "<dd>&nbsp;&nbsp;";
            text = text.Substring(text.IndexOf(l) + l.Length);
            text = Regex.Replace(text, "<.*?>", String.Empty);
            text = Regex.Replace(text, @"\r", String.Empty);
            text = Regex.Replace(text, @"\n", String.Empty);
            text = Regex.Replace(text, @"&nbsp;", String.Empty);

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();

            char[] delimiters = { ' ', '.', ',', ';', '!', '?' };

            string[] words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

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
            Dictionary<string, int> wordCounts = new Dictionary<string, int>();
            Parallel.Invoke(() =>
            {
                wordCounts = CountWords(text);
            });

            return wordCounts;
        }
    }
}
