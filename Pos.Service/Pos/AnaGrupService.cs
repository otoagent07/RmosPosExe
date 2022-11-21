using Pos.Core.Service;
using Pos.Entity.Pos;
using Pos.Repository.Repo.Pos;
using System.Collections.Generic;

namespace Pos.Service.Pos
{
    public interface IAnaGrupService : IService<AnaGrupEnt>
    {
        IEnumerable<AnaGrupEnt> GetAnaGrup(string DepartmanKodu);
    }
    public class AnaGrupService : IAnaGrupService
    {
        private readonly AnaGrupRepository anaGrupRepository;

        public AnaGrupService()
        {
            anaGrupRepository = new AnaGrupRepository("");
        }

        public bool Delete(AnaGrupEnt obj)
        {
            return anaGrupRepository.Delete(obj);
        }

        public AnaGrupEnt Get(object id)
        {
            return anaGrupRepository.Get(id);
        }

        public IEnumerable<AnaGrupEnt> GetAll()
        {
            return anaGrupRepository.GetAll();
        }

        public IEnumerable<AnaGrupEnt> GetAnaGrup(string DepartmanKodu)
        {
            return anaGrupRepository.GetAnaGrup(DepartmanKodu);
        }

        public long Insert(AnaGrupEnt obj)
        {
            return anaGrupRepository.Insert(obj);
        }

        public bool Update(AnaGrupEnt obj)
        {
            return anaGrupRepository.Update(obj);
        }
    }
}
