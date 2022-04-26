using Dapper;
using FairPos.Epylion.Models.Report;
using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Report
{
    public interface IReportRepository
    {
        List<TopUpReprintDtos> getCollectionNOStaffLedger(DateTime fromDate, DateTime toDate, string rfID);
        List<SsummaryDtos> GetInvoiceNoSSummary(DateTime fromDate, DateTime toDate, string rfID);
        List<string> GetPendingPrintedItems(DateTime fromDate, DateTime toDate, string rfId = "");
        List<StyleSize> getItemList(string supid, string prdId);
        string getEmpName(string Invoice);
        DashBoardItems GetDashBoardItem();
    }

    public class ReportRepository : BaseRepository, IReportRepository
    {
        public ReportRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

        }
        public List<TopUpReprintDtos> getCollectionNOStaffLedger(DateTime fromDate, DateTime toDate, string rfID)
        {
            string query = @"select s.CollectionNo,s.EmpID,e.RFCardNo,s.CreatedDate,e.Name from StaffLedger s INNER JOIN Employee e on e.EmpID=s.EmpID  
                                where  CreatedDate BETWEEN '" + fromDate.ToShortDateString() + "' AND  '" + toDate.ToShortDateString() + "' AND e.RFCardNo='" + rfID + "'";
            var response = _dal.Select<TopUpReprintDtos>(query, ref msg);
            return response;
        }
        public List<SsummaryDtos> GetInvoiceNoSSummary(DateTime fromDate, DateTime toDate, string rfID)
        {
            string query = @"select s.Invoice,s.EmpID,e.RFCardNo,s.SaleDT,e.Name from Ssummary s INNER JOIN Employee e on e.EmpID=s.EmpID  
                                where  SaleDT BETWEEN '" + fromDate.ToShortDateString() + "' AND  '" + toDate.ToShortDateString() + "' AND (( (RFCardNo='" + rfID + "') and ('" + rfID + "' <>'')) OR ('" + rfID + "'='') ) ";
            var response = _dal.Select<SsummaryDtos>(query, ref msg);
            return response;
        }

        public List<StyleSize> getItemList(string supid, string prdId)
        {
            string query = @"select * from StyleSize where SupID='"+supid+ "' and PrdID='"+prdId+"'";
            return _dal.Select<StyleSize>(query, ref msg);
        }

        public List<string> GetPendingPrintedItems(DateTime fromDate, DateTime toDate, string rfId = "")
        {
            string query = @"SELECT DISTINCT SO_NO FROM dbo.SALES_ORDER inner join Employee on SALES_ORDER.EmpId=Employee.EmpId
	                        WHERE IS_Print=1 AND IsCanceld=0  AND IS_APPROVED=0 
                            and CREATE_DATE BETWEEN '" + fromDate.ToShortDateString() + "' AND '" + toDate.ToShortDateString() + "' and  (( (RFCardNo='" + rfId + "') and ('" + rfId + "' <>'')) OR ('" + rfId + "'='') ) ";

            return _dapper.Query<string>(query).ToList();

        }
        public string getEmpName(string Invoice)
        {
            string query = @"select distinct e.Name from Ssummary s INNER JOIN Employee e ON s.EmpID=e.EmpID where s.Invoice='"+ Invoice + "'";

            return _dapper.Query<string>(query).FirstOrDefault();

        }

        public DashBoardItems GetDashBoardItem()
        {
            string query = @"SP_GetDashboardItems";
            var result = _dapper.Query<DashBoardItems>(query).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            return new DashBoardItems();
        }
    }
}
