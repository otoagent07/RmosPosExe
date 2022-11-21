using Pos.Core.Service;
using Pos.Entity.Pos;
using Pos.Repository.Repo.Pos;
using System.Collections.Generic;

namespace Pos.Service.Pos
{
    public interface IAraGrupService : IService<AraGrupEnt>
    {
        IEnumerable<AraGrupEnt> GetAraGrup(string DepartmanKodu, string AnaGrup);
    }
    public class AraGrupService : IAraGrupService
    {
        private readonly AraGrupRepository araGrupRepository;

        public AraGrupService()
        {
            araGrupRepository = new AraGrupRepository("");
        }

        public bool Delete(AraGrupEnt obj)
        {
            return araGrupRepository.Delete(obj);
        }

        public AraGrupEnt Get(object id)
        {
            return araGrupRepository.Get(id);
        }

        public IEnumerable<AraGrupEnt> GetAll()
        {
            return araGrupRepository.GetAll();
        }

        public IEnumerable<AraGrupEnt> GetAraGrup(string DepartmanKodu, string AnaGrup)
        {
            return araGrupRepository.GetAraGrup(DepartmanKodu, AnaGrup);
        }

        public long Insert(AraGrupEnt obj)
        {
            return araGrupRepository.Insert(obj);
        }

        public bool Update(AraGrupEnt obj)
        {
            return araGrupRepository.Update(obj);
        }
    }
}
