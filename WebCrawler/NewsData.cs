using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public sealed class NewsData
    {
        public DateTime CreatedOn { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string Url { get; private set; }

        public bool IsExist { get; set; }

        public NewsData(DateTime datetime, string title, string text, string url)
        {
            this.CreatedOn = datetime;
            this.Title = title;
            this.Text = text;
            this.Url = url;
            this.IsExist = false; // initial
        }
    }
}
