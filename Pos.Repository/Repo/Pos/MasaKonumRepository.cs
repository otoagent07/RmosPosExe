using Pos.Core.Dapper;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Repository.Repo.Pos
{
    public class MasaKonumRepository : IMasaKonumRepository
    {
        private IDapperTools dapper { get; set; }

        public MasaKonumRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }

        public bool Delete(MasaKonumEnt obj)
        {
            return dapper.Delete(obj);
        }

        public MasaKonumEnt Get(object id)
        {
            return dapper.Get<MasaKonumEnt>(id);
        }

        public IEnumerable<MasaKonumEnt> GetAll()
        {
            return dapper.GetAll<MasaKonumEnt>();
        }

        public long Insert(MasaKonumEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(MasaKonumEnt obj)
        {
            return dapper.Update(obj);
        }

        public IEnumerable<MasaKonumEnt> GetDepartmanKonum(string depKodu)
        {
            return dapper.Query<MasaKonumEnt>(@"SELECT Pkod_Konumkod, Pkod_Ad FROM Pos_Kodlar WHERE Pkod_Sinif = '14' AND Pkod_Kod = '" + depKodu + "' order by Pkod_Sira,Pkod_Konumkod");
        }
    }
}
