using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class CommissionLog
    {
        private readonly string dbConnectionString;
        public CommissionLog(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Commission { get; set; }

        public void Insert()
        {
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"INSERT INTO [dbo].[Commission_Log]
                                                   ([Transaction_ID]
                                                   ,[Transaction_Date]
                                                   ,[Commission])
                                             VALUES
                                                   (@TransID
                                                   ,@TransDate
                                                   ,@Commission)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TransID", TransactionID);
                cmd.Parameters.AddWithValue("@TransDate", TransactionDate);
                cmd.Parameters.AddWithValue("@Commission", Commission);

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
