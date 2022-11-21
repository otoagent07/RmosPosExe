using Pos.Core.Dapper;
using Pos.Entity.Param;
using Pos.Repository.Interfaces;
using System.Collections.Generic;

namespace Pos.Repository.Repo.Param
{
    public class PosParamRepository : IPosParamRepository
    {
        private IDapperTools dapper { get; set; }

        public PosParamRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }

        public bool Delete(Pos_Param obj)
        {
            return dapper.Delete(obj);
        }

        public Pos_Param Get(object id)
        {
            return dapper.Get<Pos_Param>(id);
        }

        public IEnumerable<Pos_Param> GetAll()
        {
            return dapper.GetAll<Pos_Param>();
        }

        public long Insert(Pos_Param obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(Pos_Param obj)
        {
            return dapper.Update(obj);
        }
    }
}
