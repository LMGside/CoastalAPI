using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CoastalAPIDataLayer.Models
{
    public class ErrorLog
    {
        private readonly string dbConnectionString;
        public ErrorLog(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CrashedMethod { get; set; }


        public void Insert()
        {
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"INSERT INTO [dbo].[Error_Log]
                                                           ([Created_Date]
                                                           ,[Error_Message]
                                                           ,[Exception_Message]
                                                           ,[Crashed_Method]
                                                           ,[StackTrace])
                                                     VALUES
                                                           (@CreatedDate
                                                           ,@ErrorMessage
                                                           ,@ExceptionMessage
                                                           ,@CrashedMethod
                                                           ,@StackTrace)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                cmd.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
                cmd.Parameters.AddWithValue("@ExceptionMessage", ExceptionMessage);
                cmd.Parameters.AddWithValue("@CrashedMethod", CrashedMethod);
                cmd.Parameters.AddWithValue("@StackTrace", StackTrace);

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
