using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RmosIngenicoGMP
{
    public partial class FormProfilesSelection : XtraForm
    {
        RadioButton selectedRadioButton;

        public FormProfilesSelection()
        {
            InitializeComponent();
        }

        public void SetProfileNames(IList<string> profiles)
        {
            int counter = 0;
            foreach (var item in profiles)
            {
                RadioButton r = new RadioButton();
                r.Size = new Size(200, 30);
                r.Text = item;
                r.TabIndex = counter;
                counter++;
                r.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                this.flowLayoutPanelForProfiles.Controls.Add(r);
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            selectedRadioButton = (RadioButton)sender;
        }

        private void buttonProfileSelection_Click(object sender, EventArgs e)
        {
            ST_TICKET stTicket = new ST_TICKET();
            uint Retcode = Json_GMPSmartDLL.FP3_GetTicket(Main.CurrentInterface, Main.ActiveTransactionHandle, ref stTicket, Defines.TIMEOUT_DEFAULT);
            if (Retcode != 0)
            {
                ErrorClass.DisplayErrorMessage(Retcode);
                return;
            }
            Retcode = Json_GMPSmartDLL.FP3_SetCurrencyProfileIndex(Main.CurrentInterface,
                Main.ActiveTransactionHandle, (byte)selectedRadioButton.TabIndex, stTicket, Defines.TIMEOUT_DEFAULT);

            ErrorClass.DisplayErrorMessage(Retcode);
            this.Dispose();
        }
    }
}
