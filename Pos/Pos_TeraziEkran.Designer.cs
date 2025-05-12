namespace Pos
{
    partial class Pos_TeraziEkran
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pos_TeraziEkran));
            this.digitalGauge1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge();
            this.digitalBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.txt_Sayi = new DevExpress.XtraEditors.TextEdit();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txt_UrunAdi = new DevExpress.XtraEditors.TextEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_00 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton11 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton13 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton14 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.digitalGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Sayi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UrunAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // digitalGauge1
            // 
            this.digitalGauge1.AppearanceOff.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#0FD4F2FF");
            this.digitalGauge1.AppearanceOn.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#D4F2FF");
            this.digitalGauge1.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent[] {
            this.digitalBackgroundLayerComponent1});
            this.digitalGauge1.Bounds = new System.Drawing.Rectangle(6, 6, 238, 86);
            this.digitalGauge1.DigitCount = 5;
            this.digitalGauge1.Name = "digitalGauge1";
            this.digitalGauge1.Text = "00,000";
            // 
            // digitalBackgroundLayerComponent1
            // 
            this.digitalBackgroundLayerComponent1.BottomRight = new DevExpress.XtraGauges.Core.Base.PointF2D(259.8125F, 99.96249F);
            this.digitalBackgroundLayerComponent1.Name = "b1";
            this.digitalBackgroundLayerComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.DigitalBackgroundShapeSetType.Style2;
            this.digitalBackgroundLayerComponent1.TopLeft = new DevExpress.XtraGauges.Core.Base.PointF2D(20F, 0F);
            this.digitalBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // txt_Sayi
            // 
            resources.ApplyResources(this.txt_Sayi, "txt_Sayi");
            this.txt_Sayi.Name = "txt_Sayi";
            this.txt_Sayi.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_Sayi.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("txt_Sayi.Properties.Appearance.Font")));
            this.txt_Sayi.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txt_Sayi.Properties.Appearance.Options.UseBackColor = true;
            this.txt_Sayi.Properties.Appearance.Options.UseFont = true;
            this.txt_Sayi.Properties.Appearance.Options.UseForeColor = true;
            this.txt_Sayi.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_Sayi.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_Sayi.Properties.Mask.EditMask = resources.GetString("txt_Sayi.Properties.Mask.EditMask");
            this.txt_Sayi.Properties.Mask.MaskType = ((DevExpress.XtraEditors.Mask.MaskType)(resources.GetObject("txt_Sayi.Properties.Mask.MaskType")));
            // 
            // txt_UrunAdi
            // 
            resources.ApplyResources(this.txt_UrunAdi, "txt_UrunAdi");
            this.txt_UrunAdi.Name = "txt_UrunAdi";
            this.txt_UrunAdi.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_UrunAdi.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("txt_UrunAdi.Properties.Appearance.Font")));
            this.txt_UrunAdi.Properties.Appearance.ForeColor = System.Drawing.Color.MediumBlue;
            this.txt_UrunAdi.Properties.Appearance.Options.UseBackColor = true;
            this.txt_UrunAdi.Properties.Appearance.Options.UseFont = true;
            this.txt_UrunAdi.Properties.Appearance.Options.UseForeColor = true;
            this.txt_UrunAdi.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_UrunAdi.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_UrunAdi.Properties.ReadOnly = true;
            this.txt_UrunAdi.TabStop = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txt_Sayi);
            resources.ApplyResources(this.groupControl1, "groupControl1");
            this.groupControl1.Name = "groupControl1";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.simpleButton3);
            this.groupControl2.Controls.Add(this.btn_Cikis);
            this.groupControl2.Controls.Add(this.simpleButton4);
            this.groupControl2.Controls.Add(this.btn_00);
            this.groupControl2.Controls.Add(this.btn_OK);
            this.groupControl2.Controls.Add(this.simpleButton5);
            this.groupControl2.Controls.Add(this.simpleButton6);
            this.groupControl2.Controls.Add(this.simpleButton7);
            this.groupControl2.Controls.Add(this.simpleButton8);
            this.groupControl2.Controls.Add(this.simpleButton9);
            this.groupControl2.Controls.Add(this.simpleButton10);
            this.groupControl2.Controls.Add(this.simpleButton11);
            this.groupControl2.Controls.Add(this.simpleButton12);
            this.groupControl2.Controls.Add(this.simpleButton13);
            this.groupControl2.Controls.Add(this.simpleButton14);
            resources.ApplyResources(this.groupControl2, "groupControl2");
            this.groupControl2.Name = "groupControl2";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton3.Appearance.Font")));
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            resources.ApplyResources(this.simpleButton3, "simpleButton3");
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.ImageOptions.Image")));
            this.btn_Cikis.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton4.Appearance.Font")));
            this.simpleButton4.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton4, "simpleButton4");
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_00
            // 
            this.btn_00.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_00.Appearance.Font")));
            this.btn_00.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_00, "btn_00");
            this.btn_00.Name = "btn_00";
            this.btn_00.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_OK.Appearance.Font")));
            this.btn_OK.Appearance.Options.UseFont = true;
            this.btn_OK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_OK.ImageOptions.Image")));
            this.btn_OK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            resources.ApplyResources(this.btn_OK, "btn_OK");
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton5.Appearance.Font")));
            this.simpleButton5.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton5, "simpleButton5");
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton6.Appearance.Font")));
            this.simpleButton6.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton6, "simpleButton6");
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton7.Appearance.Font")));
            this.simpleButton7.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton7, "simpleButton7");
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton8.Appearance.Font")));
            this.simpleButton8.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton8, "simpleButton8");
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton9
            // 
            this.simpleButton9.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton9.Appearance.Font")));
            this.simpleButton9.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton9, "simpleButton9");
            this.simpleButton9.Name = "simpleButton9";
            this.simpleButton9.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton10.Appearance.Font")));
            this.simpleButton10.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton10, "simpleButton10");
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton11
            // 
            this.simpleButton11.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton11.Appearance.Font")));
            this.simpleButton11.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton11, "simpleButton11");
            this.simpleButton11.Name = "simpleButton11";
            this.simpleButton11.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton12
            // 
            this.simpleButton12.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton12.Appearance.Font")));
            this.simpleButton12.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton12, "simpleButton12");
            this.simpleButton12.Name = "simpleButton12";
            this.simpleButton12.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton13
            // 
            this.simpleButton13.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton13.Appearance.Font")));
            this.simpleButton13.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton13, "simpleButton13");
            this.simpleButton13.Name = "simpleButton13";
            this.simpleButton13.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton14
            // 
            this.simpleButton14.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton14.Appearance.Font")));
            this.simpleButton14.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.simpleButton14, "simpleButton14");
            this.simpleButton14.Name = "simpleButton14";
            this.simpleButton14.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // Pos_TeraziEkran
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.txt_UrunAdi);
            this.Controls.Add(this.groupControl2);
            this.Name = "Pos_TeraziEkran";
            this.Load += new System.EventHandler(this.Pos_TeraziEkran_Load);
            ((System.ComponentModel.ISupportInitialize)(this.digitalGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Sayi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UrunAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge digitalGauge1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent digitalBackgroundLayerComponent1;
        private System.IO.Ports.SerialPort serialPort1;
        private DevExpress.XtraEditors.TextEdit txt_Sayi;
        private System.Windows.Forms.Timer timer1;
        public DevExpress.XtraEditors.TextEdit txt_UrunAdi;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton btn_00;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private DevExpress.XtraEditors.SimpleButton simpleButton9;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
        private DevExpress.XtraEditors.SimpleButton simpleButton11;
        private DevExpress.XtraEditors.SimpleButton simpleButton12;
        private DevExpress.XtraEditors.SimpleButton simpleButton13;
        private DevExpress.XtraEditors.SimpleButton simpleButton14;
    }
}