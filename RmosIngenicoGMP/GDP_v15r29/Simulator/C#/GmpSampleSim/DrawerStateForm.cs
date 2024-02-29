using DevExpress.XtraEditors;
using System;

namespace RmosIngenicoGMP
{
    public partial class DrawerStateForm : XtraForm
    {
        private Main parent;
        uint Retcode = Defines.TRAN_RESULT_OK;

        public DrawerStateForm(Main parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int state;
            if (radioButtonActive.Checked)
                state = 1;
            else
                state = 0;

            Retcode = GMPSmartDLL.FP3_SetDrawerState(Main.CurrentInterface,
                                       state,
                                       Defines.TIMEOUT_DEFAULT
                                       );
            parent.HandleErrorCode(Retcode);
            this.Close();
        }
    }
}
