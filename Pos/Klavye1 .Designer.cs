namespace Pos
{
    partial class Klavye1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Klavye1));
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.txt_Sayi = new DevExpress.XtraEditors.TextEdit();
            this.btn_V = new DevExpress.XtraEditors.SimpleButton();
            this.btn_00 = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Baslik = new DevExpress.XtraEditors.LabelControl();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.btn_0 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_3 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_2 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_1 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_6 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_5 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_4 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_9 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_8 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_7 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Sil = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_UrunAdi = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Sayi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_Cikis.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Cikis.ImageOptions.SvgImage")));
            this.btn_Cikis.Location = new System.Drawing.Point(240, 362);
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Size = new System.Drawing.Size(81, 70);
            this.btn_Cikis.TabIndex = 50;
            this.btn_Cikis.Text = "İPTAL";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // txt_Sayi
            // 
            this.txt_Sayi.EditValue = "1,000";
            this.txt_Sayi.Location = new System.Drawing.Point(12, 74);
            this.txt_Sayi.Name = "txt_Sayi";
            this.txt_Sayi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_Sayi.Properties.Appearance.Options.UseFont = true;
            this.txt_Sayi.Properties.DisplayFormat.FormatString = "f3";
            this.txt_Sayi.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_Sayi.Properties.EditFormat.FormatString = "f3";
            this.txt_Sayi.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_Sayi.Properties.Mask.EditMask = "n3";
            this.txt_Sayi.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_Sayi.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_Sayi.Size = new System.Drawing.Size(300, 46);
            this.txt_Sayi.TabIndex = 0;
            this.txt_Sayi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Sayi_KeyPress);
            // 
            // btn_V
            // 
            this.btn_V.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_V.Appearance.Options.UseFont = true;
            this.btn_V.Location = new System.Drawing.Point(164, 362);
            this.btn_V.Name = "btn_V";
            this.btn_V.Size = new System.Drawing.Size(70, 70);
            this.btn_V.TabIndex = 48;
            this.btn_V.Text = ",";
            this.btn_V.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_00
            // 
            this.btn_00.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_00.Appearance.Options.UseFont = true;
            this.btn_00.Location = new System.Drawing.Point(88, 362);
            this.btn_00.Name = "btn_00";
            this.btn_00.Size = new System.Drawing.Size(70, 70);
            this.btn_00.TabIndex = 47;
            this.btn_00.Text = "00";
            this.btn_00.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // lbl_Baslik
            // 
            this.lbl_Baslik.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Baslik.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Baslik.Appearance.Options.UseFont = true;
            this.lbl_Baslik.Appearance.Options.UseForeColor = true;
            this.lbl_Baslik.Location = new System.Drawing.Point(12, 43);
            this.lbl_Baslik.Name = "lbl_Baslik";
            this.lbl_Baslik.Size = new System.Drawing.Size(151, 25);
            this.lbl_Baslik.TabIndex = 46;
            this.lbl_Baslik.Text = "Tutar Giriniz...";
            // 
            // btn_OK
            // 
            this.btn_OK.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_OK.Appearance.Options.UseFont = true;
            this.btn_OK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_OK.ImageOptions.Image")));
            this.btn_OK.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btn_OK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btn_OK.Location = new System.Drawing.Point(240, 134);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(81, 109);
            this.btn_OK.TabIndex = 45;
            this.btn_OK.Text = "ONAYLA";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_0
            // 
            this.btn_0.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_0.Appearance.Options.UseFont = true;
            this.btn_0.Location = new System.Drawing.Point(12, 362);
            this.btn_0.Name = "btn_0";
            this.btn_0.Size = new System.Drawing.Size(70, 70);
            this.btn_0.TabIndex = 44;
            this.btn_0.Text = "0";
            this.btn_0.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_3
            // 
            this.btn_3.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_3.Appearance.Options.UseFont = true;
            this.btn_3.Location = new System.Drawing.Point(164, 286);
            this.btn_3.Name = "btn_3";
            this.btn_3.Size = new System.Drawing.Size(70, 70);
            this.btn_3.TabIndex = 43;
            this.btn_3.Text = "3";
            this.btn_3.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_2
            // 
            this.btn_2.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_2.Appearance.Options.UseFont = true;
            this.btn_2.Location = new System.Drawing.Point(88, 286);
            this.btn_2.Name = "btn_2";
            this.btn_2.Size = new System.Drawing.Size(70, 70);
            this.btn_2.TabIndex = 42;
            this.btn_2.Text = "2";
            this.btn_2.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_1
            // 
            this.btn_1.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_1.Appearance.Options.UseFont = true;
            this.btn_1.Location = new System.Drawing.Point(12, 286);
            this.btn_1.Name = "btn_1";
            this.btn_1.Size = new System.Drawing.Size(70, 70);
            this.btn_1.TabIndex = 41;
            this.btn_1.Text = "1";
            this.btn_1.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_6
            // 
            this.btn_6.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_6.Appearance.Options.UseFont = true;
            this.btn_6.Location = new System.Drawing.Point(164, 210);
            this.btn_6.Name = "btn_6";
            this.btn_6.Size = new System.Drawing.Size(70, 70);
            this.btn_6.TabIndex = 40;
            this.btn_6.Text = "6";
            this.btn_6.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_5
            // 
            this.btn_5.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_5.Appearance.Options.UseFont = true;
            this.btn_5.Location = new System.Drawing.Point(88, 210);
            this.btn_5.Name = "btn_5";
            this.btn_5.Size = new System.Drawing.Size(70, 70);
            this.btn_5.TabIndex = 39;
            this.btn_5.Text = "5";
            this.btn_5.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_4
            // 
            this.btn_4.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_4.Appearance.Options.UseFont = true;
            this.btn_4.Location = new System.Drawing.Point(12, 210);
            this.btn_4.Name = "btn_4";
            this.btn_4.Size = new System.Drawing.Size(70, 70);
            this.btn_4.TabIndex = 38;
            this.btn_4.Text = "4";
            this.btn_4.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_9
            // 
            this.btn_9.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_9.Appearance.Options.UseFont = true;
            this.btn_9.Location = new System.Drawing.Point(164, 134);
            this.btn_9.Name = "btn_9";
            this.btn_9.Size = new System.Drawing.Size(70, 70);
            this.btn_9.TabIndex = 37;
            this.btn_9.Text = "9";
            this.btn_9.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_8
            // 
            this.btn_8.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_8.Appearance.Options.UseFont = true;
            this.btn_8.Location = new System.Drawing.Point(88, 134);
            this.btn_8.Name = "btn_8";
            this.btn_8.Size = new System.Drawing.Size(70, 70);
            this.btn_8.TabIndex = 36;
            this.btn_8.Text = "8";
            this.btn_8.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_7
            // 
            this.btn_7.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_7.Appearance.Options.UseFont = true;
            this.btn_7.Location = new System.Drawing.Point(12, 134);
            this.btn_7.Name = "btn_7";
            this.btn_7.Size = new System.Drawing.Size(70, 70);
            this.btn_7.TabIndex = 35;
            this.btn_7.Text = "7";
            this.btn_7.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_Sil
            // 
            this.btn_Sil.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Sil.Appearance.Options.UseFont = true;
            this.btn_Sil.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Sil.ImageOptions.Image")));
            this.btn_Sil.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btn_Sil.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_Sil.Location = new System.Drawing.Point(240, 249);
            this.btn_Sil.Name = "btn_Sil";
            this.btn_Sil.Size = new System.Drawing.Size(81, 107);
            this.btn_Sil.TabIndex = 51;
            this.btn_Sil.Text = "SİL";
            this.btn_Sil.Click += new System.EventHandler(this.btn_Sil_Click);
            // 
            // lbl_UrunAdi
            // 
            this.lbl_UrunAdi.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_UrunAdi.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbl_UrunAdi.Appearance.Options.UseFont = true;
            this.lbl_UrunAdi.Appearance.Options.UseForeColor = true;
            this.lbl_UrunAdi.Location = new System.Drawing.Point(12, 12);
            this.lbl_UrunAdi.Name = "lbl_UrunAdi";
            this.lbl_UrunAdi.Size = new System.Drawing.Size(151, 25);
            this.lbl_UrunAdi.TabIndex = 52;
            this.lbl_UrunAdi.Text = "Tutar Giriniz...";
            // 
            // Klavye1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 444);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_UrunAdi);
            this.Controls.Add(this.btn_Sil);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.txt_Sayi);
            this.Controls.Add(this.btn_V);
            this.Controls.Add(this.btn_00);
            this.Controls.Add(this.lbl_Baslik);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_0);
            this.Controls.Add(this.btn_3);
            this.Controls.Add(this.btn_2);
            this.Controls.Add(this.btn_1);
            this.Controls.Add(this.btn_6);
            this.Controls.Add(this.btn_5);
            this.Controls.Add(this.btn_4);
            this.Controls.Add(this.btn_9);
            this.Controls.Add(this.btn_8);
            this.Controls.Add(this.btn_7);
            this.KeyPreview = true;
            this.Name = "Klavye1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Klavye1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Klavye1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Sayi.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        public DevExpress.XtraEditors.TextEdit txt_Sayi;
        private DevExpress.XtraEditors.SimpleButton btn_V;
        private DevExpress.XtraEditors.SimpleButton btn_00;
        private DevExpress.XtraEditors.LabelControl lbl_Baslik;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.SimpleButton btn_0;
        private DevExpress.XtraEditors.SimpleButton btn_3;
        private DevExpress.XtraEditors.SimpleButton btn_2;
        private DevExpress.XtraEditors.SimpleButton btn_1;
        private DevExpress.XtraEditors.SimpleButton btn_6;
        private DevExpress.XtraEditors.SimpleButton btn_5;
        private DevExpress.XtraEditors.SimpleButton btn_4;
        private DevExpress.XtraEditors.SimpleButton btn_9;
        private DevExpress.XtraEditors.SimpleButton btn_8;
        private DevExpress.XtraEditors.SimpleButton btn_7;
        private DevExpress.XtraEditors.SimpleButton btn_Sil;
        private DevExpress.XtraEditors.LabelControl lbl_UrunAdi;
    }
}