using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Pos.Ingenico
{
  public  class ErrorClass
    {
         public static IngenicoConn errCls;
        //public static GMPForm errCls;

        //public static void DisplayErrorMessage(UInt32 errorCode)
        public static string DisplayErrorMessage(UInt32 errorCode)
        {
            byte[] TempErrorBuffer = new byte[256];

            GMPSmartDLL.GetErrorMessage(errorCode, TempErrorBuffer);

//            errCls.m_lblErrorCode.Text = "Hata Kodu = 0x" + errorCode.ToString("X2").PadLeft(4, '0') + " : " + GMP_Tools.SetEncoding(TempErrorBuffer);
            return "Hata Kodu = 0x" + errorCode.ToString("X2").PadLeft(4, '0') + " : " + GMP_Tools.SetEncoding(TempErrorBuffer);
        }
    }
}
