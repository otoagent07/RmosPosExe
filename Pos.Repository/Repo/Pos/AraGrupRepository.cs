using Pos.Core.Dapper;
using Pos.Entity.Pos;
using Pos.Repository.Interfaces;
using System.Collections.Generic;

namespace Pos.Repository.Repo.Pos
{
    public class AraGrupRepository : IAraGrupRepository
    {
        private IDapperTools dapper { get; set; }

        public AraGrupRepository(string connectionString)
        {
            dapper = new DapperTools(connectionString);
        }

        public AraGrupEnt Get(object id)
        {
            return dapper.Get<AraGrupEnt>(id);
        }

        public IEnumerable<AraGrupEnt> GetAll()
        {
            return dapper.GetAll<AraGrupEnt>();
        }

        public long Insert(AraGrupEnt obj)
        {
            return dapper.Insert(obj);
        }

        public bool Update(AraGrupEnt obj)
        {
            return dapper.Update(obj);
        }

        public IEnumerable<AraGrupEnt> GetAraGrup(string DepartmanKodu, string AnaGrup)
        {
            return dapper.Query<AraGrupEnt>(@"set dateformat dmy ; 
            SELECT top 1 Rdep_Departman as Kont_Departman,Rec_Anagrup as Kont_Anagrup,
            'SIKKULLAN' as Kont_Aragrup,'SIKKULLAN' as Kont_Aragrup2,
            '(*) SIK KULLANILAN' as Kodlar_Ad,-10000 as Kodlar_Sira,
            NULL  as Kodlar_Size,NULL as Kodlar_Font,
            NULL as Kodlar_Backcolor  FROM Cst_Recete_Dep WITH(NOLOCK)       
            left join Cst_Recete on Rec_Genelkod = Rdep_Recete  
            WHERE Rdep_Departman = '" + DepartmanKodu + "' and Rec_Anagrup = '" + AnaGrup + @"' and ISNULL(Rdep_SikKullanilan,0) = 1  
            UNION ALL  
            SELECT Kont_Departman,
            Kont_Anagrup, Kont_Aragrup, 
            Kont_Anagrup +'#'+ Kont_Aragrup as Kont_Aragrup2, Ara_Grup.Kodlar_Ad,isnull(Ara_Grup.Kodlar_Sira,0),
            ISNULL(NULLIF(Ara_Grup.Kodlar_Size,''),'100;40') as Kodlar_Size,Ara_Grup.Kodlar_Font,Ara_Grup.Kodlar_Backcolor  
            FROM Pos_Grup  left join Stok_Kodlar as Ana_Grup ON Kont_Anagrup = Ana_Grup.Kodlar_Kod AND Ana_Grup.Kodlar_Sinif = '08'  
            left join Stok_Kodlar as Ara_Grup ON Kont_Aragrup = Ara_Grup.Kodlar_Kod AND Ara_Grup.Kodlar_Sinif = '09' and Ara_Grup.Kodlar_Anagrup =  Kont_Anagrup 
            WHERE Kont_Departman = '" + DepartmanKodu + @"' and Kont_Anagrup = '" + AnaGrup + "' ORDER BY Kodlar_Sira, Kodlar_Ad, Kont_Anagrup, Kont_Aragrup, Kont_Departman ");
        }

        public bool Delete(AraGrupEnt obj)
        {
            return dapper.Delete(obj);
        }
    }
}
