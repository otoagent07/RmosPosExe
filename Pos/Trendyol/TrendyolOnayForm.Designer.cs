
namespace Pos.Trendyol
{
    partial class TrendyolOnayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrendyolOnayForm));
            this.btnTrendyolSiparisTeslimEdildi = new DevExpress.XtraEditors.SimpleButton();
            this.btnTrendyolSiparisYolaCikti = new DevExpress.XtraEditors.SimpleButton();
            this.btnTrendyolSiparisHazirlandi = new DevExpress.XtraEditors.SimpleButton();
            this.btnCikis = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnTrendyolSiparisTeslimEdildi
            // 
            this.btnTrendyolSiparisTeslimEdildi.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTrendyolSiparisTeslimEdildi.Appearance.Options.UseFont = true;
            this.btnTrendyolSiparisTeslimEdildi.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnTrendyolSiparisTeslimEdildi.ImageOptions.Image")));
            this.btnTrendyolSiparisTeslimEdildi.Location = new System.Drawing.Point(452, 12);
            this.btnTrendyolSiparisTeslimEdildi.Name = "btnTrendyolSiparisTeslimEdildi";
            this.btnTrendyolSiparisTeslimEdildi.Size = new System.Drawing.Size(193, 129);
            this.btnTrendyolSiparisTeslimEdildi.TabIndex = 9;
            this.btnTrendyolSiparisTeslimEdildi.Text = "Teslim Edildi(4)";
            this.btnTrendyolSiparisTeslimEdildi.ToolTip = "Trendyoldan onaylar";
            this.btnTrendyolSiparisTeslimEdildi.Click += new System.EventHandler(this.btnTrendyolSiparisTeslimEdildi_Click);
            // 
            // btnTrendyolSiparisYolaCikti
            // 
            this.btnTrendyolSiparisYolaCikti.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTrendyolSiparisYolaCikti.Appearance.Options.UseFont = true;
            this.btnTrendyolSiparisYolaCikti.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnTrendyolSiparisYolaCikti.ImageOptions.Image")));
            this.btnTrendyolSiparisYolaCikti.Location = new System.Drawing.Point(233, 12);
            this.btnTrendyolSiparisYolaCikti.Name = "btnTrendyolSiparisYolaCikti";
            this.btnTrendyolSiparisYolaCikti.Size = new System.Drawing.Size(193, 129);
            this.btnTrendyolSiparisYolaCikti.TabIndex = 10;
            this.btnTrendyolSiparisYolaCikti.Text = "Yola Çıktı(3)";
            this.btnTrendyolSiparisYolaCikti.ToolTip = "Trendyoldan onaylar";
            this.btnTrendyolSiparisYolaCikti.Click += new System.EventHandler(this.btnTrendyolSiparisYolaCikti_Click);
            // 
            // btnTrendyolSiparisHazirlandi
            // 
            this.btnTrendyolSiparisHazirlandi.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTrendyolSiparisHazirlandi.Appearance.Options.UseFont = true;
            this.btnTrendyolSiparisHazirlandi.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnTrendyolSiparisHazirlandi.ImageOptions.Image")));
            this.btnTrendyolSiparisHazirlandi.Location = new System.Drawing.Point(12, 12);
            this.btnTrendyolSiparisHazirlandi.Name = "btnTrendyolSiparisHazirlandi";
            this.btnTrendyolSiparisHazirlandi.Size = new System.Drawing.Size(193, 129);
            this.btnTrendyolSiparisHazirlandi.TabIndex = 11;
            this.btnTrendyolSiparisHazirlandi.Text = "Hazırlandı(2)";
            this.btnTrendyolSiparisHazirlandi.ToolTip = "Trendyoldan onaylar";
            this.btnTrendyolSiparisHazirlandi.Click += new System.EventHandler(this.btnTrendyolSiparisHazirlandi_Click);
            // 
            // btnCikis
            // 
            this.btnCikis.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCikis.Appearance.Options.UseFont = true;
            this.btnCikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btnCikis.Location = new System.Drawing.Point(12, 162);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(633, 52);
            this.btnCikis.TabIndex = 10;
            this.btnCikis.Text = "ÇIKIŞ";
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // TrendyolOnayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 237);
            this.Controls.Add(this.btnTrendyolSiparisTeslimEdildi);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.btnTrendyolSiparisYolaCikti);
            this.Controls.Add(this.btnTrendyolSiparisHazirlandi);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrendyolOnayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trendyol Onay Form";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnTrendyolSiparisTeslimEdildi;
        private DevExpress.XtraEditors.SimpleButton btnTrendyolSiparisYolaCikti;
        private DevExpress.XtraEditors.SimpleButton btnTrendyolSiparisHazirlandi;
        private DevExpress.XtraEditors.SimpleButton btnCikis;
    }
}