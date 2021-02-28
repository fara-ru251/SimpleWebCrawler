using Insight.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler.DAL
{
    public interface IData : IDisposable
    {
        private const string schema = "dbo";

        [Sql(Schema = schema)]
        bool SetNewsDataIfNotExist(NewsData news);

        [Sql(Schema = schema)]
        void SetWords(string Name, int Frequency);
    }
}
