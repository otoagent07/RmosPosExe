using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RmosIngenicoGMP
{
    public partial class GetInputForm : XtraForm
    {
        public GetInputForm()
        {
            InitializeComponent();
        }


        public GetInputForm(string str, string str2, int type)
        {
            InitializeComponent();

            label1.Text = str;
            textBox1.Text = str2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
