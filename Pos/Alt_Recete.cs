using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Pos
{
    public partial class Alt_Recete : Form
    {
        public string ustReceteKodu { get; set; }
        public string ustReceteAdi { get; set; }
        public string altReceteKodu { get; set; }
        public Action<string> UrunSatAction { get; set; }
        public Func<string, decimal> GetSatilanMiktarAction { get; set; }
        
        public Alt_Recete()
        {
            InitializeComponent();
        }

        private void Alt_Recete_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            lbl_Baslik.Text = ustReceteAdi + " " + lbl_Baslik.Text;
            AltUrun_Yenile();
        }

        private void AltUrun_Yenile()
        {
            flp_Altrecete.Controls.Clear();

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Cost_Recete_Liste";
            com.Parameters.AddWithValue("@Liste_Tipi", 4);
            com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Ust_Recete", ustReceteKodu);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // Parse Kodlar_Size (format: "120;55")
                    string size = Convert.ToString(dt.Rows[i]["Kodlar_Size"]) == "" ? "100;40" : Convert.ToString(dt.Rows[i]["Kodlar_Size"]);
                    string[] sizeArray = size.Split(';');

                    // Parse Kodlar_Font (format: "Tahoma; 9pt; style=Bold")
                    Font mfont = new System.Drawing.Font("Tahoma", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    string font = Convert.ToString(dt.Rows[i]["Kodlar_Font"]);
                    if (font != "")
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        mfont = (Font)converter.ConvertFromString(font);
                    }

                    // Parse Rec_Color (format: "#C0C0C0")
                    string recColor = Convert.ToString(dt.Rows[i]["Rec_Color"]);
                    Color backColor = Color.Empty;
                    if (recColor != "")
                    {
                        backColor = ColorTranslator.FromHtml(recColor);
                    }

                    SimpleButton btn_AltRecete = new SimpleButton();
                    btn_AltRecete.Size = new System.Drawing.Size(Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]));
                    btn_AltRecete.TabIndex = 0;
                    btn_AltRecete.TabStop = false;
                    btn_AltRecete.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_AltRecete.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_AltRecete.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AltRecete.Font = mfont;
                    btn_AltRecete.Appearance.Options.UseBackColor = true;
                    if (backColor != Color.Empty)
                    {
                        btn_AltRecete.Appearance.BackColor = backColor;
                    }
                    btn_AltRecete.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;

                    if (Param.Calisma_Sekli == 0) btn_AltRecete.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Fiyat"]);
                    if (Param.Calisma_Sekli == 1) btn_AltRecete.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Dovifiyat"]);
                    btn_AltRecete.Tag = Convert.ToString(dt.Rows[i]["Rec_Genelkod"]);

                    btn_AltRecete.Click += new EventHandler(btn_AltRecete_Click);
                    
                    // Satılan miktar overlay'i ekle
                    btn_AltRecete.Paint += new PaintEventHandler(btn_AltRecete_Paint);
                    
                    flp_Altrecete.Controls.Add(btn_AltRecete);
                }
            }
        }

        void btn_AltRecete_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Urun = (SimpleButton)sender;
            string receteKodu = btn_Urun.Tag.ToString();
            
            if (UrunSatAction != null)
            {
                UrunSatAction(receteKodu);
            }
            
            // Butonları yeniden çizdir (satılan miktar overlay'i için)
            foreach (Control control in flp_Altrecete.Controls)
            {
                if (control is SimpleButton)
                {
                    control.Invalidate();
                }
            }
        }

        // Satılan miktarı hesaplayan metod
        private decimal GetSatilanMiktar(string recGenelkod)
        {
            try
            {
                if (GetSatilanMiktarAction != null)
                {
                    return GetSatilanMiktarAction(recGenelkod);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        // SimpleButton üzerine satılan miktarı çizen Paint event'i
        private void btn_AltRecete_Paint(object sender, PaintEventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            if (btn == null) return;

            string recGenelkod = btn.Tag?.ToString();
            if (string.IsNullOrEmpty(recGenelkod)) return;

            decimal satilanMiktar = GetSatilanMiktar(recGenelkod);
            if (satilanMiktar <= 0) return;

            // Sağ üst köşeye miktar yazısı çiz
            string miktarText = "x" + satilanMiktar.ToString("0");

            // Font ve brush ayarları
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Brush textBrush = new SolidBrush(Color.Blue);

            // Metin boyutunu hesapla
            SizeF textSize = e.Graphics.MeasureString(miktarText, font);

            // Sağ üst köşe pozisyonu
            int x = btn.Width - (int)textSize.Width - 5;
            int y = 5;

            // Metni çiz (transparent arka plan)
            e.Graphics.DrawString(miktarText, font, textBrush, x, y);

            // Kaynakları temizle
            font.Dispose();
            textBrush.Dispose();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            altReceteKodu = "";
            this.Close();
        }

        // Butonları yeniden çizdir (satılan miktar overlay'i için)
        public void ButonlariYenidenCiz()
        {
            foreach (Control control in flp_Altrecete.Controls)
            {
                if (control is SimpleButton)
                {
                    control.Invalidate();
                }
            }
        }

       
    }
}