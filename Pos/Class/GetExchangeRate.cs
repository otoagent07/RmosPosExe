using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Windows.Forms;


namespace Pos.Class
{
    public static class GetExchangeRate
    {

        public static DataSet FindDayRecursive(DateTime date)
        {
            DataSet set = new DataSet();
            try
            {
                String URL = "http://www.tcmb.gov.tr/kurlar/" + date.Year.ToString() + (date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString()) + "/" + (date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString()) + (date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString()) + date.Year.ToString() + ".xml";
                XmlTextReader reader = new XmlTextReader(URL);
                set.ReadXml(reader);
                return set;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    set = FindDayRecursive(date.AddDays(-1));
                }
            }
            return set;

        }
        public static DovizAl ReadRate(DateTime date, string KOD)
        {
            DovizAl A = new DovizAl();
            if (KOD == "TL")
            {
                A.Doviz_Alis = 1;
                A.Doviz_Satis = 1;
                A.Efektif_Alis = 1;
                A.Efektif_Satis = 1;
                A.Kurlar_Kodu = "TL";
                return A;
            }
            try
            {
                DataView dv, dvDate;
                DataSet set = new DataSet();
                if (date == DateTime.Today)
                {
                    String URL = "http://www.tcmb.gov.tr/kurlar/today.xml";
                    XmlTextReader reader = new XmlTextReader(URL);
                    set.ReadXml(reader);
                }
                else
                {
                    set = FindDayRecursive(date.AddDays(-1));
                }

                dv = set.Tables[1].DefaultView;
                dvDate = set.Tables[0].DefaultView;

                dv.RowFilter = "KOD='" + KOD + "'";
                string tmp = "";
                tmp = Convert.ToString(dv[0]["ForexBuying"]);


                A.Doviz_Alis = Convert.ToDecimal(tmp.Replace(".", ","));
                tmp = Convert.ToString(dv[0]["ForexSelling"]);

                A.Doviz_Satis = Convert.ToDecimal(tmp.Replace(".", ","));
                tmp = Convert.ToString(dv[0]["BanknoteBuying"]);

                A.Efektif_Alis = Convert.ToDecimal(tmp.Replace(".", ","));
                tmp = Convert.ToString(dv[0]["BanknoteSelling"]);

                A.Efektif_Satis = Convert.ToDecimal(tmp.Replace(".", ","));
                A.Kurlar_Kodu = KOD;


            }
            //catch (Exception exception)
            catch (Exception ex)

            {
                //return "Error, " + exception.Message;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GC.Collect();


            }
            return A;
        }
    }

    public class DovizAl
    {
        public decimal Doviz_Alis = 0;
        public decimal Doviz_Satis = 0;
        public decimal Efektif_Alis = 0;
        public decimal Efektif_Satis = 0;
        public string Kurlar_Kodu = "";
    }

}