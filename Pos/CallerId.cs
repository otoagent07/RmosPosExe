using Pos.Class;
using System;
using System.IO;
using System.Windows.Forms;


namespace Pos
{
    public partial class CallerId : DevExpress.XtraEditors.XtraForm
    {
        public CallerId()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {
                throw ex;
                //RHMesaj.MyMessageError(MyClass, "CallerId", "", ex);
            }
        }

        public static string MyClass = "CallerId";

        private void CallerId_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists("c:\\CALLERID"))
                {
                    Directory.CreateDirectory("c:\\CALLERID");
                }

                lbl_Dep.Text = Departman.Dep_Adi;
                axCIDv51.Hide();
                axCIDv51.Start();

                timer1.Enabled = true;

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "CallerId_Load", "", ex);
            }

           
        }

        private void axCIDv51_OnCallerID(object sender, Axcidv5callerid.ICIDv5Events_OnCallerIDEvent e)
        {
            string telno = e.phoneNumber;
            string tarih = e.dateTime;
            string line = e.line;

            string veri = telno + " - " + tarih + " - Tel" + line;
            listBoxControl1.Items.Add(veri);


            try
            {
                StreamWriter sw = File.AppendText("c:\\CALLERID\\" + DateTime.Now.ToShortDateString() + ".txt");
                sw.WriteLine(veri);
                sw.Close();
            }
            catch (Exception)
            {

            }

            string CariKod = dbtools.DegerGetir("select Cari_Kod from Pos_Cari where Cari_Tel = '" + telno + "' ");
            dbtools.execcmd("insert into Pos_CallerId(Caller_Tarih,Caller_Telno,Caller_Carikod) values(getdate(),'" + telno + "','" + CariKod + "')");

            Clipboard.SetText(e.phoneNumber);

            //CallerDetay detay = new CallerDetay();
            //detay.Tel = e.phoneNumber;
            //detay.Show();

            CallerCallCenter cc = new CallerCallCenter();
            cc.Tel = e.phoneNumber;
            cc.Show();
        }

        private void btn_Kapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_Bilgi.Text = "Cihaz : " + axCIDv51.Command("Devicemodel") + "     " + axCIDv51.Command("Serial");
        }

        private void CallerId_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            notifyIcon1.Visible = true;
            this.ShowInTaskbar = true;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "Caller Id";
            notifyIcon1.BalloonTipText = "Caller Id Çalışıyor...!!!";
            notifyIcon1.ShowBalloonTip(50);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            axCIDv51_OnCallerID(null, null);
        }
    }
}