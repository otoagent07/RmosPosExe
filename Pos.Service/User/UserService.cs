using Pos.Core.Service;
using Pos.Entity.User;
using Pos.Repository.Repo.User;
using System.Collections.Generic;

namespace Pos.Service.User
{
    public interface IUserService : IService<UserEnt>
    {
        UserEnt UserLogin(string kod, string sifre);
        UserEnt UserLogin(string kartno);
    }
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository;

        public UserService()
        {
            userRepository = new UserRepository("");
        }

        public bool Delete(UserEnt obj)
        {
            return userRepository.Delete(obj);
        }

        public UserEnt Get(object id)
        {
            return userRepository.Get(id);
        }

        public IEnumerable<UserEnt> GetAll()
        {
            return userRepository.GetAll();
        }

        public long Insert(UserEnt obj)
        {
            return userRepository.Insert(obj);
        }


        public bool Update(UserEnt obj)
        {
            return userRepository.Update(obj);
        }

        public UserEnt UserLogin(string kod, string sifre)
        {
            return userRepository.UserLogin(kod, sifre);
        }

        public UserEnt UserLogin(string kartno)
        {
            return userRepository.UserLogin(kartno);
        }
    }
}
