using FairPos.Epylion.Models;
using FairPos.Epyllion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service
{

    public interface IMenuService
    {

        List<WebMenu> SelectAll(string name);
        List<WebMenu> SelectAllChild(string ParentId);
        List<WebMenu> SelectAllChild();
        List<WebMenu> SelectAllChildByUser(string parentId, string username);
        List<WebMenu> SelectAllChildByUser(string username);
        List<WebMenu> SelectAllParent();
        List<WebMenu> SelectAllParentByUser(string userName);
        List<WebMenu> SelectByUserID(string userName);
        bool AddUpdate(List<UsersPermission> wb, ref string msg);
    }

    public class MenuService : IMenuService
    {
        IMenuRepository repository;

        public MenuService(IMenuRepository _repository)
        {
            repository = _repository;
        }


        public List<WebMenu> SelectAll(string name)
        {
            return repository.SelectAll(name);
        }

        public List<WebMenu> SelectAllChild(string ParentId)
        {
            return repository.SelectAllChild(ParentId);
        }

        public List<WebMenu> SelectAllChild()
        {
            return repository.SelectAllChild();
        }


        public List<WebMenu> SelectAllChildByUser(string parentId, string username)
        {
            return repository.SelectAllChildByUser(parentId, username);
        }

        public List<WebMenu> SelectAllChildByUser(string username)
        {
            return repository.SelectAllChildByUser(username);
        }

        public List<WebMenu> SelectAllParent()
        {
            return repository.SelectAllParent();
        }

        public List<WebMenu> SelectAllParentByUser(string userName)
        {
            return repository.SelectAllParentByUser(userName);
        }

        public List<WebMenu> SelectByUserID(string userName)
        {
            return repository.SelectByUserID(userName);
        }
        

        public bool AddUpdate(List<UsersPermission> wb, ref string msg)
        {
            return repository.AddUpdate(wb, ref msg);
        }

    }
}
