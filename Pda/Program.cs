using DevExpress.LookAndFeel;
using Pda;
using System;
using System.Windows.Forms;

namespace WindowsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            //Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new Main());
        }


        //static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        //{
        //    ErrorLog.Save(e.Exception, "Unhandled Thread Exception", Log.Log_Program.Pda);
        //    MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

        //static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    ErrorLog.Save(e.ExceptionObject as Exception, "Unhandled UI Exception", Log.Log_Program.Pda);
        //    MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled UI Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}


    }
}