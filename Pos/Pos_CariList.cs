using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Pos
{
    public partial class Pos_CariList : DevExpress.XtraEditors.XtraForm
    {
        public Pos_CariList()
        {
            InitializeComponent();
        }

        public bool Durum  = false;
        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Durum = false;
            CariKodYazi = String.Empty;
            this.Close();
        }

        public string CariKodYazi = String.Empty;
        public string CariKodu = String.Empty;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Durum = true;
            CariKodYazi = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Ad"));
            CariKodu = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
            this.Close();
        }

        private void Pos_CariList_Load(object sender, EventArgs e)
        {
            gridColumn1.FieldName = "Cari_Kod";
            gridColumn2.FieldName = "Cari_Ad";
            gridColumn3.FieldName = "Cari_Soyad";
            gridColumn4.FieldName = "Cari_Tel";
            gridColumn5.FieldName = "Cari_Adres1";
            gridColumn6.FieldName = "Cari_Adres2";
            gridColumn7.FieldName = "Cari_Adres3";
            gridColumn8.FieldName = "Cari_Funvan";
            gridColumn9.FieldName = "Cari_Fadres1";
            gridColumn10.FieldName = "Cari_Fadres2";
            gridColumn11.FieldName = "Cari_Vergidarie";
            gridColumn12.FieldName = "Cari_Vergino";
            gridColumn13.FieldName = "Cari_Mail";
            gridColumn14.FieldName = "Cari_Kart";
            gridColumn15.FieldName = "Kisi";
            gridColumn16.FieldName = "Rez_Odano";
            gridColumn17.FieldName = "Rez_Giris_tarihi";
            gridColumn18.FieldName = "Rez_Cikis_tarihi";
            gridColumn19.FieldName = "Rez_Konaklama";
            gridColumn20.FieldName = "Ac_Adi";
        }
    }
}