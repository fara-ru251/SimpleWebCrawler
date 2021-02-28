using System;
using System.Collections.Generic;
using System.Text;
using WebCrawler.DAL;

namespace WebCrawler.PersistenceHelper
{
    public static class DbHelper
    {
        private static string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=NewsDB;User ID=developer;Password=2wsx@WSX;Trusted_Connection=True;";
        public static void InsertOrSkipIfExistNewsData(ref NewsData[] news)
        {
            using (var repo = new Repository(ConnectionString).Get<IData>())
            {
                for (int i = 0; i < news.Length; i++)
                {
                    news[i].IsExist = repo.SetNewsDataIfNotExist(news[i]);
                }
            }
        }
        public static void InsertFrequentWords(IDictionary<string, int> pairs)
        {
            using (var repo = new Repository(ConnectionString).Get<IData>())
            {
                foreach (var key_value in pairs)
                {
                    repo.SetWords(key_value.Key, key_value.Value);
                }
            }
        }
    }
}
