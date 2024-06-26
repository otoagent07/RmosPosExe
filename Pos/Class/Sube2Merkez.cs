using DevExpress.XtraEditors;
using Pos.Controllers;
using Pos.Entities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Sube2Merkez
    {
        public string SubeMac { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Users { get; set; }
        public string Password { get; set; }
        public string connstr { get; set; }

        SqlDataAdapter adap;
        SqlConnection conn;
        DataSet dts;
        DataTable dt;
        SqlCommand cmd;

        public Sube2Merkez()
        {
            dt = dbtools.SelectTable(@"select 
                                        Pkod_SubeMac as [SubeMac],
                                        Pkod_Server as [Server],
                                        Pkod_Database as [Database],
                                        Pkod_User as [User],
                                        Pkod_Password as [Password]
                                        from pos_kodlar
                                        where
                                        Pkod_Sinif = 27
                                        and
                                        Pkod_MerkezSube = 'M'");

            if (dt.Rows.Count > 0)
            {
                SubeMac = Convert.ToString(dt.Rows[0]["SubeMac"]);
                Server = Convert.ToString(dt.Rows[0]["Server"]);
                Database = Convert.ToString(dt.Rows[0]["Database"]);
                Users = Convert.ToString(dt.Rows[0]["User"]);
                Password = Convert.ToString(dt.Rows[0]["Password"]);

                connstr = "Data Source='" + Server + "'; Initial Catalog=" + Database + "; Persist Security Info=True; uid='" + Users + "'; pwd='" + Password + "'";
                conn = new SqlConnection(connstr);

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("İlgili Server Bilgileri Yoktur.");
            }
        }

        public DataTable SelectTable(String sql1)
        {
            dts = new DataSet();
            adap = new SqlDataAdapter("set dateformat dmy ; " + sql1, conn);
            adap.Fill(dts, "q");
            return dts.Tables["q"];
        }

        public bool execcmd(String cmds)
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                cmd = new SqlCommand("set dateformat dmy ; " + cmds, conn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }




        public void merkezdenKendineAnaGrupAc()
        {
            try
            {
                RmosMerkez21Entities dbMerkez = new RmosMerkez21Entities(Server, Database, Users, Password);
                var merkezStokKodlar = dbMerkez.Stok_Kodlar.Where(x => x.Kodlar_Sinif == "08").ToList();

                RmosMerkez21Entities dbSube = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var subeStokKodlar = dbSube.Stok_Kodlar.Where(x => x.Kodlar_Sinif == "08").ToList();

                foreach (var merkez in merkezStokKodlar)
                {
                    var sube = subeStokKodlar.Where(x => x.Kodlar_Kod == merkez.Kodlar_Kod && x.Kodlar_Anagrup == merkez.Kodlar_Anagrup).FirstOrDefault();
                    if (sube == null) //  Insert
                    {
                        merkez.Kodlar_Id = 0;
                        dbSube.Stok_Kodlar.Add(merkez);
                        dbSube.SaveChanges();
                    }
                    else //  Update
                    {
                        merkez.Kodlar_Id = sube.Kodlar_Id;
                        dbSube.Entry(sube).CurrentValues.SetValues(merkez);
                        dbSube.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "merkezdenKendineAnaGrupAc", "", ex);
            }
        }


        public void merkezdenKendineAraGrupAc()
        {
            try
            {
                RmosMerkez21Entities dbMerkez = new RmosMerkez21Entities(Server, Database, Users, Password);
                var merkezStokKodlar = dbMerkez.Stok_Kodlar.Where(x => x.Kodlar_Sinif == "09").ToList();

                RmosMerkez21Entities dbSube = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var subeStokKodlar = dbSube.Stok_Kodlar.Where(x => x.Kodlar_Sinif == "09").ToList();

                foreach (var merkez in merkezStokKodlar)
                {
                    var sube = subeStokKodlar.Where(x => x.Kodlar_Kod == merkez.Kodlar_Kod &&  x.Kodlar_Anagrup == merkez.Kodlar_Anagrup).FirstOrDefault();
                    if (sube == null) //  Insert
                    {
                        merkez.Kodlar_Id = 0; // yeni id alması için
                        dbSube.Stok_Kodlar.Add(merkez);
                        dbSube.SaveChanges();
                    }
                    else //  Update
                    {
                        merkez.Kodlar_Id = sube.Kodlar_Id;
                        dbSube.Entry(sube).CurrentValues.SetValues(merkez);
                        dbSube.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "merkezdenKendineAraGrupAc", "", ex);
            }
        }

        //public void merkezdenKendineAraGrupAc()
        //{
        //    try
        //    {

        //        RmosMerkez21Entities dbMerkez = new RmosMerkez21Entities(Server, Database, Users, Password);
        //        var merkezStokKodlar = dbMerkez.Stok_Kodlar.Where(x => x.Kodlar_Sinif == "09").ToList();

        //        RmosMerkez21Entities dbSube = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
        //        var subeStokKodlar = dbSube.Stok_Kodlar.Where(x => x.Kodlar_Sinif == "09").ToList();


        //        foreach (var merkez in merkezStokKodlar)
        //        {
        //            bool varmi = false;
        //            foreach (var sube in subeStokKodlar)
        //            {
        //                if (merkez.Kodlar_Kod == sube.Kodlar_Kod) // update
        //                {
        //                    sube.Kodlar_Anadepo = merkez.Kodlar_Anadepo;
        //                    dbSube.Entry(sube).State = System.Data.Entity.EntityState.Modified;
        //                    dbSube.SaveChanges();
        //                    varmi = true;
        //                    break;
        //                }
        //            }

        //            if (varmi == false) // insert
        //            {
        //                dbSube.Stok_Kodlar.Add(merkez);
        //                dbSube.SaveChanges();
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        RHMesaj.MyMessageError(MyClass, "merkezdenKendineAnaGrupAc", "", ex);
        //    }
        //}

        static string MyClass = "Sube2Merkez";

        public void ReceteGetir()
        {
           
                merkezdenKendineAnaGrupAc();
                merkezdenKendineAraGrupAc();
                SqlConnection con = dbtools.conn;
           
                if (CheckConnectionIP(connstr))
                {
                    dt = new DataTable();
                    string query = @"
declare @Kodlar_KategoriFiyatTur int = (Select ISNULL(Kodlar_KategoriFiyatTur,0) From Stok_Kodlar where Kodlar_Sinif = 1 and Kodlar_Kod = '" + Departman.Dep_Kodu + @"')

                            Select 
                           [Rec_Departman]
                          ,[Rec_Grup]
                          ,[Rec_Anagrup]
                          ,[Rec_Altgrup]
                          ,[Rec_Kodu]
                          ,[Rec_Genelkod]
                          ,[Rec_Ad]
                          ,[Rec_Kisi]
                          ,case ISNULL(@Kodlar_KategoriFiyatTur,0) 
when 0 Then ISNULL(Rec_Fiyat,0) 
when 1 Then ISNULL(NULLIF(Rec_KategoriFiyat1,0),ISNULL(Rec_Fiyat,0))
when 2 Then ISNULL(NULLIF(Rec_KategoriFiyat2,0),ISNULL(Rec_Fiyat,0))
when 3 Then ISNULL(NULLIF(Rec_KategoriFiyat3,0),ISNULL(Rec_Fiyat,0))
when 4 Then ISNULL(NULLIF(Rec_KategoriFiyat4,0),ISNULL(Rec_Fiyat,0))
when 5 Then ISNULL(NULLIF(Rec_KategoriFiyat5,0),ISNULL(Rec_Fiyat,0))
else ISNULL(Rec_Fiyat,0) end Rec_Fiyat
                          ,[Rec_Kdv]
                          ,[Rec_Net]
                          ,[Rec_Dovizkodu]
                          ,[Rec_Dovifiyat]
                          ,[Rec_Urungrup]
                          ,[Rec_All]
                          ,[Rec_Ultra]
                          ,[Rec_HappyHour]
                          ,case when '" + Departman.Dep_Kodu + @"' in (SELECT fieldvalue FROM dbo.stringArray(Rec_Departman,',')) then 1 else 0 end as Rec_Pasif
                          ,[Rec_FB]
                          ,[Rec_HB]
                          ,[Rec_Pdapasif]
                          ,[Rec_Minibar]
                          ,[Rec_SiparisCikmasin]
                          ,[Rec_Miktar_Sor]
                          ,[Rec_Miktar_Gr]
                          ,[Rec_Tutar_Sor]
                          ,[Rec_Ust_Recete]
                          ,[Rec_Yarim]
                          ,[Rec_Birbucuk]
                          ,[Rec_Duble]
                          ,[Rec_Aksam_Yarim]
                          ,[Rec_Aksam_Birbucuk]
                          ,[Rec_Aksam_Duble]
                          ,[Rec_Paket_Yarim]
                          ,[Rec_Paket_Birbucuk]
                          ,[Rec_Paket_Duble]
                          ,[Rec_Santral_Minibar]
                          ,[Rec_Sira]
                          ,[Rec_Barkod]
                          ,[Rec_Terazi]
                          ,[Rec_SpUygulanmasin]
                          ,[Rec_HappyHourFiyat]
                          ,[Rec_Aksam_Tam]
                          ,[Rec_Paket_Tam]
                          ,[Rec_DetayGrup]
                          ,[Rec_Picture]
                          ,[Rec_Ad_Eng]
                          ,[Rec_Ad_Deu]
                          ,[Rec_Ad_Rus]
                          ,[Rec_Ad_Dig]
                          ,[Rec_YkasaBirim]
                          ,[Rec_UretimMiktar]
                          ,[Rec_UretimBirim]
                          ,[Rec_YS_UrunID]
                          ,[Rec_Update]
                          ,[Rec_Color]
                          ,[Rec_Printer]
                          ,[Rec_AbuyerPR]
                          ,[Rec_AciklamaAP]
                          ,[Rec_AciklamaGrup]
                          ,[Rec_GetirMenuID]
                          ,[Rec_OptionID] From Cst_Recete ";



                    dt = SelectTable(query);
                    //dt.Columns.RemoveAt(0);
                    if (dt.Rows.Count > 0)
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int durum = Convert.ToInt32(dbtools.DegerGetir("Select Count(*) From Cst_Recete Where Rec_GenelKod = '" + Convert.ToString(dt.Rows[i]["Rec_GenelKod"]) + "'"));
                            if (durum == 0)
                            {
                                using (SqlCommand cmd = new SqlCommand("InsertTableTo_CstRecete", con) { CommandType = CommandType.StoredProcedure })
                                {
                                    cmd.Parameters.AddWithValue("@Tip", 0);
                                    cmd.Parameters.AddWithValue("@Rec_Anagrup", dt.Rows[i]["Rec_Anagrup"]);
                                    cmd.Parameters.AddWithValue("@Rec_Altgrup", dt.Rows[i]["Rec_Altgrup"]);
                                    cmd.Parameters.AddWithValue("@Rec_Kodu", dt.Rows[i]["Rec_Kodu"]);
                                    cmd.Parameters.AddWithValue("@Rec_Genelkod", dt.Rows[i]["Rec_Genelkod"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ad", dt.Rows[i]["Rec_Ad"]);
                                    cmd.Parameters.AddWithValue("@Rec_Kisi", dt.Rows[i]["Rec_Kisi"]);
                                    cmd.Parameters.AddWithValue("@Rec_Fiyat", dt.Rows[i]["Rec_Fiyat"]);
                                    cmd.Parameters.AddWithValue("@Rec_Kdv", dt.Rows[i]["Rec_Kdv"]);
                                    cmd.Parameters.AddWithValue("@Rec_Net", dt.Rows[i]["Rec_Net"]);
                                    cmd.Parameters.AddWithValue("@Rec_Dovizkodu", dt.Rows[i]["Rec_Dovizkodu"]);
                                    cmd.Parameters.AddWithValue("@Rec_Dovifiyat", dt.Rows[i]["Rec_Dovifiyat"]);
                                    cmd.Parameters.AddWithValue("@Rec_Urungrup", dt.Rows[i]["Rec_Urungrup"]);
                                    cmd.Parameters.AddWithValue("@Rec_All", dt.Rows[i]["Rec_All"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ultra", dt.Rows[i]["Rec_Ultra"]);
                                    cmd.Parameters.AddWithValue("@Rec_HappyHour", dt.Rows[i]["Rec_HappyHour"]);
                                    cmd.Parameters.AddWithValue("@Rec_Pasif", dt.Rows[i]["Rec_Pasif"]);
                                    cmd.Parameters.AddWithValue("@Rec_FB", dt.Rows[i]["Rec_FB"]);
                                    cmd.Parameters.AddWithValue("@Rec_Pdapasif", dt.Rows[i]["Rec_Pdapasif"]);
                                    cmd.Parameters.AddWithValue("@Rec_Minibar", dt.Rows[i]["Rec_Minibar"]);
                                    cmd.Parameters.AddWithValue("@Rec_SiparisCikmasin", dt.Rows[i]["Rec_SiparisCikmasin"]);
                                    cmd.Parameters.AddWithValue("@Rec_Miktar_Sor", dt.Rows[i]["Rec_Miktar_Sor"]);
                                    cmd.Parameters.AddWithValue("@Rec_Miktar_Gr", dt.Rows[i]["Rec_Miktar_Gr"]);
                                    cmd.Parameters.AddWithValue("@Rec_Tutar_Sor", dt.Rows[i]["Rec_Tutar_Sor"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ust_Recete", dt.Rows[i]["Rec_Ust_Recete"]);
                                    cmd.Parameters.AddWithValue("@Rec_Yarim", dt.Rows[i]["Rec_Yarim"]);
                                    cmd.Parameters.AddWithValue("@Rec_Birbucuk", dt.Rows[i]["Rec_Birbucuk"]);
                                    cmd.Parameters.AddWithValue("@Rec_Duble", dt.Rows[i]["Rec_Duble"]);
                                    cmd.Parameters.AddWithValue("@Rec_Aksam_Yarim", dt.Rows[i]["Rec_Aksam_Yarim"]);
                                    cmd.Parameters.AddWithValue("@Rec_Aksam_Birbucuk", dt.Rows[i]["Rec_Aksam_Birbucuk"]);
                                    cmd.Parameters.AddWithValue("@Rec_Aksam_Duble", dt.Rows[i]["Rec_Aksam_Duble"]);
                                    cmd.Parameters.AddWithValue("@Rec_Paket_Yarim", dt.Rows[i]["Rec_Paket_Yarim"]);
                                    cmd.Parameters.AddWithValue("@Rec_Paket_Birbucuk", dt.Rows[i]["Rec_Paket_Birbucuk"]);
                                    cmd.Parameters.AddWithValue("@Rec_Paket_Duble", dt.Rows[i]["Rec_Paket_Duble"]);
                                    cmd.Parameters.AddWithValue("@Rec_Santral_Minibar", dt.Rows[i]["Rec_Santral_Minibar"]);
                                    cmd.Parameters.AddWithValue("@Rec_Sira", dt.Rows[i]["Rec_Sira"]);
                                    cmd.Parameters.AddWithValue("@Rec_Barkod", dt.Rows[i]["Rec_Barkod"]);
                                    cmd.Parameters.AddWithValue("@Rec_Terazi", dt.Rows[i]["Rec_Terazi"]);
                                    cmd.Parameters.AddWithValue("@Rec_SpUygulanmasin", dt.Rows[i]["Rec_SpUygulanmasin"]);
                                    cmd.Parameters.AddWithValue("@Rec_HappyHourFiyat", dt.Rows[i]["Rec_HappyHourFiyat"]);
                                    cmd.Parameters.AddWithValue("@Rec_Aksam_Tam", dt.Rows[i]["Rec_Aksam_Tam"]);
                                    cmd.Parameters.AddWithValue("@Rec_Paket_Tam", dt.Rows[i]["Rec_Paket_Tam"]);
                                    cmd.Parameters.AddWithValue("@Rec_DetayGrup", dt.Rows[i]["Rec_DetayGrup"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ad_Eng", dt.Rows[i]["Rec_Ad_Eng"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ad_Deu", dt.Rows[i]["Rec_Ad_Deu"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ad_Rus", dt.Rows[i]["Rec_Ad_Rus"]);
                                    cmd.Parameters.AddWithValue("@Rec_Ad_Dig", dt.Rows[i]["Rec_Ad_Dig"]);
                                    cmd.Parameters.AddWithValue("@Rec_YkasaBirim", dt.Rows[i]["Rec_YkasaBirim"]);
                                    cmd.Parameters.AddWithValue("@Rec_UretimMiktar", dt.Rows[i]["Rec_UretimMiktar"]);
                                    cmd.Parameters.AddWithValue("@Rec_UretimBirim", dt.Rows[i]["Rec_UretimBirim"]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("InsertTableTo_CstRecete", con) { CommandType = CommandType.StoredProcedure })
                                {
                                    if ((Convert.ToString(dt.Rows[i]["Rec_Update"]) == "" ? 0 : Convert.ToInt32(dt.Rows[i]["Rec_Update"])) == 0)
                                    {
                                        cmd.Parameters.AddWithValue("@Tip", 1);
                                        cmd.Parameters.AddWithValue("@Rec_Anagrup", dt.Rows[i]["Rec_Anagrup"]);
                                        cmd.Parameters.AddWithValue("@Rec_Altgrup", dt.Rows[i]["Rec_Altgrup"]);
                                        cmd.Parameters.AddWithValue("@Rec_Kodu", dt.Rows[i]["Rec_Kodu"]);
                                        cmd.Parameters.AddWithValue("@Rec_Genelkod", dt.Rows[i]["Rec_Genelkod"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ad", dt.Rows[i]["Rec_Ad"]);
                                        cmd.Parameters.AddWithValue("@Rec_Kisi", dt.Rows[i]["Rec_Kisi"]);
                                        cmd.Parameters.AddWithValue("@Rec_Fiyat", dt.Rows[i]["Rec_Fiyat"]);
                                        cmd.Parameters.AddWithValue("@Rec_Kdv", dt.Rows[i]["Rec_Kdv"]);
                                        cmd.Parameters.AddWithValue("@Rec_Net", dt.Rows[i]["Rec_Net"]);
                                        cmd.Parameters.AddWithValue("@Rec_Dovizkodu", dt.Rows[i]["Rec_Dovizkodu"]);
                                        cmd.Parameters.AddWithValue("@Rec_Dovifiyat", dt.Rows[i]["Rec_Dovifiyat"]);
                                        cmd.Parameters.AddWithValue("@Rec_Urungrup", dt.Rows[i]["Rec_Urungrup"]);
                                        cmd.Parameters.AddWithValue("@Rec_All", dt.Rows[i]["Rec_All"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ultra", dt.Rows[i]["Rec_Ultra"]);
                                        cmd.Parameters.AddWithValue("@Rec_HappyHour", dt.Rows[i]["Rec_HappyHour"]);
                                        cmd.Parameters.AddWithValue("@Rec_Pasif", dt.Rows[i]["Rec_Pasif"]);
                                        cmd.Parameters.AddWithValue("@Rec_FB", dt.Rows[i]["Rec_FB"]);
                                        cmd.Parameters.AddWithValue("@Rec_Pdapasif", dt.Rows[i]["Rec_Pdapasif"]);
                                        cmd.Parameters.AddWithValue("@Rec_Minibar", dt.Rows[i]["Rec_Minibar"]);
                                        cmd.Parameters.AddWithValue("@Rec_SiparisCikmasin", dt.Rows[i]["Rec_SiparisCikmasin"]);
                                        cmd.Parameters.AddWithValue("@Rec_Miktar_Sor", dt.Rows[i]["Rec_Miktar_Sor"]);
                                        cmd.Parameters.AddWithValue("@Rec_Miktar_Gr", dt.Rows[i]["Rec_Miktar_Gr"]);
                                        cmd.Parameters.AddWithValue("@Rec_Tutar_Sor", dt.Rows[i]["Rec_Tutar_Sor"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ust_Recete", dt.Rows[i]["Rec_Ust_Recete"]);
                                        cmd.Parameters.AddWithValue("@Rec_Yarim", dt.Rows[i]["Rec_Yarim"]);
                                        cmd.Parameters.AddWithValue("@Rec_Birbucuk", dt.Rows[i]["Rec_Birbucuk"]);
                                        cmd.Parameters.AddWithValue("@Rec_Duble", dt.Rows[i]["Rec_Duble"]);
                                        cmd.Parameters.AddWithValue("@Rec_Aksam_Yarim", dt.Rows[i]["Rec_Aksam_Yarim"]);
                                        cmd.Parameters.AddWithValue("@Rec_Aksam_Birbucuk", dt.Rows[i]["Rec_Aksam_Birbucuk"]);
                                        cmd.Parameters.AddWithValue("@Rec_Aksam_Duble", dt.Rows[i]["Rec_Aksam_Duble"]);
                                        cmd.Parameters.AddWithValue("@Rec_Paket_Yarim", dt.Rows[i]["Rec_Paket_Yarim"]);
                                        cmd.Parameters.AddWithValue("@Rec_Paket_Birbucuk", dt.Rows[i]["Rec_Paket_Birbucuk"]);
                                        cmd.Parameters.AddWithValue("@Rec_Paket_Duble", dt.Rows[i]["Rec_Paket_Duble"]);
                                        cmd.Parameters.AddWithValue("@Rec_Santral_Minibar", dt.Rows[i]["Rec_Santral_Minibar"]);
                                        cmd.Parameters.AddWithValue("@Rec_Sira", dt.Rows[i]["Rec_Sira"]);
                                        cmd.Parameters.AddWithValue("@Rec_Barkod", dt.Rows[i]["Rec_Barkod"]);
                                        cmd.Parameters.AddWithValue("@Rec_Terazi", dt.Rows[i]["Rec_Terazi"]);
                                        cmd.Parameters.AddWithValue("@Rec_SpUygulanmasin", dt.Rows[i]["Rec_SpUygulanmasin"]);
                                        cmd.Parameters.AddWithValue("@Rec_HappyHourFiyat", dt.Rows[i]["Rec_HappyHourFiyat"]);
                                        cmd.Parameters.AddWithValue("@Rec_Aksam_Tam", dt.Rows[i]["Rec_Aksam_Tam"]);
                                        cmd.Parameters.AddWithValue("@Rec_Paket_Tam", dt.Rows[i]["Rec_Paket_Tam"]);
                                        cmd.Parameters.AddWithValue("@Rec_DetayGrup", dt.Rows[i]["Rec_DetayGrup"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ad_Eng", dt.Rows[i]["Rec_Ad_Eng"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ad_Deu", dt.Rows[i]["Rec_Ad_Deu"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ad_Rus", dt.Rows[i]["Rec_Ad_Rus"]);
                                        cmd.Parameters.AddWithValue("@Rec_Ad_Dig", dt.Rows[i]["Rec_Ad_Dig"]);
                                        cmd.Parameters.AddWithValue("@Rec_YkasaBirim", dt.Rows[i]["Rec_YkasaBirim"]);
                                        cmd.Parameters.AddWithValue("@Rec_UretimMiktar", dt.Rows[i]["Rec_UretimMiktar"]);
                                        cmd.Parameters.AddWithValue("@Rec_UretimBirim", dt.Rows[i]["Rec_UretimBirim"]);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            
        }

        private bool CheckConnectionIP(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public async Task GonderSatis()
        {
            await SatisGonder();

        }
        //public void GonderSatis()
        //{
        //     SatisGonder();

        //}

        private async Task SatisGonder() // private async Task SatisGonder()
        {

            try
            {
                if (CheckConnectionIP(connstr))
                {
                    SqlConnection con = dbtools.conn;

                    if (con.State == ConnectionState.Closed) { con.Open(); }

                    DataTable dt = dbtools.SelectTable(@"Select Rsat_Fisno,ISNULL(Rsat_RecAP,0) as Rsat_RecAP from Cst_Recete_Satis Where Rsat_Durum = 'K' and ISNULL(Rsat_RecAP,0) != 1 and Rsat_Departman = '" + Departman.Dep_Kodu + "' Group by Rsat_Fisno,ISNULL(Rsat_RecAP,0)");

                    if (dt.Rows.Count > 0)
                    {
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Sil = 0;
                            using (SqlCommand cmd = new SqlCommand("InsertTableTo_CstRecete_Satis", conn) { CommandType = CommandType.StoredProcedure })
                            {
                                if (Convert.ToInt32(dt.Rows[i]["Rsat_RecAP"]) == 2)
                                {
                                    Sil = 1;
                                }

                                var dtRow = dbtools.SelectTable(@"Select 
                                                   [Rsat_Fisno]
                                                  ,[Rsat_Tarih]
                                                  ,[Rsat_Departman]
                                                  ,[Rsat_Recete]
                                                  ,[Rsat_Kdvoran]
                                                  ,[Rsat_Miktar]
                                                  ,[Rsat_Fiyat]
                                                  ,[Rsat_Ind]
                                                  ,[Rsat_Net]
                                                  ,[Rsat_Kdv]
                                                  ,[Rsat_Tutar]
                                                  ,[Rsat_Maliyet]
                                                  ,[Rsat_Dovizkodu]
                                                  ,[Rsat_Doviztutar]
                                                  ,[Rsat_Satistip]
                                                  ,[Rsat_Odenmez]
                                                  ,[Rsat_Odano]
                                                  ,[Rsat_Folio]
                                                  ,[Rsat_Ba]
                                                  ,[Rsat_Kapatma]
                                                  ,[Rsat_Adisyon]
                                                  ,[Rsat_Aktiffisno]
                                                  ,[Rsat_Masa]
                                                  ,[Rsat_Garson]
                                                  ,[Rsat_Kisi]
                                                  ,[Rsat_Acilis]
                                                  ,[Rsat_Kapanis]
                                                  ,[Rsat_Durum]
                                                  ,[Rsat_Cari]
                                                  ,[Rsat_Split]
                                                  ,[Rsat_Aciklama]
                                                  ,[Rsat_SiparisPr]
                                                  ,[Rsat_AdisyonPr]
                                                  ,[Rsat_Paketci]
                                                  ,[Rsat_Emiktar]
                                                  ,[Rsat_AdisPr]
                                                  ,[Rsat_Garson2]
                                                  ,[Rsat_AdisPrSayac]
                                                  ,[Rsat_Uye_Kart_Turu]
                                                  ,[Rsat_Satir_Iptal]
                                                  ,[Rsat_Pansiyon]
                                                  ,[Rsat_Happy_Hour]
                                                  ,[Rsat_MusTipi]
                                                  ,[Rsat_Uye_Id]
                                                  ,[Rsat_Uye_Ad]
                                                  ,[Rsat_Indkodu]
                                                  ,[Rsat_Indoran]
                                                  ,[Rsat_Onbdep]
                                                  ,[Rsat_Indsatirid]
                                                  ,[Rsat_Satissaat]
                                                  ,[Rsat_Dovizkur]
                                                  ,[Rsat_Satir_Iptalsaat]
                                                  ,[Rsat_Indsatirid2]
                                                  ,[Rsat_Not]
                                                  ,[Rsat_Kartno]
                                                  ,[Rsat_Pda]
                                                  ,[Rsat_Mars]
                                                  ,[Rsat_Yapildi]
                                                  ,[Rsat_Splitad]
                                                  ,[Rsat_Hesap_Kilit]
                                                  ,[Rsat_Zayi]
                                                  ,[Rsat_Ikram]
                                                  ,[Rsat_AbuyerPr]
                                                  ,[Rsat_Yapma]
                                                  ,[Rsat_ZayiNeden]
                                                  ,[Rsat_IkramNeden]
                                                  ,[Rsat_Sube]
                                                  ,[Rsat_SubeDurum]
                                                  ,[Rsat_OzelMasaAdi]
                                                  ,[Rsat_SistemDate]
                                                  ,[Rsat_Kart_ID]
                                                  ,[Rsat_AdisyonTR]
                                                  ,[Rsat_AbuyerPr2]
                                                  ,[Rsat_AbuyerPr3]
                                                  ,[Rsat_AbuyerPr4]
                                                  ,[Rsat_Ingenico_Status]
                                                  ,[Rsat_IWERep]
                                                  ,[Rsat_SiraAciklama]
                                                  ,[Rsat_YSDurum]
                                                  ,[Rsat_YSOrderID]
                                                  ,[Rsat_UrunTahsilat]
	                                              ,[Rsat_Yerliyabanci]
	                                              ,[Rsat_PR]
	                                              ,Rsat_UrunBazliHspDokum ,BankaID
                                            From Cst_Recete_Satis Where Rsat_Fisno = '" + dt.Rows[i]["Rsat_Fisno"] + "'");

                                cmd.Parameters.AddWithValue("@myTableType", dtRow);
                                cmd.Parameters.AddWithValue("@DepKodu", Departman.Dep_Kodu);
                                cmd.Parameters.AddWithValue("@Sil", Sil);
                                cmd.Parameters.AddWithValue("@Fis", dt.Rows[i]["Rsat_Fisno"]);
                                cmd.ExecuteNonQuery();

                                dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_RecAP = 1 Where Rsat_Fisno = '" + dt.Rows[i]["Rsat_Fisno"] + "'");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    public DateTime Tarih;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public void IptalGonder()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {

        if (CheckConnectionIP(connstr))
        {
            SqlConnection con = dbtools.conn;

            if (con.State == ConnectionState.Closed) { con.Open(); }

            string query = @"select Rsat_Departman,Rsat_Fisno as Rsat_Fisno
                        from Cst_Satis_Ipt 
                        Where ISNULL(Rsat_IptalAP,0) = 0
                        group by Rsat_Fisno,Rsat_Departman";
            DataTable dt = dbtools.SelectTable(query);

            if (dt.Rows.Count > 0)
            {
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    using (SqlCommand cmd = new SqlCommand("Pos_Cek_Iptal", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@Fis_No", Convert.ToString(dt.Rows[i]["Rsat_Departman"]) + "" + Convert.ToString(dt.Rows[i]["Rsat_Fisno"]));
                        cmd.Parameters.AddWithValue("@Users", User.P_Kod);
                        cmd.Parameters.AddWithValue("@Rsat_IptalNot", "GUNSONU MERKEZ IPTALI");
                        cmd.Parameters.AddWithValue("@Onb_Sil", Tarih.Date != Param.Tarih.Date ? 0 : 1);
                        cmd.ExecuteNonQuery();

                        dbtools.execcmd("Update Cst_Satis_Ipt Set Rsat_IptalAP = 1 Where Rsat_Fisno = '" + dt.Rows[i]["Rsat_Fisno"] + "'");
                    }
                }
            }
        }
    }




public void satirlariOlustur(DateEdit dateTarih)
{
    try
    {

        DbtoolsMerkez dbtoolsMerkez = new DbtoolsMerkez(Server, Database, Users, Password);
        dbtoolsMerkez.execcmd("exec Cost_Malzeme_Satis @Tarih1='" + dateTarih.DateTime.Date + "',@Tarih2='" + dateTarih.DateTime.Date + "',@YeniPos=1 ");
        Log.Log_Kaydet(Convert.ToDateTime(dbtools.DegerGetir("select getdate()")), dbtools.database, "Recete Satıs Raporu", "Satış Oluşturma", User.P_Kod, SystemInformation.ComputerName, dateTarih.DateTime.ToShortDateString() + " - " + dateTarih.DateTime.ToShortDateString() + " Tarihli Satışlar Yeniden Oluşturuldu...", "", "", dbtoolsMerkez);
    }
    catch (Exception ex)
    {
        RHMesaj.MyMessageError(MyClass, "satirlariOlustur", "", ex);
    }
}

}
}

