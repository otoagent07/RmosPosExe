using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Fis_Islem
    {
        public static void Manuel_Indirim(int Fisno, string indTipi, decimal tutar, decimal doviztutar, decimal oran, int Split, string neden = "")
        {

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Manuel_Indirim";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Ind_Tip", indTipi);
            com.Parameters.AddWithValue("@Ind_Tutar", tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Doviztutar", doviztutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Oran", oran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Turu", "MANUEL");
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Ind_User", User.P_Kod);
            com.Parameters.AddWithValue("@aciklama", neden);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Manuel_IndirimParcali(int Fisno, string indTipi, decimal tutar, decimal doviztutar, decimal oran, int Split, string neden = "", string masano = "")
        {

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Manuel_IndirimParcali";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Ind_Tip", indTipi);
            com.Parameters.AddWithValue("@Ind_Tutar", tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Doviztutar", doviztutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Oran", oran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Turu", "MANUEL");
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Ind_User", User.P_Kod);
            com.Parameters.AddWithValue("@aciklama", neden);
            com.Parameters.AddWithValue("@masano", masano);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Manuel_Bindirim(int Fisno, string indTipi, decimal tutar, decimal doviztutar, decimal oran, int Split)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Manuel_Bindirim";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Ind_Tip", indTipi);
            com.Parameters.AddWithValue("@Ind_Tutar", tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Doviztutar", doviztutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Oran", oran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Ind_Turu", "MANUEL");
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Ind_User", User.P_Kod);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Bindirim_Uygula(int Fisno, string bindTipi, decimal tutar, decimal doviztutar, decimal oran)
        {

            string query = "delete from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and Rsat_Recete=(select top 1 ISNULL(Param_Bindirim,0) as Param_Bindirim from Pos_Param)";
            dbtools.execcmd(query);


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Bindirim";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Bindirim_Recete", Param.Param_Bindirim);
            com.Parameters.AddWithValue("@Bindirim_Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Bindirim_Tip", bindTipi);
            com.Parameters.AddWithValue("@Bindirim_Tutar", tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_Doviztutar", doviztutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_Oran", oran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_User", User.P_Kod);

            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Bindirim_UygulaTipBox(int Fisno, string bindTipi, decimal tutar, decimal doviztutar, decimal oran)
        {

            string query = "delete from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and Rsat_Recete='" + Param.tipboxReceteKod + "'";
            dbtools.execcmd(query);


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Bindirim";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Bindirim_Recete", Param.tipboxReceteKod);
            com.Parameters.AddWithValue("@Bindirim_Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Bindirim_Tip", bindTipi);
            com.Parameters.AddWithValue("@Bindirim_Tutar", tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_Doviztutar", doviztutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_Oran", oran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_User", User.P_Kod);

            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }


        public static void Bindirim_UygulaParcali(int Fisno, string bindTipi, decimal tutar, decimal doviztutar, decimal oran, string masano = "")
        {

            string query = "delete from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and Rsat_Durum='A' and Rsat_Masa='" + masano + "' and  Rsat_Recete=(select top 1 ISNULL(Param_Bindirim,0) as Param_Bindirim from Pos_Param)";
            dbtools.execcmd(query);


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_BindirimParcali";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Bindirim_Recete", Param.Param_Bindirim);
            com.Parameters.AddWithValue("@Bindirim_Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Bindirim_Tip", bindTipi);
            com.Parameters.AddWithValue("@Bindirim_Tutar", tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_Doviztutar", doviztutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_Oran", oran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Bindirim_User", User.P_Kod);
            com.Parameters.AddWithValue("@masano", masano);

            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        static ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());


        public static void satisKurYenile(decimal dovizKur, string Fisno)
        {
            string dovizKur2 = dovizKur.ToString().Replace(",", ".");

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Fiyat=Rsat_Doviztutar*" + dovizKur2 + " where Rsat_Fisno='" + Fisno + "'");

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Tutar=Rsat_Fiyat where Rsat_Fisno='" + Fisno + "'");
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Net = Rsat_Tutar/(1+(Rsat_Kdvoran/100))   where Rsat_Fisno='" + Fisno + "'");
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kdv = Rsat_Tutar-(Rsat_Tutar/(1+(Rsat_Kdvoran/100))) where Rsat_Fisno='" + Fisno + "' ");
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kdv='0' where Rsat_Satistip='P' or Rsat_Satistip='O' and Rsat_Fisno='" + Fisno + "' ");


            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Fiyat=Rsat_Doviztutar*"+ dovizKur2 + " where Rsat_Fisno='"+ Fisno + "' and Rsat_Ba='B'");

            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Tutar=Rsat_Fiyat where Rsat_Fisno='"+ Fisno + "' and Rsat_Ba='B'");
            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Net = Rsat_Tutar/(1+(Rsat_Kdvoran/100))   where Rsat_Fisno='"+ Fisno + "' and Rsat_Ba='B'");
            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kdv = Rsat_Tutar-(Rsat_Tutar/(1+(Rsat_Kdvoran/100))) where Rsat_Fisno='"+ Fisno + "' and Rsat_Ba='B'");
            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kdv='0' where Rsat_Satistip='P' or Rsat_Satistip='O' and Rsat_Fisno='"+ Fisno + "' and Rsat_Ba='B'");


        }

        public static string kurYap(string odano)
        {
            string deger = "1";
            try
            {
                deger = "select Rez_Kur_uygulanan from rez where Rez_Odano='" + odano + "' and Rez_R_I_H='I'";
                deger = Fronttools.DegerGetir(deger);
                decimal kurum = 0;
                if (deger.Equals("") || deger.ToLower().Equals("null"))
                {
                    deger = "";
                }
                else
                {
                    kurum = Convert.ToDecimal(deger); //1
                }

                if (kurum == 0)
                {
                    deger = "update rez set  Rez_Kur_uygulanan ='" + Param.Doviz_Kuru.ToString().Replace(",", ".") + "' where Rez_Odano='" + odano + "' and Rez_R_I_H='I'";

                    Fronttools.execcmd(deger);
                }

                if (kurum == 1)
                {
                    string merkez = Fronttools.DegerGetir("select top 1 Fis_Doviz_me from Fishrk");

                    if (!merkez.Equals("G"))
                    {
                        string query = @"select top 1 Doviz_Alis from Kurlar where Kurlar_Tarih=(select top 1 Rez_Giris_tarihi from rez where Rez_R_I_H='I' and Rez_Odano='" + odano + @"') 
and Kurlar_Kodu=(select top 1 Fis_Extra_dovizkodu from Fishrk)
and Kurlar_Cesit=(select top 1 Fis_Doviz_me from Fishrk)";

                        string dovizAlisKur = Fronttools.DegerGetir(query).Replace(",", ".");

                        Fronttools.execcmd("update rez set  Rez_Kur_uygulanan ='" + dovizAlisKur + "' where Rez_Odano='" + odano + "' and Rez_R_I_H='I'");

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return deger;

        }

        public static void Odeme_Al(int Fisno, decimal tutar, decimal doviztutar, string kapatma, string mus_tipi, string odano, int folio, string cari, int Split, string dovizkodu, bool ads, decimal mevcutTutar = 0,string kisiyeAdSoyad="")
        {
            try
            {
                int kursonuc = -1;
                if (odano != null && odano !="null" && odano !="")
                {
                    string sorgu1 = "select isnull(Rez_Kur_uygulanan,0) as Rez_Kur_uygulanan from rez where Rez_Odano='" + odano + "' and Rez_R_I_H='I'";
                    string kisininKuru = Fronttools.DegerGetir(sorgu1);
                     kursonuc = (int)Convert.ToDecimal(kisininKuru);
                }
             

                string deger = "";

                if (odano != null && odano != "") // önbüro ise demek sonradan yapıldı. 23.09.2022 
                {
                    deger = kurYap(odano);
                }

                //Param.Param_Yukle();

                if (Param.Tesis_Tipi == 0)
                {
                    if ((folio == 0 || odano == "") && cari == "")
                    {
                        MessageBox.Show(res_man.GetString("Folio Bulunamadı...") + "\n" + res_man.GetString("Lütfen Hesabı Tekrar Kapatın..."), res_man.GetString("Uyarı"), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }

                decimal kur = Param.Doviz_Kuru;
                string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";


                if (Param.Tesis_Tipi == 0)   //Otel 
                {
                    if (Param.Calisma_Sekli == 1)   //Dövizli
                    {
                        if (Param.Doviz_Cinsi == 2) //Müşteri Giriş Günü Kuru
                        {
                            int Master_folio = Convert.ToInt32(Fronttools.DegerGetir("select top 1 isnull(Rez_Master_id,Rez_Id) from Rez WITH(NOLOCK) where Rez_Id = '" + folio.ToString() + "' "));
                            DateTime Giris_tarihi = Convert.ToDateTime(Fronttools.DegerGetir("select top 1 Rez_Giris_tarihi from Rez WITH(NOLOCK) where Rez_Id = '" + Master_folio.ToString() + "' "));
                            kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizkodu + "' and  Convert(date,Kurlar_Tarih,105) = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));
                            tutar = doviztutar * kur;
                        }
                        else
                        {
                            if (Param.Kurlar_Nerden == 0) // otel
                            {
                                kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizkodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));

                                string onburoGirisKurdanmiAlinsi = Fronttools.DegerGetir("select top 1 Fis_Hesapgir_kur_e_gk from Fishrk"); // K İSE GİRİŞTEN G ise günlük

                                if (odano != null && onburoGirisKurdanmiAlinsi.Equals("K"))
                                {
                                    string girisKur = Fronttools.DegerGetir("select top 1 Rez_Kur_uygulanan from rez where Rez_Odano='" + odano + "' and Rez_R_I_H='I'"); // K İSE GİRİŞTEN G ise günlük

                                    //tutar = doviztutar * Convert.ToDecimal(girisKur);
                                    kur = Convert.ToDecimal(girisKur);

                                    if (kur == 0)
                                    {
                                        kur = Param.Doviz_Kuru;
                                    }

                                    satisKurYenile(kur, Fisno.ToString());
                                }
                            }
                            else
                            {
                                string text = "select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizkodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'";
                                string query = Fronttools.DegerGetir(text);

                                kur = Convert.ToDecimal(query);
                            }
                            tutar = doviztutar * kur;
                        }
                    }
                    else
                    {
                        if (Param.Kurlar_Nerden == 0) // otel
                        {
                            kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizkodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));

                        }
                        else
                        {
                            kur = Convert.ToDecimal(dbtools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizkodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        }
                        tutar = doviztutar * kur;
                    }
                }
                if (Param.Tesis_Tipi == 1)
                {
                    string dovizXml = dbtools.DegerGetir("select Mkodlar_Xml from Muh_Kodlar where Mkodlar_Sinif = '02' and Mkodlar_Kod = '" + dovizkodu + "'");
                    //if (!(dovizXml == "" || dovizXml == "TL"))
                    //{
                    kur = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizkodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'),1)"));
                    tutar = doviztutar * kur;
                    //}
                }
               
                if ((Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1 && mevcutTutar != 0) || Math.Abs(mevcutTutar - tutar) <=(decimal)0.2)
                {
                    tutar = StatikSinif.getTutarKontrol(tutar, mevcutTutar);
                }


                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_Odeme";
                com.Parameters.AddWithValue("@Tarih", Param.Tarih);
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Tutar", tutar.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Doviztutar", doviztutar.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Dovizkur", kur.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Kapatma", kapatma);
                com.Parameters.AddWithValue("@Mustipi", mus_tipi);
                com.Parameters.AddWithValue("@Odano", odano);
                com.Parameters.AddWithValue("@Folio", folio);
                com.Parameters.AddWithValue("@Cari", cari);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@DovizKodu", dovizkodu);
                com.Parameters.AddWithValue("@UserKod", User.P_Kod);
                com.Parameters.AddWithValue("@Ads", ads);
                com.Parameters.AddWithValue("@kisiyeSatisAdSoyad", kisiyeAdSoyad);
                if (Departman.Kodlar_Ingenico_IWE == true) com.Parameters.AddWithValue("@Rsat_Ingenico_Status", 1);


                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) con.Close();

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, Fisno.ToString() + " Fiş Ödeme Alındı. Tutar:" + tutar.ToString() + " Kod: " + kapatma, Fisno.ToString(), "");


                //if (!deger.Equals("") && deger.Equals("1"))
                //{
                //    Fronttools.execcmd("update rez set  Rez_Kur_uygulanan ='1' where Rez_Odano='" + odano + "' and Rez_R_I_H='I'");
                //} 

                if (kursonuc == 1)
                {
                    Fronttools.execcmd("update rez set  Rez_Kur_uygulanan ='1' where Rez_Odano='" + odano + "' and Rez_R_I_H='I'");
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Odeme_Al", "", ex);
            }
        }

        public static void Onburo_At(int Fisno, string KartNo, int KartId, string ozelKod = "")
        {
            try
            {
                // Satis Hesaba Atılıyor
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_Onburo";
                com.Parameters.AddWithValue("@Tarih", Param.Tarih.Date);
                com.Parameters.AddWithValue("@Fisno", Fisno.ToString());
                com.Parameters.AddWithValue("@FolioKartNo", KartNo);// ==null?"":KartNo
                com.Parameters.AddWithValue("@FolioKartId", KartId);
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) con.Close();

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Durum = 'K' where Rsat_Fisno = '" + Fisno.ToString() + "'");


                if (ozelKod == "3" && Param.onburoikramsifiryazaktif) // 3 ikramdır
                {
                    Fronttools.execcmd("update Kumhrk set Kumhrk_Doviz_tutar='0',Kumhrk_Tutar='0',Kumhrk_Def_doviz='0',Kumhrk_Aciklama='POS: İKRAM, Fişno:" + Fisno + "' where Kumhrk_Pos_no='" + Fisno + "' and isnull(Kumhrk_Pos_no,0) > 0 ");
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Onburo_At", "", ex);
            }

        }
        public static string MyClass = "Fis_Islem";
        public static void Satir_Sil(int Id, decimal Miktar)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satir_Sil";
            com.Parameters.AddWithValue("@Rsat_Id", Id);
            com.Parameters.AddWithValue("@Miktar", Miktar);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void ServisPayi_Sil(int Id, decimal Miktar)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_ServisPayi_Sil";
            com.Parameters.AddWithValue("@Rsat_Id", Id);
            com.Parameters.AddWithValue("@Miktar", Miktar);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Satis_Tip(int Fisno, string Kapatma, string Pansiyon)
        {
            string satisTip = Convert.ToString(dbtools.DegerGetir("select Pkod_FisTipi from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Kod= '" + Kapatma + "'"));

            //if (Pansiyon == "AL" || Pansiyon == "ALL")
            //{
            //    satisTip = "V";
            //}

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Satistip = '" + satisTip + "' where Rsat_Fisno = '" + Fisno.ToString() + "' ");

        }

        public static void Fatura_Kes(int Fisno, bool tumFis, bool departmanIptal = false, string Tips = "F")
        {
            if (!departmanIptal)
            {
                if (!Departman.Fatura)
                {
                    return;
                }
            }


            string filter = string.Empty;
            if (!tumFis)
            {
                filter = " and Pkod_Fatura = 1 ";
            }

            DataTable dtFat = new DataTable();
            if (Tips == "F")
            {
                dtFat = dbtools.SelectTable(@"

                select 
                SUM(ISNULL(Rsat_Tutar,0)) as Tutar,
                MIN(Rsat_Kapatma) as Kapatma "
                    + " from Cst_Recete_Satis WITH(NOLOCK) "
                    + " LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' "
                   + " where Rsat_Fisno = '" + Convert.ToString(Fisno) + "' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A' "
                    + " group by Pkod_Fatura");
                // " + filter

            }
            else
            {
                dtFat = dbtools.SelectTable(@"

                select 
                SUM(ISNULL(Rsat_Tutar,0)) as Tutar,
                MIN(Rsat_Kapatma) as Kapatma "
                   + " from Cst_Recete_Satis WITH(NOLOCK) "
                   + " LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' "
                  + " where Rsat_Kart_ID = '" + Convert.ToString(Fisno) + "' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A' "
                   + " group by Pkod_Fatura");
                // " + filter
            }
            decimal tutar = 0;
            for (int i = 0; i < dtFat.Rows.Count; i++)
            {
                tutar += Convert.ToDecimal(dtFat.Rows[i]["Tutar"]);
            }
            string odemeKodu = dtFat.Rows.Count > 0 ? Convert.ToString(dtFat.Rows[0]["Kapatma"]) : "";

            Fatura fat = new Fatura();
            fat.Tag = Fisno.ToString();
            fat.Tip = Tips;
            fat.Odemekodu = odemeKodu;
            fat.TL_Tutar = tutar;
            fat.ShowDialog();


        }

        public static void Not_Ekle(int Fisno, string Not)
        {
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Not = '" + Not + "' where Rsat_Fisno = '" + Fisno + "' ");
        }

        public static void SiparisNotUpdate(int Id, string SiparisNot)
        {
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Aciklama = '" + SiparisNot.ToString() + "' where Rsat_Id = '" + Id.ToString() + "'");
        }

        public static string SiparisNotGetir(int Id)
        {
            return dbtools.DegerGetir("select Rsat_Aciklama from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Id = '" + Id.ToString() + "'");
        }

        public static void Mars_Update(int Fisno, int maxId)
        {
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Mars = 1 where Rsat_Fisno = '" + Fisno.ToString() + "' and Rsat_Id <= '" + maxId.ToString() + "'");
        }

        public static int Satir_Kopyala(int Rsat_Id)
        {
            return Convert.ToInt32(dbtools.DegerGetir("INSERT INTO Cst_Recete_Satis "
                + " SELECT     Rsat_Fisno, Rsat_Tarih, Rsat_Departman, Rsat_Recete, Rsat_Kdvoran, Rsat_Miktar, Rsat_Fiyat, Rsat_Ind, Rsat_Net, Rsat_Kdv, Rsat_Tutar, Rsat_Maliyet, Rsat_Dovizkodu, Rsat_Doviztutar, "
                + "     Rsat_Satistip, Rsat_Odenmez, Rsat_Odano, Rsat_Folio, Rsat_Ba, Rsat_Kapatma, Rsat_Adisyon, Rsat_Aktiffisno, Rsat_Masa, Rsat_Garson, Rsat_Kisi, Rsat_Acilis, Rsat_Kapanis, Rsat_Durum, "
                + "     Rsat_Cari, Rsat_Split, Rsat_Aciklama, Rsat_SiparisPr, Rsat_AdisyonPr, Rsat_Paketci, Rsat_Emiktar, Rsat_AdisPr, Rsat_Garson2, Rsat_AdisPrSayac, Rsat_Uye_Kart_Turu, Rsat_Satir_Iptal, "
                + "     Rsat_Pansiyon, Rsat_Happy_Hour, Rsat_MusTipi, Rsat_Uye_Id, Rsat_Uye_Ad, Rsat_Indkodu, Rsat_Indoran, Rsat_Onbdep, Rsat_Indsatirid, Rsat_Satissaat, Rsat_Dovizkur, Rsat_Satir_Iptalsaat, "
                + "     Rsat_Indsatirid2, Rsat_Not, Rsat_Kartno, Rsat_Pda, Rsat_Mars, Rsat_Yapildi, Rsat_Splitad, Rsat_Hesap_Kilit,[Rsat_Zayi],[Rsat_Ikram],[Rsat_AbuyerPr],[Rsat_Yapma],[Rsat_ZayiNeden],[Rsat_IkramNeden],[Rsat_Sube],[Rsat_SubeDurum] "
                + " FROM   Cst_Recete_Satis "
                + " WHERE     Rsat_Id =  " + Rsat_Id
                + " select SCOPE_IDENTITY()"));
        }

        public static DataTable Doviz_Dagilim(decimal Bakiye)
        {
            string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
            decimal kur = Convert.ToDecimal(dbtools.DegerGetir("select ISNULL((select ISNULL(" + Param.Doviz_Turu + ",0) from Kurlar where Kurlar_Kodu = '" + Param.Doviz_Kodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' and Kurlar_Cesit = '" + kur_cesit + "'),4)"));

            Bakiye = Bakiye * kur;

            string query = "select Mkodlar_Kod,Mkodlar_Ad,ISNULL(" + Param.Doviz_Turu + ",0) as Kur, "
                + "convert(decimal(18,4),(" + Bakiye.ToString().Replace(",", ".") + " / ISNULL(" + Param.Doviz_Turu + ",0))) as Doviz "
                + " from Muh_Kodlar  "
                + " left join Kurlar on Kurlar_Kodu = Mkodlar_Kod and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' and Kurlar_Cesit = '" + kur_cesit + "' "
                + " where Mkodlar_Sinif = '02' "
                //+ " and Mkodlar_Xml <> 'TL' "
                + " and Doviz_Alis > 0 "
                + " order by Mkodlar_Kod";

            DataTable dt = dbtools.SelectTable(query);
            return dt;
        }

        public static DataTable Doviz_DagilimFront(decimal Bakiye)
        {
            string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
            decimal kur = Convert.ToDecimal(Fronttools.DegerGetir("select ISNULL((select ISNULL(" + Param.Doviz_Turu + ",0) from Kurlar where Kurlar_Kodu = '" + Param.Doviz_Kodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' and Kurlar_Cesit = '" + kur_cesit + "'),1)"));

            Bakiye = Bakiye * kur;

            return Fronttools.SelectTable("select Kodlar_Kod as Mkodlar_Kod,Kodlar_Ad as Mkodlar_Ad,ISNULL(" + Param.Doviz_Turu + ",0) as Kur, "
                + "convert(decimal(18,2),(" + Bakiye.ToString().Replace(",", ".") + " / NULLIF(ISNULL(" + Param.Doviz_Turu + ",0),0))) as Doviz "
                + " from Kodlar "
                + " left join Kurlar on Kurlar_Kodu = Kodlar_Kod  and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' and Kurlar_Cesit = '" + kur_cesit + "' "
                + " where Kodlar_Sinif = '02'"
                + " and Doviz_Alis > 0"
                + " order by Kodlar_Kod");
        }

        public static DataTable Doviz_DagilimFront2(decimal Bakiye, string KurkoDu)
        {
            string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
            decimal kur = Convert.ToDecimal(Fronttools.DegerGetir("select ISNULL((select ISNULL(" + Param.Doviz_Turu + ",0) from Kurlar where Kurlar_Kodu = '" + KurkoDu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' and Kurlar_Cesit = '" + kur_cesit + "'),1)"));

            Bakiye = Bakiye * kur;

            return Fronttools.SelectTable("select Kodlar_Kod as Mkodlar_Kod,Kodlar_Ad as Mkodlar_Ad,ISNULL(" + Param.Doviz_Turu + ",0) as Kur, "
                + "convert(decimal(18,2),(" + Bakiye.ToString().Replace(",", ".") + " / NULLIF(ISNULL(" + Param.Doviz_Turu + ",0),0))) as Doviz "
                + " from Kodlar "
                + " left join Kurlar on Kurlar_Kodu = Kodlar_Kod  and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' and Kurlar_Cesit = '" + kur_cesit + "' "
                + " where Kodlar_Sinif = '02'"
                + " and Doviz_Alis > 0"
                + " order by Kodlar_Kod");
        }

        public static void Tutar_Duzelt(int Id, decimal tutar)
        {
            string query = "";


            if (Param.Calisma_Sekli == 1)   //Dövizli
            {

                decimal TL_Tutar = tutar * Param.Doviz_Kuru;
                string tl_tutar2 = TL_Tutar.ToString().Replace(",", ".");

                query = "update Cst_Recete_Satis set Rsat_Doviztutar = " + tutar.ToString().Replace(",", ".") + ", "
                + " Rsat_Tutar = " + TL_Tutar.ToString().Replace(",", ".") + " , "
                + " Rsat_Net = " + tl_tutar2 + "/((100+Rsat_Kdvoran)/100), "
                + " Rsat_Kdv = " + tl_tutar2 + "-(" + tl_tutar2 + ")/ ((100+Rsat_Kdvoran)/100)"
                + " where Rsat_Id = " + Id;

                dbtools.execcmd(query);

            }
            else
            {
                string tutar2 = tutar.ToString().Replace(",", ".");


                query = "update Cst_Recete_Satis set Rsat_Tutar = " + tutar.ToString().Replace(",", ".") + " , "
                + " Rsat_Net = " + tutar2 + "/((100+Rsat_Kdvoran)/100), "
                + " Rsat_Kdv = " + tutar2 + "-(" + tutar2 + ")/ ((100+Rsat_Kdvoran)/100) ,"
                + " Rsat_Doviztutar = " + (tutar / Param.Doviz_Kuru).ToString().Replace(",", ".") + " "
                + " where Rsat_Id = " + Id;

                dbtools.execcmd(query);


            }
        }

        public static void Tutar_DuzeltYedek(int Id, decimal tutar)
        {
            if (Param.Calisma_Sekli == 1)   //Dövizli
            {
                decimal TL_Tutar = tutar * Param.Doviz_Kuru;
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Doviztutar = " + tutar.ToString().Replace(",", ".") + ", "
                + " Rsat_Tutar = " + TL_Tutar.ToString().Replace(",", ".") + " , "
                + " Rsat_Net = " + TL_Tutar.ToString().Replace(",", ".") + " * (100 - Rsat_Kdvoran) / 100, "
                + " Rsat_Kdv = " + TL_Tutar.ToString().Replace(",", ".") + " * Rsat_Kdvoran /100 "
                + " where Rsat_Id = " + Id);
            }
            else
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Tutar = " + tutar.ToString().Replace(",", ".") + " , "
                + " Rsat_Net = " + tutar.ToString().Replace(",", ".") + " * (100 - Rsat_Kdvoran) / 100, "
                + " Rsat_Kdv = " + tutar.ToString().Replace(",", ".") + " * Rsat_Kdvoran /100, "
                + " Rsat_Doviztutar = " + (tutar / Param.Doviz_Kuru).ToString().Replace(",", ".") + " "
                + " where Rsat_Id = " + Id);
            }
        }

        public static void Zayi(int Id, int Fisno, string neden, decimal miktar)
        {


            if (StatikSinif.getDovizlimi())
            {
                dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_SiparisPr = 0, Rsat_Zayi = 1,Rsat_Miktar = " + miktar.ToString().Replace(",", ".") + ",Rsat_Tutar = 0,Rsat_Doviztutar=0,Rsat_ZayiNeden = '" + neden + "' where Rsat_Id = '" + Id + "'");
            }
            else
            {
                dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_SiparisPr = 0, Rsat_Zayi = 1,Rsat_Miktar = " + miktar.ToString().Replace(",", ".") + ",Rsat_Tutar = 0,Rsat_ZayiNeden = '" + neden + "' where Rsat_Id = '" + Id + "'");
            }

            ServisPayi(Fisno);

        }

        public static void Kisi_Sayisi(int Fisno, int Kisi_Sayisi)
        {
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kisi = '" + Kisi_Sayisi + "' where Rsat_Fisno = '" + Fisno + "'");
        }

        public static void ServisPayi(int Fisno)
        {
            dbtools.execcmd("exec Pos_ServisPayi @Fisno = " + Fisno);
        }
    }
}
