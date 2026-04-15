using Pos.YemekSepeti;
using System;
using System.Data;

namespace Pos.Class
{
    public class Departman
    {
        public static string Dep_Kodu { get; set; }
        public static string Dep_Adi { get; set; }
        public static string onburoFrontDbName { get; set; } // 15.04.2026 tarihinde önbüro adresi artık bu olacak şekilde ekledik
        public static int Sorgu_Sekli { get; set; }
        public static bool Siparis { get; set; }
        public static bool Adisyon { get; set; }
        public static bool Fatura { get; set; }
        public static int Dep_Kdv { get; set; }
        public static bool Kisi_Sor { get; set; }
        public static bool IlkSiparis { get; set; }
        public static bool Aktif_Kur { get; set; }
        public static bool Garson_Sor { get; set; }
        public static bool Kodlar_Hesap_Adisyon { get; set; }
        public static string Sube_Ad { get; set; }

        public static string MKodlar_P_DovizCins { get; set; }
        public static string MKodlar_P_DovizTuru { get; set; }
        public static string MKodlar_P_OtelKodu { get; set; }
        public static string MKodlar_P_SirketKodu { get; set; }

        public static bool Kodlar_Kuver_Sat { get; set; }
        public static string Kodlar_Kuver_Recete { get; set; }
        public static bool Kodlar_ServisPayi { get; set; }
        public static string Kodlar_Servis_Recete { get; set; }
        public static decimal Kodlar_Servis_Yuzde { get; set; }
        public static bool Kodlar_Mars { get; set; }
        public static bool Kodlar_Kisisorma_Pda { get; set; }
        public static bool Kodlar_Pr_Posta { get; set; }

        public static bool Kodlar_YazSipNedSor { get; set; }

        public static string Kodlar_PosSubeKod { get; set; }

        public static int Kodlar_RecBarSis { get; set; }

        public static bool Kodlar_AndPos_NFC { get; set; }

        public static string Kodlar_NFCKapatma { get; set; }

        public static bool Kodlar_PaketFiyat { get; set; }

        public static bool Kodlar_YS_Aktif { get; set; }

        public static bool Kodlar_Ingenico { get; set; }

        public static int Kodlar_IngenicoCon { get; set; }

        public static bool Kodlar_Ingenico_IWE { get; set; }

        public static bool Kodlar_GGFiyat { get; set; }

        public static bool Kodlar_GGAktif { get; set; }

        public static bool Kodlar_YerliYabanci { get; set; }

        public static bool Kodlar_PRSor { get; set; }

        public static bool Kodlar_Andro_Signature { get; set; }

        public static string Kodlar_YSMac { get; set; }

        public static bool Kodlar_SmsAktif { get; set; }
        public static string Kodlar_SmsBaslik { get; set; }
        public static string Kodlar_SmsKadi { get; set; }
        public static string Kodlar_SmsSifre { get; set; }
        public static string Kodlar_Getir_appSecretKey { get; set; }
        public static bool Kodlar_Getir_AP { get; set; }
        public static string Kodlar_Getir_restaurantSecretKey { get; set; }
        public static string Kodlar_Ingenico_IP { get; set; }
        public static int Kodlar_Ingenico_Port { get; set; }

        public static bool Kodlar_Beko { get; set; }



