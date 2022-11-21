using DevExpress.XtraWaitForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Pos
{
    public partial class WaitFormRmos : WaitForm
    {
        

        public WaitFormRmos()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;

            string cap = "Lütfen Bekleyiniz...";
            string des = "Yükleniyor...";

            switch (Langs.Default.Dil)
            {
                case "en-US":
                    cap = "Please Wait...";
                    des = "Loading...";
                    break;
            }

           

            progressPanel1.Caption = cap;
            progressPanel1.Description = des;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}