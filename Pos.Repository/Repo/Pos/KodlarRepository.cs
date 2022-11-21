using Pos.Core.Dapper;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using System.Collections.Generic;

namespace Pos.Repository.Repo.Pos
{
    public class KodlarRepository : IKodlarRepository
    {
        private IDapperTools dapper { get; set; }

        public KodlarRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }

        public KodlarEnt Get(object id)
        {
            return dapper.Get<KodlarEnt>(id);
        }

        public IEnumerable<KodlarEnt> GetAll()
        {
            return dapper.GetAll<KodlarEnt>();
        }

        public long Insert(KodlarEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(KodlarEnt obj)
        {
            return dapper.Update(obj);
        }

        public IEnumerable<KodlarEnt> GetKodlarSinif(string Sinif)
        {
            return dapper.Query<KodlarEnt>(@"Select * From Pos_Kodlar Pkod_Sinif = @Pkod_Sinif", new { @Pkod_Sinif = Sinif });
        }

        public bool Delete(KodlarEnt obj)
        {
            return dapper.Delete(obj);
        }
    }
}
