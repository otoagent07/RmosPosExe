using Pos.Core.Repository;
using Pos.Entity.Pos;
using System.Collections.Generic;

namespace Pos.Repository.Interfaces
{
    public interface IMasaKonumRepository : IRepository<MasaKonumEnt>
    {
        IEnumerable<MasaKonumEnt> GetDepartmanKonum(string depKodu);
    }
}
