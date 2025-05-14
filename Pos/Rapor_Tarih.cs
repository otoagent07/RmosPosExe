using DevExpress.XtraSplashScreen;
using Pos.Class;
using Pos.Controllers;
using Pos.Setting;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Rapor_Tarih : DevExpress.XtraEditors.XtraForm
    {
        public Rapor_Tarih()
        {
            InitializeComponent();
        }

        private void Yuk()
        {
            DataTable dt = dbtools.SelectTable("select Kodlar_Kod as Kodu,Kodlar_Ad as Adi from Stok_Kodlar as s where s.Kodlar_Sinif = '01' and Kodlar_Satis = 1");
            if (dt.Rows.Count > 0)
            {
                lookUpEdit1.Properties.DataSource = dt;
                lookUpEdit1.Properties.DisplayMember = "Adi";
                lookUpEdit1.Properties.ValueMember = "Kodu";
                lookUpEdit1.CheckAll();
                //lookUpEdit1.SetEditValue(Departman.Dep_Kodu);

                lookUpEdit1.Enabled = true;
            }

            DataTable dtSub = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '27' order by Pkod_Kod");
            if (dtSub.Rows.Count > 0)
            {
                chkCombo_Sube.Properties.DataSource = dtSub;
                chkCombo_Sube.Properties.DisplayMember = "Pkod_Ad";
                chkCombo_Sube.Properties.ValueMember = "Pkod_Kod";
                if (Set_Sube.Default.Sube == "") { chkCombo_Sube.CheckAll(); } else { chkCombo_Sube.SetEditValue(Departman.Dep_Kodu); }
                chkCombo_Sube.Enabled = true;
            }
        }

        private void Rapor_Tarih_Load(object sender, EventArgs e)
        {
            //Param.Param_Yukle();
            date_Tarih1.EditValue = Param.Tarih;
            if (checkEdit1.Checked == true)
            {
                Yuk();
            }
            else
            {
                lookUpEdit1.Properties.DataSource = null;
            }
        }

        public bool cikis = false;

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());


        public static string MyClass = "Rapor_Tarih";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {

                //Set_Sube.Default.Sube = Convert.ToString(chkCombo_Sube.EditValue);
                //Set_Sube.Default.Save();


                if (date_Tarih1.EditValue == null)
                {
                    MessageBox.Show(res_man.GetString("Tarih Alanı Boş Geçilemez."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                cikis = true;
                this.Close();

            }
            catch(Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "simpleButton1_Click", "",ex);
            }

        }

        


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            cikis = false;
            this.Close();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked == true)
            {
                DataTable dt = dbtools.SelectTable("select Kodlar_Kod as Kodu,Kodlar_Ad as Adi from Stok_Kodlar as s where s.Kodlar_Sinif = '01' and Kodlar_Satis = 1");
                if (dt.Rows.Count > 0)
                {
                    lookUpEdit1.Properties.DataSource = dt;
                    lookUpEdit1.Properties.DisplayMember = "Adi";
                    lookUpEdit1.Properties.ValueMember = "Kodu";
                    lookUpEdit1.CheckAll();
                    lookUpEdit1.Enabled = true;
                }
            }
            else
            {
                lookUpEdit1.Enabled = false;
                lookUpEdit1.Properties.DataSource = null;
                lookUpEdit1.EditValue = null;
            }
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit2.Checked == true)
            {
                DataTable dtSub = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '27' order by Pkod_Kod");
                if (dtSub.Rows.Count > 0)
                {
                    chkCombo_Sube.Properties.DataSource = dtSub;
                    chkCombo_Sube.Properties.DisplayMember = "Pkod_Ad";
                    chkCombo_Sube.Properties.ValueMember = "Pkod_Kod";
                    if (Set_Sube.Default.Sube == "") { chkCombo_Sube.CheckAll(); } else { chkCombo_Sube.SetEditValue(Set_Sube.Default.Sube); }
                    chkCombo_Sube.Enabled = true;
                }
            }
            else
            {
                chkCombo_Sube.Properties.DataSource = null;
                chkCombo_Sube.EditValue = null;
                chkCombo_Sube.Enabled = false;
            }
        }
    }
}