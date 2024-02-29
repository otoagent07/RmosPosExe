using DevExpress.XtraEditors;
using System;
using System.Text;

namespace RmosIngenicoGMP
{
    public partial class UserDataForm : XtraForm
    {
        private Main parent;
        uint Retcode = Defines.TRAN_RESULT_OK;

        public UserDataForm(Main parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string data = textBoxUserData.Text.Trim();
            byte[] userData = Encoding.ASCII.GetBytes(data);
            if (parent.m_rbBatchMode.Checked)
            {
                byte[] buffer = new byte[1024];
                int bufferLen = 0;

                // start
                bufferLen = (int)GMPSmartDLL.prepare_SendUserData(buffer, buffer.Length, userData, userData.Length);
                parent.AddIntoCommandBatch("prepare_SendUserData", Defines.GMP3_FISCAL_PRINTER_MODE_REQ, buffer, bufferLen);

            }else
            {
                Retcode = GMPSmartDLL.FP3_SendUserData(Main.CurrentInterface,
                                       this.parent.ACTIVE_TRX_HANDLE,
                                       userData,
                                       userData.Length,
                                       Defines.TIMEOUT_DEFAULT
                                       );
                parent.HandleErrorCode(Retcode);
            }
        }

        private void buttonAl_Click(object sender, EventArgs e)
        {
            textBoxUserData.Text = "";
            byte[] userData = new byte[64];
            int length = 0;
            Retcode = GMPSmartDLL.FP3_GetUserData(Main.CurrentInterface,
                                        this.parent.ACTIVE_TRX_HANDLE,
                                        userData,
                                        ref length,
                                        Defines.TIMEOUT_DEFAULT
                                        );

            textBoxUserData.Text = Encoding.ASCII.GetString(userData);
            parent.HandleErrorCode(Retcode);
        }
    }
}
