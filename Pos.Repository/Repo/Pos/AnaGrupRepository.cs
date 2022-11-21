using Pos.Core.Dapper;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using System.Collections.Generic;

namespace Pos.Repository.Repo.Pos
{
    public class AnaGrupRepository : IAnaGrupRepository
    {
        private IDapperTools dapper { get; set; }

        public AnaGrupRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }

        public AnaGrupEnt Get(object id)
        {
            return dapper.Get<AnaGrupEnt>(id);
        }

        public IEnumerable<AnaGrupEnt> GetAll()
        {
            return dapper.GetAll<AnaGrupEnt>();
        }

        public long Insert(AnaGrupEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(AnaGrupEnt obj)
        {
            return dapper.Update(obj);
        }

        public IEnumerable<AnaGrupEnt> GetAnaGrup(string DepartmanKodu)
        {
            return dapper.Query<AnaGrupEnt>(@"Exec Pos_Sorgu @Dep_Kodu ='" + DepartmanKodu + "', @Sorgu_Tipi = '0'");
        }

        public bool Delete(AnaGrupEnt obj)
        {
            return dapper.Delete(obj);
        }
    }
}
