using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace Pos.Class
{
    public class ErrorLog
    {
        public string Program { get; set; }

        public DateTime? ErrorDate { get; set; }

        public string ErrorShortDescription { get; set; }

        public string ExceptionType { get; set; }

        public string FileName { get; set; }

        public int? LineNumber { get; set; }

        public string MethodName { get; set; }

        public string ClassName { get; set; }

        public string ApplicationName { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public string InnerException { get; set; }

        public string InnerExceptionMessage { get; set; }

        public bool? IsProduction { get; set; }


        public static void Save(Exception ex, string errorDescription,Pos.Class.Log.Log_Program program)
        {

            ErrorLog log = new ErrorLog();

            log.Program = program.ToString();

            if (errorDescription != null && errorDescription != "")
            {
                log.ErrorShortDescription = errorDescription;
            }
            log.ExceptionType = ex.GetType().FullName;
            var stackTrace = new StackTrace(ex, true);
            var allFrames = stackTrace.GetFrames();
            //foreach (var frame in allFrames)
            //{
            //    log.FileName = frame.GetFileName();
            //    log.LineNumber = frame.GetFileLineNumber();
            //    log.MethodName = frame.GetMethod().Name;
            //    log.ClassName = frame.GetMethod().DeclaringType.ToString();
            //}
            if (allFrames.Length > 0)
            {
                var frame = allFrames[allFrames.Length - 1];
                log.FileName = frame.GetFileName();
                log.LineNumber = frame.GetFileLineNumber();
                log.MethodName = frame.GetMethod().Name;
                log.ClassName = frame.GetMethod().DeclaringType.ToString();
            }

            try
            {
                log.ApplicationName = Assembly.GetCallingAssembly().GetName().Name;
            }
            catch
            {
                log.ApplicationName = "";
            }

            log.ErrorMessage = ex.Message;
            log.StackTrace = ex.StackTrace;
            if (ex.InnerException != null)
            {
                log.InnerException = ex.InnerException.ToString();
                log.InnerExceptionMessage = ex.InnerException.Message;
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                log.IsProduction = false;
            }

            Kaydet(log);
        }

        private static void Kaydet(ErrorLog log)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Error_Log_Kaydet";
            com.Parameters.AddWithValue("@Program", log.Program);
            com.Parameters.AddWithValue("@ErrorDate", log.ErrorDate);
            com.Parameters.AddWithValue("@ErrorShortDescription", log.ErrorShortDescription);
            com.Parameters.AddWithValue("@ExceptionType", log.ExceptionType);
            com.Parameters.AddWithValue("@FileName", log.FileName);
            com.Parameters.AddWithValue("@LineNumber", log.LineNumber);
            com.Parameters.AddWithValue("@MethodName", log.MethodName);
            com.Parameters.AddWithValue("@ClassName", log.ClassName);
            com.Parameters.AddWithValue("@ApplicationName", log.ApplicationName);
            com.Parameters.AddWithValue("@ErrorMessage", log.ErrorMessage);
            com.Parameters.AddWithValue("@StackTrace", log.StackTrace);
            com.Parameters.AddWithValue("@InnerException", log.InnerException);
            com.Parameters.AddWithValue("@InnerExceptionMessage", log.InnerExceptionMessage);
            com.Parameters.AddWithValue("@IsProduction", log.IsProduction);
            com.ExecuteNonQuery();
        }

    }
}
