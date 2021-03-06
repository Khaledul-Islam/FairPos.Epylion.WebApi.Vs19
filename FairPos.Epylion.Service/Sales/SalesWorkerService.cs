using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Sales
{
    public interface ISalesWorkerService
    {
        List<TempSalesOrder> TempSalesOrderList(string UserId, string CounterId, string stocktype);
        TempSalesOrder TempSalesOrder(string UserId, string CounterId, string stocktype);
        Employee GetEmployeeById(string empId);
        Employee GetEmployeeByRFId(string rfid);
        List<Ssummary> GetInvoiceByEmployee(string empId);
        List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "All");
        bool TempSalesOrderSave(Buy buy, out string errMsg);
        bool TempSalesOrderDelete(TempSalesOrder temp, out string errMsg);
        string SalesOrderSave(List<TempSalesOrder> olist, out string errMsg);
    }
    public class SalesWorkerService : ISalesWorkerService
    {
        private readonly ISalesWorkerRepository _repo;

        public SalesWorkerService(ISalesWorkerRepository repo)
        {
            _repo = repo;
        }

        public Employee GetEmployeeById(string empId)
        {
            return _repo.GetEmployeeById(empId);
        }

        public Employee GetEmployeeByRFId(string rfid)
        {
            return _repo.GetEmployeeByRFId(rfid);
        }

        public List<Ssummary> GetInvoiceByEmployee(string empId)
        {
            return _repo.GetInvoiceByEmployee( empId);
        }

        public List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "All")
        {
            return _repo.GetProductsByOrgBarcode(PrdId, isconversionItem);
        }

        public string SalesOrderSave(List<TempSalesOrder> olist, out string errMsg)
        {
           return _repo.SalesOrderSave(olist,out errMsg);
        }

        public TempSalesOrder TempSalesOrder(string UserId, string CounterId, string stocktype)
        {
            return _repo.TempSalesOrder(UserId, CounterId, stocktype);
        }

        public bool TempSalesOrderDelete(TempSalesOrder temp, out string errMsg)
        {
            return _repo.TempSalesOrderDelete(temp, out errMsg);
        }

        public List<TempSalesOrder> TempSalesOrderList(string UserId, string CounterId, string stocktype)
        {
            return _repo.TempSalesOrderList(UserId, CounterId, stocktype);
        }

        public bool TempSalesOrderSave(Buy buy, out string errMsg)
        {
            return _repo.TempSalesOrderSave(buy, out errMsg);
        }
    }
}
