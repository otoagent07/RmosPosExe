namespace Pos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alt_Recete));
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.flp_Altrecete = new System.Windows.Forms.FlowLayoutPanel();
            this.lbl_Baslik = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // btn_Cikis
            // 
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Cikis.ImageOptions.SvgImage")));
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // flp_Altrecete
            // 
            resources.ApplyResources(this.flp_Altrecete, "flp_Altrecete");
            this.flp_Altrecete.Name = "flp_Altrecete";
            // 
            // lbl_Baslik
            // 
            resources.ApplyResources(this.lbl_Baslik, "lbl_Baslik");
            this.lbl_Baslik.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("lbl_Baslik.Appearance.Font")));
            this.lbl_Baslik.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Baslik.Appearance.Options.UseFont = true;
            this.lbl_Baslik.Appearance.Options.UseForeColor = true;
            this.lbl_Baslik.Name = "lbl_Baslik";
            // 
            // Alt_Recete
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.lbl_Baslik);
            this.Controls.Add(this.flp_Altrecete);
            this.Controls.Add(this.btn_Cikis);
            this.Name = "Alt_Recete";
            this.Load += new System.EventHandler(this.Alt_Recete_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private System.Windows.Forms.FlowLayoutPanel flp_Altrecete;
        private DevExpress.XtraEditors.LabelControl lbl_Baslik;

    }
}