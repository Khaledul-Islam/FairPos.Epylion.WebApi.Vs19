using FairPos.Epylion.Models;
using System;
using System.Collections.Generic;

namespace FairPos.Epyllion.Repository
{
    public interface IUserCounterRepository
    {
        List<UserCounter> SelectAll();
    }
    public class UserCounterRepository : BaseRepository, IUserCounterRepository
    {
        public UserCounterRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [CounterId] ,[CounterName] ,[IsActive] ,[CreatedBy] ,[CreateDate] FROM [dbo].[UserCounter]";
        }

        public List<UserCounter> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<UserCounter>(query, ref msg);
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }
    }
}
