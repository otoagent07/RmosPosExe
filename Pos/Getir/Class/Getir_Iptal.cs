using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pos.Getir.Class
{
    public partial class Getir_Iptal : XtraForm
    {
        public Getir_Iptal()
        {
            InitializeComponent();
        }

        public string OrderID = "";

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void btnLoad()
        {
           DataTable dt = ConvertDataTableList.ListToDataTable(getirApi.getRequestS<List<GetirCancel.Root>>(GetirStatik.requestOrderBase + "/" + OrderID + "/cancel-options", GetirToken.apitoken));

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_Urun = new SimpleButton();
                    btn_Urun.Size = new Size(250, 70);
                    btn_Urun.TabIndex = 0;
                    btn_Urun.TabStop = false;
                    btn_Urun.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_Urun.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_Urun.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Urun.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_Urun.Appearance.Options.UseBackColor = false;
                    btn_Urun.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_Urun.Tag = Convert.ToString(dt.Rows[i]["id"]);
                    btn_Urun.Text = Convert.ToString(dt.Rows[i]["message"]);

                    btn_Urun.Click += new EventHandler(btn_Urun_Click);
                    flowLayoutPanel1.Controls.Add(btn_Urun);
                }
            }
        }

        GetirApi getirApi = new GetirApi();
        void btn_Urun_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Urun = (SimpleButton)sender;

            GetirCancel.PostCancelResponse result = getirApi.PostFoodOrderCancel(btn_Urun.Tag.ToString(), btn_Urun.Text, GetirToken.apitoken, OrderID);
            if (result == null)
            {
                MessageBox.Show("Sipariş Daha Önce İptal Edilmiş...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dbtools.execcmd("Update GetirYemek_Order Set GOrder_status = '1600' where GOrder_id = '" + OrderID + "'");
            this.Close();
        }
        private void Getir_Iptal_Load(object sender, EventArgs e)
        {
            btnLoad();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}