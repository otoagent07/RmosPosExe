using Pos.Class;
using Pos.Getir.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Pos.Getir.Extension
{
    public class GetirTools
    {
        public static bool InsertOrder(List<GetirOrderResponse.Root> order, string table)
        {
            bool result = false;
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            SqlConnection con = new SqlConnection(dbtools.connstr);
            con.Open();

            try
            {
                foreach (var data in order)
                {
                    values.Clear();
                    foreach (var item in data.GetType().GetProperties())
                    {
                        values.Add(new KeyValuePair<string, string>("GOrder_" + item.Name, item.GetValue(data).ToString()));
                    }

                    string xQry = getInsertCommand(table, values);
                    SqlCommand cmdi = new SqlCommand(xQry, con);
                    cmdi.ExecuteNonQuery();
                }
                result = true;
            }
            catch (Exception ex)
            { 
                throw ex; 
            }
            finally 
            { 
                con.Close(); 
            }
            return result;
        }


        private static string getInsertCommand(string table, List<KeyValuePair<string, string>> values)
        {
            string query = null;
            query += "INSERT INTO " + table + " ( ";
            foreach (var item in values)
            {
                query += item.Key;
                query += ", ";
            }
            query = query.Remove(query.Length - 2, 2);
            query += ") VALUES ( ";
            foreach (var item in values)
            {
            //    if (item.Key.GetType().Name == "System.Int") // or any other numerics
            //    {
            //        query += item.Value;
            //    }
            //    else
            //    {
                    query += "'";
                    query += item.Value;
                    query += "'";
                //}
                query += ", ";
            }
            query = query.Remove(query.Length - 2, 2);
            query += ")";
            return query;
        }
    }
}
