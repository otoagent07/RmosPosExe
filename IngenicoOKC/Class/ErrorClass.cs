using System;

namespace  IngenicoOKC.Class
{
    class ErrorClass
    {
        public static IngenicoCase errCls;
        public static string DisplayErrorMessage(UInt32 errorCode)
        {
            byte[] TempErrorBuffer = new byte[256];

            GMPSmartDLL.GetErrorMessage(errorCode, TempErrorBuffer);
            
            return "Hata Kodu = 0x" + errorCode.ToString("X2").PadLeft(4, '0') + " : " + GMP_Tools.SetEncoding(TempErrorBuffer);
        }
    }
}
