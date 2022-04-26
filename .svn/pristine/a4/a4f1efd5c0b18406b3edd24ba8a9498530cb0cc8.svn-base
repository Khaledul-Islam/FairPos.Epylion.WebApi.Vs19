using FairPos.Epylion.Models;
using FairPos.Epyllion.Repository;
using System.Collections.Generic;

namespace FairPos.Epylion.Service
{
    public interface IUserCounterService
    {
        List<UserCounter> SelectAll();
    }
    public class UserCounterService : IUserCounterService
    {
        IUserCounterRepository repository;

        public UserCounterService(IUserCounterRepository _repo)
        {
            repository = _repo;
        }
        public List<UserCounter> SelectAll()
        {
            return repository.SelectAll();
        }
    }
}
