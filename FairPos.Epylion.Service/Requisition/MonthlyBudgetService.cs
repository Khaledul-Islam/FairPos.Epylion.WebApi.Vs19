using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Requisition;
using FairPos.Epyllion.Repository.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Requisition
{
    public interface IMonthlyBudgetService
    {
        bool SaveMonthlyBudget(MonthlyBudget budget, out string errMsg);
        List<MonthlyBudget> GetShopBudget(int year, string ShopID, out string errMsg);
        List<YearList> YearList();
        decimal GetCurrentMonthShopBudget(int year, string month, string ShopID, out string errMsg);
    }

    public class MonthlyBudgetService : IMonthlyBudgetService
    {
        private readonly IMonthlyBudgetRepository _repo;

        public MonthlyBudgetService(IMonthlyBudgetRepository repo)
        {
            _repo = repo;
        }

        public decimal GetCurrentMonthShopBudget(int year, string month, string ShopID, out string errMsg)
        {
            return _repo.GetCurrentMonthShopBudget(year, month, ShopID, out errMsg);
        }

        public List<MonthlyBudget> GetShopBudget(int year, string ShopID, out string errMsg)
        {
            return _repo.GetShopBudget(year,  ShopID, out errMsg);
        }

        public bool SaveMonthlyBudget(MonthlyBudget budget, out string errMsg)
        {
            return _repo.SaveMonthlyBudget(budget, out errMsg);
        }

        public List<YearList> YearList()
        {
            return _repo.YearList();
        }
    }
}
