using Dapper;
using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Requisition;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Requisition
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FileName: MonthlyBudgetRepository.cs
    //FileType: C# Source file
    //Author : SHUVO
    //Created On : 10-01-2022
    //Last Modified On :
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IMonthlyBudgetRepository
    {
        bool SaveMonthlyBudget(MonthlyBudget budget, out string errMsg);
        List<MonthlyBudget> GetShopBudget(int year, string ShopID, out string errMsg);
        decimal GetCurrentMonthShopBudget(int year,string month,string ShopID,out string errMsg);
        List<YearList> YearList();
    }
    public class MonthlyBudgetRepository : BaseRepository, IMonthlyBudgetRepository
    {
        public MonthlyBudgetRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
        }

        public decimal GetCurrentMonthShopBudget(int year, string month, string ShopID, out string errMsg)
        {
            errMsg = string.Empty;
            string query = "select " + month + " as Budget from MonthlyBudget where ShopID='" + ShopID + "' and Year='" + year + "'";
            decimal budget = _dapper.Query<decimal>(query).FirstOrDefault();
            if(budget <= 0)
            {
                errMsg = "No data found";
                return 0;
            }
            return budget;
        }

        public List<MonthlyBudget> GetShopBudget(int year,string ShopID, out string errMsg)
        {
            errMsg = string.Empty;
            var shoplist = _dal.Select<ShopList>("SELECT * FROM ShopList", ref msg).ToList();
            var budgetList = _dal.Select<MonthlyBudget>("SELECT * FROM MonthlyBudget where Year='"+year+ "'", ref msg).ToList();
            if (shoplist.Count == 0)
            {
                errMsg = "Shop not configured yet";
                return null;
            }
            List<MonthlyBudget> lstBudget = new();
            foreach (var item in shoplist)
            {
                MonthlyBudget budget = new();
                budget.ShopID = item.ShopID;
                budget.ShopName = item.ShopName;
                var budgetShopList = budgetList.Where(a => a.ShopID == item.ShopID).ToList();
                if (budgetShopList.Count != 0)
                {
                    foreach (var data in budgetShopList)
                    {
                        budget.April = data.April;
                        budget.August = data.August;
                        budget.budgetID = data.budgetID;
                        budget.CreatedBy = data.CreatedBy;
                        budget.CreatedDate = data.CreatedDate;
                        budget.December = data.December;
                        budget.February = data.February;
                        budget.January = data.January;
                        budget.July = data.July;
                        budget.June = data.June;
                        budget.March = data.March;
                        budget.May = data.May;
                        budget.November = data.November;
                        budget.October = data.October;
                        budget.September = data.September;
                        budget.Year = data.Year;
                    }
                }
                lstBudget.Add(budget);
            }

            return lstBudget;
        }

        public bool SaveMonthlyBudget(MonthlyBudget budget, out string errMsg)
        {
            errMsg = string.Empty;
            CompositeModel composite = new();
            composite.AddRecordSet<MonthlyBudget>(budget, OperationMode.InsertOrUpdaet, "budgetID", "", "ShopID,Year", "");
            bool response = _dal.InsertUpdateComposite(composite, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
            }
            return response;
        }

        public List<YearList> YearList()
        {
            var start_year = DateTime.Now.Year;
            List<YearList> lstyear = new();
            for (var i = start_year; i < start_year + 5; i++)
            {
                YearList year = new();
                year.Year = i;
                lstyear.Add(year);
            }
            return lstyear;
        }
    }
}
