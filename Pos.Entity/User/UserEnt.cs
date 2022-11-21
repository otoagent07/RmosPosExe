using Dapper.Contrib.Extensions;

namespace Pos.Entity.User
{
    [Table("Pos_User")]
    public class UserEnt
    {
        [Key]
        public int P_Id { get; set; }

        public string P_Kod { get; set; }

        public string P_Sifre { get; set; }

        public string P_Ad { get; set; }

        public string P_Soyad { get; set; }

        public string P_Kart { get; set; }

        public bool? G_Miktarduzelt { get; set; }

        public bool? G_Tutarduzelt { get; set; }

        public bool? G_Satirsil { get; set; }

        public bool? G_Indirim_Satis { get; set; }

        public bool? G_Hesapdokumu { get; set; }

        public bool? G_Odemeal { get; set; }

        public bool? G_Odemesil { get; set; }

        public bool? G_Indirim_Hesap { get; set; }

        public bool? G_Yazdirkapat { get; set; }

        public bool? G_Yazdirmadankapat { get; set; }

        public bool? G_Bindirim { get; set; }

        public bool? M_Masatakip { get; set; }

        public bool? M_Satis { get; set; }

        public bool? M_Masatransfer { get; set; }

        public bool? M_Malzemetransfer { get; set; }

        public bool? M_Ozelmasa { get; set; }

        public bool? M_Odakontrol { get; set; }

        public bool? M_Masakilitle { get; set; }

        public bool? M_Hesapkapatma { get; set; }

        public bool? D_Direksatis { get; set; }

        public bool? R_Raporlar { get; set; }

        public bool? R_Detay { get; set; }

        public bool? R_XZ { get; set; }

        public bool? R_Mahsupkes { get; set; }

        public bool? R_Fisiptal { get; set; }

        public bool? A_Ayarlar { get; set; }

        public bool? A_Parametre { get; set; }

        public bool? A_Print { get; set; }

        public bool? A_Odeme { get; set; }

        public bool? A_Entegre { get; set; }

        public bool? A_Masa { get; set; }

        public bool? A_Cari { get; set; }

        public bool? A_HH { get; set; }

        public bool? A_Kullanici { get; set; }

        public bool? P_Gunsonu { get; set; }

        public int? P_Kulturu { get; set; }

        public string P_Departman { get; set; }

        public bool? Pda_Masatakip { get; set; }

        public bool? Pda_Satis { get; set; }

        public bool? Pda_Satirsil { get; set; }

        public bool? Pda_Miktarduzelt { get; set; }

        public bool? Pda_Hesap { get; set; }

        public bool? Pda_Masatr { get; set; }

        public bool? Pda_Malztr { get; set; }

        public bool? Pda_Ozelmasa { get; set; }

        public bool? Pda_Odakontrol { get; set; }

        public bool? Pda_Direksatis { get; set; }

        public bool? M_SatisRelogin { get; set; }

        public bool? M_HesapTr { get; set; }

        public bool? A_Kasa { get; set; }

        public bool? K_Kasa { get; set; }

        public bool? And_Satis { get; set; }

        public bool? And_Satirsil { get; set; }

        public bool? And_Miktarduzelt { get; set; }

        public bool? And_Tutarduzelt { get; set; }

        public bool? And_Hesap { get; set; }

        public bool? And_Ozelmasa { get; set; }

        public bool? And_MasaTr { get; set; }

        public bool? And_Giris { get; set; }

        public bool? R_Fisiptalgecmis { get; set; }

        public string P_Posta { get; set; }

        public int? P_Indirim_Yuzde { get; set; }

        public int? P_Bindirim_Yuzde { get; set; }

        public string P_Sabit_Masa { get; set; }

        public bool? M_MasaAc { get; set; }

        public bool? M_BaskaMasa { get; set; }

        public bool? G_Satirsil_Y { get; set; }

        public bool? M_GarsonDegistir { get; set; }

        public bool? G_Zayi { get; set; }

        public bool? G_Ikram { get; set; }

        public bool? M_KisiSayisi { get; set; }

        public bool? R_MasaGeri { get; set; }

        public bool? M_SiparisTekrar { get; set; }

        public bool? Pda_HesapDok { get; set; }

        public bool? H_HizliSatis { get; set; }

        public bool? R_TopluIsle { get; set; }

        public bool? And_HesapDokum { get; set; }

        public bool? And_HesapOdeme { get; set; }

        public bool? And_MalzTransfer { get; set; }

        public bool? S_Sp_Sil { get; set; }

        public bool? ExtraFolio { get; set; }

        public bool? And_Yarim { get; set; }

        public bool? And_Tam { get; set; }

        public bool? And_Bucuk { get; set; }

        public bool? And_Duble { get; set; }

        public bool? Pos_SubeTrf { get; set; }

        public bool? Pos_AdisyonPr { get; set; }

        public bool? Pos_OdemeDegistir { get; set; }

        public bool? And_SatisSiparisBtn { get; set; }

        public bool? Pos_ArtiEksi_Aktif { get; set; }

        public bool? Pos_MasaAnlikDurum { get; set; }

        public bool? Pos_MasaUrunSil { get; set; }

        public bool? Pos_IWERep { get; set; }

        public bool? Pos_KartF_CheckOut { get; set; }

        public bool? Pos_SatirSilYetkili { get; set; }

        public bool? Pos_MasaDirekS { get; set; }

        public bool? Pos_MasaPaketS { get; set; }

        public bool? Pos_YS_YetkiReddet { get; set; }

        public bool? Pos_YarimDubleAlan { get; set; }

        public string Pos_Culture { get; set; }

        public bool? Pos_ReceteTanimlama { get; set; }

        public bool? And_BackEmpty { get; set; }

        public bool? Pos_FixMenu { get; set; }

        public bool? Pos_HesapArti { get; set; }

        public bool? User_AP { get; set; }
    }
}
