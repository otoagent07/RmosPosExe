using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class Pos_TeraziEkran : DevExpress.XtraEditors.XtraForm
    {
        public Pos_TeraziEkran()
        {
            InitializeComponent();
        }

        string Param_ComPort;
        int Param_BaudRate;
        int Param_DataBits;
        string Param_Parity;
        decimal Param_StopBits;
        string Param_FlowControl;

        private async Task YukleAsync()
        {
            Param.Param_Yukle();
            Param_ComPort = Param.Param_ComPort;
            Param_BaudRate = Convert.ToInt32(Param.Param_BaudRate);
            Param_DataBits = Convert.ToInt32(Param.Param_DataBits);
            Param_StopBits = Convert.ToDecimal(Param.Param_StopBits);
            Param_Parity = Convert.ToString(Param.Param_Parity);
            Param_FlowControl = Param.Param_FlowControl;

            try
            {
                //Com port seçimi
                serialPort1.PortName = Param_ComPort;

                if (Param_Parity == "Odd")
                {
                    serialPort1.Parity = Parity.Odd;
                }
                else if (Param_Parity == "Even")
                {
                    serialPort1.Parity = Parity.Even;
                }
                else if (Param_Parity == "Mark")
                {
                    serialPort1.Parity = Parity.Mark;
                }
                else if (Param_Parity == "Space")
                {
                    serialPort1.Parity = Parity.Space;
                }

                serialPort1.DtrEnable = true;
                serialPort1.RtsEnable = false;


                serialPort1.BaudRate = Convert.ToInt32(Param_BaudRate);
                serialPort1.DataBits = Convert.ToInt32(Param_DataBits);


                serialPort1.ReadTimeout = 1000;  // 1 saniye sonra timeout atsın
                serialPort1.WriteTimeout = 1000;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(Okuma);
            await Task.Delay(3000);
            Baglan();

        }
        private void Okuma(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string veri = serialPort1.ReadExisting(); // Alternatif olarak ReadLine yerine kullanılabilir
                if (string.IsNullOrEmpty(veri)) return;

                if (veri.StartsWith("S"))
                {
                    string sonuc = veri.Replace("S", "").Replace("\r", "").Replace("kg", "").Replace("k", "").Trim();
                    ekranabas(sonuc);
                }
            }
            catch (Exception ex)
            {
                // Hata loglaması yapabilirsiniz
                Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show("Seri Port Okuma Hatası: " + ex.Message);
                }));
            }
        }

        //private void Okuma(object s, SerialDataReceivedEventArgs e)
        //{
        //    var veri = serialPort1.ReadExisting();
        //    if (veri.StartsWith("S"))
        //    {
        //        string Sonuc = veri.Replace("S", "").Replace("\r", "").Replace("kg", "").Replace("k", "").Trim();
        //        ekranabas(Convert.ToString(Sonuc));
        //    }
        //}

        public delegate void ricdegis(string text);

        public void ekranabas(string s)
        {
            if (this.InvokeRequired)
            {
                ricdegis degis = new ricdegis(ekranabas);
                this.Invoke(degis, s);
            }
            else
            {
                //richTextBox1.AppendText(s);
                //richTextBox1.AppendText("\n" + s);
                txt_Sayi.Text = s;
            }
        }

        int controller = 1;
        private void Baglan()
        {
            if (controller > 0)
            {
                controller *= -1;

                try
                {
                    if (serialPort1.IsOpen == false)  //Seri port açık değilse seri port açılıyor
                    {
                        serialPort1.Open();           //Seri porttan veri iletişimi böylece başlamış oluyor   
                    }
                }
                catch
                {
                    this.Text = "Baglantı Yok...";
                    //this.BackColor = Color.Red;
                    return;
                }

                this.Text = "Baglandı...     ";
                //this.BackColor = Color.LightGreen;


            }
        }

        private async void Pos_TeraziEkran_Load(object sender, EventArgs e)
        {
            await YukleAsync();
            txt_Sayi.Focus();
        }


        public decimal DonenDeger = 0;
        public bool Kapanis = false;


        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            DonenDeger = 0;
            Kapanis = false;
            serialPort1.Close();
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            DonenDeger = Convert.ToDecimal(txt_Sayi.EditValue.ToString().Replace(".", ","));
            Kapanis = true;
            serialPort1.Close();
            this.Close();
        }

        public decimal sayi = 0;
        public bool MiktarGR = false;
        public bool Cikis = false;
        bool ilk = true;
        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            if (ilk) txt_Sayi.Text = String.Empty;
            SimpleButton btn_Sayi = (SimpleButton)sender;
            txt_Sayi.Text = txt_Sayi.Text + btn_Sayi.Text;
            ilk = false;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            txt_Sayi.Text = "0,000";
            ilk = true;
            txt_Sayi.Focus();
        }

        
    }
}