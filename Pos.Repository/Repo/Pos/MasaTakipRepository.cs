using Pos.Core.Dapper;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Pos.Repository.Repo.Pos
{
    public class MasaTakipRepository : IMasaTakipRepository
    {
        private IDapperTools dapper { get; set; }

        public MasaTakipRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }

        public bool Delete(MasaTakipEnt obj)
        {
            return dapper.Delete(obj);
        }

        public MasaTakipEnt Get(object id)
        {
            return dapper.Get<MasaTakipEnt>(id);
        }

        public IEnumerable<MasaTakipEnt> GetAll()
        {
            return dapper.GetAll<MasaTakipEnt>();
        }

        public long Insert(MasaTakipEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(MasaTakipEnt obj)
        {
            return dapper.Update(obj);
        }

        public IEnumerable<MasaTakipEnt> GetDepartmanMasa(string depKodu, DateTime Tarih, string KonumFilter, string paketFilter, string Masalar, string siralama)
        {
            return dapper.Query<MasaTakipEnt>(@"select Masa_No, Masa_Ozel,MIN(Rsat_MusTipi) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                            + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                            + " Masa_Ad,MAX(CAST(ISNULL(Rsat_Pda,0) as int)) as Rsat_Pda,isnull(Masa_Paket,0) as Masa_Paket, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                            + " COUNT(Rez_Id) as RezSayisi, "
                            + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk "
                            + " from Pos_Masa WITH(NOLOCK) "
                            + " left join Cst_Recete_Satis WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                            //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                            + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + depKodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = Cst_Recete_Satis.Rsat_Fisno order by 1 desc ) "
                            + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                            + " left join Pos_Rez on Rez_Dep = '" + depKodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Tarih + "' "
                            + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                            + " where Masa_Depart = '" + depKodu + "' and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                            + " group by Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                            + siralama);
        }
    }
}
