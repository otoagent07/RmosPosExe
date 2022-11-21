using Dapper.Contrib.Extensions;
using System;

namespace Pos.Entity.Pos
{
    [Table("Pos_Kodlar")]
    public class KodlarEnt
    {
        [Key]
        public int Pkod_Id { get; set; }

        public string Pkod_Kod { get; set; }

        public string Pkod_Ad { get; set; }

        public string Pkod_Sinif { get; set; }

        public int? Pkod_Sorgu { get; set; }

        public int? Pkod_Ozelkod { get; set; }

        public bool? Pkod_Kasagiris { get; set; }

        public bool? Pkod_Kasacikis { get; set; }

        public bool? Pkod_Tekoda { get; set; }

        public string Pkod_Odano { get; set; }

        public string Pkod_Urungrup { get; set; }

        public string Pkod_OnburoKod { get; set; }

        public string Pkod_Konumkod { get; set; }

        public string Pkod_Ustgrup { get; set; }

        public string Pkod_Altgrup { get; set; }

        public int? Pkod_Satir { get; set; }

        public TimeSpan? Pkod_Hh_Bas { get; set; }

        public TimeSpan? Pkod_Hh_Bit { get; set; }

        public decimal? Pkod_Hh_Oran { get; set; }

        public bool? Pkod_Fatura { get; set; }

        public bool? Pkod_OnBuroKapatma { get; set; }

        public string Pkod_OnBuroKapatma_Departman { get; set; }

        public string Pkod_FisTipi { get; set; }

        public string Pkod_Mac { get; set; }

        public int? Pkod_Ciktisayisi { get; set; }

        public string PKod_Font { get; set; }

        public string Pkod_Dep { get; set; }

        public string Pkod_Printer { get; set; }

        public string Pkod_Ek_Pr1 { get; set; }

        public string Pkod_Ek_Pr2 { get; set; }

        public string Pkod_Ek_Pr3 { get; set; }

        public string Pkod_Ip { get; set; }

        public int? Pkod_Port { get; set; }

        public string Pkod_Ek1_Ip { get; set; }

        public int? Pkod_Ek1_Port { get; set; }

        public string Pkod_Ek2_Ip { get; set; }

        public int? Pkod_Ek2_Port { get; set; }

        public string Pkod_Ek3_Ip { get; set; }

        public int? Pkod_Ek3_Port { get; set; }

        public string Pkod_Ekran { get; set; }

        public int? Pkod_Sira { get; set; }

        public string Pkod_Bosrenk { get; set; }

        public string Pkod_Dolurenk { get; set; }

        public string Pkod_Hesaprenk { get; set; }

        public string Pkod_Posta { get; set; }

        public string Pkod_AbuyerPr { get; set; }

        public bool? Pkod_PaketNot { get; set; }

        public string Pkod_Server { get; set; }

        public string Pkod_Database { get; set; }

        public string Pkod_User { get; set; }

        public string Pkod_Password { get; set; }

        public string Pkod_MerkezSube { get; set; }

        public string Pkod_SubeMac { get; set; }

        public string Pkod_KapatmaKodu { get; set; }

        public string Pkod_KapatmaHesabi { get; set; }

        public bool? Pkod_DirekBakiye { get; set; }

        public string Pkod_MuhasebeBorc { get; set; }

        public string Pkod_MuhasebeAlacak { get; set; }

        public bool? Pkod_MuhasebeAktif { get; set; }

        public int? Pkod_AciklamaY { get; set; }

        public int? Pkod_AciklamaG { get; set; }

        public string Pkod_LinkServer { get; set; }

        public bool? Pkod_AdisyonPr { get; set; }

        public string Pkod_AbuyerPr2 { get; set; }

        public string Pkod_AbuyerPr3 { get; set; }

        public string Pkod_AbuyerPr4 { get; set; }

        public int? Pkod_YKasaid { get; set; }

        public int? Pkod_banka { get; set; }

        public int? Pkod_YS_OdemeID { get; set; }

        public string Pkod_AbuyerIP { get; set; }

        public int? Pkod_AbuyerPort { get; set; }

        public string Pkod_OdemeBtnRenk { get; set; }

        public string Pkod_AndroBosrenk { get; set; }

        public string Pkod_AndroDolurenk { get; set; }

        public string Pkod_AndroHesaprenk { get; set; }

        public string Pkod_IWEPayment { get; set; }

    }
}
