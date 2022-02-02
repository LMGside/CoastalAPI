using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Settings
    {
        private readonly string dbConnectionString;

        public Settings(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int SalesToday { get; set; }
        public int UniqueSalesToday { get; set; }
        public decimal MaxWithdrawal { get; set; }
        public bool Auto_0 { get; set; }
        public bool Auto_1 { get; set; }
        public bool Auto_2 { get; set; }

        public Settings Get()
        {
            Settings settings = null;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT TOP (1) [Sales_Today]
                                              ,[Unique_Sales_Today]
                                              ,[Max_Withdrawal_Amount]
                                              ,[Auto_0]
                                              ,[Auto_1]
                                              ,[Auto_2]
                                          FROM [CoastalFinancialDB_Mfundo].[dbo].[Settings]", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    settings = new Settings(this.dbConnectionString)
                    {
                        SalesToday = Convert.ToInt32(reader["Sales_Today"] as int?),
                        UniqueSalesToday = Convert.ToInt32(reader["Unique_Sales_Today"] as int?),
                        MaxWithdrawal = Convert.ToDecimal(reader["Max_Withdrawal_Amount"]),
                        Auto_0 = Convert.ToBoolean(reader["Auto_0"] as bool?),
                        Auto_1 = Convert.ToBoolean(reader["Auto_1"] as bool?),
                        Auto_2 = Convert.ToBoolean(reader["Auto_2"] as bool?)
                    };
                }

                con.Close();
            }
            return settings;
        }
    }
}
