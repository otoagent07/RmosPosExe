using Pos.Class;
using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Pos_ExtraFolio_BakiyeTRF : DevExpress.XtraEditors.XtraForm
    {
        public Pos_ExtraFolio_BakiyeTRF()
        {
            InitializeComponent();
        }


        public int KartID, FolioID; public string KartNo;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CardF_Listele()
        {
            gridControl1.DataSource = Fronttools.SelectTable(@"SELECT 
               [ID]
              ,[CardF_No]
              ,[CardF_RezID]
              ,[CardF_Odano]
              ,[CardF_Ad]
              ,[CardF_Soyad]
             FROM [dbo].[KartF] Where CardF_R_I_H = 'I'");


        }

        private void TransferEt()
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                return;
            }

            int yKartID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID"));
            int yFolioID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CardF_RezID"));
            string yKartNo = Convert.ToString(gridView1.GetFocusedRowCellValue("CardF_No"));

            Fronttools.execcmd("Update Kumhrk Set Kumhrk_Rez_id = '" + yFolioID + "', Kumhrk_Kart_id = '" + yKartID + "' where Kumhrk_Rez_id = '" + FolioID + "' and Kumhrk_Kart_id = '" + KartID + "'");


            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart_Transfer, Log.Log_Islem.Duzelt, KartNo + " Nolu Kartın Bakiye ve Harcamaları, " + yKartNo + " Nolu Karta Aktarılmıştır.", "", "");

            MessageBox.Show(res_man.GetString("Transfer Edildi."), "Uyarı");
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            TransferEt();      
            
        }

        private void Pos_ExtraFolio_BakiyeTRF_Load(object sender, EventArgs e)
        {
            CardF_Listele();
        }
    }
}