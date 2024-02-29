using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmosIngenicoGMP.Rmos
{
    public class Ayarlar
    {
        public bool fisteMiktar = false;
        public bool fisteUrun = false;
        public int port = 8910;
        public string dosyaYolu = "c:\\u\\sr\\yazarkasa";
        public string host = "127.0.0.1";
        public string Server = "RMOSB50";
        public string Database = "Back2019";
        public string User = "sa";
        public string Sifre = "123";
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
        public decimal KdvOran = 0;

    }
    class OdemeSatir
    {
        public int OdemeKodu = 1;
        public int Tutar = 0;
        public int Banka = 0;
        public string Adi = "";
    }
}
