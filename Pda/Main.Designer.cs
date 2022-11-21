namespace Pda
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.btn_DepDegistir = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btn_MasaTakip = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Kur = new DevExpress.XtraEditors.LabelControl();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Kullanici = new DevExpress.XtraEditors.LabelControl();
            this.btn_DirekSatis = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Departman = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Tarih = new DevExpress.XtraEditors.LabelControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_Relogin = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(34, 248);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(64, 13);
            this.labelControl8.TabIndex = 22;
            this.labelControl8.Text = "Günlük Kur   :";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Appearance.Options.UseForeColor = true;
            this.labelControl7.Location = new System.Drawing.Point(34, 229);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(65, 13);
            this.labelControl7.TabIndex = 21;
            this.labelControl7.Text = "Kullanıcı        :";
            // 
            // btn_DepDegistir
            // 
            this.btn_DepDegistir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DepDegistir.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_DepDegistir.Appearance.Options.UseFont = true;
            this.btn_DepDegistir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_DepDegistir.ImageOptions.Image")));
            this.btn_DepDegistir.Location = new System.Drawing.Point(30, 91);
            this.btn_DepDegistir.Name = "btn_DepDegistir";
            this.btn_DepDegistir.Size = new System.Drawing.Size(185, 37);
            this.btn_DepDegistir.TabIndex = 17;
            this.btn_DepDegistir.Text = "Departman Değiştir";
            this.btn_DepDegistir.Click += new System.EventHandler(this.btn_DepDegistir_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Appearance.Options.UseForeColor = true;
            this.labelControl6.Location = new System.Drawing.Point(34, 210);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(63, 13);
            this.labelControl6.TabIndex = 20;
            this.labelControl6.Text = "Departman  :";
            // 
            // btn_MasaTakip
            // 
            this.btn_MasaTakip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_MasaTakip.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_MasaTakip.Appearance.Options.UseFont = true;
            this.btn_MasaTakip.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_MasaTakip.ImageOptions.Image")));
            this.btn_MasaTakip.Location = new System.Drawing.Point(30, 11);
            this.btn_MasaTakip.Name = "btn_MasaTakip";
            this.btn_MasaTakip.Size = new System.Drawing.Size(185, 37);
            this.btn_MasaTakip.TabIndex = 11;
            this.btn_MasaTakip.Text = "Masa Takip";
            this.btn_MasaTakip.Click += new System.EventHandler(this.btn_MasaTakip_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Appearance.Options.UseForeColor = true;
            this.labelControl5.Location = new System.Drawing.Point(34, 190);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 13);
            this.labelControl5.TabIndex = 19;
            this.labelControl5.Text = "Tarih            :";
            // 
            // lbl_Kur
            // 
            this.lbl_Kur.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Kur.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbl_Kur.Appearance.Options.UseFont = true;
            this.lbl_Kur.Appearance.Options.UseForeColor = true;
            this.lbl_Kur.Location = new System.Drawing.Point(104, 247);
            this.lbl_Kur.Name = "lbl_Kur";
            this.lbl_Kur.Size = new System.Drawing.Size(12, 13);
            this.lbl_Kur.TabIndex = 18;
            this.lbl_Kur.Text = "...";
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cikis.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.ImageOptions.Image")));
            this.btn_Cikis.Location = new System.Drawing.Point(126, 131);
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Size = new System.Drawing.Size(89, 37);
            this.btn_Cikis.TabIndex = 15;
            this.btn_Cikis.Text = "Cıkıs";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // lbl_Kullanici
            // 
            this.lbl_Kullanici.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Kullanici.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbl_Kullanici.Appearance.Options.UseFont = true;
            this.lbl_Kullanici.Appearance.Options.UseForeColor = true;
            this.lbl_Kullanici.Location = new System.Drawing.Point(104, 228);
            this.lbl_Kullanici.Name = "lbl_Kullanici";
            this.lbl_Kullanici.Size = new System.Drawing.Size(12, 13);
            this.lbl_Kullanici.TabIndex = 16;
            this.lbl_Kullanici.Text = "...";
            // 
            // btn_DirekSatis
            // 
            this.btn_DirekSatis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DirekSatis.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_DirekSatis.Appearance.Options.UseFont = true;
            this.btn_DirekSatis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_DirekSatis.ImageOptions.Image")));
            this.btn_DirekSatis.Location = new System.Drawing.Point(30, 51);
            this.btn_DirekSatis.Name = "btn_DirekSatis";
            this.btn_DirekSatis.Size = new System.Drawing.Size(185, 37);
            this.btn_DirekSatis.TabIndex = 12;
            this.btn_DirekSatis.Text = "Direk Satış";
            this.btn_DirekSatis.Click += new System.EventHandler(this.btn_DirekSatis_Click);
            // 
            // lbl_Departman
            // 
            this.lbl_Departman.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Departman.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbl_Departman.Appearance.Options.UseFont = true;
            this.lbl_Departman.Appearance.Options.UseForeColor = true;
            this.lbl_Departman.Location = new System.Drawing.Point(104, 209);
            this.lbl_Departman.Name = "lbl_Departman";
            this.lbl_Departman.Size = new System.Drawing.Size(12, 13);
            this.lbl_Departman.TabIndex = 14;
            this.lbl_Departman.Text = "...";
            // 
            // lbl_Tarih
            // 
            this.lbl_Tarih.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Tarih.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbl_Tarih.Appearance.Options.UseFont = true;
            this.lbl_Tarih.Appearance.Options.UseForeColor = true;
            this.lbl_Tarih.Location = new System.Drawing.Point(104, 189);
            this.lbl_Tarih.Name = "lbl_Tarih";
            this.lbl_Tarih.Size = new System.Drawing.Size(12, 13);
            this.lbl_Tarih.TabIndex = 13;
            this.lbl_Tarih.Text = "...";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_Relogin
            // 
            this.btn_Relogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Relogin.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Relogin.Appearance.Options.UseFont = true;
            this.btn_Relogin.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Relogin.ImageOptions.Image")));
            this.btn_Relogin.Location = new System.Drawing.Point(30, 131);
            this.btn_Relogin.Name = "btn_Relogin";
            this.btn_Relogin.Size = new System.Drawing.Size(95, 37);
            this.btn_Relogin.TabIndex = 23;
            this.btn_Relogin.Text = "Re Login";
            this.btn_Relogin.Click += new System.EventHandler(this.btn_Relogin_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 273);
            this.Controls.Add(this.btn_Relogin);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.btn_DepDegistir);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.btn_MasaTakip);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.lbl_Kur);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.lbl_Kullanici);
            this.Controls.Add(this.btn_DirekSatis);
            this.Controls.Add(this.lbl_Departman);
            this.Controls.Add(this.lbl_Tarih);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main v2.1";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton btn_DepDegistir;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btn_MasaTakip;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl lbl_Kur;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.LabelControl lbl_Kullanici;
        private DevExpress.XtraEditors.SimpleButton btn_DirekSatis;
        private DevExpress.XtraEditors.LabelControl lbl_Departman;
        private DevExpress.XtraEditors.LabelControl lbl_Tarih;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btn_Relogin;
    }
}