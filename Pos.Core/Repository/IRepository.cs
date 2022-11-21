using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pos.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(object id);

        IEnumerable<T> GetAll();

        long Insert(T obj);

        bool Update(T obj);

        bool Delete(T obj);
    }
}
