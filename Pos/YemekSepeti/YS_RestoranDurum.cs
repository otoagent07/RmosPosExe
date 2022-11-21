using Pos.Class;
using System;
using System.Windows.Forms;

namespace Pos.YemekSepeti
{
    public partial class YS_RestoranDurum : DevExpress.XtraEditors.XtraForm
    {
        public YS_RestoranDurum()
        {
            InitializeComponent();
        }

        private string Check()
        {
            string Durum = "";
            return Durum = Convert.ToString(dbtools.DegerGetir("Select ISNULL(YS_OpenClosed,'ACIK') From YS_Restaurant where YS_CatagoryName = '" + YS_RestoInfo.YS_CatagoryName + "'"));
        }

        IntegrationWebService1.Integration iws = new IntegrationWebService1.Integration();
        private void YS_RestoranDurum_Load(object sender, EventArgs e)
        {
            labelControl1.Text = "";
            iws.AuthHeaderValue = new IntegrationWebService1.AuthHeader();
            iws.AuthHeaderValue.UserName = YS_AuthHeader.ah.UserName;
            iws.AuthHeaderValue.Password = YS_AuthHeader.ah.Password;
            labelControl1.Text = Check();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            iws.UpdateRestaurantStateAsync(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, IntegrationWebService1.RestaurantStates.Open);
            dbtools.execcmd("Update YS_Restaurant set YS_OpenClosed = 'AÇIK' where YS_CatagoryName = '" + YS_RestoInfo.YS_CatagoryName + "'");
            labelControl1.Text = Check();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            iws.UpdateRestaurantStateAsync(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, IntegrationWebService1.RestaurantStates.HugeDemand);
            dbtools.execcmd("Update YS_Restaurant set YS_OpenClosed = 'YOĞUN' where YS_CatagoryName = '" + YS_RestoInfo.YS_CatagoryName + "'");
            labelControl1.Text = Check();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            iws.UpdateRestaurantStateAsync(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, IntegrationWebService1.RestaurantStates.Closed);
            dbtools.execcmd("Update YS_Restaurant set YS_OpenClosed = 'KAPALI' where YS_CatagoryName = '" + YS_RestoInfo.YS_CatagoryName + "'");
            labelControl1.Text = Check();
        }
    }
}