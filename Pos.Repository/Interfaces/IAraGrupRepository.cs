using Pos.Core.Repository;
using Pos.Entity.Pos;
using System.Collections.Generic;

namespace Pos.Repository.Interfaces
{
    public interface IAraGrupRepository : IRepository<AraGrupEnt>
    {
        IEnumerable<AraGrupEnt> GetAraGrup(string DepartmanKodu,string AnaGrup);
    }
}
