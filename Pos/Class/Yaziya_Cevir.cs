using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Class
{
    class Yaziya_Cevir
    {
        public static string Cevir(decimal tutar)
        {
            string sTutar = tutar.ToString("F2").Replace('.', ','); // Replace('.',',') ondalık ayracının . olma durumu için
            string lira = sTutar.Substring(0, sTutar.IndexOf(',')); //tutarın tam kısmı    
            string kurus = sTutar.Substring(sTutar.IndexOf(',') + 1, 2);
            string yazi = "";
            string[] birler = { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };
            string[] onlar = { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Altmış", "Yetmiş", "Seksen", "Doksan" };
            string[] binler = { "Katrilyon", "Trilyon", "Milyar", "Milyon", "Bin", "" }; //KATRİLYON'un önüne ekleme yapılarak artırabilir.     

            int grupSayisi = 6; //sayıdaki 3'lü grup sayısı. katrilyon içi 6. (1.234,00 daki grup sayısı 2'dir.) 
            //KATRİLYON'un başına ekleyeceğiniz her değer için grup sayısını artırınız.     

            lira = lira.PadLeft(grupSayisi * 3, '0'); //sayının soluna '0' eklenerek sayı 'grup sayısı x 3' basakmaklı yapılıyor.                         

            string grupDegeri;

            for (int i = 0; i < grupSayisi * 3; i += 3) //sayı 3'erli gruplar halinde ele alınıyor.   
            {
                grupDegeri = "";

                if (lira.Substring(i, 1) != "0")
                    grupDegeri += birler[Convert.ToInt32(lira.Substring(i, 1))] + "Yüz"; //yüzler 

                if (grupDegeri == "BirYüz") //biryüz düzeltiliyor. 
                    grupDegeri = "Yüz";

                grupDegeri += onlar[Convert.ToInt32(lira.Substring(i + 1, 1))]; //onlar 

                grupDegeri += birler[Convert.ToInt32(lira.Substring(i + 2, 1))]; //birler    

                if (grupDegeri != "") //binler
                    grupDegeri += binler[i / 3];   
                    if (grupDegeri == "BirBin") //birbin düzeltiliyor.       
                        grupDegeri = "Bin";

                yazi += grupDegeri;
            }

            if (yazi != "")
                yazi += " TL ";

            int yaziUzunlugu = yazi.Length;

            if (kurus.Substring(0, 1) != "0") //kuruş onlar    
                yazi += onlar[Convert.ToInt32(kurus.Substring(0, 1))];

            if (kurus.Substring(1, 1) != "0") //kuruş birler      
                yazi += birler[Convert.ToInt32(kurus.Substring(1, 1))];

            if (yazi.Length > yaziUzunlugu)
                yazi += " Kr.";
            else
                yazi += ""; // SIFIR Kr.
            return yazi;
        }
    }
}
