namespace Pda
{
    partial class Masa_Ozel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Masa_Ozel));
            this.txt_Ozelmasa = new DevExpress.XtraEditors.TextEdit();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Ozelmasa.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_Ozelmasa
            // 
            this.txt_Ozelmasa.Location = new System.Drawing.Point(6, 7);
            this.txt_Ozelmasa.Name = "txt_Ozelmasa";
            this.txt_Ozelmasa.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Ozelmasa.Size = new System.Drawing.Size(220, 22);
            this.txt_Ozelmasa.TabIndex = 0;
            // 
            // btn_OK
            // 
            this.btn_OK.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btn_OK.Appearance.Options.UseFont = true;
            this.btn_OK.Image = ((System.Drawing.Image)(resources.GetObject("btn_OK.Image")));
            this.btn_OK.Location = new System.Drawing.Point(151, 32);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 27);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // Masa_Ozel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 61);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_Ozelmasa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Masa_Ozel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ozel Masa";
            ((System.ComponentModel.ISupportInitialize)(this.txt_Ozelmasa.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_Ozelmasa;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
    }
}