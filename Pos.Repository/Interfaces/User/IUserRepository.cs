using Pos.Core.Repository;
using Pos.Entity.User;

namespace Pos.Repository.Interfaces.User
{
    public interface IUserRepository : IRepository<UserEnt>
    {
        UserEnt UserLogin(string kod, string sifre);
        UserEnt UserLogin(string kartno);
    }
}
