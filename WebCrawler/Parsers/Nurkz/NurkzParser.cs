using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler.Parsers.Nurkz
{
    internal sealed class NurkzParser : IParser<NewsData>
    {
        public NewsData Parse(IHtmlDocument document, string url)
        {
            //retrieve "title", only first match interested
            var title_element = document.QuerySelector("h1.article__headline"); // can be null

            if (title_element == null)
            {
                return null;
            }

            var title = title_element.TextContent;
            //Console.WriteLine(title);

            //retrieve "datetime", only first match interested
            var datetime_element = document.QuerySelector("time")?.GetAttribute("datetime"); // can be null

            if (datetime_element == null)
            {
                return null;
            }

            DateTime time = DateTime.Parse(datetime_element);
            //Console.WriteLine(time);

            //article "text", only first match interested
            var outer_article = document.QuerySelector("div.formatted-body.io-article-body.js-article-body"); // can be null

            var text_elements = outer_article?.Children;

            if (text_elements == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var element in text_elements.OfType<IHtmlParagraphElement>())
            {
                sb.AppendLine(element.TextContent);
                //Console.WriteLine(element.TextContent);
            }

            return new NewsData(time, title, sb.ToString(), url);
        }
    }
}
