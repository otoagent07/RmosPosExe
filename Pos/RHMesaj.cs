using DevExpress.XtraBars.Alerter;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Pos
{
    public class RHMesaj
    {
        public static string MyClassName = "MyMesaj";

        public static void alertMesaj(string icerik,int saniye=60)
        {
            string header = "RMOS POS";
            if (Program.main != null)
            {
                AlertInfo alertInfo = new AlertInfo(header, icerik);
                Program.main.alertControl2.AutoFormDelay = saniye*1000;
                Program.main.alertControl2.FormLocation = AlertFormLocation.BottomRight;
                Program.main.alertControl2.ShowPinButton = true;
                Program.main.alertControl2.Show(Program.main, alertInfo);
            }
        }

        public static void alertMesajSagUst(string header, string icerik, int saniye = 60)
        {
            if (Program.main != null)
            {
                AlertInfo alertInfo = new AlertInfo(header, icerik);
                Program.main.alertControl2.AutoFormDelay = saniye * 1000;
                Program.main.alertControl2.FormLocation = AlertFormLocation.TopRight;
                Program.main.alertControl2.ShowPinButton = true;
                Program.main.alertControl2.Show(Program.main, alertInfo);
            }
        }


        public static void toastMesaj(string icerik)
        {
            string header = "RMOS POS";

            // AŞAĞISI TOAST MESAJ
            if (Program.main != null)
            {
                Program.main.toastNotificationsManager1.Notifications[0].Header = header;
                Program.main.toastNotificationsManager1.Notifications[0].Body = icerik;
                Program.main.toastNotificationsManager1.ShowNotification(Program.main.toastNotificationsManager1.Notifications[0]);
            }
        }

        public static void toastMesajGetir(string icerik)
        {
            string header = "RMOS POS";

            // AŞAĞISI TOAST MESAJ
            if (Program.main != null)
            {
                Program.main.toastNotificationsManager2.Notifications[0].Header = header;
                Program.main.toastNotificationsManager2.Notifications[0].Body = icerik;
                Program.main.toastNotificationsManager2.ShowNotification(Program.main.toastNotificationsManager2.Notifications[0]);
            }
        }

        public static string GetCallingMethod(StackTrace st, string MethodName)
        {
            string str = "";
            try
            {
                StackFrame[] frames = st.GetFrames();
                for (int i = 0; i < st.FrameCount; i++)
                {
                    if (frames[i].GetMethod().Name.Equals(MethodName))
                    {

                        //  str = frames[i].GetMethod().ReflectedType.FullName + "." + frames[i].GetMethod().Name + "() Line -> " + frames[i].GetFileLineNumber();
                        str = frames[i].GetFileLineNumber() + "";
                    }
                }
            }
            catch (Exception) {; }
            return str;
        }


        /// <summary>
        ///  Sadece Bilgilendirme Mesajı. İcon'suz
        /// </summary>
        /// <param name="icerik"></param>
        public static void MyMessage(string icerik)
        {
            try
            {
                MessageBox.Show(new Form { TopMost = true }, icerik, "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "HATA->MyClassName.MyMessage()->" + ex.Message);
            }
        }

        public static System.Threading.Timer _timeoutTimer;
        public static string _caption;

        public static void MyMessageDelay(string icerik)
        {
            string caption = "BİLGİ";
            int timeout = 2000;

            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
                MessageBox.Show(icerik, caption);
        }
        public static void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Hata Mesajı!
        /// </summary>
        /// <param name="pClass"></param>
        /// <param name="pMetot"></param>
        /// <param name="pIcerik"></param>
        /// <param name="pException"></param>
        public static void MyMessageError(string pClass, string pMetot, string pIcerik, Exception pException)
        {
            try
            {
                if (pIcerik.Equals(""))
                {
                    pIcerik = "Beklenmedik Hata!";
                }
                var st = new StackTrace(pException, true);
                MessageBox.Show(new Form { TopMost = true }, pClass + "." + pMetot + "() Satir-> " + GetCallingMethod(st, pMetot) + "\n" + pIcerik + "\n" + pException.Message, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "HATA->MessageBoxControl.MyMessageError()->" + ex.Message);
            }
        }

        /// <summary>
        /// Uyarı Mesajı
        /// </summary>
        /// <param name="pIcerik"></param>
        public static void MyMessageInformation(string pIcerik)
        {
            try
            {
                MessageBox.Show(new Form { TopMost = true }, pIcerik, "UYARI!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA->MessageBoxControl.MyMessageInformation()->" + ex.Message);
            }
        }


        /// <summary>
        /// Evet Hayır Mesajı
        /// </summary>
        /// <param name="pClass"></param>
        /// <param name="pMetot"></param>
        /// <param name="pIcerik"></param>
        /// <returns></returns>
        public static bool MyMessageConfirmation(string pIcerik)
        {
            try
            {
              

                 DialogResult dialogResult = MessageBox.Show(pIcerik, "ONAY", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);


                if (dialogResult == DialogResult.Yes)
                {
                    return true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "HATA->MessageBoxControl.MyMessageConfirmation()->" + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Evet Hayır Mesajı
        /// </summary>
        /// <param name="pClass"></param>
        /// <param name="pMetot"></param>
        /// <param name="pIcerik"></param>
        /// <returns></returns>
        public static bool MyMessageConfirmationDetay(string pClass, string pMetot, string pIcerik)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(pIcerik, "ONAY", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    return true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form { TopMost = true }, "HATA->MessageBoxControl.MyMessageConfirmation()->" + ex.Message);
                return false;
            }
        }
    }
}
