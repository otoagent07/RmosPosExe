using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Fronttools
    {
        public static StreamReader oku = new StreamReader("RmosSirket.ini");
        public static string server = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string users = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string pwd = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string database = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");


        public static string connstr = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";

        //Data Adres*******************
        static DataSet dt;
        static SqlDataAdapter adap;
        public static SqlConnection conn = new SqlConnection(connstr);
        static SqlCommand cmd = null;
        public static int cust_cag = 0;

        public static string DB_Server;
        public static string DB_Database;
        public static string DB_User;
        public static string DB_Pwd;
        public static string DB_LinkServer;

        public static string Otel_Kodu;
        public static string Sirket_Kodu;


        public static void conyenile(string database)
        {
            try
            {
                connstr = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";
                conn = new SqlConnection(connstr);

                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fronttools.conyenile metodunda hata " + ex.Message);
            }
           

        }

        public static bool execcmd(String cmds)
        {
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                cmd = new SqlCommand(filter + cmds, conn);
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

        //////////////////////////////////

        public static String CheckDB()
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //////////////////////////////

        public static String SelectTekData(String querytable, String where, String istenendeger)
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
            conn.Open();

            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");

            dt = new DataSet();
            String str2 = "select " + istenendeger + " from " + querytable + " " + where;
            adap = new SqlDataAdapter(filter + str2, conn);
            adap.Fill(dt, "tbl1");
            String str3;
            if (dt.Tables["tbl1"].Rows.Count > 0)
            {
                str3 = dt.Tables["tbl1"].Rows[0][0].ToString();
            }
            else
            {
                str3 = "";
            }
            return str3;
        }

        /// ////////////////////

        public static String DegerGetir(String sql)
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
            conn.Open();
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");
            dt = new DataSet();
            String str2 = sql;
            adap = new SqlDataAdapter(filter + str2, conn);

            adap.Fill(dt, "tbl1");
            String str3;
            if (dt.Tables["tbl1"].Rows.Count > 0)
            {
                str3 = dt.Tables["tbl1"].Rows[0][0].ToString();
            }
            else
            {
                str3 = "";
            }
            return str3;
        }

        ///////////////////////////////////////

        public static DataTable SelectTable(String sql1)
        {
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
            conn.Open();

            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");
            dt = new DataSet();
            string query = filter + sql1;
            adap = new SqlDataAdapter(query, conn);
            adap.Fill(dt, "q");
            return dt.Tables["q"];
        }

        public static string IsimSoyisim(int Folio)
        {
            return Fronttools.DegerGetir("select isnull(Rez_Adi_1,'') + ' ' +isnull(Rez_Adi_2,'') as Rezadsoyad from Rez where Rez_Id = '" + Folio + "'");
        }

        public static string GirisCikisTarih(int Folio)
        {
            return Fronttools.DegerGetir("select CONVERT(varchar,Rez_Giris_tarihi,104) + ' - ' + CONVERT(varchar,Rez_Cikis_tarihi,104) from Rez where Rez_Id = '" + Folio + "'");
        }

        public static string KartNo(int Folio)
        {
            return Fronttools.DegerGetir("select Rez_Kartno from Rez where Rez_Id = '" + Folio + "'");
        }

        public static DataTable NFCBilgiler(string KartID)
        {
            DataTable dt = new DataTable();

            dt = Fronttools.SelectTable(@"      Select
                                    CardF_RezID as Rez_Id,
                                    CardF_Odano as Rez_Odano,
                                    CardF_Ad as Rez_Adi_1,
                                    CardF_Soyad as Rez_Adi_2,
                                    CardF_No as Rez_Kartno,
                                    null as Rez_Konaklama,
                                    CardF_GirisTrh as Rez_Giris_tarihi,
                                    CardF_CikisTrh as Rez_Cikis_tarihi,
                                    null as Kisi,
                                    null as Ac_Adi,
                                    null as Rez_odeme,
                                    CardF_RezID as Rez_Master_id,
                                    ID as ID,
                                    ''

                                    from KartF with(NOLOCK) where CardF_R_I_H = 'I' and KartF.CardF_No = '" + KartID + "'");

            return dt;
        }


        private static string Bakiye(string FolioID, string Kart_ID)
        {
            string bakiye = "0";

            decimal Borc = 0, Alacak = 0;

            string query = "exec StpKumhrk_Bul @xKumhrk_Tarih = '" + Param.Tarih.ToString("yyyy-MM-dd") + "',@xKumhrk_Sirket='" + Fronttools.Sirket_Kodu + "',@xKumhrk_Posting_kodu=N'P',@xKumhrk_Re=N'E',@xKumhrk_Rez_id=N'" + FolioID + "',@xtip=55, @Kumhrk_Kart_id = '" + Kart_ID + "'";
            DataTable dt = Fronttools.SelectTable(query);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Param.Calisma_Sekli == 0)
                    {
                        if (Convert.ToString(dt.Rows[i]["Kumhrk_Ba"]) == "B")
                        {
                            Borc += Convert.ToDecimal(dt.Rows[i]["Tl_Tutar"]);
                        }
                        else
                        {
                            Alacak += Convert.ToDecimal(dt.Rows[i]["Tl_Tutar"]);
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[i]["Kumhrk_Ba"]) == "B")
                        {
                            Borc += Convert.ToDecimal(dt.Rows[i]["Dv_Tutar"]);
                        }
                        else
                        {
                            Alacak += Convert.ToDecimal(dt.Rows[i]["Dv_Tutar"]);
                        }
                    }

                }
                bakiye = Borc.ToString("N2");
                bakiye = Alacak.ToString("N2");
                bakiye = (Convert.ToDecimal(Borc) - Convert.ToDecimal(Alacak)).ToString("N2");
            }


            return bakiye;
        }

        public static decimal BalanceBul(int Folio, string KartNo,string kartId= "-1")
        {

            if (Departman.Kodlar_AndPos_NFC)
            {
                string bakiye = Bakiye(Folio.ToString(), kartId);
                decimal bakiyem = Convert.ToDecimal(bakiye);
                return bakiyem;
            }
            else
            {
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

        }

        public static decimal NFC_BalanceBul(int Folio, int KartID)
        {



            //DataTable dt = Fronttools.SelectTable("select	ISNULL(SUM(case when Kumhrk_Ba = 'A' then Kumhrk_Tutar else (-1) * Kumhrk_Tutar end),0) as TL_Bakiye, "
            //                                        + "         ISNULL(SUM(case when Kumhrk_Ba = 'A' then Kumhrk_Doviz_tutar else (-1) * Kumhrk_Doviz_tutar end),0) as Doviz_Bakiye "
            //                                        + " from Kumhrk  "
            //                                        + " where Kumhrk_Re = 'E'  And ISNULL(Kumhrk_Kart_id,'') = '" + KartID + "' and Kumhrk_Rez_id = '" + Folio + "'");

            // AŞAĞISI SONRADAN DÜZENLENDİ RAMBO

            DataTable dt = Fronttools.SelectTable("select	ISNULL(SUM(case when Kumhrk_Ba = 'A' then Kumhrk_Tutar else (-1) * Kumhrk_Tutar end),0) as TL_Bakiye, "
                                                   + "         ISNULL(SUM(case when Kumhrk_Ba = 'A' then Kumhrk_Doviz_tutar else (-1) * Kumhrk_Doviz_tutar end),0) as Doviz_Bakiye "
                                                   + " from Kumhrk  "
                                                   + " where Kumhrk_Re = 'E'  And  Kumhrk_Rez_id = '" + Folio + "'");


            if (Param.Calisma_Sekli == 1) //Dovizli
            {
                return Convert.ToDecimal(dt.Rows[0]["Doviz_Bakiye"]);
            }
            else
            {
                return Convert.ToDecimal(dt.Rows[0]["TL_Bakiye"]);
            }
        }


        public static decimal NFCBakiye(int Folio, int KartID)
        {
            DataTable dt = Fronttools.SelectTable("exec StpKumhrk_Bul @xKumhrk_Tarih='" + Param.Tarih + "',@xKumhrk_Sirket='001',@xKumhrk_Posting_kodu=N'P',@xKumhrk_Re=N'E',@xKumhrk_Rez_id='" + Folio + "',@xtip=56, @Kumhrk_Kart_id = '" + KartID + "'");

            decimal bakiye = 0;

            if (dt.Rows.Count > 0)
            {
                bakiye = Convert.ToDecimal(dt.Rows[0]["Bakiye"]);
            }

            return bakiye;
        }


        public static string CardFIsim(int KartID)
        {
            return Fronttools.DegerGetir("select isnull(CardF_Ad,'') + ' ' +isnull(CardF_Soyad,'') as CardAdSoyad from KartF where ID = '" + KartID + "'");
        }

        public static decimal KurGetir(DateTime tarih, string kurkodu)
        {
            string cins;
            switch (Param.Doviz_Cinsi)
            {
                case 0:
                    cins = "M";
                    break;
                case 1:
                    cins = "E";
                    break;
                case 2:
                    cins = "M"; //Giriş Günü Kuru
                    break;
                default:
                    cins = "M";
                    break;
            }

            string query = "select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + cins + "' and Kurlar_Kodu = '" + kurkodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'";
            DataTable k = Fronttools.SelectTable(query);
            if (k.Rows.Count > 0)
            {
                return Convert.ToDecimal(k.Rows[0][Param.Doviz_Turu]);
            }
            else
            {
                return 0;
            }
        }

        public static int MasterFolioBul(int Folio)
        {
            DataTable dt = Fronttools.SelectTable("select top 1 case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Id from Rez with(nolock) where Rez_Id = '" + Folio.ToString() + "' "
                                                    + " AND  Rez_R_I_H = 'I' AND (Rez_Master_detay = 'M' OR Rez_Master_detay = 'E' OR Rez_Master_detay = 'D')");
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["Rez_Id"]);
            }
            else
            {
                return 0;
            }
        }

        public static bool HesapKapali_Kontrol(int Folio)
        {
            int masterFolio = MasterFolioBul(Folio);
            string Rez_Kapat_eh = Fronttools.DegerGetir("select ISNULL(Rez_Kapat_eh,'H') as Rez_Kapat_eh from Rez WITH(NOLOCK) where Rez_Id = '" + masterFolio.ToString() + "'");

            if (Rez_Kapat_eh == "E")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string LimitUyarı_Bul(int Folio)
        {
            string deger =Fronttools.DegerGetir("select ISNULL(Rez_limit_uyari_eh,'H') from Rez WITH(NOLOCK) Where Rez_Id = '" + Folio.ToString() + "'");
            return deger;
        }

        public static string CardFLimitUyarı_Bul(string KartID)
        {
            return Fronttools.DegerGetir("select ISNULL(CardF_Limit_Uyari,'E') from KartF WITH(NOLOCK) Where ID = '" + KartID.ToString() + "'");
        }

        public static string NFC_LimitUyarı_Bul(int Folio, string KartNo)
        {
            return Fronttools.DegerGetir("select CardF_MusteriTipi from KartF WITH(NOLOCK) Where CardF_RezID = '" + Folio.ToString() + "' and CardF_No = '" + KartNo + "'");
        }

        public static decimal Folio_LimitTutar_Bul(int Folio)
        {
            return Convert.ToDecimal(Fronttools.DegerGetir("select ISNULL(Rez_limit,0) from Rez WITH(NOLOCK) Where Rez_Id = '" + Folio.ToString() + @"'"));
        }

        public static bool Folio_LimitBakiye_Bul(int Folio)
        {
            int aa = Convert.ToInt32(Fronttools.DegerGetir("select ISNULL((select ISNULL(Rez_Limit_bakiye_10,0) from Rez WITH(NOLOCK) Where Rez_Id = '" + Folio.ToString() + "'),0)"));

            if (Param.Param_LimitFolio == true)
            {
                aa = 1;

            }

            if (aa == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string DovizAdi(string kod)
        {
            return Fronttools.DegerGetir("select Kodlar_Ad from Kodlar where Kodlar_Sinif = '02' and Kodlar_Kod = '" + kod + "'");
        }

        public static int RezID_Getir(string Oda)
        {
            return Convert.ToInt32(Fronttools.DegerGetir("select Rez_Id from Rez where Rez_R_I_H = 'I' and Rez_Odano = '" + Oda + "' and Rez_Master_detay = 'M'"));
        }

        public static bool Fis_Limit_bakiye_10_Bul()
        {
            return Convert.ToBoolean(Fronttools.DegerGetir("select ISNULL(Fis_Limit_bakiye_10,0) from Fishrk "));
        }
    }
}
