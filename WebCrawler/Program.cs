using Abot2.Crawler;
using Abot2.Poco;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.DAL;
using WebCrawler.Parsers;
using WebCrawler.Parsers.Nurkz;
using WebCrawler.PersistenceHelper;
using WebCrawler.TextProcessingLogic;

namespace WebCrawler
{
    class Program
    {
        public static BlockingCollection<NewsData> news_list = new BlockingCollection<NewsData>();

        public static ConcurrentDictionary<string, int> words_dictionary = new ConcurrentDictionary<string, int>(
                                            StringComparer.Create(CultureInfo.InvariantCulture,
                                            CompareOptions.IgnoreCase));


        static async Task Main(string[] args)
        {
            #region Crawler
            Console.WriteLine("Demo starting up!");

            IParserSettings settings = new NurkzSettings();
            IParser<NewsData> parser = new NurkzParser();
            await DemoSimpleCrawler(settings, parser);

            var news = news_list.ToArray(); // converting BlockingCollection<T> to array
            DbHelper.InsertOrSkipIfExistNewsData(ref news);

            var existing_news = news.Where(w => w.IsExist).Select(s => s.Text).ToArray(); // exclude existing in DB "News"
            var words = TextProcessingHelper.ConcatenateMultipleTextToWords(existing_news);

            TextProcessingHelper.RemoveOrDecreaseFrequentWords(ref words_dictionary, words);

            DbHelper.InsertFrequentWords(words_dictionary);

            Console.WriteLine("Press to terminate...");
            Console.ReadKey();
            #endregion
        }

        private static async Task DemoSimpleCrawler<T>(IParserSettings parserSettings, IParser<T> parser) where T : class
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 20, //Only crawl 50 pages
                MinCrawlDelayPerDomainMilliSeconds = 1000, //Wait this many millisecs between requests
            };
            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted; // event

            //crawler.ShouldCrawlPageDecisionMaker = CrawlPage; // delegate

            crawler.CrawlBag.Parser = parser;
            crawler.CrawlBag.Settings = parserSettings;


            var crawlResult = await crawler.CrawlAsync(new Uri(parserSettings.BaseUrl));
        }

        private static void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {

            if (e.CrawledPage.HttpRequestException != null)
            {
                return;
            }

            IParserSettings settings = e.CrawlContext.CrawlBag.Settings;
            if (!settings.IsPageParseAllowed(e.CrawledPage))
            {
                return;
            }

            IParser<NewsData> parser = e.CrawlContext.CrawlBag.Parser;

            var news = parser.Parse(e.CrawledPage.AngleSharpHtmlDocument, e.CrawledPage.Uri.AbsoluteUri);

            if (news == null) // something went wrong
            {
                return;
            }

            news_list.Add(news);


            var splitted = TextProcessingHelper.TextSplittingAndRemovingSymbols(news.Text);
            TextProcessingHelper.CountFrequentWords(ref words_dictionary, splitted);



            Console.WriteLine(e.CrawledPage.Uri);
            Console.WriteLine("=================================");
        }

    }

    
    public class Node
    {
        bool endOfWord = false;
        Dictionary<char, Node> Children = new Dictionary<char, Node>();

        public Node() { }
        public Node(bool endOfWord)
        {
            this.endOfWord = endOfWord;
        }

        public void AddWord(ref Node root, string word)
        {
            // a pointer to traverse the trie without damaging
            // the original reference
            Node node = root;

            // start traversal at root
            for (int i = 0; i < word.Length; ++i)
            {
                // if the current character does not exist as a child
                // to current node, add it
                if (node.Children.ContainsKey(word[i]) == false)
                {
                    node.Children.Add(word[i], new Node());
                }
                    

                // move traversal pointer to current word
                node = node.Children[word[i]];

                // if current word is the last one, mark it with
                // phrase Id
                if (i == word.Length - 1)
                {
                    node.endOfWord = true;
                }
            }
        }

        public bool FindAnyWord(ref Node root, string[] words)
        {

            // starting traversal at trie root and first
            // word in text
            for (int i = 0; i < words.Length; i++)
            {
                // a pointer to traverse the trie without damaging
                // the original reference
                Node node = root;

                var word = words[i];
                for (int j = 0; j < word.Length;)
                {
                    // if current node has current char as a child
                    // move both node and words pointer forward
                    if (node.Children.ContainsKey(word[j]))
                    {
                        // move trie pointer forward
                        node = node.Children[word[j]];

                        // move words pointer forward
                        ++j;
                    }
                    else
                    {
                        // current node does not have current
                        // char in its children
                        break;
                    }
                   
                }
                // one case remains, char pointer as reached the end
                // and the loop is over but the trie pointer is pointing to
                // a endOfWord
                if (node.endOfWord == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
