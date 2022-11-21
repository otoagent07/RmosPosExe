using Pos.Core.Repository;
using Pos.Entity.Pos;
using System.Collections.Generic;

namespace Pos.Repository.Interfaces
{
    public interface ICstReceteSatisRepository : IRepository<CstReceteSatisEnt>
    {
        IEnumerable<CstReceteSatisEnt> GetReceteList(string Fisno);
    }
}
