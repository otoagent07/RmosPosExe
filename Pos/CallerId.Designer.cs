namespace Pos
{
    partial class CallerId
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallerId));
            this.listBoxControl1 = new DevExpress.XtraEditors.ListBoxControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Bilgi = new DevExpress.XtraEditors.LabelControl();
            this.axCIDv51 = new Axcidv5callerid.AxCIDv5();
            this.btn_Kapat = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Dep = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axCIDv51)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxControl1.Location = new System.Drawing.Point(12, 63);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(496, 214);
            this.listBoxControl1.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.simpleButton1.Location = new System.Drawing.Point(13, 283);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(109, 39);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Gizle";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // lbl_Bilgi
            // 
            this.lbl_Bilgi.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lbl_Bilgi.Appearance.Options.UseFont = true;
            this.lbl_Bilgi.Location = new System.Drawing.Point(13, 13);
            this.lbl_Bilgi.Name = "lbl_Bilgi";
            this.lbl_Bilgi.Size = new System.Drawing.Size(0, 16);
            this.lbl_Bilgi.TabIndex = 2;
            // 
            // axCIDv51
            // 
            this.axCIDv51.Location = new System.Drawing.Point(430, 294);
            this.axCIDv51.Name = "axCIDv51";
            this.axCIDv51.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCIDv51.OcxState")));
            this.axCIDv51.Size = new System.Drawing.Size(22, 22);
            this.axCIDv51.TabIndex = 3;
            this.axCIDv51.OnCallerID += new Axcidv5callerid.ICIDv5Events_OnCallerIDEventHandler(this.axCIDv51_OnCallerID);
            // 
            // btn_Kapat
            // 
            this.btn_Kapat.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Kapat.Appearance.Options.UseFont = true;
            this.btn_Kapat.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Kapat.ImageOptions.SvgImage")));
            this.btn_Kapat.Location = new System.Drawing.Point(128, 283);
            this.btn_Kapat.Name = "btn_Kapat";
            this.btn_Kapat.Size = new System.Drawing.Size(109, 39);
            this.btn_Kapat.TabIndex = 4;
            this.btn_Kapat.Text = "Çıkış";
            this.btn_Kapat.Click += new System.EventHandler(this.btn_Kapat_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Rmos Caller Id";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(271, 284);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "simpleButton2";
            this.simpleButton2.Visible = false;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // lbl_Dep
            // 
            this.lbl_Dep.Appearance.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.lbl_Dep.Appearance.Options.UseFont = true;
            this.lbl_Dep.Location = new System.Drawing.Point(13, 13);
            this.lbl_Dep.Name = "lbl_Dep";
            this.lbl_Dep.Size = new System.Drawing.Size(182, 33);
            this.lbl_Dep.TabIndex = 6;
            this.lbl_Dep.Text = "labelControl1";
            // 
            // CallerId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 334);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_Dep);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.btn_Kapat);
            this.Controls.Add(this.axCIDv51);
            this.Controls.Add(this.lbl_Bilgi);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.listBoxControl1);
            this.Name = "CallerId";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CallerId";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CallerId_FormClosing);
            this.Load += new System.EventHandler(this.CallerId_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axCIDv51)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl listBoxControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl lbl_Bilgi;
        private Axcidv5callerid.AxCIDv5 axCIDv51;
        private DevExpress.XtraEditors.SimpleButton btn_Kapat;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl lbl_Dep;
    }
}