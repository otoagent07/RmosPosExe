
namespace Pos.Forms
{
    partial class TutarGirForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TutarGirForm));
            this.btnVazgec = new DevExpress.XtraEditors.SimpleButton();
            this.btnIadeYap = new DevExpress.XtraEditors.SimpleButton();
            this.txtTutar = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtTutar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnVazgec
            // 
            this.btnVazgec.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnVazgec.Appearance.Options.UseFont = true;
            this.btnVazgec.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnVazgec.ImageOptions.Image")));
            this.btnVazgec.Location = new System.Drawing.Point(251, 114);
            this.btnVazgec.Name = "btnVazgec";
            this.btnVazgec.Size = new System.Drawing.Size(183, 76);
            this.btnVazgec.TabIndex = 5;
            this.btnVazgec.Text = "Vazgeç";
            this.btnVazgec.Click += new System.EventHandler(this.btnVazgec_Click);
            // 
            // btnIadeYap
            // 
            this.btnIadeYap.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIadeYap.Appearance.Options.UseFont = true;
            this.btnIadeYap.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnIadeYap.ImageOptions.Image")));
            this.btnIadeYap.Location = new System.Drawing.Point(21, 114);
            this.btnIadeYap.Name = "btnIadeYap";
            this.btnIadeYap.Size = new System.Drawing.Size(210, 76);
            this.btnIadeYap.TabIndex = 4;
            this.btnIadeYap.Text = "İADE YAP";
            this.btnIadeYap.Click += new System.EventHandler(this.btnIadeYap_Click);
            // 
            // txtTutar
            // 
            this.txtTutar.EditValue = "";
            this.txtTutar.EnterMoveNextControl = true;
            this.txtTutar.Location = new System.Drawing.Point(140, 29);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTutar.Properties.Appearance.Options.UseFont = true;
            this.txtTutar.Properties.AutoHeight = false;
            this.txtTutar.Size = new System.Drawing.Size(278, 60);
            this.txtTutar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(18, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 36);
            this.label1.TabIndex = 6;
            this.label1.Text = "İade Tutarını \r\nGiriniz";
            // 
            // TutarGirForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 202);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnVazgec);
            this.Controls.Add(this.btnIadeYap);
            this.Controls.Add(this.txtTutar);
            this.Name = "TutarGirForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İade Tutarını Giriniz";
            this.Load += new System.EventHandler(this.TutarGirForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtTutar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnVazgec;
        private DevExpress.XtraEditors.SimpleButton btnIadeYap;
        private DevExpress.XtraEditors.TextEdit txtTutar;
        private System.Windows.Forms.Label label1;
    }
}