using FairPos.Epylion.Models.Report;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Report
{
    public interface IReportService
    {
        List<TopUpReprintDtos> getCollectionNOStaffLedger(DateTime fromDate, DateTime toDate, string rfID);
        List<string> GetPendingPrintedItems(DateTime fromDate, DateTime toDate, string rfId = "");
        List<SsummaryDtos> GetInvoiceNoSSummary(DateTime fromDate, DateTime toDate, string rfID);
        List<StyleSize> getItemList(string supid, string prdId);
        string getEmpName(string Invoice);
        DashBoardItems GetDashBoardItem();
    }

    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public List<TopUpReprintDtos> getCollectionNOStaffLedger(DateTime fromDate, DateTime toDate, string rfID)
        {
            return _repo.getCollectionNOStaffLedger(fromDate, toDate, rfID);
        }

        public DashBoardItems GetDashBoardItem()
        {
            return _repo.GetDashBoardItem();
        }

        public string getEmpName(string Invoice)
        {
            return _repo.getEmpName(Invoice);
        }

        public List<SsummaryDtos> GetInvoiceNoSSummary(DateTime fromDate, DateTime toDate, string rfID)
        {
            return _repo.GetInvoiceNoSSummary(fromDate, toDate, rfID);
        }

        public List<StyleSize> getItemList(string supid, string prdId)
        {
            return _repo.getItemList(supid,prdId);
        }

        public List<string> GetPendingPrintedItems(DateTime fromDate, DateTime toDate, string rfId = "")
        {
            return _repo.GetPendingPrintedItems(fromDate, toDate, rfId);
        }
    }
}
