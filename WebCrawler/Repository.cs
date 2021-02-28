using Insight.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace WebCrawler
{
    public class Repository
    {
        private readonly string connectionString;

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public T Get<T>() where T : class
        {
            var connection = this.GetConnection();
            return connection.As<T>();
        }

        public DbConnection GetDbConnection()
        {
            var connection = GetConnection();
            return connection;
        }


        private DbConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            return connection;
        }
    }

}
