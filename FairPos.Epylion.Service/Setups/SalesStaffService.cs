using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface ISalesStaffService
    {
        bool saveManagementStaffTopUp(TopUp model);
        List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "N");
        bool TempSalesOrderSave(Buy buy, out string errMsg);
        string SalesOrderSave(List<TempSalesOrder> olist, out string errMsg);
    }

    public class SalesStaffService : ISalesStaffService
    {
        private readonly ISalesStaffRepository _service;

        public SalesStaffService(ISalesStaffRepository service)
        {
            _service = service;
        }


        public List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "N")
        {
            return _service.GetProductsByOrgBarcode(PrdId, isconversionItem);
        }

        public string SalesOrderSave(List<TempSalesOrder> olist, out string errMsg)
        {
            return _service.SalesOrderSave(olist, out errMsg);
        }

        public bool saveManagementStaffTopUp(TopUp model)
        {
            return _service.saveManagementStaffTopUp(model);
        }

        public bool TempSalesOrderSave(Buy buy, out string errMsg)
        {
            return _service.TempSalesOrderSave(buy, out errMsg);
        }
    }
}
