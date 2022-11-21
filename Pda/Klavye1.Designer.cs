namespace Pda
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
            this.simpleButton11 = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Baslik = new DevExpress.XtraEditors.LabelControl();
            this.btn_Ok = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton0 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Sil = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Sayi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.Image")));
            this.btn_Cikis.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_Cikis.Location = new System.Drawing.Point(171, 211);
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Size = new System.Drawing.Size(40, 40);
            this.btn_Cikis.TabIndex = 50;
            this.btn_Cikis.TabStop = false;
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // txt_Sayi
            // 
            this.txt_Sayi.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.txt_Sayi.Location = new System.Drawing.Point(33, 41);
            this.txt_Sayi.Name = "txt_Sayi";
            this.txt_Sayi.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_Sayi.Properties.Appearance.Options.UseFont = true;
            this.txt_Sayi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Sayi.Properties.DisplayFormat.FormatString = "f2";
            this.txt_Sayi.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_Sayi.Properties.EditFormat.FormatString = "f2";
            this.txt_Sayi.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_Sayi.Properties.Mask.EditMask = "n2";
            this.txt_Sayi.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_Sayi.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_Sayi.Size = new System.Drawing.Size(177, 28);
            this.txt_Sayi.TabIndex = 0;
            // 
            // simpleButton11
            // 
            this.simpleButton11.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton11.Appearance.Options.UseFont = true;
            this.simpleButton11.Location = new System.Drawing.Point(125, 211);
            this.simpleButton11.Name = "simpleButton11";
            this.simpleButton11.Size = new System.Drawing.Size(40, 40);
            this.simpleButton11.TabIndex = 48;
            this.simpleButton11.TabStop = false;
            this.simpleButton11.Text = ",";
            this.simpleButton11.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // lbl_Baslik
            // 
            this.lbl_Baslik.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Baslik.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Baslik.Location = new System.Drawing.Point(33, 19);
            this.lbl_Baslik.Name = "lbl_Baslik";
            this.lbl_Baslik.Size = new System.Drawing.Size(95, 16);
            this.lbl_Baslik.TabIndex = 46;
            this.lbl_Baslik.Text = "Tutar Giriniz...";
            // 
            // btn_Ok
            // 
            this.btn_Ok.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Ok.Appearance.Options.UseFont = true;
            this.btn_Ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_Ok.Image")));
            this.btn_Ok.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_Ok.Location = new System.Drawing.Point(171, 73);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(40, 86);
            this.btn_Ok.TabIndex = 45;
            this.btn_Ok.TabStop = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // simpleButton0
            // 
            this.simpleButton0.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton0.Appearance.Options.UseFont = true;
            this.simpleButton0.Location = new System.Drawing.Point(33, 211);
            this.simpleButton0.Name = "simpleButton0";
            this.simpleButton0.Size = new System.Drawing.Size(40, 40);
            this.simpleButton0.TabIndex = 44;
            this.simpleButton0.TabStop = false;
            this.simpleButton0.Text = "0";
            this.simpleButton0.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.Location = new System.Drawing.Point(125, 165);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(40, 40);
            this.simpleButton3.TabIndex = 43;
            this.simpleButton3.TabStop = false;
            this.simpleButton3.Text = "3";
            this.simpleButton3.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(79, 165);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(40, 40);
            this.simpleButton2.TabIndex = 42;
            this.simpleButton2.TabStop = false;
            this.simpleButton2.Text = "2";
            this.simpleButton2.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(33, 165);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(40, 40);
            this.simpleButton1.TabIndex = 41;
            this.simpleButton1.TabStop = false;
            this.simpleButton1.Text = "1";
            this.simpleButton1.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton6.Appearance.Options.UseFont = true;
            this.simpleButton6.Location = new System.Drawing.Point(125, 119);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(40, 40);
            this.simpleButton6.TabIndex = 40;
            this.simpleButton6.TabStop = false;
            this.simpleButton6.Text = "6";
            this.simpleButton6.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton5.Appearance.Options.UseFont = true;
            this.simpleButton5.Location = new System.Drawing.Point(79, 119);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(40, 40);
            this.simpleButton5.TabIndex = 39;
            this.simpleButton5.TabStop = false;
            this.simpleButton5.Text = "5";
            this.simpleButton5.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton4.Appearance.Options.UseFont = true;
            this.simpleButton4.Location = new System.Drawing.Point(33, 119);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(40, 40);
            this.simpleButton4.TabIndex = 38;
            this.simpleButton4.TabStop = false;
            this.simpleButton4.Text = "4";
            this.simpleButton4.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton9
            // 
            this.simpleButton9.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton9.Appearance.Options.UseFont = true;
            this.simpleButton9.Location = new System.Drawing.Point(125, 73);
            this.simpleButton9.Name = "simpleButton9";
            this.simpleButton9.Size = new System.Drawing.Size(40, 40);
            this.simpleButton9.TabIndex = 37;
            this.simpleButton9.TabStop = false;
            this.simpleButton9.Text = "9";
            this.simpleButton9.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton8.Appearance.Options.UseFont = true;
            this.simpleButton8.Location = new System.Drawing.Point(79, 73);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(40, 40);
            this.simpleButton8.TabIndex = 36;
            this.simpleButton8.TabStop = false;
            this.simpleButton8.Text = "8";
            this.simpleButton8.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton7.Appearance.Options.UseFont = true;
            this.simpleButton7.Location = new System.Drawing.Point(33, 73);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(40, 40);
            this.simpleButton7.TabIndex = 35;
            this.simpleButton7.TabStop = false;
            this.simpleButton7.Text = "7";
            this.simpleButton7.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.simpleButton10.Appearance.Options.UseFont = true;
            this.simpleButton10.Location = new System.Drawing.Point(79, 211);
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Size = new System.Drawing.Size(40, 40);
            this.simpleButton10.TabIndex = 47;
            this.simpleButton10.TabStop = false;
            this.simpleButton10.Text = "00";
            this.simpleButton10.Click += new System.EventHandler(this.btn_Sayi_Click);
            // 
            // btn_Sil
            // 
            this.btn_Sil.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Sil.Appearance.Options.UseFont = true;
            this.btn_Sil.Image = ((System.Drawing.Image)(resources.GetObject("btn_Sil.Image")));
            this.btn_Sil.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_Sil.Location = new System.Drawing.Point(171, 165);
            this.btn_Sil.Name = "btn_Sil";
            this.btn_Sil.Size = new System.Drawing.Size(40, 40);
            this.btn_Sil.TabIndex = 51;
            this.btn_Sil.TabStop = false;
            this.btn_Sil.Click += new System.EventHandler(this.btn_Sil_Click);
            // 
            // Klavye1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 271);
            this.Controls.Add(this.btn_Sil);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.txt_Sayi);
            this.Controls.Add(this.simpleButton11);
            this.Controls.Add(this.simpleButton10);
            this.Controls.Add(this.lbl_Baslik);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.simpleButton0);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton9);
            this.Controls.Add(this.simpleButton8);
            this.Controls.Add(this.simpleButton7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Klavye1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Klavye";
            this.Load += new System.EventHandler(this.Klavye1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Sayi.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        public DevExpress.XtraEditors.TextEdit txt_Sayi;
        private DevExpress.XtraEditors.SimpleButton simpleButton11;
        private DevExpress.XtraEditors.LabelControl lbl_Baslik;
        private DevExpress.XtraEditors.SimpleButton btn_Ok;
        private DevExpress.XtraEditors.SimpleButton simpleButton0;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton9;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
        private DevExpress.XtraEditors.SimpleButton btn_Sil;
    }
}