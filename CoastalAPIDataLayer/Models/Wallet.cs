using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Wallet
    {
        private readonly string dbConnectionString;
        public Wallet(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }
        public int ID { get; set; }
        public decimal Balance { get; set; }

        public Wallet Get(int id)
        {
            Wallet wallet = null;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Balance]
                                          FROM [CoastalFinancialDB_Mfundo].[dbo].[Wallet]
                                          WHERE ID = @ID ", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    wallet = new Wallet(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Balance = Convert.ToDecimal(reader["Balance"])
                    };
                }

                con.Close();
            }
            return wallet;
        }

        public bool Delete(int id)
        {
            int affectedRows = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"DELETE [dbo].[Wallet]
                                         WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }

        public bool Update(int id)
        {
            int affectedRows = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"DELETE [dbo].[Wallet]
                                         WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }
    }
}
