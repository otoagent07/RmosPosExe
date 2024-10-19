
namespace Pos
{
    partial class BarkodUrunBilgisiForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarkodUrunBilgisiForm));
            this.txtBarkod = new DevExpress.XtraEditors.TextEdit();
            this.btnCikis = new DevExpress.XtraEditors.SimpleButton();
            this.txtAd = new DevExpress.XtraEditors.TextEdit();
            this.txtTutar = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarkod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTutar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBarkod
            // 
            this.txtBarkod.EditValue = "54564646546465";
            this.txtBarkod.EnterMoveNextControl = true;
            this.txtBarkod.Location = new System.Drawing.Point(256, 12);
            this.txtBarkod.Name = "txtBarkod";
            this.txtBarkod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtBarkod.Properties.Appearance.Options.UseFont = true;
            this.txtBarkod.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBarkod.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtBarkod.Size = new System.Drawing.Size(434, 40);
            this.txtBarkod.TabIndex = 0;
            this.txtBarkod.Leave += new System.EventHandler(this.txtBarkod_Leave);
            // 
            // btnCikis
            // 
            this.btnCikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCikis.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCikis.Appearance.Options.UseFont = true;
            this.btnCikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCikis.ImageOptions.Image")));
            this.btnCikis.Location = new System.Drawing.Point(532, 243);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(167, 82);
            this.btnCikis.TabIndex = 152;
            this.btnCikis.Text = "Çıkış\r\n(Esc)";
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // txtAd
            // 
            this.txtAd.EditValue = "DÖNER";
            this.txtAd.Location = new System.Drawing.Point(12, 73);
            this.txtAd.Name = "txtAd";
            this.txtAd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAd.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.txtAd.Properties.Appearance.Options.UseFont = true;
            this.txtAd.Properties.Appearance.Options.UseForeColor = true;
            this.txtAd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAd.Properties.AutoHeight = false;
            this.txtAd.Size = new System.Drawing.Size(678, 89);
            this.txtAd.TabIndex = 0;
            // 
            // txtTutar
            // 
            this.txtTutar.EditValue = "15 TL";
            this.txtTutar.Location = new System.Drawing.Point(12, 168);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTutar.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.txtTutar.Properties.Appearance.Options.UseFont = true;
            this.txtTutar.Properties.Appearance.Options.UseForeColor = true;
            this.txtTutar.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTutar.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtTutar.Size = new System.Drawing.Size(678, 64);
            this.txtTutar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 39);
            this.label1.TabIndex = 153;
            this.label1.Text = "BARKOD NO :";
            // 
            // BarkodUrunBilgisiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 331);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.txtAd);
            this.Controls.Add(this.txtBarkod);
            this.Name = "BarkodUrunBilgisiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Barkod Ürün Bilgisi";
            this.Load += new System.EventHandler(this.BarkodUrunBilgisiForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtBarkod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTutar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtBarkod;
        private DevExpress.XtraEditors.SimpleButton btnCikis;
        private DevExpress.XtraEditors.TextEdit txtAd;
        private DevExpress.XtraEditors.TextEdit txtTutar;
        private System.Windows.Forms.Label label1;
    }
}