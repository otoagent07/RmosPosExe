using Pos.Core.Service;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using Pos.Repository.Repo.Pos;
using System.Collections.Generic;

namespace Pos.Service.Pos
{
    public interface IMasaKonumService : IService<MasaKonumEnt>
    {
        IEnumerable<MasaKonumEnt> GetDepartmanKonum(string depKodu);
    }
    public class MasaKonumService : IMasaKonumService
    {
        private readonly IMasaKonumRepository masaKonumRepository;

        public MasaKonumService()
        {
            masaKonumRepository = new MasaKonumRepository("");
        }

        public bool Delete(MasaKonumEnt obj)
        {
            return masaKonumRepository.Delete(obj);
        }

        public MasaKonumEnt Get(object id)
        {
            return masaKonumRepository.Get(id);
        }

        public IEnumerable<MasaKonumEnt> GetAll()
        {
            return masaKonumRepository.GetAll();
        }

        public IEnumerable<MasaKonumEnt> GetDepartmanKonum(string depKodu)
        {
            return masaKonumRepository.GetDepartmanKonum(depKodu);
        }
        public long Insert(MasaKonumEnt obj)
        {
            return masaKonumRepository.Insert(obj);
        }

        public bool Update(MasaKonumEnt obj)
        {
            return masaKonumRepository.Update(obj);
        }
    }
}
