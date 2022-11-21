namespace Pos
{
    partial class IngenicoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IngenicoForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.m_lstBankErrorMessage = new System.Windows.Forms.ListBox();
            this.m_listPayment = new System.Windows.Forms.ListBox();
            this.m_listTransaction = new System.Windows.Forms.ListBox();
            this.m_tvEcho = new System.Windows.Forms.TreeView();
            this.m_txtInputData = new System.Windows.Forms.TextBox();
            this.m_treeHandleList = new System.Windows.Forms.TreeView();
            this.m_listBatchCommand = new System.Windows.Forms.ListView();
            this.m_comboBoxCurrency = new System.Windows.Forms.ComboBox();
            this.lbl_AktifHandle = new System.Windows.Forms.Label();
            this.cmb_Dep = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbl_Zaman = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Tarih_In = new DevExpress.XtraEditors.LabelControl();
            this.lblHata = new DevExpress.XtraEditors.LabelControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.barManager2 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.txtStatus = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatus.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1357, 667);
            this.label1.TabIndex = 2;
            this.label1.Text = "INGENICO V.001";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.m_lstBankErrorMessage);
            this.groupControl5.Controls.Add(this.m_listPayment);
            this.groupControl5.Controls.Add(this.m_listTransaction);
            this.groupControl5.Controls.Add(this.m_tvEcho);
            this.groupControl5.Controls.Add(this.m_txtInputData);
            this.groupControl5.Controls.Add(this.m_treeHandleList);
            this.groupControl5.Controls.Add(this.m_listBatchCommand);
            this.groupControl5.Controls.Add(this.m_comboBoxCurrency);
            this.groupControl5.Controls.Add(this.lbl_AktifHandle);
            this.groupControl5.Controls.Add(this.cmb_Dep);
            this.groupControl5.Controls.Add(this.textBox1);
            this.groupControl5.Controls.Add(this.lbl_Zaman);
            this.groupControl5.Controls.Add(this.lbl_Tarih_In);
            this.groupControl5.Controls.Add(this.lblHata);
            this.groupControl5.Controls.Add(this.listBox1);
            this.groupControl5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl5.Location = new System.Drawing.Point(0, 493);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(1357, 174);
            this.groupControl5.TabIndex = 11;
            this.groupControl5.Text = "Ingenico";
            this.groupControl5.Visible = false;
            // 
            // m_lstBankErrorMessage
            // 
            this.m_lstBankErrorMessage.FormattingEnabled = true;
            this.m_lstBankErrorMessage.Location = new System.Drawing.Point(956, 25);
            this.m_lstBankErrorMessage.Name = "m_lstBankErrorMessage";
            this.m_lstBankErrorMessage.Size = new System.Drawing.Size(132, 43);
            this.m_lstBankErrorMessage.TabIndex = 194;
            // 
            // m_listPayment
            // 
            this.m_listPayment.FormattingEnabled = true;
            this.m_listPayment.Location = new System.Drawing.Point(640, 25);
            this.m_listPayment.Name = "m_listPayment";
            this.m_listPayment.Size = new System.Drawing.Size(137, 43);
            this.m_listPayment.TabIndex = 193;
            // 
            // m_listTransaction
            // 
            this.m_listTransaction.FormattingEnabled = true;
            this.m_listTransaction.Location = new System.Drawing.Point(533, 25);
            this.m_listTransaction.Name = "m_listTransaction";
            this.m_listTransaction.Size = new System.Drawing.Size(101, 43);
            this.m_listTransaction.TabIndex = 192;
            // 
            // m_tvEcho
            // 
            this.m_tvEcho.Location = new System.Drawing.Point(458, 25);
            this.m_tvEcho.Name = "m_tvEcho";
            this.m_tvEcho.Size = new System.Drawing.Size(69, 37);
            this.m_tvEcho.TabIndex = 191;
            // 
            // m_txtInputData
            // 
            this.m_txtInputData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtInputData.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.m_txtInputData.Location = new System.Drawing.Point(783, 25);
            this.m_txtInputData.MaxLength = 12;
            this.m_txtInputData.Multiline = true;
            this.m_txtInputData.Name = "m_txtInputData";
            this.m_txtInputData.Size = new System.Drawing.Size(171, 25);
            this.m_txtInputData.TabIndex = 190;
            this.m_txtInputData.Text = "100";
            this.m_txtInputData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // m_treeHandleList
            // 
            this.m_treeHandleList.Location = new System.Drawing.Point(370, 25);
            this.m_treeHandleList.Name = "m_treeHandleList";
            this.m_treeHandleList.Size = new System.Drawing.Size(82, 37);
            this.m_treeHandleList.TabIndex = 189;
            // 
            // m_listBatchCommand
            // 
            this.m_listBatchCommand.HideSelection = false;
            this.m_listBatchCommand.Location = new System.Drawing.Point(286, 25);
            this.m_listBatchCommand.Name = "m_listBatchCommand";
            this.m_listBatchCommand.Size = new System.Drawing.Size(78, 37);
            this.m_listBatchCommand.TabIndex = 188;
            this.m_listBatchCommand.UseCompatibleStateImageBehavior = false;
            // 
            // m_comboBoxCurrency
            // 
            this.m_comboBoxCurrency.FormattingEnabled = true;
            this.m_comboBoxCurrency.Location = new System.Drawing.Point(111, 47);
            this.m_comboBoxCurrency.Name = "m_comboBoxCurrency";
            this.m_comboBoxCurrency.Size = new System.Drawing.Size(169, 21);
            this.m_comboBoxCurrency.TabIndex = 187;
            // 
            // lbl_AktifHandle
            // 
            this.lbl_AktifHandle.AutoSize = true;
            this.lbl_AktifHandle.Location = new System.Drawing.Point(67, 65);
            this.lbl_AktifHandle.Name = "lbl_AktifHandle";
            this.lbl_AktifHandle.Size = new System.Drawing.Size(40, 13);
            this.lbl_AktifHandle.TabIndex = 186;
            this.lbl_AktifHandle.Text = "Handle";
            // 
            // cmb_Dep
            // 
            this.cmb_Dep.FormattingEnabled = true;
            this.cmb_Dep.Location = new System.Drawing.Point(111, 25);
            this.cmb_Dep.Name = "cmb_Dep";
            this.cmb_Dep.Size = new System.Drawing.Size(169, 21);
            this.cmb_Dep.TabIndex = 185;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(5, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 13;
            // 
            // lbl_Zaman
            // 
            this.lbl_Zaman.Location = new System.Drawing.Point(5, 49);
            this.lbl_Zaman.Name = "lbl_Zaman";
            this.lbl_Zaman.Size = new System.Drawing.Size(63, 13);
            this.lbl_Zaman.TabIndex = 12;
            this.lbl_Zaman.Text = "labelControl4";
            // 
            // lbl_Tarih_In
            // 
            this.lbl_Tarih_In.Location = new System.Drawing.Point(5, 65);
            this.lbl_Tarih_In.Name = "lbl_Tarih_In";
            this.lbl_Tarih_In.Size = new System.Drawing.Size(56, 13);
            this.lbl_Tarih_In.TabIndex = 11;
            this.lbl_Tarih_In.Text = "lbl_Tarih_In";
            // 
            // lblHata
            // 
            this.lblHata.Location = new System.Drawing.Point(5, 84);
            this.lblHata.Name = "lblHata";
            this.lblHata.Size = new System.Drawing.Size(33, 13);
            this.lblHata.TabIndex = 10;
            this.lblHata.Text = "lblHata";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(2, 103);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1353, 69);
            this.listBox1.TabIndex = 9;
            // 
            // barManager2
            // 
            this.barManager2.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager2.DockControls.Add(this.barDockControl1);
            this.barManager2.DockControls.Add(this.barDockControl2);
            this.barManager2.DockControls.Add(this.barDockControl3);
            this.barManager2.DockControls.Add(this.barDockControl4);
            this.barManager2.Form = this;
            this.barManager2.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManager2.MaxItemId = 2;
            this.barManager2.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.bar3.BarAppearance.Normal.Options.UseFont = true;
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "FİŞ İPTAL";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "ÖKC İPTAL";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.ImageOptions.Image")));
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Manager = this.barManager2;
            this.barDockControl1.Size = new System.Drawing.Size(1357, 0);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 667);
            this.barDockControl2.Manager = this.barManager2;
            this.barDockControl2.Size = new System.Drawing.Size(1357, 42);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 0);
            this.barDockControl3.Manager = this.barManager2;
            this.barDockControl3.Size = new System.Drawing.Size(0, 667);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(1357, 0);
            this.barDockControl4.Manager = this.barManager2;
            this.barDockControl4.Size = new System.Drawing.Size(0, 667);
            // 
            // txtStatus
            // 
            this.txtStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtStatus.Location = new System.Drawing.Point(0, 0);
            this.txtStatus.MenuManager = this.barManager2;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(1357, 96);
            this.txtStatus.TabIndex = 16;
            this.txtStatus.Visible = false;
            // 
            // IngenicoForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 709);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.groupControl5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.barDockControl3);
            this.Controls.Add(this.barDockControl4);
            this.Controls.Add(this.barDockControl2);
            this.Controls.Add(this.barDockControl1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IngenicoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ingenico";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Ingenico_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IngenicoForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            this.groupControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatus.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        public System.Windows.Forms.TreeView m_tvEcho;
        public System.Windows.Forms.TextBox m_txtInputData;
        public System.Windows.Forms.TreeView m_treeHandleList;
        private System.Windows.Forms.ListView m_listBatchCommand;
        private System.Windows.Forms.ComboBox m_comboBoxCurrency;
        private System.Windows.Forms.Label lbl_AktifHandle;
        private System.Windows.Forms.ComboBox cmb_Dep;
        private System.Windows.Forms.TextBox textBox1;
        private DevExpress.XtraEditors.LabelControl lbl_Zaman;
        private DevExpress.XtraEditors.LabelControl lbl_Tarih_In;
        private DevExpress.XtraEditors.LabelControl lblHata;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox m_listTransaction;
        private System.Windows.Forms.ListBox m_listPayment;
        private System.Windows.Forms.ListBox m_lstBankErrorMessage;
        private DevExpress.XtraBars.BarManager barManager2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraEditors.MemoEdit txtStatus;
    }
}