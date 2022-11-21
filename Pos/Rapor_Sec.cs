using DevExpress.XtraReports.UI;
using Pos.Class;
using Pos.Print;
using System;
using System.Data;

namespace Pos
{
    public partial class Rapor_Sec : DevExpress.XtraEditors.XtraForm
    {
        public Rapor_Sec()
        {
            InitializeComponent();
        }

        private void Rapor_Sec_Load(object sender, EventArgs e)
        {
            btn_Raporlar.Enabled = User.R_Raporlar;
            btn_Istatistik.Enabled = User.R_Raporlar;
            btn_GuNSonu.Enabled = User.P_Gunsonu;
            btn_GunsonuMail.Enabled = User.P_Gunsonu;
        }

        private void btn_Raporlar_Click(object sender, EventArgs e)
        {
            Raporlar r = new Raporlar();
            r.ShowDialog();

            this.Close();
        }

        private void btn_Istatistik_Click(object sender, EventArgs e)
        {
            Raporlar2 r = new Raporlar2();
            r.ShowDialog();

            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataTable dtDep = new DataTable();
        private void btn_GuNSonu_Click(object sender, EventArgs e)
        {
            Rapor_Tarih r = new Rapor_Tarih();
            r.ShowDialog();

            if (r.cikis == false)
            {
                return;
            }


            Gun_Sonu g = new Gun_Sonu();

            string[] dep_Parcala;

            if (Convert.ToString(r.lookUpEdit1.EditValue) != "")
            {
                dep_Parcala = Convert.ToString(r.lookUpEdit1.EditValue.ToString().Replace(" ", "")).Split(',');

                dtDep.Columns.Add("dep");

                foreach (string a in dep_Parcala)
                {
                    dtDep.Rows.Add(a);
                }
            }
            if (dtDep.Rows.Count > 0)
            {
                for (int i = 0; i < dtDep.Rows.Count; i++)
                {
                    g.Rapor_Gunsonu_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(dtDep.Rows[i]["dep"]),Convert.ToString(r.chkCombo_Sube.EditValue)).ShowPreview();                  
                }
            }
            

            g.Rapor_Gunsonu2_Pr(r.date_Tarih1.DateTime.Date,Convert.ToString(r.chkCombo_Sube.EditValue)).ShowPreview();
          

            var report3 = g.Rapor_Gunsonu3_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue)); // ekrandan
            report3.CreateDocument();
            var report4 = g.Rapor_Gunsonu4_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report4.CreateDocument();
            var report5 = g.Rapor_Gunsonu5_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report5.CreateDocument();
            var report6 = g.Rapor_Gunsonu6_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report6.CreateDocument();
            var report7 = g.Rapor_Gunsonu7_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report7.CreateDocument();

            report3.Pages.AddRange(report4.Pages);
            report3.Pages.AddRange(report5.Pages);
            report3.Pages.AddRange(report6.Pages);
            report3.Pages.AddRange(report7.Pages);

            report3.PrintingSystem.ContinuousPageNumbering = true;

            ReportPrintTool printTool = new ReportPrintTool(report3);
            printTool.ShowPreview();




            FisPr pr = new FisPr();
            string kasaRapor = pr.KasaGunlukOzetString(r.date_Tarih1.DateTime, r.date_Tarih1.DateTime);
            Rapor_Kasa raporKasa = new Rapor_Kasa();
            raporKasa.xrLabel1.Text = kasaRapor;
            raporKasa.txtTarih.Text = "Tarih :" + Param.Tarih.ToString("dd.MM.yyyy");
            raporKasa.ShowPreview();

            this.Close();

       
        }
        public static string MyClass = "Rapor_Sec";
        private void btn_GunsonuMail_Click(object sender, EventArgs e)
        {
            try
            {
                Rapor_Tarih r = new Rapor_Tarih();
                r.ShowDialog();

                if (r.cikis == false)
                {
                    return;
                }

                Gun_Sonu g = new Gun_Sonu();
                g.Mail_Gonder(r.date_Tarih1.DateTime.Date, Convert.ToString(r.lookUpEdit1.EditValue), Convert.ToString(r.chkCombo_Sube.EditValue));

                this.Close();
            }
            catch(Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btn_GunsonuMail_Click", "",ex);
            }
           
        }
    }
}