        public static string Dep_Param_Yukle()
        {
            string sonuc;

            DataTable dt = dbtools.SelectTable("select Mkodlar_Sirket_DB as onburoFrontDbName,Kodlar_Ad,isnull(Kodlar_Sorgu,0) as Kodlar_Sorgu, isnull(Kodlar_Siparis,0) as Kodlar_Siparis, isnull(Kodlar_Adisyon,0) as Kodlar_Adisyon, "
                    + " isnull(Kodlar_Fatura,0) as Kodlar_Fatura, isnull(Kodlar_Kdv,0) as Kodlar_Kdv, isnull(Kodlar_Kisi,0) as Kodlar_Kisi, isnull(Kodlar_IlkSiparis,0) as Kodlar_IlkSiparis, "
                    + " isnull(Kodlar_Aktif_Kur,0) as Kodlar_Aktif_Kur, isnull(Kodlar_Garson,0) as Kodlar_Garson,isnull(Kodlar_Hesap_Adisyon,0) as Kodlar_Hesap_Adisyon2, "
                    + " Mkodlar_Ad,MKodlar_P_DovizCins,MKodlar_P_DovizTuru,MKodlar_P_OtelKodu,MKodlar_P_SirketKodu, "
                    + " isnull(Kodlar_Kuver_Sat,0) as Kodlar_Kuver_Sat2,Kodlar_Kuver_Recete,isnull(Kodlar_ServisPayi,0) as Kodlar_ServisPayi2,Kodlar_Servis_Recete,isnull(Kodlar_Servis_Yuzde,0) as Kodlar_Servis_Yuzde2, "
                    + " isnull(Kodlar_Mars,0) as Kodlar_Mars2,isnull(Kodlar_Kisisorma_Pda,0) as Kodlar_Kisisorma_Pda2,ISNULL(Kodlar_Pr_Posta,0) as Kodlar_Pr_Posta2,ISNULL(Kodlar_YazSipNedSor,0) as Kodlar_YazSipNedSor2,Kodlar_PosSubeKod, "
                    + " ISNULL(Kodlar_RecBarSis,0) as Kodlar_RecBarSis, ISNULL(Kodlar_AndPos_NFC,0) as Kodlar_AndPos_NFC, ISNULL(Kodlar_PaketFiyat,0) as Kodlar_PaketFiyat , ISNULL(Kodlar_YS_Aktif,0) as Kodlar_YS_Aktif, "
                    + " ISNULL(Kodlar_NFCKapatma,'') as Kodlar_NFCKapatma, ISNULL(Kodlar_Ingenico,0) as Kodlar_Ingenico, ISNULL(Kodlar_IngenicoCon,0) as Kodlar_IngenicoCon, ISNULL(Kodlar_Ingenico_IWE,0) as Kodlar_Ingenico_IWE, "
                    + " ISNULL(Kodlar_GGFiyat,0) as Kodlar_GGFiyat , ISNULL(Kodlar_GGAktif,0) as Kodlar_GGAktif, ISNULL(Kodlar_YerliYabanci,0) as Kodlar_YerliYabanci, ISNULL(Kodlar_PRSor,0) as Kodlar_PRSor, "
                    + " ISNULL(Kodlar_Andro_Signature,0) as Kodlar_Andro_Signature,ISNULL(Kodlar_YSMac,'') as Kodlar_YSMac,ISNULL(Kodlar_SmsAktif,0) as Kodlar_SmsAktif, "
                    + " ISNULL(Kodlar_SmsBaslik,'') as Kodlar_SmsBaslik, ISNULL(Kodlar_SmsKadi,'') as Kodlar_SmsKadi, ISNULL(Kodlar_SmsSifre,'') as Kodlar_SmsSifre, "
                    + " ISNULL(Kodlar_Getir_appSecretKey,'') as Kodlar_Getir_appSecretKey , ISNULL(Kodlar_Getir_restaurantSecretKey,'') as Kodlar_Getir_restaurantSecretKey, "
                    + " ISNULL(Kodlar_Ingenico_IP,'127.0.0.1') as Kodlar_Ingenico_IP, ISNULL(Kodlar_Ingenico_Port,'8910') as Kodlar_Ingenico_Port, "
                    + " ISNULL(Kodlar_Getir_AP,0) as Kodlar_Getir_AP,"
                    + " ISNULL(Kodlar_Beko,0) as Kodlar_Beko "
                    + " from Stok_Kodlar with(nolock) "
                    + " left join Muh_Kodlar on Kodlar_Sirket = Mkodlar_Kod and Mkodlar_Sinif = '07' "
                    + " where  Kodlar_Sinif = '01' and Kodlar_Kod = '" + Dep_Kodu + "'");

            if (dt.Rows.Count > 0)
            {
                Sorgu_Sekli = Convert.ToInt32(dt.Rows[0]["Kodlar_Sorgu"]);
                Siparis = Convert.ToBoolean(dt.Rows[0]["Kodlar_Siparis"]);
                Adisyon = Convert.ToBoolean(dt.Rows[0]["Kodlar_Adisyon"]);
                Fatura = Convert.ToBoolean(dt.Rows[0]["Kodlar_Fatura"]);
                Dep_Kdv = Convert.ToInt32(dt.Rows[0]["Kodlar_Kdv"]);
                Kisi_Sor = Convert.ToBoolean(dt.Rows[0]["Kodlar_Kisi"]);
                IlkSiparis = Convert.ToBoolean(dt.Rows[0]["Kodlar_IlkSiparis"]);
                Aktif_Kur = Convert.ToBoolean(dt.Rows[0]["Kodlar_Aktif_Kur"]);
                Garson_Sor = Convert.ToBoolean(dt.Rows[0]["Kodlar_Garson"]);
                Kodlar_Hesap_Adisyon = Convert.ToBoolean(dt.Rows[0]["Kodlar_Hesap_Adisyon2"]);
                Dep_Adi = Convert.ToString(dt.Rows[0]["Kodlar_Ad"]);
                Sube_Ad = Convert.ToString(dt.Rows[0]["Mkodlar_Ad"]);


                onburoFrontDbName = Convert.ToString(dt.Rows[0]["onburoFrontDbName"]);


                Fronttools.conyenile(onburoFrontDbName); // 15.04.2026 eklendi.


                MKodlar_P_DovizCins = Convert.ToString(dt.Rows[0]["MKodlar_P_DovizCins"]);
                MKodlar_P_DovizTuru = Convert.ToString(dt.Rows[0]["MKodlar_P_DovizTuru"]);
                MKodlar_P_OtelKodu = Convert.ToString(dt.Rows[0]["MKodlar_P_OtelKodu"]);
                MKodlar_P_SirketKodu = Convert.ToString(dt.Rows[0]["MKodlar_P_SirketKodu"]);

                Kodlar_Kuver_Sat = Convert.ToBoolean(dt.Rows[0]["Kodlar_Kuver_Sat2"]);
                Kodlar_Kuver_Recete = Convert.ToString(dt.Rows[0]["Kodlar_Kuver_Recete"]);
                Kodlar_ServisPayi = Convert.ToBoolean(dt.Rows[0]["Kodlar_ServisPayi2"]);
                Kodlar_Servis_Recete = Convert.ToString(dt.Rows[0]["Kodlar_Servis_Recete"]);
                Kodlar_Servis_Yuzde = Convert.ToDecimal(dt.Rows[0]["Kodlar_Servis_Yuzde2"]);
                Kodlar_Mars = Convert.ToBoolean(dt.Rows[0]["Kodlar_Mars2"]);
                Kodlar_Kisisorma_Pda = Convert.ToBoolean(dt.Rows[0]["Kodlar_Kisisorma_Pda2"]);
                Kodlar_Pr_Posta = Convert.ToBoolean(dt.Rows[0]["Kodlar_Pr_Posta2"]);

                Kodlar_YazSipNedSor = Convert.ToBoolean(dt.Rows[0]["Kodlar_YazSipNedSor2"]);
                Kodlar_PosSubeKod = Convert.ToString(dt.Rows[0]["Kodlar_PosSubeKod"]);
                Kodlar_RecBarSis = Convert.ToInt32(dt.Rows[0]["Kodlar_RecBarSis"]);
                Kodlar_AndPos_NFC = Convert.ToBoolean(dt.Rows[0]["Kodlar_AndPos_NFC"]);
                Kodlar_PaketFiyat = Convert.ToBoolean(dt.Rows[0]["Kodlar_PaketFiyat"]);
                Kodlar_YS_Aktif = Convert.ToBoolean(dt.Rows[0]["Kodlar_YS_Aktif"]);
                Kodlar_NFCKapatma = Convert.ToString(dt.Rows[0]["Kodlar_NFCKapatma"]);
                Kodlar_Ingenico = Convert.ToBoolean(dt.Rows[0]["Kodlar_Ingenico"]);
                Kodlar_IngenicoCon = Convert.ToInt32(dt.Rows[0]["Kodlar_IngenicoCon"]);
                Kodlar_Ingenico_IWE = Convert.ToBoolean(dt.Rows[0]["Kodlar_Ingenico_IWE"]);
                Kodlar_GGFiyat = Convert.ToBoolean(dt.Rows[0]["Kodlar_GGFiyat"]);
                Kodlar_GGAktif = Convert.ToBoolean(dt.Rows[0]["Kodlar_GGAktif"]);
                Kodlar_YerliYabanci = Convert.ToBoolean(dt.Rows[0]["Kodlar_YerliYabanci"]);
                Kodlar_PRSor = Convert.ToBoolean(dt.Rows[0]["Kodlar_PRSor"]);
                Kodlar_Andro_Signature = Convert.ToBoolean(dt.Rows[0]["Kodlar_Andro_Signature"]);
                Kodlar_YSMac = Convert.ToString(dt.Rows[0]["Kodlar_YSMac"]);

                Kodlar_SmsAktif = Convert.ToBoolean(dt.Rows[0]["Kodlar_SmsAktif"]);
                Kodlar_Beko = Convert.ToBoolean(dt.Rows[0]["Kodlar_Beko"]);
                Kodlar_SmsBaslik = Convert.ToString(dt.Rows[0]["Kodlar_SmsBaslik"]);
                Kodlar_SmsKadi = Convert.ToString(dt.Rows[0]["Kodlar_SmsKadi"]);
                Kodlar_SmsSifre = Convert.ToString(dt.Rows[0]["Kodlar_SmsSifre"]);


                Kodlar_Getir_AP = Convert.ToBoolean(dt.Rows[0]["Kodlar_Getir_AP"]);
                Kodlar_Getir_appSecretKey = Convert.ToString(dt.Rows[0]["Kodlar_Getir_appSecretKey"]);
                Kodlar_Getir_restaurantSecretKey = Convert.ToString(dt.Rows[0]["Kodlar_Getir_restaurantSecretKey"]);

                Kodlar_Ingenico_IP = Convert.ToString(dt.Rows[0]["Kodlar_Ingenico_IP"]);
                Kodlar_Ingenico_Port = Convert.ToInt32(dt.Rows[0]["Kodlar_Ingenico_Port"]);



                if (Kodlar_YS_Aktif == true)
                {
                    YS_AuthHeader.YS_Yukle(Dep_Kodu);
                }

                sonuc = OnbServerAdres();
            }
            else
            {
                sonuc = "Departman Parametreleri Yüklenemedi";
            }

            return sonuc;
        }

