using DevExpress.XtraEditors;
using System;

namespace RmosIngenicoGMP
{
    public partial class PassForm : XtraForm
    {
        public PassForm()
        {
            InitializeComponent();
        }

        public string m_PASS = "";

        private void m_btnSendPass_Click(object sender, EventArgs e)
        {
            m_PASS = m_txtGetPass.Text;
            this.Close();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;            
        }
    }
}
