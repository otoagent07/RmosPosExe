using Pos.Core.Service;
using Pos.Entity.Pos;
using Pos.Repository.Repo.Pos;
using System.Collections.Generic;

namespace Pos.Service.Pos
{
    public interface IKodlarService : IService<KodlarEnt>
    {
        IEnumerable<KodlarEnt> GetKodlarSinif(string Sinif);
    }
    public class KodlarService : IKodlarService
    {
        private readonly KodlarRepository kodlarRepository;

        public KodlarService()
        {
            kodlarRepository = new KodlarRepository("");
        }

        public bool Delete(KodlarEnt obj)
        {
            return kodlarRepository.Delete(obj);
        }

        public KodlarEnt Get(object id)
        {
            return kodlarRepository.Get(id);
        }

        public IEnumerable<KodlarEnt> GetAll()
        {
            return kodlarRepository.GetAll();
        }

        public IEnumerable<KodlarEnt> GetKodlarSinif(string Sinif)
        {
            return kodlarRepository.GetKodlarSinif(Sinif);
        }

        public long Insert(KodlarEnt obj)
        {
            return kodlarRepository.Insert(obj);
        }

        public bool Update(KodlarEnt obj)
        {
            return kodlarRepository.Update(obj);
        }
    }
}
