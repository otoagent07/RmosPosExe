using DevExpress.XtraEditors;
using RmosIngenicoGMP.Models;
using System;

namespace RmosIngenicoGMP
{
    public partial class Promotion : XtraForm
    {

        public Promotion()
        {
            InitializeComponent();
        }

        private void m_btnAddPromotion_Click(object sender, EventArgs e)
        {
            PromotionModel promotion = PromotionModel.Instance;
            promotion.Amount = Convert.ToUInt32(m_txtPromotionAmount.Text);
            promotion.Message = m_txtPromotionText.Text;
            promotion.Type = PromotionType.DISCOUNT;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
