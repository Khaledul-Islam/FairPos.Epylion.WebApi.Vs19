using FairPos.Epylion.Models.Sales;
using FairPos.Epyllion.Repository.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Sales
{
    public interface ISalesOrderPrintService
    {
        List<string> PendingSalesOrder();
        List<string> PendingSalesOrderStaff();
        bool IS_SO_PRINTED(string so_no);
        string SalesOrderDeliverySave(List<SALES_ORDER> model);
        string SalesOrderDeliverySaveStaff(List<SALES_ORDER> oList);
    }

    public class SalesOrderPrintService : ISalesOrderPrintService
    {
        private readonly ISalesOrderPrintRepository _repo;

        public SalesOrderPrintService(ISalesOrderPrintRepository repo)
        {
            _repo = repo;
        }

        public bool IS_SO_PRINTED(string so_no)
        {
            return _repo.IS_SO_PRINTED(so_no);
        }

        public List<string> PendingSalesOrder()
        {
            return _repo.PendingSalesOrder();
        }
        public List<string> PendingSalesOrderStaff()
        {
            return _repo.PendingSalesOrderStaff();
        }

        public string SalesOrderDeliverySaveStaff(List<SALES_ORDER> oList)
        {
            return _repo.SalesOrderDeliverySaveStaff(oList);
        }

        public string SalesOrderDeliverySave(List<SALES_ORDER> model)
        {
            return _repo.SalesOrderDeliverySave(model);
        }
    }
}
