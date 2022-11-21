using System;
using System.Net;

namespace  IngenicoOKC.Class
{
    public static class cevir
    {
        static public bool objToBool(object Bilgi, bool varsayilan = false)
        {
            if (Bilgi == null) return varsayilan;
            if (string.IsNullOrEmpty(Bilgi.ToString()))
                return varsayilan;

            switch (Bilgi.ToString())
            {
                case "0": return false;
                case "H": return false;
                case "FALSE": return false;
                case "false": return false;
                case "False": return false;
                case "1": return true;
                case "E": return true;
                case "TRUE": return true;
                case "true": return true;
                case "True": return true;
                default: return varsayilan;
            }
        }
        static public string boolTo1_0(bool bilgi)
        {
            string sonuc = "0";

            if (bilgi) sonuc = "1";
            else
                sonuc = "0";

            return sonuc;
        }
        static public int objToInt32(object Bilgi, int varsayilan = 0)
        {
            if (Bilgi == null) return varsayilan;
            if (string.IsNullOrEmpty(Bilgi.ToString().Trim()) || !int.TryParse(Bilgi.ToString().Trim(), out varsayilan))
                return varsayilan;
            else
                return Convert.ToInt32(Bilgi);

        }
        static long IPToInt(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder(
                 (int)IPAddress.Parse(addr).Address);
        }
        static string IPToAddr(long address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
            // This also works:
            // return new IPAddress((uint) IPAddress.HostToNetworkOrder(
            //    (int) address)).ToString();
        }
        static public int tutarToIngc(decimal tutar)
        {
            return Convert.ToInt32(tutar * 100);
        }
        static public decimal objToDecimal(Object Bilgi, decimal varsayilan = 0, bool temizle = false)
        {
            if (Bilgi == null) return varsayilan;
            if (string.IsNullOrEmpty(Bilgi.ToString().Trim())) return varsayilan;

            //if (Bilgi.GetType() != typeof(decimal)) return varsayilan;

            string veri = Bilgi.ToString().Trim();

            if (temizle)
            {
                veri = tutarTemizle(Bilgi).ToString().Trim();

            }

            veri = veri.Replace(".", ",");
            if (string.IsNullOrEmpty(veri.ToString().Trim()))
                return varsayilan;
            else
                return Convert.ToDecimal(veri.ToString().Trim());

        }
        static public object tutarTemizle(object tutar)
        {
            object sonuc = null;
            if (tutar == null) return sonuc;

            string veri = cevir.objToStr(tutar).Trim();
            if (string.IsNullOrEmpty(veri)) return sonuc;

            string[] rakamlar = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ",", ".", "-" };
            string temiz = "";
            foreach (char item in veri)
            {
                if (Array.IndexOf(rakamlar, item.ToString()) >= 0)
                    temiz += item.ToString();
            }

            sonuc = temiz;

            return sonuc;
        }


        static public string objToStr(object Bilgi, string varsayilan = "")
        {

            if (Bilgi == null) return varsayilan;
            /*
            string sonuc = "";
            try
            {
                sonuc = (string)Bilgi;
            }
            catch
            {
                sonuc = varsayilan; 

            }
            */
            if (string.IsNullOrEmpty(Bilgi.ToString()))
                return varsayilan;
            else
                return Bilgi.ToString();


        }
        static public DateTime objToDateTime(object Bilgi)
        {
            DateTime varsayilan = new DateTime(1, 1, 1);

            if (Bilgi == null) return varsayilan;

            if (string.IsNullOrEmpty(Bilgi.ToString()))
                return varsayilan;
            else
                return Convert.ToDateTime(Bilgi);

        }

        static public string baglantiAdresiAl(object gln)
        {
            Ayarlar prm = new Ayarlar();
            prm = (Ayarlar)gln;

            string baglantiAdresi = "";

            baglantiAdresi = "Data Source=" + prm.Server +
                             ";Initial Catalog=" + prm.Database;

            baglantiAdresi += ";Persist Security Info=True;User ID=" + prm.User + ";Password=" + prm.Sifre;

            return baglantiAdresi;

        }

    }
}
