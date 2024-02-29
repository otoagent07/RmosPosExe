using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace RmosIngenicoGMP
{
    public partial class CashierForm : XtraForm
    {
        public static Main cashierCls;

        public CashierForm()
        {
            InitializeComponent();
        }

        private void m_btnCashierLogOut_Click(object sender, EventArgs e)
        {
            int Retcode = GMPSmartDLL.FP3_FunctionCashierLogout(Main.CurrentInterface);

        }

    	int numberOfTotalCashiers;
	    short activeCashier;
        private void CashierForm_Load(object sender, EventArgs e)
        {
            RefreshCashierInfo();
        }

        private void RefreshCashierInfo()
        {
            Main gf = new Main();
            m_ListCashiers.Items.Clear();
            UInt32 Retcode;
            int numberOfTotalRecordsReceived = 0;
            ST_CASHIER[] stCashierTable = new ST_CASHIER[10];


            Retcode = Json_GMPSmartDLL.FP3_GetCashierTable(Main.CurrentInterface, ref numberOfTotalCashiers, ref numberOfTotalRecordsReceived, ref stCashierTable, 10, ref activeCashier);

            if (Retcode != 0)
            {
                gf.HandleErrorCode(Retcode);
                return;
            }
            gf.HandleErrorCode(Retcode);

            if (Retcode == Defines.TRAN_RESULT_OK)
            {
                gf.m_echoTestMenuItem_Click(null, null);
            }

            m_activeCashierIndex.Text = activeCashier.ToString();
            if (activeCashier < numberOfTotalCashiers)
                m_activeCashierName.Text = stCashierTable[activeCashier].name;

            for (int i = 0; i < numberOfTotalCashiers; i++)
            {

                ListViewItem item1 = new ListViewItem(stCashierTable[i].index.ToString());
                item1.SubItems.Add(stCashierTable[i].name);
                item1.SubItems.Add(stCashierTable[i].flags.ToString("X8"));

                m_ListCashiers.Items.Add(item1);
            }
        }

        private void m_ListCashiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PassForm pf = new PassForm();
            DialogResult dr = pf.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                if (m_ListCashiers.SelectedItems.Count > 0)
                {
                    Main gf = new Main();
                    UInt32 Retcode = GMPSmartDLL.FP3_FunctionCashierLogin(Main.CurrentInterface, m_ListCashiers.Items.IndexOf(m_ListCashiers.SelectedItems[0]), pf.m_PASS);
                    gf.HandleErrorCode(Retcode);
                }
            }
        }

        private void m_btnAddCashier_Click(object sender, EventArgs e)
        {
            PassForm pf = new PassForm();
            DialogResult dr = pf.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                Main gf = new Main();
                UInt32 Retcode = GMPSmartDLL.FP3_FunctionAddCashier(Main.CurrentInterface, Convert.ToUInt16(m_txtCashierIndex.Text), m_txtCashierName.Text, m_txtCashierPassword.Text, pf.m_PASS);
                gf.HandleErrorCode(Retcode);

                RefreshCashierInfo();
            }
        }
    }
}

