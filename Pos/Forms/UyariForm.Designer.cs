
namespace Pos.Forms
{
    partial class UyariForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnYes = new DevExpress.XtraEditors.SimpleButton();
            this.txtUyariMesaj = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::Pos.Properties.Resources.rmosultimate;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(547, 185);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnYes
            // 
            this.btnYes.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYes.Appearance.Options.UseFont = true;
            this.btnYes.Location = new System.Drawing.Point(12, 297);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(523, 54);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "TAMAM";
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // txtUyariMesaj
            // 
            this.txtUyariMesaj.AutoSize = true;
            this.txtUyariMesaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtUyariMesaj.Location = new System.Drawing.Point(6, 198);
            this.txtUyariMesaj.Name = "txtUyariMesaj";
            this.txtUyariMesaj.Size = new System.Drawing.Size(66, 24);
            this.txtUyariMesaj.TabIndex = 2;
            this.txtUyariMesaj.Text = "label1";
            // 
            // UyariForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(547, 363);
            this.Controls.Add(this.txtUyariMesaj);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UyariForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UyariForm";
            this.Load += new System.EventHandler(this.UyariForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.SimpleButton btnYes;
        private System.Windows.Forms.Label txtUyariMesaj;
    }
}