using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Class
{
    class NOTLAR
    {
        /*
          if (Departman.Kodlar_AndPos_NFC == true && User.ExtraFolio == false) //  && User.ExtraFolio == false
                {
                    gridControl1.DataSource = Fronttools.SelectTable("SELECT Rez_Id,Rez_Odano,Rez_Adi_1 , Rez_Adi_2, Rez_Kartno,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi, "
                                      + " convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi,Rez_Odeme, "
                                      + " case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Master_Id,0 as ID,Kodlar_Ad "
                                      + " FROM Rez WİTH(NOLOCK) left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu "
                                      + " left join Kodlar WITH(NOLOCK) on Kodlar_Sinif = '10' and Rez_Odeme = Kodlar_Kod "
                                      + " WHERE  Rez_R_I_H = 'I' " + filtre + "");


                }
                else if (Departman.Kodlar_AndPos_NFC == false)
                {

                    filtre = "";
                    if (chk_OdaNo.Checked)
                    {
                        filtre = " and Rez_Odano like N'" + txt_Arama.Text + "%' ";
                    }
                    if (chk_KartNo.Checked)
                    {
                        Rez_Kartno = "rez.Rez_Kartno"; // 04.08.2022 de değiştirildi . özhan bey istedi
                        filtre = " and " + Rez_Kartno + " like N'" + dataKart + "%' ";
                    }
                    if (chk_Ad.Checked)
                    {
                        filtre = " and Rez_Adi_1 like N'" + txt_Arama.Text + "%' ";
                    }
                    if (chk_Soyad.Checked)
                    {
                        filtre = " and Rez_Adi_2 like N'" + txt_Arama.Text + "%' ";
                    }

                    string sorgu = @"select Rez_Id,
                                    Rez_Odano,
                                    Rez_Adi_1 ,
                                    Rez_Adi_2, 
                                     rez.Rez_Kartno as Rez_Kartno,
                                    Rez_Konaklama,
                                    Rez_Giris_tarihi,
                                    Rez_Cikis_tarihi, 
                                    convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,
                                    Ac_Adi,
                                    Rez_Odeme, 
                                    case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Master_Id,
                                    rez.Rez_Id as ID,
                                    Kodlar_Ad  
                                    FROM Rez WITH(NOLOCK) 
                                    left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu 
                                    left join Kodlar WITH(NOLOCK) on Kodlar_Sinif = '10' and Rez_Odeme = Kodlar_Kod where rez.Rez_R_I_H='I' " + filtre;



                    gridControl1.DataSource = Fronttools.SelectTable(sorgu);

                }
                else
                {
                    string sorgu = @"select Rez_Id,
                                    Rez_Odano,
                                    Rez_Adi_1 ,
                                    Rez_Adi_2, 
                                    CardF_No as Rez_Kartno,
                                    Rez_Konaklama,
                                    Rez_Giris_tarihi,
                                    Rez_Cikis_tarihi, 
                                    convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,
                                    Ac_Adi,
                                    Rez_Odeme, 
                                    case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Master_Id,
                                    KartF.ID as ID,
                                    Kodlar_Ad  
                                    FROM Rez WITH(NOLOCK) 
                                    left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu 
                                    left join Kodlar WITH(NOLOCK) on Kodlar_Sinif = '10' and Rez_Odeme = Kodlar_Kod 
                                    left join KartF on  CardF_RezID = Rez_Id WHERE  Rez_R_I_H = 'I' " + filtre;


                    gridControl1.DataSource = Fronttools.SelectTable(sorgu);
                }
         */
        /*
          public RmosMerkez21Entities(string server, string veritab, string user, string sifre)
                : base("metadata=res://*;"
            + "provider=System.Data.SqlClient;"
            + "provider connection string=';"
          + "Data Source=" + server + ";"
          + "Initial Catalog=" + veritab + ";"
          + "Persist Security Info=True;"
          + "User ID=" + user + ";Password=" + sifre + ";"
          + "MultipleActiveResultSets=True';")
            {
            }


         public static decimal BalanceBul(int Folio, string KartNo)
        {

            //DataTable dt = Fronttools.SelectTable("select	ISNULL(SUM(case when Kumhrk_Ba = 'B' then Kumhrk_Tutar else (-1) * Kumhrk_Tutar end),0) as TL_Bakiye, "
            //                                        + "         ISNULL(SUM(case when Kumhrk_Ba = 'B' then Kumhrk_Doviz_tutar else (-1) * Kumhrk_Doviz_tutar end),0) as Doviz_Bakiye "
            //                                        + " from Kumhrk  "
            //                                        + " where Kumhrk_Re = 'E' and (Kumhrk_Rez_id = (select case when Rez_Master_detay = 'M' then Rez_Id else Rez_Master_id end from Rez where Rez_Id = '" + Folio + @"') 
            //                                        And (ISNULL(Kumhrk_Kart_id,'') = '" + KartNo + "'))");


            DataTable dt = Fronttools.SelectTable("select	ISNULL(SUM(case when Kumhrk_Ba = 'B' then Kumhrk_Tutar else (-1) * Kumhrk_Tutar end),0) as TL_Bakiye, "
                                                    + "         ISNULL(SUM(case when Kumhrk_Ba = 'B' then Kumhrk_Doviz_tutar else (-1) * Kumhrk_Doviz_tutar end),0) as Doviz_Bakiye "
                                                    + " from Kumhrk  "
                                                    + " where Kumhrk_Re = 'E' and (Kumhrk_Rez_id = (select case when Rez_Master_detay = 'M' then Rez_Id else Rez_Master_id end from Rez where Rez_Id = '" + Folio + @"') 
                                                    )");

            if (Param.Calisma_Sekli == 1) //Dovizli
            {
                return Convert.ToDecimal(dt.Rows[0]["Doviz_Bakiye"]);
            }
            else
            {
                return Convert.ToDecimal(dt.Rows[0]["TL_Bakiye"]);
            }
        }
         */
    }
}
