namespace Pda
{
    partial class Odeme_Tip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Odeme_Tip));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.flp_Kapatma = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(7, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(123, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Ödeme Tipini Seçiniz";
            // 
            // flp_Kapatma
            // 
            this.flp_Kapatma.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flp_Kapatma.Location = new System.Drawing.Point(3, 26);
            this.flp_Kapatma.Name = "flp_Kapatma";
            this.flp_Kapatma.Size = new System.Drawing.Size(245, 263);
            this.flp_Kapatma.TabIndex = 1;
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cikis.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.Image")));
            this.btn_Cikis.Location = new System.Drawing.Point(218, -2);
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Size = new System.Drawing.Size(33, 28);
            this.btn_Cikis.TabIndex = 4;
            this.btn_Cikis.TabStop = false;
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // Odeme_Tip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 292);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.flp_Kapatma);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Odeme_Tip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   ";
            this.Load += new System.EventHandler(this.Odeme_Tip_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.FlowLayoutPanel flp_Kapatma;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
    }
}