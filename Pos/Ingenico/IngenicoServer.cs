using DevExpress.XtraGrid;
using Newtonsoft.Json;
using Pos.Class;
using SimpleTCP;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Pos.Ingenico
{
    public partial class IngenicoServer : Form
    {
        SimpleTcpClient client;
        decimal bakiye=0;
        DataTable dtGelen;
        public IngenicoServer(DataTable dtGelen,decimal bakiye)
        {
            InitializeComponent();

            this.dtGelen = dtGelen;
            this.bakiye = bakiye;
        }
        private void IngenicoServer_Load(object sender, EventArgs e)
        {
            gridYenile();

            setAllEnable(false);

            txtStatus.Text = "";
            txtStatus.Visible = false;
            CheckForIllegalCrossThreadCalls = false;

            lbl_Durum.Text = "YAZARKASAYA FİŞ GÖNDERİLİYOR..";

            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.AutoTrimStrings = true;
            client.DataReceived += client_DataReceived;

            client.Connect(Departman.Kodlar_Ingenico_IP, Departman.Kodlar_Ingenico_Port);

            if (!client.TcpClient.Connected)
                client.Connect(Departman.Kodlar_Ingenico_IP, Departman.Kodlar_Ingenico_Port);

            Json = FisGetir(Fisno, 2);


            client.WriteLine("SATIS;" + Json);

        }

        void client_DataReceived(object sender, SimpleTCP.Message e)
        {
            
             
                 System.Threading.Tasks.Task.Factory.StartNew(() =>
                 {
                     this.Invoke(new MethodInvoker(async () =>
                     {
                         try
                         {
                             setAllEnable(false);

                             txtStatus.Text = e.MessageString;

                             txtStatus.Text = txtStatus.Text.Remove(txtStatus.Text.Length - 1);
                             if (txtStatus.Text == "2067")
                             {
                                 lbl_Durum.Text = "FİŞ LİMİTİ AŞILDI.\nFİŞ İPTAL EDİLİYOR..";
                                 client.WriteLine("FISIPTAL;" + null);

                                 setAllEnable(true);
                             }
                             if (txtStatus.Text == "61443")
                             {
                                 lbl_Durum.Text = "ZAMAN AŞIMINA UĞRANDI..";
                                 setAllEnable(true);
                             }
                             if (txtStatus.Text == "61468")
                             {
                                 lbl_Durum.Text = "SİSTEM SUANDA MESGUL..\nCİHAZI KONTROL EDİNİZ...";
                                 setAllEnable(true);
                             }
                             if (txtStatus.Text == "2086")
                             {
                                 lbl_Durum.Text = "ÖDEME GERÇEKLEŞTİRİLEMEDİ..\n";
                                 setAllEnable(true);
                             }
                             if (txtStatus.Text == "2085")
                             {
                                 lbl_Durum.Text = "ÖDEME GERÇEKLEŞTİRİLEMEDİ..\n";
                                 setAllEnable(true);
                             }
                             if (txtStatus.Text == "2317")
                             {
                                 lbl_Durum.Text = "OKC ÜZERİNDE HANDLE YOK..\nEKRANI KAPATIP ÖDEMEYİ TEKRAR GÖNDERİNİZ..";
                                 setAllEnable(true);
                             }
                             string[] veri = txtStatus.Text.Split(';');
                             if (veri[0] == "OK")
                             {
                                 Finish(veri[1]);
                             }
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show(ex.Message);

                         }
                     }));


             //        txtStatus.Invoke((MethodInvoker)delegate ()
             //        {
                        
             //});
            });
        }

        public void setAllEnable(bool setVisible)
        {
            m_btnPaymentCreditAgain.Enabled = setVisible;
            m_btn_031.Enabled = setVisible;
            m_btnReloadTransaction.Enabled = setVisible;
            m_btnVoidAllTicket_059.Enabled = setVisible;
        }

      public  SonucTicket sonucTicket = new SonucTicket();
        private void Finish(string Sonuc)
        {
            
            sonucTicket = JsonConvert.DeserializeObject<SonucTicket>(Sonuc);
            if (sonucTicket.EJNo != 0)
            {
                dbtools.execcmd(@"INSERT INTO [dbo].[Pos_Ingenico]
           ([Tarih]
           ,[Fisno]
           ,[Sonuc]
           ,[Kullanici]
           ,[SistemTarih]
            ,[ZNo]
            ,[YFisno]
            ,[EJNo])
              VALUES('" + Param.Tarih.Date + @"', '" + Fisno + "','" + Sonuc + "','" + User.P_Kod + "',Getdate(),'" + sonucTicket.ZNo.ToString() + "','" + sonucTicket.FNo.ToString() + "','" + sonucTicket.EJNo.ToString() + sonucTicket.BkmID + "')");

                DonenDeger = true;

                this.Close();
            }
        }


        

        public bool DonenDeger = false;

        public int Fisno = 0;



        public void gridYenile()
        {
            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Ba";
            gridColumn7.FieldName = "Rsat_Id";

            gridControl1.DataSource = dtGelen;

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn5.Visible = true;
                //gridColumn5.VisibleIndex = 6;
            }
            else
            {
                gridColumn4.Visible = true;
                //gridColumn4.VisibleIndex = 6;
            }


            txtToplamTutar.Text = bakiye.ToString();
        }
        private string FisGetir(int fis, int tip)
        {
            string JSONresult = "";

            try
            {
                SqlConnection conn = new SqlConnection(dbtools.connstr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("spymz_yazarkasa", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@tip", SqlDbType.NVarChar).Value = tip;
                //cmd.Parameters.Add("@tarih", SqlDbType.Date).Value = tarih;
                cmd.Parameters.Add("@fisno", SqlDbType.Int).Value = fis;
                //cmd.Parameters.Add("@miktarGoster", SqlDbType.NVarChar).Value = false;
                //cmd.Parameters.Add("@urunGoster", SqlDbType.NVarChar).Value = false;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return JSONresult = JsonConvert.SerializeObject(dt);
            }
            catch
            {
                return "";
            }
        }

        private void m_btn_043_Click(object sender, EventArgs e)
        {
            setAllEnable(false);

            Json = FisGetir(Fisno, 2);
            client.WriteLine("ODEME;" + Json);
            lbl_Durum.Text = "ÖDEME TEKRAR GÖNDERİLİYOR...";
        }

        private void m_btnVoidAllTicket_059_Click(object sender, EventArgs e)
        {
            setAllEnable(false);
            client.WriteLine("FISIPTAL;" + null);
            lbl_Durum.Text = "FİŞ İPTAL EDİLİYOR...";
            DonenDeger = false;
            this.Close();

        }

        private void m_btn_031_Click(object sender, EventArgs e)
        {
            setAllEnable(false);

            client.WriteLine("FISKAPAT;" + null);
            lbl_Durum.Text = "FİŞ KAPATILIYOR...";
            DonenDeger = true;
            this.Close();
        }

        private void m_btnReloadTransaction_Click(object sender, EventArgs e)
        {
            setAllEnable(false);
            client.WriteLine("YENIDENYUKLE;" + null);
            lbl_Durum.Text = "ÖKC FİŞİ YENİLENİYOR...";
        }

        private void m_btnPaymentCash_Click(object sender, EventArgs e)
        {
            Json = FisGetir(Fisno, 2);
            client.WriteLine("ODEMENAKIT;" + Json);
            lbl_Durum.Text = "ÖDEMENİN TAMAMI NAKİT\nGÖNDERİLİYOR...";
        }

        string Json = "";
        private void button2_Click(object sender, EventArgs e)
        {
            Json = FisGetir(Fisno, 2);
            client.WriteLine("SATIS;" + Json);
            lbl_Durum.Text = "SATIŞ TEKRAR GÖNDERİLİYOR...";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client.WriteLine("SATIS;" + Json);
        }

        private void m_btnPaymentCredit_Click(object sender, EventArgs e)
        {
            Json = FisGetir(Fisno, 2);
            client.WriteLine("ODEMEKK;" + Json);
            lbl_Durum.Text = "ÖDEMENİN TAMAMI K.K\nGÖNDERİLİYOR...";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Json = FisGetir(Fisno, 2);
            client.WriteLine("KAGITBITINCE;" + Json);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Json = FisGetir(Fisno, 2);
            client.WriteLine("OKCRESET;" + Json);
        }

        private void IngenicoServer_Shown(object sender, EventArgs e)
        {
            

        }

       
    }
}
