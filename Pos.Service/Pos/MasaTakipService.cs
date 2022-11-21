using Pos.Core.Service;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using Pos.Repository.Repo.Pos;
using System;
using System.Collections.Generic;

namespace Pos.Service.Pos
{
    public interface IMasaTakipService : IService<MasaTakipEnt>
    {
        IEnumerable<MasaTakipEnt> GetDepartmanMasa(string depKodu, DateTime Tarih, string KonumFilter, string paketFilter, string Masalar, string siralama);
    }
    public class MasaTakipService : IMasaTakipService
    {
        private readonly IMasaTakipRepository MasaTakipRepository;

        public MasaTakipService()
        {
            MasaTakipRepository = new MasaTakipRepository("");
        }

        public bool Delete(MasaTakipEnt obj)
        {
            return MasaTakipRepository.Delete(obj);
        }

        public MasaTakipEnt Get(object id)
        {
            return MasaTakipRepository.Get(id);
        }

        public IEnumerable<MasaTakipEnt> GetAll()
        {
            return MasaTakipRepository.GetAll();
        }

        public IEnumerable<MasaTakipEnt> GetDepartmanMasa(string depKodu, DateTime Tarih, string KonumFilter, string paketFilter, string Masalar, string siralama)
        {
            return MasaTakipRepository.GetDepartmanMasa(depKodu, Tarih, KonumFilter, paketFilter, Masalar, siralama);
        }

        public long Insert(MasaTakipEnt obj)
        {
            return MasaTakipRepository.Insert(obj);
        }

        public bool Update(MasaTakipEnt obj)
        {
            return MasaTakipRepository.Update(obj);
        }
    }
}
