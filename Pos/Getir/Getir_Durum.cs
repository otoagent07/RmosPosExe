using DevExpress.XtraEditors;
using Pos.Class;
using Pos.Getir.Class;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pos.Getir
{
    public partial class Getir_Durum : DevExpress.XtraEditors.XtraForm
    {
        public int deliveryType = 0;

        public string OrderID = "";
        public string fisno= "0";
        public string GOrder_confirmationId = "";

        //public int ID = 0;
        public Getir_Durum()
        {
            InitializeComponent();
        }

        string durum="";
        public Getir_Durum(string durum,string fisno,string GOrder_confirmationId)
        {
            InitializeComponent();
            this.durum = durum;
            this.fisno = fisno;
            this.GOrder_confirmationId = GOrder_confirmationId;

        }

        int Status = 0;

        private void btnLoad()
        {
            DataTable dt = new DataTable("Durum");
            dt.Columns.Add("status", typeof(int));
            dt.Columns.Add("adi", typeof(string));
            dt.Columns.Add("link", typeof(string));


            //if (durum.Contains("Kurye yola çıktı"))
            //{
            //    dt.Rows.Add(900, (deliveryType == 1 ? "Getir Kuryesi\nTeslim Etti." : "Restoren Kurye\nTeslim Edildi."), (deliveryType == 1 ? "handover" : "deliver"));
            //}
            //else if (durum.ToLower().Contains("hazırlanıyor"))
            //{
            //    dt.Rows.Add(700, (deliveryType == 2 ? "Restoren Kurye\nYola Çıktı" : "Getir Kuryesine\nTeslim Edildi. Yola Çıktı"), "prepare");
            //}

            
                dt.Rows.Add(900, (deliveryType == 1 ? "Getir Kuryesi\nTeslim Etti." : "Restoren Kurye\nTeslim Edildi."), (deliveryType == 1 ? "handover" : "deliver"));
                dt.Rows.Add(700, (deliveryType == 2 ? "Restoren Kurye\nYola Çıktı" : "Getir Kuryesine\nTeslim Edildi. Yola Çıktı"), "prepare");


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_Urun = new SimpleButton();
                    btn_Urun.Size = new Size(250, 90);
                    btn_Urun.TabIndex = 0;
                    btn_Urun.TabStop = false;
                    btn_Urun.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_Urun.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_Urun.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Urun.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_Urun.Appearance.Options.UseBackColor = false;
                    btn_Urun.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_Urun.Tag = Convert.ToString(dt.Rows[i]["link"]) + ";" + Convert.ToString(dt.Rows[i]["status"]);
                    btn_Urun.Text = Convert.ToString(dt.Rows[i]["adi"]);

                    btn_Urun.Click += new EventHandler(btn_Urun_Click);

                    


                    flowLayoutPanel1.Controls.Add(btn_Urun);
                }

                foreach (SimpleButton item in flowLayoutPanel1.Controls)
                {
                    if (durum.Contains("Kurye yola çıktı") && item.Text.ToLower().Contains("Kurye yola çıktı"))
                    {
                        item.Enabled = false;
                    }
                    else if (durum.ToLower().Contains("hazırlanıyor") && item.Text.ToLower().Contains("restoren kurye\nteslim edildi."))
                    {
                        item.Enabled = false;
                    }else if (durum.Contains("Kurye yola çıktı") && item.Text.ToLower().Contains("restoren kurye\nyola çıktı"))
                    {
                        item.Enabled = false;
                    }

                }
            }

        }
        GetirApi getirApi = new GetirApi();
        void btn_Urun_Click(object sender, EventArgs e)
        {
            try
            {
                SimpleButton btn_Urun = (SimpleButton)sender;

                string[] veri = btn_Urun.Tag.ToString().Split(';');
                GetirOnay result = getirApi.getOrderPrepareDeliverHandover(GetirToken.apitoken, OrderID, veri[0].ToString());
                if (result.result)
                {
                    dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_GetirDurum = '" + veri[1] + "' where Rsat_GetirOrderID = '" + OrderID + "'");

                    if (Main.ayarlar.paketOtoKapat && veri[1].ToString().Equals("900"))
                    {
                        HesapKapa2();
                    }
                }

                

                this.Close();
            }
            catch(Exception ex)
            {
                RHMesaj.alertMesaj("Getir_Durum->btn_Urun_Click " + ex.Message);
            }
           
        }

        private void HesapKapa2()
        {
            string query = @"select Pkod_Kod from Pos_Kodlar where  Pkod_Sinif='11'  and Pkod_Getir_PaymentID =
(select top 1 GetirPayment_id from GetirYemek_Order dd
left join GetirYemek_Payment ss on ss.GetirPayment_type=dd.GOrder_paymentMethod
where GOrder_confirmationId='"+ GOrder_confirmationId + "' )";

            string Kapatma = dbtools.DegerGetir(query);

            if (Main.ayarlar.paketOtoKapat == false)
            {
                return;
            }

            if (Kapatma.Equals("") || fisno.Equals("0"))
            {
                return;
            }



            string Masa_No = Convert.ToString(dbtools.DegerGetir("select Rsat_Masa From Cst_Recete_Satis Where Rsat_Fisno = '" + fisno + "' Group by Rsat_Masa"));
            Hesap hes = new Hesap();
            hes.Tag = fisno;
            hes.Masa_No = Masa_No;


            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");



            if (dt == null || dt.Rows.Count < 1)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
            }


            hes.look_Kapatma.Properties.DataSource = dt;


            hes.look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
            hes.look_Kapatma.Properties.ValueMember = "Pkod_Kod";

            hes.look_Kapatma.EditValue = Kapatma == "" ? null : Kapatma;
            hes.Split = 0;
            hes.Splitad = "";

            hes.Visible = true;
            hes.otoYazdirmadanKapat = true;
            hes.Show();

        }

        private void Getir_Durum_Load(object sender, EventArgs e)
        {
            btnLoad();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}