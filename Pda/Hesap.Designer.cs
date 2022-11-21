namespace Pda
{
    partial class Hesap
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.lbl_Bilgi = new DevExpress.XtraEditors.LabelControl();
            this.txt_Odemetutari = new DevExpress.XtraEditors.TextEdit();
            this.look_Kapatma = new DevExpress.XtraEditors.LookUpEdit();
            this.btn_HesapDok = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit4 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Hesapno = new DevExpress.XtraEditors.TextEdit();
            this.btn_Tutar = new DevExpress.XtraEditors.SimpleButton();
            this.btn_HesapAra = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Odeme = new DevExpress.XtraEditors.SimpleButton();
            this.btn_YazdirKapat = new DevExpress.XtraEditors.SimpleButton();
            this.btn_OdemeSil = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Indirim = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_Yazdirmadankapat = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Odemetutari.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_Kapatma.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Hesapno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Ba";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "Ödeme Şekli";
            this.textEdit1.Location = new System.Drawing.Point(4, 19);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(68, 20);
            this.textEdit1.TabIndex = 0;
            this.textEdit1.TabStop = false;
            // 
            // lbl_Bilgi
            // 
            this.lbl_Bilgi.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lbl_Bilgi.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbl_Bilgi.Location = new System.Drawing.Point(4, 3);
            this.lbl_Bilgi.Name = "lbl_Bilgi";
            this.lbl_Bilgi.Size = new System.Drawing.Size(9, 13);
            this.lbl_Bilgi.TabIndex = 1;
            this.lbl_Bilgi.Text = "...";
            // 
            // txt_Odemetutari
            // 
            this.txt_Odemetutari.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Odemetutari.EditValue = "0,00";
            this.txt_Odemetutari.Location = new System.Drawing.Point(73, 42);
            this.txt_Odemetutari.Name = "txt_Odemetutari";
            this.txt_Odemetutari.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.txt_Odemetutari.Properties.Appearance.Options.UseFont = true;
            this.txt_Odemetutari.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Odemetutari.Properties.Mask.EditMask = "n2";
            this.txt_Odemetutari.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_Odemetutari.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_Odemetutari.Size = new System.Drawing.Size(84, 20);
            this.txt_Odemetutari.TabIndex = 8;
            // 
            // look_Kapatma
            // 
            this.look_Kapatma.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.look_Kapatma.Location = new System.Drawing.Point(73, 19);
            this.look_Kapatma.Name = "look_Kapatma";
            this.look_Kapatma.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.look_Kapatma.Properties.Appearance.Options.UseFont = true;
            this.look_Kapatma.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 10F);
            this.look_Kapatma.Properties.AppearanceDropDown.Options.UseFont = true;
            this.look_Kapatma.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.look_Kapatma.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_Kapatma.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Pkod_Ad", "Pkod_Ad")});
            this.look_Kapatma.Properties.NullText = "";
            this.look_Kapatma.Properties.ShowFooter = false;
            this.look_Kapatma.Properties.ShowHeader = false;
            this.look_Kapatma.Size = new System.Drawing.Size(84, 20);
            this.look_Kapatma.TabIndex = 0;
            // 
            // btn_HesapDok
            // 
            this.btn_HesapDok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_HesapDok.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_HesapDok.Appearance.Options.UseFont = true;
            this.btn_HesapDok.Appearance.Options.UseTextOptions = true;
            this.btn_HesapDok.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_HesapDok.Location = new System.Drawing.Point(159, 17);
            this.btn_HesapDok.Name = "btn_HesapDok";
            this.btn_HesapDok.Size = new System.Drawing.Size(73, 22);
            this.btn_HesapDok.TabIndex = 11;
            this.btn_HesapDok.TabStop = false;
            this.btn_HesapDok.Text = "Hesap Dök";
            this.btn_HesapDok.Click += new System.EventHandler(this.btn_HesapDok_Click);
            // 
            // textEdit3
            // 
            this.textEdit3.EditValue = "Tutar";
            this.textEdit3.Location = new System.Drawing.Point(4, 42);
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.textEdit3.Properties.Appearance.Options.UseFont = true;
            this.textEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit3.Properties.ReadOnly = true;
            this.textEdit3.Size = new System.Drawing.Size(68, 20);
            this.textEdit3.TabIndex = 5;
            this.textEdit3.TabStop = false;
            // 
            // textEdit4
            // 
            this.textEdit4.EditValue = "Hesap";
            this.textEdit4.Location = new System.Drawing.Point(4, 65);
            this.textEdit4.Name = "textEdit4";
            this.textEdit4.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.textEdit4.Properties.Appearance.Options.UseFont = true;
            this.textEdit4.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit4.Properties.ReadOnly = true;
            this.textEdit4.Size = new System.Drawing.Size(68, 20);
            this.textEdit4.TabIndex = 6;
            this.textEdit4.TabStop = false;
            // 
            // txt_Hesapno
            // 
            this.txt_Hesapno.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Hesapno.Location = new System.Drawing.Point(73, 65);
            this.txt_Hesapno.Name = "txt_Hesapno";
            this.txt_Hesapno.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.txt_Hesapno.Properties.Appearance.Options.UseFont = true;
            this.txt_Hesapno.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Hesapno.Size = new System.Drawing.Size(84, 20);
            this.txt_Hesapno.TabIndex = 1;
            this.txt_Hesapno.Click += new System.EventHandler(this.txt_Hesapno_Click);
            this.txt_Hesapno.Leave += new System.EventHandler(this.txt_Hesapno_Leave);
            // 
            // btn_Tutar
            // 
            this.btn_Tutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Tutar.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.btn_Tutar.Appearance.Options.UseFont = true;
            this.btn_Tutar.Location = new System.Drawing.Point(159, 42);
            this.btn_Tutar.Name = "btn_Tutar";
            this.btn_Tutar.Size = new System.Drawing.Size(24, 20);
            this.btn_Tutar.TabIndex = 9;
            this.btn_Tutar.Text = "...";
            this.btn_Tutar.Click += new System.EventHandler(this.btn_Tutar_Click);
            // 
            // btn_HesapAra
            // 
            this.btn_HesapAra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_HesapAra.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F);
            this.btn_HesapAra.Appearance.Options.UseFont = true;
            this.btn_HesapAra.Location = new System.Drawing.Point(159, 65);
            this.btn_HesapAra.Name = "btn_HesapAra";
            this.btn_HesapAra.Size = new System.Drawing.Size(24, 20);
            this.btn_HesapAra.TabIndex = 2;
            this.btn_HesapAra.Text = "...";
            this.btn_HesapAra.Click += new System.EventHandler(this.btn_HesapAra_Click);
            // 
            // btn_Odeme
            // 
            this.btn_Odeme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Odeme.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_Odeme.Appearance.Options.UseFont = true;
            this.btn_Odeme.Location = new System.Drawing.Point(186, 42);
            this.btn_Odeme.Name = "btn_Odeme";
            this.btn_Odeme.Size = new System.Drawing.Size(46, 43);
            this.btn_Odeme.TabIndex = 10;
            this.btn_Odeme.TabStop = false;
            this.btn_Odeme.Text = "Ödeme";
            this.btn_Odeme.Click += new System.EventHandler(this.btn_Odeme_Click);
            // 
            // btn_YazdirKapat
            // 
            this.btn_YazdirKapat.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_YazdirKapat.Appearance.Options.UseFont = true;
            this.btn_YazdirKapat.Appearance.Options.UseTextOptions = true;
            this.btn_YazdirKapat.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_YazdirKapat.Location = new System.Drawing.Point(4, 89);
            this.btn_YazdirKapat.Name = "btn_YazdirKapat";
            this.btn_YazdirKapat.Size = new System.Drawing.Size(92, 22);
            this.btn_YazdirKapat.TabIndex = 3;
            this.btn_YazdirKapat.Text = "Yazdır Kapat";
            this.btn_YazdirKapat.Click += new System.EventHandler(this.btn_YazdirKapat_Click);
            // 
            // btn_OdemeSil
            // 
            this.btn_OdemeSil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OdemeSil.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_OdemeSil.Appearance.Options.UseFont = true;
            this.btn_OdemeSil.Appearance.Options.UseTextOptions = true;
            this.btn_OdemeSil.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_OdemeSil.Location = new System.Drawing.Point(4, 235);
            this.btn_OdemeSil.Name = "btn_OdemeSil";
            this.btn_OdemeSil.Size = new System.Drawing.Size(68, 22);
            this.btn_OdemeSil.TabIndex = 5;
            this.btn_OdemeSil.TabStop = false;
            this.btn_OdemeSil.Text = "Ödeme Sil";
            this.btn_OdemeSil.Click += new System.EventHandler(this.btn_OdemeSil_Click);
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cikis.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.Appearance.Options.UseTextOptions = true;
            this.btn_Cikis.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_Cikis.Location = new System.Drawing.Point(164, 235);
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Size = new System.Drawing.Size(68, 22);
            this.btn_Cikis.TabIndex = 7;
            this.btn_Cikis.TabStop = false;
            this.btn_Cikis.Text = "Çıkış";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // btn_Indirim
            // 
            this.btn_Indirim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Indirim.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_Indirim.Appearance.Options.UseFont = true;
            this.btn_Indirim.Appearance.Options.UseTextOptions = true;
            this.btn_Indirim.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_Indirim.Location = new System.Drawing.Point(78, 235);
            this.btn_Indirim.Name = "btn_Indirim";
            this.btn_Indirim.Size = new System.Drawing.Size(68, 22);
            this.btn_Indirim.TabIndex = 6;
            this.btn_Indirim.TabStop = false;
            this.btn_Indirim.Text = "İndirim";
            this.btn_Indirim.Click += new System.EventHandler(this.btn_Indirim_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(4, 115);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(228, 116);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Column = this.gridColumn6;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "A";
            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.Caption = "Urun";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 120;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.Caption = "Miktar";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 40;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn3.Caption = "..";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 20;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.Caption = "Tutar";
            this.gridColumn4.DisplayFormat.FormatString = "n2";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn4.Width = 55;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.Caption = "D.Tutar";
            this.gridColumn5.DisplayFormat.FormatString = "n2";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn5.Width = 55;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Id";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            // 
            // btn_Yazdirmadankapat
            // 
            this.btn_Yazdirmadankapat.Appearance.Font = new System.Drawing.Font("Tahoma", 7.25F, System.Drawing.FontStyle.Bold);
            this.btn_Yazdirmadankapat.Appearance.Options.UseFont = true;
            this.btn_Yazdirmadankapat.Appearance.Options.UseTextOptions = true;
            this.btn_Yazdirmadankapat.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_Yazdirmadankapat.Location = new System.Drawing.Point(102, 89);
            this.btn_Yazdirmadankapat.Name = "btn_Yazdirmadankapat";
            this.btn_Yazdirmadankapat.Size = new System.Drawing.Size(120, 22);
            this.btn_Yazdirmadankapat.TabIndex = 12;
            this.btn_Yazdirmadankapat.Text = "Yazdırmadan Kapat";
            this.btn_Yazdirmadankapat.Click += new System.EventHandler(this.btn_Yazdirmadankapat_Click);
            // 
            // Hesap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 261);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Yazdirmadankapat);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btn_Indirim);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.btn_OdemeSil);
            this.Controls.Add(this.btn_YazdirKapat);
            this.Controls.Add(this.btn_Odeme);
            this.Controls.Add(this.btn_HesapAra);
            this.Controls.Add(this.btn_Tutar);
            this.Controls.Add(this.txt_Hesapno);
            this.Controls.Add(this.textEdit4);
            this.Controls.Add(this.textEdit3);
            this.Controls.Add(this.btn_HesapDok);
            this.Controls.Add(this.look_Kapatma);
            this.Controls.Add(this.txt_Odemetutari);
            this.Controls.Add(this.lbl_Bilgi);
            this.Controls.Add(this.textEdit1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Hesap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hesap";
            this.Load += new System.EventHandler(this.Hesap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Odemetutari.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_Kapatma.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Hesapno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl lbl_Bilgi;
        private DevExpress.XtraEditors.TextEdit txt_Odemetutari;
        private DevExpress.XtraEditors.LookUpEdit look_Kapatma;
        private DevExpress.XtraEditors.SimpleButton btn_HesapDok;
        private DevExpress.XtraEditors.TextEdit textEdit3;
        private DevExpress.XtraEditors.TextEdit textEdit4;
        private DevExpress.XtraEditors.TextEdit txt_Hesapno;
        private DevExpress.XtraEditors.SimpleButton btn_Tutar;
        private DevExpress.XtraEditors.SimpleButton btn_HesapAra;
        private DevExpress.XtraEditors.SimpleButton btn_Odeme;
        private DevExpress.XtraEditors.SimpleButton btn_YazdirKapat;
        private DevExpress.XtraEditors.SimpleButton btn_OdemeSil;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton btn_Indirim;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.SimpleButton btn_Yazdirmadankapat;
    }
}