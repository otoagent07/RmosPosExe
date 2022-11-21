using Pos.Core.Service;
using Pos.Entity.Param;
using Pos.Repository.Interfaces;
using Pos.Repository.Repo.Param;
using System.Collections.Generic;

namespace Pos.Service.Param
{
    public interface IPosParamService : IService<Pos_Param>
    {

    }
    public class PosParamService : IPosParamService
    {

        private readonly IPosParamRepository PosParamRepository;

        public PosParamService()
        {
            PosParamRepository = new PosParamRepository("");
        }
   
        public bool Delete(Pos_Param obj)
        {
            return PosParamRepository.Delete(obj);
        }

        public Pos_Param Get(object id)
        {
            return PosParamRepository.Get(id);
        }

        public IEnumerable<Pos_Param> GetAll()
        {
            return PosParamRepository.GetAll();
        }

        public long Insert(Pos_Param obj)
        {
            return PosParamRepository.Insert(obj);
        }

        public bool Update(Pos_Param obj)
        {
            return PosParamRepository.Update(obj);
        }
    }
}
