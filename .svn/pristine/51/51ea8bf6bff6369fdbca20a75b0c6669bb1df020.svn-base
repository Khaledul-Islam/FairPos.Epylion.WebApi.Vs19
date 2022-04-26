using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository
{
    public class DBConnectionProvider : IDBConnectionProvider
    {
        public static string DbConnection;
        public string GetDbConnection()
        {
            return DbConnection;
        }

        public void SetDbConnectionString(string connectionString)
        {
            DbConnection = connectionString;
        }

    }

    public interface IDBConnectionProvider
    {
        string GetDbConnection();
        void SetDbConnectionString(string connectionString);
    }
}
