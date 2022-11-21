using Pos.Core.Repository;
using Pos.Entity.Pos;
using System.Collections.Generic;

namespace Pos.Repository.Interfaces
{
    public interface IAnaGrupRepository : IRepository<AnaGrupEnt>
    {
        IEnumerable<AnaGrupEnt> GetAnaGrup(string DepartmanKodu);
    }
}
