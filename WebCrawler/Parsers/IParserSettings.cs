using Abot2.Poco;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler.Parsers
{
    public interface IParserSettings
    {
        string BaseUrl { get; set; }

        bool IsPageParseAllowed(CrawledPage page);
    }
}
