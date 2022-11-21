namespace Pos
{
    partial class Pos_XtraFolio_OdemeTipi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pos_XtraFolio_OdemeTipi));
            this.textEdit8 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.look_OdemeDep = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_OdemeDep.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEdit8
            // 
            resources.ApplyResources(this.textEdit8, "textEdit8");
            this.textEdit8.Name = "textEdit8";
            this.textEdit8.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("textEdit8.Properties.Appearance.Font")));
            this.textEdit8.Properties.Appearance.ForeColor = System.Drawing.Color.MidnightBlue;
            this.textEdit8.Properties.Appearance.Options.UseFont = true;
            this.textEdit8.Properties.Appearance.Options.UseForeColor = true;
            this.textEdit8.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit8.Properties.ReadOnly = true;
            this.textEdit8.TabStop = false;
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
            // look_OdemeDep
            // 
            resources.ApplyResources(this.look_OdemeDep, "look_OdemeDep");
            this.look_OdemeDep.Name = "look_OdemeDep";
            this.look_OdemeDep.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("look_OdemeDep.Properties.Appearance.Font")));
            this.look_OdemeDep.Properties.Appearance.Options.UseFont = true;
            this.look_OdemeDep.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.look_OdemeDep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("look_OdemeDep.Properties.Buttons"))))});
            this.look_OdemeDep.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("look_OdemeDep.Properties.Columns"), resources.GetString("look_OdemeDep.Properties.Columns1")),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("look_OdemeDep.Properties.Columns2"), resources.GetString("look_OdemeDep.Properties.Columns3"))});
            this.look_OdemeDep.Properties.MaxLength = 100;
            this.look_OdemeDep.Properties.NullText = resources.GetString("look_OdemeDep.Properties.NullText");
            // 
            // Pos_XtraFolio_OdemeTipi
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.look_OdemeDep);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.textEdit8);
            this.Name = "Pos_XtraFolio_OdemeTipi";
            this.Load += new System.EventHandler(this.Pos_XtraFolio_OdemeTipi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_OdemeDep.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit8;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LookUpEdit look_OdemeDep;
    }
}