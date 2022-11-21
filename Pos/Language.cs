using Pos.Class;
using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Pos
{
    public partial class Language : DevExpress.XtraEditors.XtraForm
    {
        public Language()
        {
            InitializeComponent();
        }

        private void Language_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Kod", typeof(string));
            dt.Columns.Add("Ad", typeof(string));

            dt.Rows.Add("tr-TR", "TÜRKÇE");
            dt.Rows.Add("en-US", "ENGLISH");
            dt.Rows.Add("ru", "RUSSIAN");


            look_Lang.Properties.DataSource = dt;
            look_Lang.Properties.DisplayMember = "Ad";
            look_Lang.Properties.ValueMember = "Kod";

            look_Lang.EditValue = Langs.Default.Dil == "" ? "tr-TR" : Langs.Default.Dil;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Langs.Default.Dil = look_Lang.EditValue.ToString();
                Langs.Default.Save();


                CultureInfo culture = new CultureInfo(Langs.Default.Dil == "" ? "tr-TR" : Langs.Default.Dil); // en-US
                //culture = new CultureInfo(User.Pos_Culture);
                //culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
                culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
                //culture.DateTimeFormat.DateSeparator = ".";
                culture.DateTimeFormat.ShortTimePattern = "HH:mm";
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                Localize.ApplicationLanguage(culture.TwoLetterISOLanguageName);


                Program.main.dilYenile();
                Program.main.login.dilYenile();
            }
            catch (Exception ex)
            {

            }

            this.Close();

        }
    }
}