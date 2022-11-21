using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;
using System.Data.SqlClient;
using System.Resources;
using System.Reflection;

namespace Pos
{
    public partial class FisTopluIsleme : DevExpress.XtraEditors.XtraForm
    {
        public FisTopluIsleme()
        {
            InitializeComponent();
        }

        private void FisTopluIsleme_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime = Param.Tarih;

            string filter = "";
            if (User.P_Departman != "")
            {
                filter = " AND Kodlar_Kod IN ('" + User.P_Departman.Replace(", ", "','") + "')";
            }
            DataTable dt_Dep = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar with(nolock) where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 1 " + filter + " order by Kodlar_Kod");
            look_Dep.Properties.DataSource = dt_Dep;
            look_Dep.Properties.DisplayMember = "Kodlar_Ad";
            look_Dep.Properties.ValueMember = "Kodlar_Kod";
        }

        private void look_Dep_EditValueChanged(object sender, EventArgs e)
        {
            string depKod = Convert.ToString(look_Dep.EditValue);

            if (string.IsNullOrEmpty(depKod)) return;

            decimal nakitTutar = Convert.ToDecimal(dbtools.DegerGetir(@"select SUM(Rsat_Tutar)  as Tutar
from Cst_Recete_Satis
left join Pos_Kodlar on Pkod_Kod = Rsat_Kapatma and Pkod_Sinif = '11'
where CONVERT(date,Rsat_Tarih) = '" + dateEdit1.DateTime.Date + "' and Rsat_Ba = 'A' and Pkod_Ozelkod = 0 and Rsat_Durum = 'K' and Rsat_Departman = '" + depKod + "'"));

            decimal kkTutar = Convert.ToDecimal(dbtools.DegerGetir(@"select SUM(Rsat_Tutar)  as Tutar
from Cst_Recete_Satis
left join Pos_Kodlar on Pkod_Kod = Rsat_Kapatma and Pkod_Sinif = '11'
where CONVERT(date,Rsat_Tarih) = '" + dateEdit1.DateTime.Date + "' and Rsat_Ba = 'A' and Pkod_Ozelkod = 6 and Rsat_Durum = 'K' and Rsat_Departman = '" + depKod + "'"));

            spn_NakitToplam.Value = nakitTutar;
            spn_KKToplam.Value = kkTutar;

            spn_KKIsle.Value = kkTutar;
            spn_NakitIsle.Value = nakitTutar;

        }

        private void btn_KKIsle_Click(object sender, EventArgs e)
        {
            try
            {
                string depKod = Convert.ToString(look_Dep.EditValue);

                if (string.IsNullOrEmpty(depKod)) return;

                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_TopluIsle";

                com.Parameters.AddWithValue("@FisTarih", dateEdit1.DateTime.Date);
                com.Parameters.AddWithValue("@depKod", depKod);
                com.Parameters.AddWithValue("@OzelKod", 6);
                com.Parameters.AddWithValue("@Tutar", spn_KKIsle.Value);

                com.ExecuteNonQuery();
                con.Close();

                MessageBox.Show(res_man.GetString("Kredi Kartları İşlendi."));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string depKod = Convert.ToString(look_Dep.EditValue);

                if (string.IsNullOrEmpty(depKod)) return;

                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_TopluIsle";

                com.Parameters.AddWithValue("@FisTarih", dateEdit1.DateTime.Date);
                com.Parameters.AddWithValue("@depKod", depKod);
                com.Parameters.AddWithValue("@OzelKod", 0);
                com.Parameters.AddWithValue("@Tutar", spn_KKIsle.Value);

                com.ExecuteNonQuery();
                con.Close();

                MessageBox.Show(res_man.GetString("Nakitler İşlendi."));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}