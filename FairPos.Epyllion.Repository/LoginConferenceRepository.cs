﻿using FairPos.Epylion.Models;
using FIK.DAL;
using System;
using System.Collections.Generic;

namespace FairPos.Epyllion.Repository
{
    public interface ILoginConferenceRepository
    {
        bool Insert(LoginConference model);
        bool Delete(LoginConference model);
        List<LoginConference> SelectAll();
    }

    public class LoginConferenceRepository : BaseRepository, ILoginConferenceRepository
    {
        public LoginConferenceRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [ShopID] ,[CounterId] ,[UserId] ,[LoginTime] FROM [dbo].[LoginConference]";
        }

        public bool Delete(LoginConference model)
        {
            bool r = _dal.Delete<LoginConference>(model, "ShopID,CounterId,UserId", "", ref msg);
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }
        public bool Insert(LoginConference model)
        {
            CompositeModel compositeModel = new CompositeModel();
            compositeModel.AddRecordSet<LoginConference>(model, OperationMode.Insert, "", "", "", "");
            bool r = _dal.InsertUpdateComposite(compositeModel, ref msg);
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);
            return r;
        }
        public List<LoginConference> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<LoginConference>(query, ref msg);
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }
    }
}
