namespace Pda
{
    partial class Urun
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.flp_AnaGrup = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Geri = new DevExpress.XtraEditors.SimpleButton();
            this.flp_AltGrup = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Geri);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(234, 23);
            this.panelControl1.TabIndex = 3;
            // 
            // flp_AnaGrup
            // 
            this.flp_AnaGrup.AutoScroll = true;
            this.flp_AnaGrup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flp_AnaGrup.Dock = System.Windows.Forms.DockStyle.Top;
            this.flp_AnaGrup.Location = new System.Drawing.Point(0, 23);
            this.flp_AnaGrup.Name = "flp_AnaGrup";
            this.flp_AnaGrup.Size = new System.Drawing.Size(234, 74);
            this.flp_AnaGrup.TabIndex = 4;
            // 
            // btn_Geri
            // 
            this.btn_Geri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Geri.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_Geri.Appearance.Options.UseFont = true;
            this.btn_Geri.Location = new System.Drawing.Point(0, 0);
            this.btn_Geri.Name = "btn_Geri";
            this.btn_Geri.Size = new System.Drawing.Size(62, 23);
            this.btn_Geri.TabIndex = 2;
            this.btn_Geri.Text = "Geri";
            // 
            // flp_AltGrup
            // 
            this.flp_AltGrup.AutoScroll = true;
            this.flp_AltGrup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flp_AltGrup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_AltGrup.Location = new System.Drawing.Point(0, 97);
            this.flp_AltGrup.Name = "flp_AltGrup";
            this.flp_AltGrup.Size = new System.Drawing.Size(234, 164);
            this.flp_AltGrup.TabIndex = 5;
            // 
            // Urun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 261);
            this.ControlBox = false;
            this.Controls.Add(this.flp_AltGrup);
            this.Controls.Add(this.flp_AnaGrup);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Urun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Urun";
            this.Load += new System.EventHandler(this.Urun_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Geri;
        private System.Windows.Forms.FlowLayoutPanel flp_AnaGrup;
        private System.Windows.Forms.FlowLayoutPanel flp_AltGrup;

    }
}