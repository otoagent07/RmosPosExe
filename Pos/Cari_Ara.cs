using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class Cari_Ara : DevExpress.XtraEditors.XtraForm
    {
        public Cari_Ara()
        {
            InitializeComponent();
        }

        public string Cari_Kod;
        public int Oda_Kart;

        private void Cari_Ara_Load(object sender, EventArgs e)
        {
            this.BringToFront();

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
            if (Convert.ToString(this.Tag) == "O") checkButton4.Text = "Oda No";
        }

        private void simpleButton48_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Kisi_Ara()
        {
            if (Convert.ToString(this.Tag) != "O")
            {
                DataTable dt = new DataTable();
                if (textEdit1.Text.Length > 0)
                {
                    if (checkButton1.Checked == true) dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Ad like '" + textEdit1.EditValue + "%' ");
                    if (checkButton2.Checked == true) dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Soyad like '" + textEdit1.EditValue + "%' ");
                    if (checkButton3.Checked == true) dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Tel like '" + textEdit1.EditValue + "%' ");
                    if (checkButton4.Checked == true) dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kod like '" + textEdit1.EditValue + "%' ");
                    if (checkButton5.Checked == true) dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kart like '" + textEdit1.EditValue + "%' ");

                    gridControl1.DataSource = dt;
                }
                else
                {
                    dt.Rows.Clear();
                    gridControl1.DataSource = dt;
                }
            }

            if (Convert.ToString(this.Tag) == "O")
            {
                DataTable dt = new DataTable();
                if (textEdit1.Text.Length > 0)
                {
                    string sorgu = "";
                    if (checkButton1.Checked == true) sorgu = "  and Rez_Adi_1 like '" + textEdit1.EditValue + "%' ";
                    if (checkButton2.Checked == true) sorgu = "  and Rez_Adi_2 like '" + textEdit1.EditValue + "%' ";
                    if (checkButton5.Checked == true) sorgu = "  and Rez_Kartno like '" + textEdit1.EditValue + "%' ";
                    if (checkButton4.Checked == true) sorgu = "  and Rez_Odano like '" + textEdit1.EditValue + "%' ";


                    dt = Fronttools.SelectTable("select Rez_Id,Rez_Odano as Cari_Kod, Rez_Adi_1 as Cari_Ad, Rez_Adi_2 as Cari_Soyad,Rez_Kartno as Cari_Kart, "
    //+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D'  then p.Kimlik_Adres1 else p2.Kimlik_Adres1 end as Cari_Fadres1,"
    + " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adres1 else p2.Kimlik_Adres1 end as Cari_Fadres1,"
    + " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adres2 else p2.Kimlik_Adres2 end as Cari_Fadres2,"
    + " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adresil else p2.Kimlik_Adresil end as Il,"
    + " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adresilce else p2.Kimlik_Adresilce end as Ilce,"
    + " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Vergidaire else p2.Kimlik_Vergidaire end as Cari_Vergidarie,"
    + " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Vergino else p2.Kimlik_Vergino end as Cari_Vergino, "
    + " Rez_Odano,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi,convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi "
    + " from Rez r "
    + " left join Acenta on r.Rez_Macenta = Acenta.Ac_Kodu"
    + " left join Previl p on r.Rez_Previl_id = p.Kimlik_Id"
    + " left join Previl p2 on r.Rez_Adi_1 = p2.Kimlik_Ad and r.Rez_Adi_2 = p2.Kimlik_Soyad"
    + " where r.Rez_R_I_H = 'I' " + sorgu + " order by  r.Rez_Odano ");

                    gridControl1.DataSource = dt;
                }
                else
                {
                    dt.Rows.Clear();
                    gridControl1.DataSource = dt;
                }

            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            Kisi_Ara();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(this.Tag) != "O")
            {
                DataTable dt = dbtools.SelectTable("select * from Pos_Cari order by Cari_Ad ");
                gridControl1.DataSource = dt;
            }

            if (Convert.ToString(this.Tag) == "O")
            {
                DataTable dt = Fronttools.SelectTable("select Rez_Id,Rez_Odano as Cari_Kod, Rez_Adi_1 as Cari_Ad, Rez_Adi_2 as Cari_Soyad,Rez_Kartno as Cari_Kart, "
+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adres1 else p2.Kimlik_Adres1 end as Cari_Fadres1,"
+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adres2 else p2.Kimlik_Adres2 end as Cari_Fadres2,"
+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adresil else p2.Kimlik_Adresil end as Il,"
+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Adresilce else p2.Kimlik_Adresilce end as Ilce,"
+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Vergidaire else p2.Kimlik_Vergidaire end as Cari_Vergidarie,"
+ " case when Rez_Master_detay = 'M' or Rez_Master_detay = 'D' then p.Kimlik_Vergino else p2.Kimlik_Vergino end as Cari_Vergino "
+ " from Rez r "
+ " left join Previl p on r.Rez_Previl_id = p.Kimlik_Id"
+ " left join Previl p2 on r.Rez_Adi_1 = p2.Kimlik_Ad and r.Rez_Adi_2 = p2.Kimlik_Soyad"
+ " where r.Rez_R_I_H = 'I' order by  r.Rez_Odano ");

                gridControl1.DataSource = dt;
            }
        }

        private void Btn_1_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "1";
        }

        private void Btn_2_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "2";
        }

        private void Btn_3_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "3";
        }

        private void Btn_4_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "4";
        }

        private void Btn_5_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "5";
        }

        private void Btn_6_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "6";
        }

        private void Btn_7_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "7";
        }

        private void Btn_8_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "8";
        }

        private void Btn_9_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "9";
        }

        private void Btn_0_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "0";
        }

        private void Back_Space_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text.Length > 0) textEdit1.Text = textEdit1.Text.Substring(0, textEdit1.Text.Length - 1);
        }

        private void Btn_Q_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Q";
        }

        private void Btn_W_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "W";
        }

        private void Btn_E_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "E";
        }

        private void Btn_R_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "R";
        }

        private void Btn_T_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "T";
        }

        private void Btn_Y_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Y";
        }

        private void Btn_U_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "U";
        }

        private void Btn_I_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "I";
        }

        private void Btn_O_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "O";
        }

        private void Btn_P_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "P";
        }

        private void Btn_Ğ_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Ğ";
        }

        private void Btn_Ü_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Ü";
        }

        private void Btn_A_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "A";
        }

        private void Btn_S_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "S";
        }

        private void Btn_D_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "D";
        }

        private void Btn_F_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "F";
        }

        private void Btn_G_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "G";
        }

        private void Btn_H_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "H";
        }

        private void Btn_J_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "J";
        }

        private void Btn_K_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "K";
        }

        private void Btn_L_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "L";
        }

        private void Btn_Ş_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Ş";
        }

        private void Btn_İ_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "İ";
        }

        private void Btn_Z_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Z";
        }

        private void Btn_X_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "X";
        }

        private void Btn_C_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "C";
        }

        private void Btn_V_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "V";
        }

        private void Btn_B_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "B";
        }

        private void Btn_N_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "N";
        }

        private void Btn_M_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "M";
        }

        private void Btn_Ö_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Ö";
        }

        private void Btn_Ç_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + "Ç";
        }

        private void Nokta_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + ".";
        }

        private void Space_Click(object sender, EventArgs e)
        {
            textEdit1.Text = textEdit1.Text + " ";
        }

        private void simpleButton47_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(this.Tag) == "H")
            {
                //Hesap.Oda_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                //Hesap.Rez_No = "";
                //Hesap.Pansiyon = "";
                //Hesap.Misafir_Adi = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Ad")) + " " + Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Soyad"));
                //Hesap.Kart_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kart"));
                //Hesap.Cari = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                this.Close();
            }

            if (Convert.ToString(this.Tag) == "C")
            {
                //Cari_Hesap.Cari_Kod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                //Cari_Hesap.Cari_Ad = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Ad")) + " " + Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Soyad"));
                this.Close();
            }

            if (Convert.ToString(this.Tag) == "F")
            {
                //Fatura.xAd = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Funvan"));
                //Fatura.xAdres1 = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Fadres1"));
                //Fatura.xAdres2 = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Fadres2"));
                //Fatura.xVergi_Daire = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Vergidarie"));
                //Fatura.xVergiNo = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Vergino"));
                this.Close();
            }

            if (Convert.ToString(this.Tag) == "S")
            {
                Cari_Kod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                this.Close();
            }

            if (Convert.ToString(this.Tag) == "O")
            {
                //Hesap.Oda_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                //Hesap.Rez_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Id"));
                //Hesap.Pansiyon = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Konaklama"));//
                //Hesap.Misafir_Adi = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Ad")) + " " + Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Soyad"));
                //Hesap.Kart_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kart"));
                //Hesap.Cari = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                this.Close();
            }
        }

    }
}