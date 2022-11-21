namespace Pda
{
    partial class Kart_Oku
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
            this.txt_KartNo = new DevExpress.XtraEditors.TextEdit();
            this.btn_Ara = new DevExpress.XtraEditors.SimpleButton();
            this.lblHata = new DevExpress.XtraEditors.LabelControl();
            this.lblKartNo = new DevExpress.XtraEditors.LabelControl();
            this.lblOdaNo = new DevExpress.XtraEditors.LabelControl();
            this.lblAdSoyad = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl92 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txt_KartNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_KartNo
            // 
            this.txt_KartNo.Location = new System.Drawing.Point(2, 23);
            this.txt_KartNo.Name = "txt_KartNo";
            this.txt_KartNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_KartNo.Size = new System.Drawing.Size(158, 22);
            this.txt_KartNo.TabIndex = 0;
            // 
            // btn_Ara
            // 
            this.btn_Ara.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Ara.Appearance.Options.UseFont = true;
            this.btn_Ara.Location = new System.Drawing.Point(162, 22);
            this.btn_Ara.Name = "btn_Ara";
            this.btn_Ara.Size = new System.Drawing.Size(62, 23);
            this.btn_Ara.TabIndex = 1;
            this.btn_Ara.Text = "Ara";
            this.btn_Ara.Click += new System.EventHandler(this.btn_Ara_Click);
            // 
            // lblHata
            // 
            this.lblHata.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblHata.Location = new System.Drawing.Point(7, 4);
            this.lblHata.Name = "lblHata";
            this.lblHata.Size = new System.Drawing.Size(0, 13);
            this.lblHata.TabIndex = 2;
            // 
            // lblKartNo
            // 
            this.lblKartNo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblKartNo.Location = new System.Drawing.Point(79, 90);
            this.lblKartNo.Name = "lblKartNo";
            this.lblKartNo.Size = new System.Drawing.Size(5, 13);
            this.lblKartNo.TabIndex = 39;
            this.lblKartNo.Text = "-";
            // 
            // lblOdaNo
            // 
            this.lblOdaNo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblOdaNo.Location = new System.Drawing.Point(79, 71);
            this.lblOdaNo.Name = "lblOdaNo";
            this.lblOdaNo.Size = new System.Drawing.Size(5, 13);
            this.lblOdaNo.TabIndex = 38;
            this.lblOdaNo.Text = "-";
            // 
            // lblAdSoyad
            // 
            this.lblAdSoyad.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAdSoyad.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblAdSoyad.Location = new System.Drawing.Point(79, 52);
            this.lblAdSoyad.Name = "lblAdSoyad";
            this.lblAdSoyad.Size = new System.Drawing.Size(5, 13);
            this.lblAdSoyad.TabIndex = 37;
            this.lblAdSoyad.Text = "-";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(2, 90);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(50, 13);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Text = "Kart No : ";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(2, 71);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 13);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "Oda No : ";
            // 
            // labelControl92
            // 
            this.labelControl92.Appearance.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold);
            this.labelControl92.Location = new System.Drawing.Point(2, 51);
            this.labelControl92.Name = "labelControl92";
            this.labelControl92.Size = new System.Drawing.Size(71, 14);
            this.labelControl92.TabIndex = 34;
            this.labelControl92.Text = "Ad Soyad : ";
            // 
            // Kart_Oku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 113);
            this.Controls.Add(this.lblKartNo);
            this.Controls.Add(this.lblOdaNo);
            this.Controls.Add(this.lblAdSoyad);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl92);
            this.Controls.Add(this.lblHata);
            this.Controls.Add(this.btn_Ara);
            this.Controls.Add(this.txt_KartNo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Kart_Oku";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kart No";
            this.Load += new System.EventHandler(this.Kart_Oku_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_KartNo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_KartNo;
        private DevExpress.XtraEditors.SimpleButton btn_Ara;
        private DevExpress.XtraEditors.LabelControl lblHata;
        private DevExpress.XtraEditors.LabelControl lblKartNo;
        private DevExpress.XtraEditors.LabelControl lblOdaNo;
        private DevExpress.XtraEditors.LabelControl lblAdSoyad;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl92;
    }
}