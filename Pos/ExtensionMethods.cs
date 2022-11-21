using Pos.Class;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Pos
{
    public static class ExtensionMethods
    {
        public static void SablonKaydet(this DevExpress.XtraGrid.Views.Grid.GridView dgv, string formName, int tip = 1)
        {

            MemoryStream GoruntuFile = new MemoryStream();
            dgv.SaveLayoutToStream(GoruntuFile);

            BinaryReader br = new BinaryReader(GoruntuFile);
            byte[] fileContent = new byte[GoruntuFile.Length];
            fileContent = GoruntuFile.ToArray();

            SablonSil(dgv, formName, tip);

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandText = (@"INSERT INTO User_Dizayn(Diz_User,Diz_Form,Diz_Rapor,Diz_XML, Diz_Tip, Diz_Sabit) 
                                 VALUES (@Diz_User,@Diz_Form,@Diz_Rapor,@Diz_XML,@Diz_Tip,@Diz_Sabit)");

            com.Parameters.AddWithValue("@Diz_User", User.P_Kod);
            com.Parameters.AddWithValue("@Diz_Form", formName);
            com.Parameters.AddWithValue("@Diz_Rapor", dgv.Name);
            com.Parameters.AddWithValue("@Diz_XML", fileContent);
            com.Parameters.AddWithValue("@Diz_Tip", tip);
            com.Parameters.AddWithValue("@Diz_Sabit", true);
            com.ExecuteNonQuery();
            con.Close();
            br.Close();
            GoruntuFile.Close();

            System.Windows.Forms.MessageBox.Show("Şablon Eklenmiştir.");
        }

        public static void SablonSil(this DevExpress.XtraGrid.Views.Grid.GridView dgv, string formName, int tip = 1)
        {
            dbtools.execcmd(@"Delete From User_Dizayn 
                                    Where Diz_User = '" + User.P_Kod + @"' 
                                        AND Diz_Form = '" + formName + @"' 
                                        AND Diz_Tip = '" + tip + @"' 
                                        AND Diz_Rapor = '" + dgv.Name + "'");

            //System.Windows.Forms.MessageBox.Show("Şablon Silinmiştir.");
        }

        public static void SablonYukle(this DevExpress.XtraGrid.Views.Grid.GridView dgv, string formName, int tip = 1)
        {
            DataTable dt = dbtools.SelectTable(@"Select * 
                                                 From User_Dizayn 
                                                 Where Diz_User = '" + User.P_Kod + @"' 
                                                    AND Diz_Form = '" + formName + @"' 
                                                    AND Diz_Tip = '" + tip + @"' 
                                                    AND Diz_Rapor = '" + dgv.Name + "'");

            if (dt.Rows.Count == 0)
                dt = dbtools.SelectTable(@"Select * 
                                            From User_Dizayn 
                                            Where Diz_User = '" + User.P_Kod + @"' 
                                                AND Diz_Form = '" + formName + @"' 
                                                AND Diz_Tip = '" + tip + @"' 
                                                AND Diz_Rapor = 'Sabit'");

            if (dt.Rows.Count > 0)
                dgv.RestoreLayoutFromStream(new MemoryStream((byte[])dt.Rows[0]["Diz_XML"]));

        }



    }
}
