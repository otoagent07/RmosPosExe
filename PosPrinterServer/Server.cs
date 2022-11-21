using SimpleTCP;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace PosPrinterServer
{
    public partial class Server : DevExpress.XtraEditors.XtraForm
    {
        public Server()
        {
            InitializeComponent();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Sistemde IPv4 adresi olan ağ bağdaştırıcıları yok!");
        }

        private void Server_Load(object sender, EventArgs e)
        {
            txt_Host.Text = GetLocalIPAddress();
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; //Enter
            server.AutoTrimStrings = true;
            server.DataReceived += server_DataReceived;
            server.StringEncoder = Encoding.UTF8;
        }
        private bool paketDogru(string msj)
        {
            msj = msj.Trim();
            char c = (char)19; //Convert.ToChar(msj.Substring(msj.Length - 1, 1));

            //int asciiCode = (int)c;

            return msj.EndsWith(c.ToString());
        }

        string mesajYorumla(string msj)
        {
            string sonuc = "OK;";
            //msj = msj.Substring(0, msj.Length - 1); //en sondaki kontrol karekterini kaldır
            msj = msj.Remove(msj.Length - 1);

            return sonuc;
        }
        void server_DataReceived(object sender, SimpleTCP.Message e)
        {
            listBox1.Invoke((MethodInvoker)delegate ()
            {
                listBox1.Items.Add(e.MessageString);
                if (paketDogru(e.MessageString))
                {
                    //                    e.ReplyLine(string.Format("OK->:{0}", e.MessageString));
                    string gelenIp = e.TcpClient.Client.LocalEndPoint.ToString();

                    e.ReplyLine(mesajYorumla(e.MessageString)); 

                }
                else
                    e.ReplyLine(string.Format("HATA SonlandırmaYok ->:{0}", e.MessageString));


                //e.ReplyLine(string.Format("OK->:{0}", e.MessageString));
            });

        }


        private void chkBtn_Start_CheckedChanged(object sender, EventArgs e)
        {
            chkBtn_Start.Appearance.BackColor = Color.YellowGreen;
            if (chkBtn_Start.Checked == true)
            {
                //chkBtn_Stop.Checked = false;
                chkBtn_Stop.Appearance.BackColor = Color.Transparent;
            }

            Start(Convert.ToInt32(txt_Port.Text));
        }

        private void chkBtn_Stop_CheckedChanged(object sender, EventArgs e)
        {
            chkBtn_Stop.Appearance.BackColor = Color.Tomato;
            if (chkBtn_Stop.Checked == true)
            {
                //chkBtn_Start.Checked = false;
                chkBtn_Start.Appearance.BackColor = Color.Transparent;
            }

            Stop();
        }

        SimpleTcpServer server;

        private string Start(int port)
        {
            string sonuc = "";
            lbl_Info.Text = "Server Starting ...";
            //IPAddress ip = new IPAddress(long.Parse(txtHost.Text));

            IPAddress address = IPAddress.Parse(txt_Host.Text);
            byte[] bytes = address.GetAddressBytes();
            IPAddress ip = new IPAddress(bytes);

            try
            {
                server.Start(ip, Convert.ToInt32(txt_Port.Text));
                //listBox1.Items.Add("Server Started");

            }
            catch (Exception e)
            {
                sonuc = e.ToString();
                MessageBox.Show("Port açılamadı");
            }

            //if (sonuc == "")
            //    lblCihazPortIcon.Tag = "1";

            //cihazOnOff(lblCihazPortIcon.Tag.ToString());
            return sonuc;
        }

        private string Stop()
        {
            string sonuc = "";
            if (server.IsStarted)
            {
                try
                {
                    server.Stop();
                    lbl_Info.Text = "Server Closed";

                }
                catch (Exception e)
                {

                    sonuc = e.ToString();
                }
            }

            //lblCihazPortIcon.Tag = "0";
            //cihazOnOff(lblCihazPortIcon.Tag.ToString());
            return sonuc;

        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            Tanimlama d = new Tanimlama();
            d.ShowDialog();
        }
    }
}