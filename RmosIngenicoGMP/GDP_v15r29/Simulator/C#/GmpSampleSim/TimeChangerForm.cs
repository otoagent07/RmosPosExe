using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace RmosIngenicoGMP
{
    public partial class TimeChangerForm : XtraForm
    {
        string date;
        string time;

        public TimeChangerForm()
        {
            InitializeComponent();
        }

        public void SetTime(string date, string time)
        {
            textBoxDateChanger.Text = date;
            textBoxTimeChanger.Text = time;
        }

        private void buttonChangeTime_Click(object sender, EventArgs e)
        {
            date = textBoxDateChanger.Text;
            time = textBoxTimeChanger.Text;
           
            this.DialogResult = DialogResult.OK;
            this.Visible = false;
        }

        public string getTime() { return time; }
        public string getDate() { return date; }

    }
}
