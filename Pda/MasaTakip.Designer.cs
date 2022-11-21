namespace Pda
{
    partial class MasaTakip
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btn_Satis = new DevExpress.XtraBars.BarButtonItem();
            this.btn_Hesap = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btn_MasaTr = new DevExpress.XtraBars.BarButtonItem();
            this.btn_MalzTr = new DevExpress.XtraBars.BarButtonItem();
            this.btn_OzelMasa = new DevExpress.XtraBars.BarButtonItem();
            this.btn_OdaKontrol = new DevExpress.XtraBars.BarButtonItem();
            this.btn_HesapDokum = new DevExpress.XtraBars.BarButtonItem();
            this.btn_Mars = new DevExpress.XtraBars.BarButtonItem();
            this.btn_KisiSayisi = new DevExpress.XtraBars.BarButtonItem();
            this.btn_Yenile = new DevExpress.XtraBars.BarButtonItem();
            this.btn_Cikis = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.bartxt_FisNo = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.barspn_Refresh = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemSpinEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_Satis,
            this.btn_Hesap,
            this.barSubItem1,
            this.btn_Yenile,
            this.btn_Cikis,
            this.barspn_Refresh,
            this.bartxt_FisNo,
            this.btn_MasaTr,
            this.btn_MalzTr,
            this.btn_OzelMasa,
            this.btn_OdaKontrol,
            this.btn_HesapDokum,
            this.btn_Mars,
            this.btn_KisiSayisi,
            this.barButtonItem1});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 16;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1,
            this.repositoryItemSpinEdit2,
            this.repositoryItemSpinEdit3});
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarItemVertIndent = 10;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Satis),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Hesap),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Yenile),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Cikis)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btn_Satis
            // 
            this.btn_Satis.Caption = "Satis";
            this.btn_Satis.Id = 0;
            this.btn_Satis.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Satis.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_Satis.Name = "btn_Satis";
            this.btn_Satis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Satis_ItemClick);
            // 
            // btn_Hesap
            // 
            this.btn_Hesap.Caption = "Hesap";
            this.btn_Hesap.Id = 1;
            this.btn_Hesap.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Hesap.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_Hesap.Name = "btn_Hesap";
            this.btn_Hesap.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Hesap_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Islem";
            this.barSubItem1.Id = 2;
            this.barSubItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barSubItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_MasaTr),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_MalzTr),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_OzelMasa),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_OdaKontrol),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_HesapDokum),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Mars),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_KisiSayisi),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btn_MasaTr
            // 
            this.btn_MasaTr.Caption = "Masa Transfer";
            this.btn_MasaTr.Id = 8;
            this.btn_MasaTr.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_MasaTr.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_MasaTr.Name = "btn_MasaTr";
            this.btn_MasaTr.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_MasaTr_ItemClick);
            // 
            // btn_MalzTr
            // 
            this.btn_MalzTr.Caption = "Malzeme Transfer";
            this.btn_MalzTr.Id = 9;
            this.btn_MalzTr.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_MalzTr.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_MalzTr.Name = "btn_MalzTr";
            this.btn_MalzTr.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_MalzTr_ItemClick);
            // 
            // btn_OzelMasa
            // 
            this.btn_OzelMasa.Caption = "Özel Masa";
            this.btn_OzelMasa.Id = 10;
            this.btn_OzelMasa.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_OzelMasa.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_OzelMasa.Name = "btn_OzelMasa";
            this.btn_OzelMasa.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_OzelMasa_ItemClick);
            // 
            // btn_OdaKontrol
            // 
            this.btn_OdaKontrol.Caption = "Oda Kontrol";
            this.btn_OdaKontrol.Id = 11;
            this.btn_OdaKontrol.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_OdaKontrol.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_OdaKontrol.Name = "btn_OdaKontrol";
            this.btn_OdaKontrol.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_OdaKontrol_ItemClick);
            // 
            // btn_HesapDokum
            // 
            this.btn_HesapDokum.Caption = "Hesap Dökümü";
            this.btn_HesapDokum.Id = 12;
            this.btn_HesapDokum.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_HesapDokum.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_HesapDokum.Name = "btn_HesapDokum";
            this.btn_HesapDokum.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_HesapDokum_ItemClick);
            // 
            // btn_Mars
            // 
            this.btn_Mars.Caption = "Mars";
            this.btn_Mars.Id = 13;
            this.btn_Mars.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Mars.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_Mars.Name = "btn_Mars";
            this.btn_Mars.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Mars_ItemClick);
            // 
            // btn_KisiSayisi
            // 
            this.btn_KisiSayisi.Caption = "Kisi Sayısı Duzelt";
            this.btn_KisiSayisi.Id = 14;
            this.btn_KisiSayisi.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_KisiSayisi.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_KisiSayisi.Name = "btn_KisiSayisi";
            this.btn_KisiSayisi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_KisiSayisi_ItemClick);
            // 
            // btn_Yenile
            // 
            this.btn_Yenile.Caption = "Yenile";
            this.btn_Yenile.Id = 3;
            this.btn_Yenile.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Yenile.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_Yenile.Name = "btn_Yenile";
            this.btn_Yenile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Yenile_ItemClick);
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Caption = "Cıkıs";
            this.btn_Cikis.Id = 4;
            this.btn_Cikis.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Cikis.ItemAppearance.Normal.Options.UseFont = true;
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Cikis_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.bartxt_FisNo, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barspn_Refresh)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            this.bar3.Visible = false;
            // 
            // bartxt_FisNo
            // 
            this.bartxt_FisNo.Caption = "Fis No:";
            this.bartxt_FisNo.Edit = this.repositoryItemSpinEdit3;
            this.bartxt_FisNo.Id = 7;
            this.bartxt_FisNo.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bartxt_FisNo.ItemAppearance.Normal.Options.UseFont = true;
            this.bartxt_FisNo.Name = "bartxt_FisNo";
            // 
            // repositoryItemSpinEdit3
            // 
            this.repositoryItemSpinEdit3.AutoHeight = false;
            this.repositoryItemSpinEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit3.Name = "repositoryItemSpinEdit3";
            this.repositoryItemSpinEdit3.ReadOnly = true;
            // 
            // barspn_Refresh
            // 
            this.barspn_Refresh.Caption = "barEditItem1";
            this.barspn_Refresh.Edit = this.repositoryItemSpinEdit1;
            this.barspn_Refresh.Id = 5;
            this.barspn_Refresh.Name = "barspn_Refresh";
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            this.repositoryItemSpinEdit1.ReadOnly = true;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(244, 34);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 246);
            this.barDockControlBottom.Size = new System.Drawing.Size(244, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 34);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 212);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(244, 34);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 212);
            // 
            // repositoryItemSpinEdit2
            // 
            this.repositoryItemSpinEdit2.AutoHeight = false;
            this.repositoryItemSpinEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
            this.repositoryItemSpinEdit2.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 34);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(244, 212);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Satılan Ürünler";
            this.barButtonItem1.Id = 15;
            this.barButtonItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.barButtonItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // MasaTakip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 271);
            this.ControlBox = false;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MasaTakip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Masa Takip";
            this.Load += new System.EventHandler(this.MasaTakip_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btn_Satis;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraBars.BarButtonItem btn_Hesap;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btn_Yenile;
        private DevExpress.XtraBars.BarButtonItem btn_Cikis;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraBars.BarEditItem barspn_Refresh;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit2;
        private DevExpress.XtraBars.BarEditItem bartxt_FisNo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit3;
        private DevExpress.XtraBars.BarButtonItem btn_MasaTr;
        private DevExpress.XtraBars.BarButtonItem btn_MalzTr;
        private DevExpress.XtraBars.BarButtonItem btn_OzelMasa;
        private DevExpress.XtraBars.BarButtonItem btn_OdaKontrol;
        private DevExpress.XtraBars.BarButtonItem btn_HesapDokum;
        private DevExpress.XtraBars.BarButtonItem btn_Mars;
        private DevExpress.XtraBars.BarButtonItem btn_KisiSayisi;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}