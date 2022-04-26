﻿using FairPos.Epylion.Models.Requisition;
using FairPos.Epyllion.Repository.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Requisition
{
    public interface IAutoRequisitionService
    {
        List<AutoRequistionTemp> GetTempRequistionData(out string errMsg);
        List<AutoRequistionTemp> GetTempRequistionDataByUser(string user, string shopid, out string errMsg);
        List<AutoRequistion> GetChallanByDateNonReject(string shopid);
        bool GenerateRequistion(string userID, string ShopID, out string errMsg);
        bool RemoveAllTempRequistion(string userId, string shopid, out string errMsg);
        bool UpdateActualQty(AutoRequistionTemp model);
        string AutoRequisitionSave(List<AutoRequistionTemp> olist, out string errMsg);
    }

    public class AutoRequisitionService : IAutoRequisitionService
    {
        private readonly IAutoRequisitionRepository _repo;

        public AutoRequisitionService(IAutoRequisitionRepository repo)
        {
            _repo = repo;
        }

        public string AutoRequisitionSave(List<AutoRequistionTemp> olist, out string errMsg)
        {
            return _repo.AutoRequisitionSave(olist, out errMsg);
        }

        public bool GenerateRequistion(string userID, string ShopID, out string errMsg)
        {
            return _repo.GenerateRequistion(userID,ShopID, out errMsg);
        }

        public List<AutoRequistion> GetChallanByDateNonReject(string shopid)
        {
            return _repo.GetChallanByDateNonReject( shopid);
        }

        public List<AutoRequistionTemp> GetTempRequistionData(out string errMsg)
        {
            return _repo.GetTempRequistionData(out errMsg);
        }

        public List<AutoRequistionTemp> GetTempRequistionDataByUser(string user, string shopid, out string errMsg)
        {
            return _repo.GetTempRequistionDataByUser(user,shopid, out errMsg);
        }

        public bool RemoveAllTempRequistion(string userId, string shopid, out string errMsg)
        {
            return _repo.RemoveAllTempRequistion(userId,shopid,out errMsg);
        }

        public bool UpdateActualQty(AutoRequistionTemp model)
        {
            return _repo.UpdateActualQty(model);
        }
    }
}
