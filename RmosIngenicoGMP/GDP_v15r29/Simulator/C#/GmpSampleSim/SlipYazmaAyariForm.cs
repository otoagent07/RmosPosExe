using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace RmosIngenicoGMP
{
    public partial class SlipYazmaAyariForm : XtraForm
    {
        private Main parent;

        public SlipYazmaAyariForm(Main parent)
        {
            this.parent = parent;
            InitializeComponent();

            ulong activeFlags = GetSlipWriteOption();
            
            if ((activeFlags & Defines.GMP3_OPTION_DONT_PRINT_MERCHANT_SLIPS) == 0) // flag is unset
                radioButtonPassive.Checked = true;
            else
                radioButtonActive.Checked = true;
        }

        private ulong GetSlipWriteOption()
        {
            UInt64 activeFlags = 0;
            uint Retcode = Defines.TRAN_RESULT_OK;

            Retcode = GMPSmartDLL.FP3_OptionFlags(Main.CurrentInterface,
                                        this.parent.ACTIVE_TRX_HANDLE, 
                                        ref activeFlags,
                                        0, //Defines.GMP3_OPTION_DONT_PRINT_MERCHANT_SLIPS, 
                                        0, 
                                        Defines.TIMEOUT_DEFAULT
                                        );
            if(Retcode != Defines.TRAN_RESULT_OK)
            {
                MessageBox.Show("Pairing yapılması gerekiyor!");
            }
            return activeFlags;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            UInt64 activeFlags = 0;
            uint Retcode = Defines.TRAN_RESULT_OK;

            if (radioButtonPassive.Checked)
            {
                Retcode = GMPSmartDLL.FP3_OptionFlags(Main.CurrentInterface,
                                       this.parent.ACTIVE_TRX_HANDLE,
                                       ref activeFlags,
                                       Defines.GMP3_OPTION_DONT_PRINT_MERCHANT_SLIPS,
                                       0,
                                       Defines.TIMEOUT_DEFAULT
                                       );
            }
            else
            {
                Retcode = GMPSmartDLL.FP3_OptionFlags(Main.CurrentInterface,
                                      this.parent.ACTIVE_TRX_HANDLE,
                                      ref activeFlags,
                                      0,
                                      Defines.GMP3_OPTION_DONT_PRINT_MERCHANT_SLIPS,
                                      Defines.TIMEOUT_DEFAULT
                                      );
            }

            if (Retcode != Defines.TRAN_RESULT_OK)
            {
                MessageBox.Show("Pairing yapılması gerekiyor!");
            }

            this.Dispose();
        }
    }
}
