namespace PosPrinterServer
{
    partial class Server
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
            this.textEdit125 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Host = new DevExpress.XtraEditors.TextEdit();
            this.txt_Port = new DevExpress.XtraEditors.TextEdit();
            this.chkBtn_Start = new DevExpress.XtraEditors.CheckButton();
            this.chkBtn_Stop = new DevExpress.XtraEditors.CheckButton();
            this.lbl_Info = new DevExpress.XtraEditors.LabelControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkButton1 = new DevExpress.XtraEditors.CheckButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit125.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Host.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Port.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEdit125
            // 
            this.textEdit125.EditValue = "Host";
            this.textEdit125.Enabled = false;
            this.textEdit125.Location = new System.Drawing.Point(12, 12);
            this.textEdit125.Name = "textEdit125";
            this.textEdit125.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textEdit125.Properties.Appearance.Options.UseFont = true;
            this.textEdit125.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit125.Properties.ReadOnly = true;
            this.textEdit125.Size = new System.Drawing.Size(80, 22);
            this.textEdit125.TabIndex = 151;
            this.textEdit125.TabStop = false;
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "Port";
            this.textEdit1.Enabled = false;
            this.textEdit1.Location = new System.Drawing.Point(12, 35);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(80, 22);
            this.textEdit1.TabIndex = 152;
            this.textEdit1.TabStop = false;
            // 
            // txt_Host
            // 
            this.txt_Host.EnterMoveNextControl = true;
            this.txt_Host.Location = new System.Drawing.Point(93, 12);
            this.txt_Host.Name = "txt_Host";
            this.txt_Host.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_Host.Properties.Appearance.Options.UseFont = true;
            this.txt_Host.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Host.Size = new System.Drawing.Size(140, 22);
            this.txt_Host.TabIndex = 153;
            // 
            // txt_Port
            // 
            this.txt_Port.EditValue = "8910";
            this.txt_Port.EnterMoveNextControl = true;
            this.txt_Port.Location = new System.Drawing.Point(93, 35);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_Port.Properties.Appearance.Options.UseFont = true;
            this.txt_Port.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Port.Size = new System.Drawing.Size(51, 22);
            this.txt_Port.TabIndex = 154;
            // 
            // chkBtn_Start
            // 
            this.chkBtn_Start.Location = new System.Drawing.Point(239, 11);
            this.chkBtn_Start.Name = "chkBtn_Start";
            this.chkBtn_Start.Size = new System.Drawing.Size(89, 46);
            this.chkBtn_Start.TabIndex = 155;
            this.chkBtn_Start.Text = "Start";
            this.chkBtn_Start.CheckedChanged += new System.EventHandler(this.chkBtn_Start_CheckedChanged);
            // 
            // chkBtn_Stop
            // 
            this.chkBtn_Stop.Location = new System.Drawing.Point(334, 11);
            this.chkBtn_Stop.Name = "chkBtn_Stop";
            this.chkBtn_Stop.Size = new System.Drawing.Size(89, 46);
            this.chkBtn_Stop.TabIndex = 156;
            this.chkBtn_Stop.Text = "Stop";
            this.chkBtn_Stop.CheckedChanged += new System.EventHandler(this.chkBtn_Stop_CheckedChanged);
            // 
            // lbl_Info
            // 
            this.lbl_Info.Location = new System.Drawing.Point(12, 73);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(63, 13);
            this.lbl_Info.TabIndex = 157;
            this.lbl_Info.Text = "labelControl1";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(12, 92);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(411, 95);
            this.listBox1.TabIndex = 159;
            // 
            // checkButton1
            // 
            this.checkButton1.Location = new System.Drawing.Point(239, 63);
            this.checkButton1.Name = "checkButton1";
            this.checkButton1.Size = new System.Drawing.Size(184, 23);
            this.checkButton1.TabIndex = 160;
            this.checkButton1.Text = "Tanimlama";
            this.checkButton1.CheckedChanged += new System.EventHandler(this.checkButton1_CheckedChanged);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 199);
            this.Controls.Add(this.checkButton1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.chkBtn_Stop);
            this.Controls.Add(this.chkBtn_Start);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.txt_Host);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.textEdit125);
            this.Name = "Server";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Server_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit125.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Host.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Port.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit125;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        public DevExpress.XtraEditors.TextEdit txt_Host;
        public DevExpress.XtraEditors.TextEdit txt_Port;
        private DevExpress.XtraEditors.CheckButton chkBtn_Start;
        private DevExpress.XtraEditors.CheckButton chkBtn_Stop;
        private DevExpress.XtraEditors.LabelControl lbl_Info;
        private System.Windows.Forms.ListBox listBox1;
        private DevExpress.XtraEditors.CheckButton checkButton1;
    }
}