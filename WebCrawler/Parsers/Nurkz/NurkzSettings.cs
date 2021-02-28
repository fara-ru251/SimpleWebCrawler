using Abot2.Poco;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler.Parsers.Nurkz
{
    internal class NurkzSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://www.nur.kz";

        public bool IsPageParseAllowed(CrawledPage page)
        {
            Match match = Regex.Match(page.Uri.AbsoluteUri, @"(https?:\/\/www\.|https:\/\/)?[a-z0-9]+\.[a-z]{2}(\/[a-z0-9]+){1,3}(\/[0-9]+([\-\.]{1}[a-z0-9]+)+)\/");
            if (!match.Success)
            {
                return false;
            }

            return true;
        }
    }
}
