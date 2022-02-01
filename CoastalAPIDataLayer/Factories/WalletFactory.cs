using CoastalAPIDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Factories
{
    public class WalletFactory
    {
        private readonly string dbConnectionString;
        public WalletFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Wallet Create(Action<Wallet> initalizer)
        {
            var wallet = new Wallet(this.dbConnectionString);
            initalizer(wallet);
            return wallet;
        }

        public bool AddDeposit(int id, decimal amount)
        {
            int affected = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"UPDATE [dbo].[Wallet]
                                               SET [Balance] = Balance + @Amount
                                             WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Amount", amount);
                affected = cmd.ExecuteNonQuery();

                con.Close();
            }

            return affected > 0;
        }

        public bool WithdrawDeposit(int id, decimal amount)
        {
            int affected = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"UPDATE [dbo].[Wallet]
                                               SET [Balance] = Balance - @Amount
                                             WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Amount", amount);
                affected = cmd.ExecuteNonQuery();

                con.Close();
            }

            return affected > 0;
        }
    }
}
