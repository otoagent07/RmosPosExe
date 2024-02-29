using System;

namespace RmosIngenicoGMP
{
    class ErrorClass
    {
        public static Main errCls;

        public static void DisplayErrorMessage(UInt32 errorCode)
        {
            byte[] TempErrorBuffer = new byte[256];

            GMPSmartDLL.GetErrorMessage(errorCode, TempErrorBuffer);

            errCls.m_lblErrorCode.Text = "Hata Kodu = 0x" + errorCode.ToString("X2").PadLeft(4, '0') + " : " + GMP_Tools.SetEncoding(TempErrorBuffer);
        }
    }
}
