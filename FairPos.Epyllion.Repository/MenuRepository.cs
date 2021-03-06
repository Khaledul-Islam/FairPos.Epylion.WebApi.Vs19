using FairPos.Epylion.Models;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository
{
    public interface IMenuRepository
    {
        bool AddUpdate(List<UsersPermission> wb, ref string message);
        List<WebMenu> SelectAllChildByUser(string userName);
        List<WebMenu> SelectAllChildByUser(string parentID, string userName);
        List<WebMenu> SelectAllParentByUser(string userName);
        List<WebMenu> SelectAllChild();
        List<WebMenu> SelectAllChild(string parentID);
        List<WebMenu> SelectAllParent();
        List<WebMenu> SelectByUserID(string userType);
        List<WebMenu> SelectAll(string defaultValue);
    }
    public class MenuRepository : BaseRepository, IMenuRepository
    {

        IUsersWebRepository usersWebRepository;

        public MenuRepository(IDBConnectionProvider dBConnectionProvider, IUsersWebRepository _usersWebRepository) : base(dBConnectionProvider)
        {
            usersWebRepository = _usersWebRepository;
            baseQuery = @"select UsersMenuId,UserId,MenuId
                    FROM UsersPermission";
        }



        public List<WebMenu> SelectAll(string defaultValue)
        {

            List<WebMenu> oList = new List<WebMenu>();

            //parent WebMenu
            oList.Add(new WebMenu { ID = "1", MenuOwner = "All", ParentID = "", Text = "Home", URL = "/", URL_DIV = "", CLASS = "mdi mdi-bullseye", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "2", MenuOwner = "All", ParentID = "", Text = "Dashboard", URL = "#", URL_DIV = "", CLASS = "mdi mdi-gauge", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "3", MenuOwner = "Head Office", ParentID = "", Text = "Admin Panel", URL = "#", URL_DIV = "adminPages", CLASS = "mdi mdi-email", HasThirdLayer = false, SPAN_CLASS = "has-arrow", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4", MenuOwner = "Head Office", ParentID = "", Text = "Setup", URL = "#", URL_DIV = "Setup", CLASS = "mdi mdi-settings", HasThirdLayer = false, SPAN_CLASS = "has-arrow", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5", MenuOwner = "Shop", ParentID = "", Text = "Operations", URL = "#", URL_DIV = "operationPage", CLASS = "mdi mdi-file", SPAN_CLASS = "has-arrow", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "6", MenuOwner = "Head Office", ParentID = "", Text = "Requisition", URL = "#", URL_DIV = "requisitionPage", CLASS = "mdi mdi-file", SPAN_CLASS = "has-arrow", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "7", MenuOwner = "Shop", ParentID = "", Text = "Sales Worker", URL = "#", URL_DIV = "salesworkerpage", CLASS = "mdi mdi-cash", SPAN_CLASS = "has-arrow", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "8", MenuOwner = "Shop", ParentID = "", Text = "Sales Staff", URL = "#", URL_DIV = "salesstaffpage", CLASS = "mdi mdi-cash", SPAN_CLASS = "has-arrow", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "9", MenuOwner = "Shop", ParentID = "", Text = "Transfer", URL = "#", URL_DIV = "transferPage", CLASS = "mdi mdi-sync", SPAN_CLASS = "has-arrow", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10", MenuOwner = "Head Office", ParentID = "", Text = "Reports", URL = "#", URL_DIV = "reportpage", CLASS = "mdi mdi-file", SPAN_CLASS = "has-arrow", HasThirdLayer = false, DefaultActionName = "" });

            //Admin Area Child
            oList.Add(new WebMenu { ID = "3-1", ParentID = "3", Text = "User register", URL = "/user-management", URL_DIV = "", CLASS = "fas fa-chess-board", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "User_SelectAll_Grid" });
            oList.Add(new WebMenu { ID = "3-2", ParentID = "3", Text = "User Permission", URL = "/userpermission", URL_DIV = "", CLASS = "fas fa-chess-board", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "SelectAllParent" });
            //oList.Add(new WebMenu { ID = "2-3", ParentID = "2", Text = "Global Setup", URL = "#/globalsetup", URL_DIV = "", CLASS = "", SPAN_CLASS = "fa fa-adjust", DefaultActionName = "GlobalSetupGetDetails" });

            //Setup
            oList.Add(new WebMenu { ID = "4-1", ParentID = "4", Text = "Product List", URL = "/setups/product-list", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-2", ParentID = "4", Text = "Supplier List", URL = "/setups/supplier-list", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-3!", ParentID = "4", Text = "Measure Unit", URL = "/setups/measureUnit", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-4!", ParentID = "4", Text = "Item List", URL = "/setups/items", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-5!", ParentID = "4", Text = "Employee List", URL = "/setups/employeesync", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-6!", ParentID = "4", Text = "Employee Item", URL = "/setups/employee-item", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-7!", ParentID = "4", Text = "Family Category", URL = "/setups/family-category", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-8!", ParentID = "4", Text = "Family Categ. Limit", URL = "/setups/family-category-limit", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-9", ParentID = "4", Text = "Shop List", URL = "/setups/shop-list", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-10", ParentID = "4", Text = "Item Approve", URL = "/setups/items-approve", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-11", ParentID = "4", Text = "Software Setting", URL = "/setups/software-setting", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "4-12", ParentID = "4", Text = "Top-UP", URL = "/setups/managementstaff-topup", URL_DIV = "", CLASS = "fas fa-barcode", SPAN_CLASS = "", HasThirdLayer = false, DefaultActionName = "" });

            //Operation
            oList.Add(new WebMenu { ID = "5-1", ParentID = "5", Text = "Purchase Order", URL = "/operations/purchase-order", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-2", ParentID = "5", Text = "Arrival", URL = "/operations/arrival", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-3", ParentID = "5", Text = "Quality Control", URL = "/operations/quality-control", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-4", ParentID = "5", Text = "Conversion Item", URL = "/operations/conversion-item", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-5", ParentID = "5", Text = "Damage Loss", URL = "/operations/damage-loss", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-6", ParentID = "5", Text = "Return", URL = "/operations/return", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-7", ParentID = "5", Text = "Arrival Edit", URL = "/operations/arrival-update", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-8", ParentID = "5", Text = "Inventory", URL = "/operations/inventory", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-9", ParentID = "5", Text = "Purchase Order Edit", URL = "/operations/purchase-order-update", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-10", ParentID = "5", Text = "Price Change", URL = "/operations/price-change", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "5-11", ParentID = "5", Text = "Price Change Approve", URL = "/operations/circular-price-change-approve", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });
            //oList.Add(new WebMenu { ID = "5-12", ParentID = "5", Text = "Discount", URL = "/operations/discount", URL_DIV = "", CLASS = "fas fa-braille", SPAN_CLASS = "", DefaultActionName = "" });

            //Requisition
            oList.Add(new WebMenu { ID = "6-1", ParentID = "6", Text = "Monthly Budget", URL = "/requisition/monthly-budget", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "6-2", ParentID = "6", Text = "Auto Requisition", URL = "/requisition/auto-requisition", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "6-3", ParentID = "6", Text = "Requisition Approval", URL = "/requisition/requisition-approval", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "6-4", ParentID = "6", Text = "Requisition to PO", URL = "/requisition/requisition-to-purchase-order", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "6-5", ParentID = "6", Text = "Requisition PO Approval", URL = "/requisition/requisition-to-purchase-order-approval", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
           // oList.Add(new WebMenu { ID = "6-6", ParentID = "6", Text = "Requisition to PO/ALL", URL = "/requisition/requisition-to-purchase-order-all", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });

            //sales-worker
            oList.Add(new WebMenu { ID = "7-1", ParentID = "7", Text = "Sales Order", URL = "/sales/sales-order", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "7-2", ParentID = "7", Text = "Sales Order Print", URL = "/sales/sales-order-print", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "7-3", ParentID = "7", Text = "Sales Order Delivery", URL = "/sales/sales-order-delivery", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "7-4", ParentID = "7", Text = "Void Sale", URL = "/sales/void-sales", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });

            //sales-staff
            oList.Add(new WebMenu { ID = "8-1", ParentID = "8", Text = "Sales Order", URL = "/sales/sales-order-staff", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "8-2", ParentID = "8", Text = "Sales Order Print", URL = "/sales/sales-order-print-staff", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "8-3", ParentID = "8", Text = "Sales Order Delivery", URL = "/sales/sales-order-delivery-staff", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "8-4", ParentID = "8", Text = "Void Sale", URL = "/sales/void-sales-staff", URL_DIV = "", CLASS = "fas fa-money", SPAN_CLASS = "", DefaultActionName = "" });


            //transfer
            oList.Add(new WebMenu { ID = "9-1", ParentID = "9", Text = "ShopToShop", URL = "/transfer/shop-to-shop", URL_DIV = "", CLASS = "fas fa-sync", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "9-2", ParentID = "9", Text = "MainToStaffWorker", URL = "/transfer/mainstock-to-staff-worker", URL_DIV = "", CLASS = "fas fa-sync", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "9-3", ParentID = "9", Text = "WorkerToMainStaff", URL = "/transfer/worker-to-main-staff", URL_DIV = "", CLASS = "fas fa-sync", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "9-4", ParentID = "9", Text = "StaffToMainWorker", URL = "/transfer/staff-to-main-worker", URL_DIV = "", CLASS = "fas fa-sync", SPAN_CLASS = "", DefaultActionName = "" });


            //report
            oList.Add(new WebMenu { ID = "10-1", ParentID = "10", Text = "Invoice Reprint", URL = "/report/invoice-reprint", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-2", ParentID = "10", Text = "Sales Order Reprint", URL = "/report/sales-order-reprint", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-3", ParentID = "10", Text = "Sales Report", URL = "/report/sales-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-4", ParentID = "10", Text = "Stock Report", URL = "/report/stock-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-5", ParentID = "10", Text = "Purchase Order", URL = "/report/purchase-order-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-6", ParentID = "10", Text = "QC Report", URL = "/report/QC-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-7", ParentID = "10", Text = "Damage Report", URL = "/report/damage-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-8", ParentID = "10", Text = "Supplier Return", URL = "/report/supplier-return-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-9", ParentID = "10", Text = "ReOrder Item Report", URL = "/report/re-order-item-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-10", ParentID = "10", Text = "Arrival Report", URL = "/report/arrival-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-11", ParentID = "10", Text = "Inventory Report", URL = "/report/inventory-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-12", ParentID = "10", Text = "Auto Requisition", URL = "/report/auto-requisition-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-13", ParentID = "10", Text = "Stock Transfer", URL = "/report/stock-transfer-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-14", ParentID = "10", Text = "TopUP Reprint", URL = "/report/topup-reprint", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-15", ParentID = "10", Text = "TopUP Report", URL = "/report/topup-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
            oList.Add(new WebMenu { ID = "10-16", ParentID = "10", Text = "Staff Balance Report", URL = "/report/staff-balance-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });
           // oList.Add(new WebMenu { ID = "10-17", ParentID = "10", Text = "Staff Email Report", URL = "/report/staff-email-report", URL_DIV = "", CLASS = "fas fa-file", SPAN_CLASS = "", DefaultActionName = "" });



            return oList;

        }

        public List<WebMenu> SelectByUserID(string userType)
        {
            List<WebMenu> ListAll = SelectAll("");

            //var data = _repository.GetMany(m => m.UserName == userType).FirstOrDefault();

            whereCluase = " where UserId = '" + userType + "' ";
            query = string.Format("{0} {1}", baseQuery, whereCluase);

            var data = _dal.Select<UsersPermission>(query, ref msg);

            //Query = "SELECT userName,MenusIDs FROM WebUserMenu WHERE userName='" + UserName + "'";

            //oR = sQLDal.Select(Query);
            List<WebMenu> oList = new List<WebMenu>();
            if (data != null)
            {
               
                foreach (var s in data)
                {
                    if (string.IsNullOrEmpty(s.MenuId))
                        continue;

                    var dataF = ListAll.Find(m => m.ID == s.MenuId);
                    if (dataF != null)
                        oList.Add(dataF);
                }

                return oList;
            }
            else return null;
        }


        public List<WebMenu> SelectAllParent()
        {
            List<WebMenu> ListAll = SelectAll("").FindAll(m => m.ParentID == "");
            return ListAll;
        }


        public List<WebMenu> SelectAllChild(string parentID)
        {
            List<WebMenu> ListAll = SelectAll("").FindAll(m => m.ParentID == parentID);
            return ListAll;
        }

        public List<WebMenu> SelectAllChild()
        {
            List<WebMenu> ListAll = SelectAll("").FindAll(m => m.ParentID != "");
            return ListAll;
        }


      



        public List<WebMenu> SelectAllParentByUser(string userName)
        {
            List<WebMenu> ListAll = SelectAll("").FindAll(m => m.ParentID == "");

            List<WebMenu> filterList = new List<WebMenu>();


            whereCluase = " where UserId = '" + userName + "' ";
            query = string.Format("{0} {1}", baseQuery, whereCluase);

            var data = _dal.Select<UsersPermission>(query, ref msg);

            if (data == null)
                return null;

            foreach (var s in data)
            {
                WebMenu ms = ListAll.Find(m => m.ID == s.MenuId);
                if (ms != null)
                    filterList.Add(ms);
            }

            return filterList;
        }


        public List<WebMenu> SelectAllChildByUser(string parentID, string userName)
        {
            List<WebMenu> ListAll = SelectAll("").FindAll(m => m.ParentID == parentID);


            List<WebMenu> filterList = new List<WebMenu>();


            whereCluase = " where UserId = '" + userName + "' ";
            query = string.Format("{0} {1}", baseQuery, whereCluase);

            var data = _dal.Select<UsersPermission>(query, ref msg);

            if (data == null)
                return null;

            foreach (var s in data)
            {
                WebMenu ms = ListAll.Find(m => m.ID == s.MenuId);
                if (ms != null)
                    filterList.Add(ms);
            }

            return filterList;
        }


        public List<WebMenu> SelectAllChildByUser(string userName)
        {
            List<WebMenu> ListAll = SelectAll("");


            List<WebMenu> filterList = new List<WebMenu>();

            whereCluase = " where UserId = '" + userName + "' ";
            query = string.Format("{0} {1}", baseQuery, whereCluase);

            var data = _dal.Select<UsersPermission>(query, ref msg);

            if (data == null)
                return null;

            foreach (var s in data)
            {
                WebMenu ms = ListAll.Find(m => m.ID == s.MenuId);
                if (ms != null)
                    filterList.Add(ms);
            }

            return filterList;
        }


        public bool AddUpdate(List<UsersPermission> wb, ref string message)
        {
            //List<WebMenu> objects
            //string menuID = "";
            //foreach (WebMenu m in objects)
            //{
            //    menuID += m.ID + ",";
            //}


            CompositeModel compositeModel = new CompositeModel();

            //UserMenus wb = new UserMenus();
            //wb.MenuIds = menuID;
            //wb.UserName = userType;

            List<WebMenu> webMenus = SelectAllParentByUser(wb[0].UserId);

            if(webMenus.Count > 0)
                compositeModel.AddRecordSet<UsersPermission>(wb, OperationMode.Delete, "", "", "UserId", "");

            compositeModel.AddRecordSet<UsersPermission>(wb, OperationMode.Insert, "UsersMenuId", "UserId,MenuId", "", "");

            return _dal.InsertUpdateComposite(compositeModel, ref message);


        }




    }
}
