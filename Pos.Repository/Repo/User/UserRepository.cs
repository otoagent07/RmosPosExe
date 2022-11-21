using Pos.Core.Dapper;
using Pos.Entity.User;
using Pos.Repository.Interfaces.User;
using System.Collections.Generic;
using System.Linq;

namespace Pos.Repository.Repo.User
{
    public class UserRepository : IUserRepository
    {
        private IDapperTools dapper { get; set; }

        public UserRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }
        public bool Delete(UserEnt obj)
        {
            return dapper.Delete(obj);
        }

        public UserEnt Get(object id)
        {
            return dapper.Get<UserEnt>(id);
        }

        public IEnumerable<UserEnt> GetAll()
        {
            return dapper.GetAll<UserEnt>();
        }

        public long Insert(UserEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(UserEnt obj)
        {
            return dapper.Update(obj);
        }

        public UserEnt UserLogin(string kod, string sifre)
        {
            return dapper.Query<UserEnt>("select * from RmosMuh..Pos_User WITH(NOLOCK) where P_Kod = @P_Kod and P_Sifre = @P_Sifre", new { @P_Kod = kod, @P_Sifre = sifre }).FirstOrDefault();
        }

        public UserEnt UserLogin(string kartno)
        {
            return dapper.Query<UserEnt>("select * from RmosMuh..Pos_User WITH(NOLOCK) where P_Kart = @P_Kart", new { @P_Kart = kartno }).FirstOrDefault();
        }
    }
}