        public static bool Dep_OtoSecim()
        {
            DataTable dtMac = dbtools.SelectTable("SELECT isnull(P_Tek,0) as P_Tek, P_Mac, P_Dep  FROM  RmosMuh.dbo.P_Bilg WHERE P_Mac='" + dbtools.MacAdresi() + "'");
            if (dtMac!=null && dtMac.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtMac.Rows[0]["P_Tek"]))
                {
                    string dep = Convert.ToString(dtMac.Rows[0]["P_Dep"]);

                    Dep_Kodu = Convert.ToString(dep);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static DataTable MuhasebeKod_Getir(DateTime Tarih1, DateTime Tarih2, string CariTF)
        {
            return dbtools.SelectTable("exec Muh_Plan_Liste @Tarih1='" + Tarih1 + "', @Tarih2='" + Tarih2 + "', @Yenile=0, @Plan_MuhCari = '" + CariTF + "'");
        }
        private static string OnbServerAdres()
        {
            //Önbüro Adresinin Bulunması
            DataTable dt = (dbtools.SelectTable("SELECT ISNULL(Param_Onburo,0) as Param_Onburo FROM  Pos_Param where Param_Id = '1'"));

            if (Convert.ToInt32(dt.Rows[0][0]) == 1)
            {
                DataTable dtOnb = dbtools.SelectTable("select Mkodlar_Sirket_Server,Mkodlar_Sirket_DB,Mkodlar_Sirket_User,Mkodlar_Sirket_Password, "
                                                + "         case when len(Mkodlar_Sirket_LinkServer) > 0 then Mkodlar_Sirket_LinkServer else '' end as Mkodlar_Sirket_LinkServer, "
                                                + "         MKodlar_P_OtelKodu,MKodlar_P_SirketKodu "
                                                + " from Muh_Kodlar "
                                                + " where Mkodlar_Sinif = '07' and Mkodlar_Kod = ( "
                                                + "            select top 1 Kodlar_Sirket "
                                                + "            from Stok_Kodlar "
                                                + "            where Kodlar_Sinif = '01' and Kodlar_Kod= '" + Departman.Dep_Kodu + "')");


                if (dtOnb.Rows.Count > 0)
                {
                    Fronttools.DB_Server = Convert.ToString(dtOnb.Rows[0]["Mkodlar_Sirket_Server"]);
                    Fronttools.DB_Database = Convert.ToString(dtOnb.Rows[0]["Mkodlar_Sirket_DB"]);
                    Fronttools.DB_User = Convert.ToString(dtOnb.Rows[0]["Mkodlar_Sirket_User"]);
                    Fronttools.DB_Pwd = Convert.ToString(dtOnb.Rows[0]["Mkodlar_Sirket_Password"]);
                    Fronttools.DB_LinkServer = Convert.ToString(dtOnb.Rows[0]["Mkodlar_Sirket_LinkServer"]);

                    Fronttools.Otel_Kodu = Convert.ToString(dtOnb.Rows[0]["MKodlar_P_OtelKodu"]);
                    Fronttools.Sirket_Kodu = Convert.ToString(dtOnb.Rows[0]["MKodlar_P_SirketKodu"]);

                    string ret2 = Fronttools.CheckDB();
                    if (ret2 != "OK")
                    {
                        if (Fronttools.DB_Server != "" && Fronttools.DB_Database != "" && Fronttools.DB_User != "" && Fronttools.DB_Pwd != "")
                        {
                            Fronttools.connstr = "Data Source=" + Fronttools.DB_Server + ";Initial Catalog=" + Fronttools.DB_Database + "; Persist Security Info=True; uid='" + Fronttools.DB_User + "'; pwd='" + Fronttools.DB_Pwd + "'";
                            Fronttools.conn.ConnectionString = Fronttools.connstr;

                            ret2 = Fronttools.CheckDB();
                            if (ret2 != "OK")
                            {
                                return "Önbüro Server Hatası Satış Yapmayınız...";
                            }
                            else
                            {
                                return "OK";
                            }
                        }
                        else
                        {
                            return "OK";
                        }
                    }
                    return "OK";
                }
                else
                {
                    return "Önbüro Server Hatası Satış Yapmayınız...";
                }

            }
            return "OK";
        }
    }
}