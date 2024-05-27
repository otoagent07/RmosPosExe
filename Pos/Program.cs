using DevExpress.LookAndFeel;
using Pos;
using Pos.Class;
using Pos.Controllers;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Pos
{
    static class Program
    {
        public static string[] disDegerler;
        public static Main main;
        
        [STAThread]
        //[Obsolete]
        static void Main(string[] CalismaParametreleri)
        {
            try
            {
                StatikModel.wait_loadingAc();

                if (CalismaParametreleri.Length > 0)
                    disDegerler = CalismaParametreleri;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);


                try
                {
                    CultureInfo culture = new CultureInfo(Langs.Default.Dil == "" ? "tr-TR" : Langs.Default.Dil);
                    //culture = new CultureInfo(User.Pos_Culture);
                    //culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
                    culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
                    //culture.DateTimeFormat.DateSeparator = ".";
                    culture.DateTimeFormat.ShortTimePattern = "HH:mm";
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    Localize.ApplicationLanguage(culture.TwoLetterISOLanguageName);
                }
                catch (Exception ex)
                {
                    //// 27.05.2024 eklendi
                    CultureInfo culture = new CultureInfo("tr-TR");
                    Langs.Default.Dil = "tr-TR";
                    Langs.Default.Save();

                    culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
                    culture.DateTimeFormat.ShortTimePattern = "HH:mm";
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    Localize.ApplicationLanguage(culture.TwoLetterISOLanguageName);
                }
              
               

                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.OfficeSkins.Register();
                DevExpress.UserSkins.BonusSkins.Register();
                UserLookAndFeel.Default.SetSkinStyle("Money Twins");

                //Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                if (CalismaParametreleri.Length > 0)
                {
                    bool reloginmi = false;
                    foreach (var item in CalismaParametreleri)
                    {
                        if (item.Contains("!!!")) reloginmi = true;
                    }
                    if (reloginmi)
                    {
                        main = new Main(CalismaParametreleri);
                        Application.Run(main);
                        return;
                    }
                }

                main = new Main();
                Application.Run(main);
            }
            catch(Exception ex)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine("Hata Oluştu:");
                errorMessage.AppendLine("Message: " + ex.Message);
                errorMessage.AppendLine("StackTrace: " + ex.StackTrace);

                Exception innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    errorMessage.AppendLine("Inner Exception: " + innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                MessageBox.Show(errorMessage.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StatikModel.wait_loadingKapat();
            }

            
        }

        //static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        //{
        //    ErrorLog.Save(e.Exception, "Unhandled Thread Exception", Log.Log_Program.Pos);
        //    MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

        //static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    ErrorLog.Save(e.ExceptionObject as Exception, "Unhandled UI Exception", Log.Log_Program.Pos);
        //    MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled UI Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}
    }
}