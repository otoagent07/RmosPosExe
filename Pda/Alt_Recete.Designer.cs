namespace Pda
{
    partial class Alt_Recete
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_Baslik = new DevExpress.XtraEditors.LabelControl();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.flp_Altrecete = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_Baslik);
            this.panel1.Controls.Add(this.btn_Cikis);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 24);
            this.panel1.TabIndex = 0;
            // 
            // lbl_Baslik
            // 
            this.lbl_Baslik.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lbl_Baslik.Location = new System.Drawing.Point(4, 6);
            this.lbl_Baslik.Name = "lbl_Baslik";
            this.lbl_Baslik.Size = new System.Drawing.Size(9, 13);
            this.lbl_Baslik.TabIndex = 54;
            this.lbl_Baslik.Text = "...";
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cikis.Appearance.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.Location = new System.Drawing.Point(155, 1);
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Size = new System.Drawing.Size(47, 23);
            this.btn_Cikis.TabIndex = 53;
            this.btn_Cikis.Text = "Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // flp_Altrecete
            // 
            this.flp_Altrecete.AutoScroll = true;
            this.flp_Altrecete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_Altrecete.Location = new System.Drawing.Point(0, 24);
            this.flp_Altrecete.Name = "flp_Altrecete";
            this.flp_Altrecete.Size = new System.Drawing.Size(202, 224);
            this.flp_Altrecete.TabIndex = 1;
            // 
            // Alt_Recete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 248);
            this.ControlBox = false;
            this.Controls.Add(this.flp_Altrecete);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Alt_Recete";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Alt_Recete_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flp_Altrecete;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.LabelControl lbl_Baslik;
    }
}