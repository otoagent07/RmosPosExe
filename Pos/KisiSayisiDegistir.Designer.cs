namespace Pos
{
    partial class KisiSayisiDegistir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KisiSayisiDegistir));
            this.txt_KisiSayisi = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit145 = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_KisiSayisi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit145.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_KisiSayisi
            // 
            resources.ApplyResources(this.txt_KisiSayisi, "txt_KisiSayisi");
            this.txt_KisiSayisi.Name = "txt_KisiSayisi";
            this.txt_KisiSayisi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_KisiSayisi.Properties.Mask.EditMask = resources.GetString("txt_KisiSayisi.Properties.Mask.EditMask");
            this.txt_KisiSayisi.Properties.Mask.MaskType = ((DevExpress.XtraEditors.Mask.MaskType)(resources.GetObject("txt_KisiSayisi.Properties.Mask.MaskType")));
            // 
            // simpleButton2
            // 
            resources.ApplyResources(this.simpleButton2, "simpleButton2");
            this.simpleButton2.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton2.Appearance.Font")));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            resources.ApplyResources(this.simpleButton1, "simpleButton1");
            this.simpleButton1.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton1.Appearance.Font")));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // textEdit145
            // 
            resources.ApplyResources(this.textEdit145, "textEdit145");
            this.textEdit145.Name = "textEdit145";
            this.textEdit145.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit145.Properties.ReadOnly = true;
            this.textEdit145.TabStop = false;
            // 
            // KisiSayisiDegistir
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textEdit145);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txt_KisiSayisi);
            this.Name = "KisiSayisiDegistir";
            this.Load += new System.EventHandler(this.KisiSayisiDegistir_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_KisiSayisi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit145.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_KisiSayisi;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit textEdit145;
    }
}