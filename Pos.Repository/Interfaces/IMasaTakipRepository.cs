using Pos.Core.Repository;
using Pos.Entity.Pos;
using System;
using System.Collections.Generic;

namespace Pos.Repository.Interfaces
{
    public interface IMasaTakipRepository : IRepository<MasaTakipEnt>
    {
        IEnumerable<MasaTakipEnt> GetDepartmanMasa(string depKodu, DateTime Tarih, string KonumFilter, string paketFilter, string Masalar, string siralama);
    }
}
