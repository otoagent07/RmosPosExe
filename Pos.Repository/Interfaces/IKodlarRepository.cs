using Pos.Core.Repository;
using Pos.Entity.Pos;
using System.Collections.Generic;

namespace Pos.Repository.Interfaces
{
    public interface IKodlarRepository : IRepository<KodlarEnt>
    {
        IEnumerable<KodlarEnt> GetKodlarSinif(string Sinif);
    }
}
