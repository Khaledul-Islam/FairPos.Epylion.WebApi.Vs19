using System.Data;
using System.Data.SqlClient;

namespace FairPos.Epyllion.Repository
{
    public class BaseRepository
    {
        public FIK.DAL.SQL _dal;
        public string query ;
        public string msg;
        public string baseQuery;
        public string whereCluase;
        public string offsetCluase;

        private string _selectQuery;
        public readonly IDbConnection _dapper;

        public BaseRepository(IDBConnectionProvider dBConnectionProvider)
        {
            msg = "";
            _dal = new FIK.DAL.SQL(dBConnectionProvider.GetDbConnection());
            _dapper = new SqlConnection(dBConnectionProvider.GetDbConnection());
        }


        // GetMaxId("CounterId","000","001","Counter")
        public string GetMaxId(string coloumName, string rightStringFormat, string initialValue, string tableName)
        {
            decimal maxId = 0;
            _selectQuery = "SELECT ISNULL(MAX(RIGHT(" + coloumName + ", " + rightStringFormat.Length + ")) + 1, " + initialValue + ") AS maxID " +
                               " FROM  " + tableName + " ";

            var  dt = _dal.SelectFirstColumn(_selectQuery, ref msg);

           
                decimal.TryParse(dt, out maxId);
            if (maxId == 0)
                maxId = 1;

            return maxId.ToString(rightStringFormat);
        }

     

        public string GetMaxIdWithPrfix2(string coloumName, string rightStringLength, string initialValue, string tableName, string prefix)
        {
            decimal maxId = 0;

            _selectQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(" + coloumName + "," + (prefix.Length + 1).ToString() + ", " + rightStringLength.Length + ") as int)) + 1, " + initialValue + ") AS maxID " +
                               " FROM  " + tableName + " where left(" + coloumName + "," + prefix.Length + ")='" + prefix + "' ";


            var dt = _dal.SelectFirstColumn(_selectQuery, ref msg);
            decimal.TryParse(dt, out maxId);

           
            return prefix + maxId.ToString((rightStringLength));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coloumName"></param>
        /// <param name="rightStringLength">000</param>
        /// <param name="initialValue">001</param>
        /// <param name="tableName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetMaxIdWithPrfix(string coloumName, string rightStringLength, string initialValue, string tableName, string prefix)
        {
            decimal maxId = 0;

            _selectQuery = "SELECT ISNULL(MAX(RIGHT(" + coloumName + ", " + rightStringLength.Length + ")) + 1, " + initialValue + ") AS maxID " +
                               " FROM  " + tableName + " where left(" + coloumName + "," + prefix.Length + ")='" + prefix + "' ";


            var dt = _dal.SelectFirstColumn(_selectQuery, ref msg);
            decimal.TryParse(dt, out maxId);


            return prefix + maxId.ToString((rightStringLength));

        }

  


     
        public string GetMaxId(string coloumName, string tableName)
        {
            decimal maxId = 0;

            _selectQuery = "SELECT  MAX(" + coloumName + ") +1 AS " + coloumName + " FROM " + tableName;

            var dt = _dal.SelectFirstColumn(_selectQuery, ref msg);
            decimal.TryParse(dt, out maxId);

            if (maxId == 0)
                maxId = 1;

            return maxId.ToString();

        }



    }
}
