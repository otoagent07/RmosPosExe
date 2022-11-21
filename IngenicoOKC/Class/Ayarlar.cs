using System;

namespace IngenicoOKC.Class
{
    class Ayarlar
    {
        public bool fisteMiktar = false;
        public bool fisteUrun = false;
        public int port = 8910;
        public string dosyaYolu = "c:\\u\\sr\\yazarkasa";
        public string host = "127.0.0.1";
        public string Server = "";
        public string Database = "";
        public string User = "";
        public string Sifre = "";

        private string dipnot = "";
        public string Dipnot
        {
            get
            {
                if (dipnot.Length > 32) return dipnot = dipnot.Substring(0, 31); else return dipnot;
            }
            set { this.dipnot = value; }
        }
        private string indnot = "** INDIRIM **";
        public string Indnot
        {
            get
            {
                if (indnot.Length > 32) return indnot = indnot.Substring(0, 31); else return indnot;
            }
            set { this.indnot = value; }
        }

        public string SatisProgram = "M";
    }

    class PluSatir
    {
        public string Adi = "";
        public string Barkod = "";
        public int KisimId = 0;
        public int Miktar = 0;
        public int Tutar = 0;
        public bool DiplomatKdvli = false;
        public bool DiplomatKdvsiz = false;
        public byte BirimTipi = 0; //0 Adet 2 KiloGram
    }
    class OdemeSatir
    {
        public int OdemeKodu = 1;
        public int Tutar = 0;
        public int Banka = 0;
        public string Adi = "";
    }
    class FaturaBilgi
    {
        public bool Irsaliye = false;
        public string Tckno = "";
        public string Vergino = "";
        public string No = "";
        public byte Tipi = 0; //0:Kagıt Fatura 1:E-Fatura 2:E-Arsiv
        public int Nakit = 0;
        public int KK = 0;
        public int Diger = 0;
        public int Tutar = 0;
        public DateTime Tarih;
    }

    class PKod
    {
        public string Kod = string.Empty;
        public string Ad = string.Empty;
        public string Sinif = string.Empty;
        public int Ykasaid = 0;
        public int Banka = 0;
    }

}
