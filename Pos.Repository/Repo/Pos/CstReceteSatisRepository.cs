using Pos.Core.Dapper;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using System.Collections.Generic;

namespace Pos.Repository.Repo.Pos
{
    public class CstReceteSatisRepository : ICstReceteSatisRepository
    {
        private IDapperTools dapper { get; set; }

        public CstReceteSatisRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }
        public bool Delete(CstReceteSatisEnt obj)
        {
            return dapper.Delete(obj);
        }

        public CstReceteSatisEnt Get(object id)
        {
            return dapper.Get<CstReceteSatisEnt>(id);
        }

        public IEnumerable<CstReceteSatisEnt> GetAll()
        {
            return dapper.GetAll<CstReceteSatisEnt>();
        }

        public long Insert(CstReceteSatisEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(CstReceteSatisEnt obj)
        {
            return dapper.Update(obj);
        }

        public IEnumerable<CstReceteSatisEnt> GetReceteList(string Fisno)
        {
            return dapper.Query<CstReceteSatisEnt>(@"Exec Pos_Satis @Fisno ='" + Fisno + "', @Rapor_Tipi = '0'");
        }
    }
}
