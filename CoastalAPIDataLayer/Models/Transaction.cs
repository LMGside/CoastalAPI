using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Transaction
    {
        private readonly string dbConnectionString;
        public Transaction(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }
        public int ID { get; set; }
        public int Buyer { get; set; }
        public int Seller { get; set; }
        public int Asset { get; set; }
        public decimal Amount { get; set; }
        public bool Auto_Sale { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime Date_Transaction_Requested { get; set; }
        public DateTime? Date_Transaction_Approved { get; set; }
        public string Who_Approved { get; set; }

        public enum TransactionStatus
        {
            Success = 0,
            Fail = 1,
            Approved = 2,
            Rejected = 3
        }

        public int Insert()
        {
            int ID = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"INSERT INTO [dbo].[Transaction]
                                                   ([Buyer]
                                                   ,[Seller]
                                                   ,[Asset]
                                                   ,[Amount]
                                                   ,[Auto_Sale]
                                                   ,[Status]
                                                   ,[Date_Transaction_Requested]
                                                   ,[Date_Transaction_Approved]
                                                   ,[Who_Approved])
                                             VALUES
                                                   (@Buyer
                                                   ,@Seller
                                                   ,@Asset
                                                   ,@Amount
                                                   ,@Auto_Sale
                                                   ,@Status
                                                   ,@DateRequested
                                                   ,@DateApproved
                                                   ,@WhoApproved);

                                                    SELECT SCOPE_IDENTITY();", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Buyer", Buyer);
                cmd.Parameters.AddWithValue("@Seller", Seller);
                cmd.Parameters.AddWithValue("@Asset", Asset);
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@Auto_Sale", Auto_Sale);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@DateRequested", Date_Transaction_Requested);
                cmd.Parameters.AddWithValue("@DateApproved", Date_Transaction_Approved);
                cmd.Parameters.AddWithValue("@WhoApproved", Who_Approved);

                ID = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
            return ID;
        }

        public Transaction Get()
        {
            Transaction tra = null;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Buyer]
                                              ,[Seller]
                                              ,[Asset]
                                              ,[Amount]
                                              ,[Auto_Sale]
                                              ,[Status]
                                              ,[Date_Transaction_Requested]
                                              ,[Date_Transaction_Approved]
                                              ,[Who_Approved]
                                          FROM [dbo].[Transaction]
                                          WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", ID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Buyer = Convert.ToInt32(reader["Buyer"] as int?),
                        Seller = Convert.ToInt32(reader["Seller"] as int?),
                        Asset = Convert.ToInt32(reader["Asset"] as int?),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
                        Date_Transaction_Requested = Convert.ToDateTime((reader["Date_Transaction_Requested"] as DateTime?).GetValueOrDefault()),
                        Date_Transaction_Approved = Convert.ToDateTime((reader["Date_Transaction_Approved"] as DateTime?).GetValueOrDefault()),
                        Who_Approved = reader["Who_Approved"].ToString() ?? ""
                    };
                }
                con.Close();
            }

            return tra;
        }

        public bool Update(int id)
        {
            int affectedRows = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"UPDATE [dbo].[Transaction]
                                           SET [Buyer] = @Buyer
                                              ,[Seller] = @Seller
                                              ,[Asset] = @Asset
                                              ,[Amount] = @Amount
                                              ,[Auto_Sale] = @Auto_Sale
                                              ,[Status] = @Status
                                              ,[Date_Transaction_Requested] = @DateRequested
                                              ,[Date_Transaction_Approved] = @DateApproved
                                              ,[Who_Approved] = @WhoApproved
                                         WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Buyer", Buyer);
                cmd.Parameters.AddWithValue("@Seller", Seller);
                cmd.Parameters.AddWithValue("@Asset", Asset);
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@Auto_Sale", Auto_Sale);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@DateRequested", Date_Transaction_Requested);
                cmd.Parameters.AddWithValue("@DateApproved", Date_Transaction_Approved);
                cmd.Parameters.AddWithValue("@WhoApproved", Who_Approved);
                cmd.Parameters.AddWithValue("@ID", id);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }
    }
}
