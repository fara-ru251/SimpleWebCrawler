using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler.TextProcessingLogic
{
    internal static class TextProcessingHelper
    {
        public static string[] TextSplittingAndRemovingSymbols(string text)
        {
            // Strip Invalid Characters from a String, leave only 'whitespace', '@', '-'
            var stripped = Regex.Replace(text, @"[^\w @-]", "", RegexOptions.None);

            // Split stripped text
            var splitted = stripped.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return splitted;
        }

        public static void CountFrequentWords(ref ConcurrentDictionary<string, int> dictionary, string[] words)
        {
            foreach (var word in words)
            {
                var upper_word = word.ToUpper(CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(upper_word) || string.IsNullOrWhiteSpace(upper_word))
                {
                    // WARNING
                    continue;
                }
                
                // insert into dictionary only uppercase characters 
                dictionary.AddOrUpdate(upper_word, 1, (key, oldValue) => oldValue + 1);
            }
        }

        public static void RemoveOrDecreaseFrequentWords(ref ConcurrentDictionary<string, int> dictionary, string[] words)
        {
            foreach (var word in words)
            {
                var upper_word = word.ToUpper(CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(upper_word) || string.IsNullOrWhiteSpace(upper_word))
                {
                    // WARNING
                    continue;
                }

                // remove or decrease count
                
                dictionary.TryGetValue(upper_word, out int value);

                if (value > 1) // more than 1 occurrence
                {
                    dictionary.AddOrUpdate(upper_word, 1, (key, oldValue) => oldValue - 1);
                }
                else
                {
                    dictionary.TryRemove(upper_word, out int val);
                }
            }
        }

        public static string[] ConcatenateMultipleTextToWords(string[] texts)
        {
            List<string[]> str_arr_list = new List<string[]>();
            foreach (var text in texts)
            {
                str_arr_list.Add(TextSplittingAndRemovingSymbols(text));
            }

            return str_arr_list.SelectMany(s => s).ToArray();
        }
    }
}
