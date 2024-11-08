using Pos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class SifreDegistirForm : Form
    {
        public SifreDegistirForm()
        {
            InitializeComponent();
        }

        private void btnSifreDegis_Click(object sender, EventArgs e)
        {
            if (txtYeniSifre.Text!= txtYeniSifre1.Text)
            {
                MessageBox.Show("Yeni kartno'lar uyuşmuyor");
                txtYeniSifre.Focus();
                return;
            }

            string eskisifre = txtMevcutSifre.Text;
            string yenisifre = txtYeniSifre1.Text;

            string sorgu2 = $@"select count(*) as toplam from RmosMuh.dbo.Pos_User with(nolock) where P_Kart = '{eskisifre}'";
            string varmi2 = dbtools.DegerGetir(sorgu2);

            if (varmi2 == "0")
            {
                MessageBox.Show("Mevcut kartno bulunamadı!");
                txtMevcutSifre.Focus();
                return;
            }


            string sorgu = $@"select count(*) as toplam from RmosMuh.dbo.Pos_User with(nolock) where P_Kart = '{yenisifre}'";
            string varmi = dbtools.DegerGetir(sorgu);

            if (varmi!="0")
            {
                MessageBox.Show("Bu kartno kullanılmaktadır.");
                return;
            }

            string sorgu1 = $@"update RmosMuh.dbo.Pos_User set P_Kart='{yenisifre}' where  P_Kart = '{eskisifre}'";
            dbtools.execcmdR(sorgu1);

            MessageBox.Show("Kart'nonuz başarılı bir şekilde değiştirilmiştir.");


            User.P_Kart = yenisifre;
            this.Close();

        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SifreDegistirForm_Load(object sender, EventArgs e)
        {
            txtMevcutSifre.Select();
            txtMevcutSifre.Focus();
        }
    }
}
