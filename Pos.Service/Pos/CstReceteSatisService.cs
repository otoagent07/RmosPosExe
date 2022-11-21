using Pos.Core.Service;
using Pos.Entity.Pos;
using Pos.Repository.Repo.Pos;
using System.Collections.Generic;

namespace Pos.Service.Pos
{
    public interface ICstReceteSatisService : IService<CstReceteSatisEnt>
    {
        IEnumerable<CstReceteSatisEnt> GetReceteList(string Fisno);
    }

    public class CstReceteSatisService : ICstReceteSatisService
    {
        private readonly CstReceteSatisRepository cstReceteSatisRepository;

        public CstReceteSatisService()
        {
            cstReceteSatisRepository = new CstReceteSatisRepository("");
        }

        public bool Delete(CstReceteSatisEnt obj)
        {
            return cstReceteSatisRepository.Delete(obj);
        }  

        public CstReceteSatisEnt Get(object id)
        {
            return cstReceteSatisRepository.Get(id);
        }

        public IEnumerable<CstReceteSatisEnt> GetAll()
        {
            return cstReceteSatisRepository.GetAll();
        }

        public IEnumerable<CstReceteSatisEnt> GetReceteList(string Fisno)
        {
            return cstReceteSatisRepository.GetReceteList(Fisno);
        }

        public long Insert(CstReceteSatisEnt obj)
        {
            return cstReceteSatisRepository.Insert(obj);
        }

        public bool Update(CstReceteSatisEnt obj)
        {
            return cstReceteSatisRepository.Update(obj);
        }


    }
}
