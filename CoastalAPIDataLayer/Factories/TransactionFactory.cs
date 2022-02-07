using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoastalAPIDataLayer.Models;

namespace CoastalAPIDataLayer.Factories
{
    public class TransactionFactory
    {
        private readonly string dbConnectionString;
        public TransactionFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Transaction Create(Action<Transaction> initalizer)
        {
            var transaction = new Transaction(this.dbConnectionString);
            initalizer(transaction);
            return transaction;
        }

        public List<Transaction> GetDayTransactions(DateTime date)
        {
            Transaction tra = null;
            List<Transaction> transactionList = new List<Transaction>();
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE Date_Transaction_Requested = @Date", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Buyer = Convert.ToInt32(reader["Buyer"] as int?),
                        Name1 = reader["Name1"].ToString() ?? "",
                        Surname1 = reader["Surname1"].ToString() ?? "",
                        Seller = Convert.ToInt32(reader["Seller"] as int?),
                        Name2 = reader["Name2"].ToString() ?? "",
                        Surname2 = reader["Surname2"].ToString() ?? "",
                        Asset = Convert.ToInt32(reader["Asset"] as int?),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (Transaction.TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
                        Date_Transaction_Requested = Convert.ToDateTime((reader["Date_Transaction_Requested"] as DateTime?).GetValueOrDefault()),
                        Date_Transaction_Approved = Convert.ToDateTime((reader["Date_Transaction_Approved"] as DateTime?).GetValueOrDefault()),
                        Who_Approved = reader["Who_Approved"].ToString() ?? ""
                    };



                    transactionList.Add(tra);
                }
                con.Close();
            }

            return transactionList;
        }

        public List<Transaction> GetUsersTransactions(int buyer)
        {
            Transaction tra = null;
            List<Transaction> transactionList = new List<Transaction>();
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE [Buyer] = @User", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@User", buyer);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Name1 = reader["Name1"].ToString() ?? "",
                        Surname1 = reader["Surname1"].ToString() ?? "",
                        Seller = Convert.ToInt32(reader["Seller"] as int?),
                        Name2 = reader["Name2"].ToString() ?? "",
                        Surname2 = reader["Surname2"].ToString() ?? "",
                        Asset = Convert.ToInt32(reader["Asset"] as int?),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (Transaction.TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
                        Date_Transaction_Requested = Convert.ToDateTime((reader["Date_Transaction_Requested"] as DateTime?).GetValueOrDefault()),
                        Date_Transaction_Approved = Convert.ToDateTime((reader["Date_Transaction_Approved"] as DateTime?).GetValueOrDefault()),
                        Who_Approved = reader["Who_Approved"].ToString() ?? ""
                    };

                    transactionList.Add(tra);
                }
                con.Close();
            }

            return transactionList;
        }

        public List<Transaction> GetDateRangeTransactions(DateTime startDate, DateTime endDate)
        {
            Transaction tra = null;
            List<Transaction> transactionList = new List<Transaction>();
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE [Date_Transaction_Requested] BETWEEN @StartDate AND @EndDate", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        Name1 = reader["Name1"].ToString() ?? "",
                        Surname1 = reader["Surname1"].ToString() ?? "",
                        Seller = Convert.ToInt32(reader["Seller"] as int?),
                        Name2 = reader["Name2"].ToString() ?? "",
                        Surname2 = reader["Surname2"].ToString() ?? "",
                        Asset = Convert.ToInt32(reader["Asset"] as int?),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (Transaction.TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
                        Date_Transaction_Requested = Convert.ToDateTime((reader["Date_Transaction_Requested"] as DateTime?).GetValueOrDefault()),
                        Date_Transaction_Approved = Convert.ToDateTime((reader["Date_Transaction_Approved"] as DateTime?).GetValueOrDefault()),
                        Who_Approved = reader["Who_Approved"].ToString() ?? ""
                    };

                    transactionList.Add(tra);
                }
                con.Close();
            }

            return transactionList;
        }

        public List<Transaction> SuccessfulTransactions()
        {
            Transaction tra = null;
            List<Transaction> transactionList = new List<Transaction>();
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE [Status] = 2", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Name1 = reader["Name1"].ToString() ?? "",
                        Surname1 = reader["Surname1"].ToString() ?? "",
                        Seller = Convert.ToInt32(reader["Seller"] as int?),
                        Name2 = reader["Name2"].ToString() ?? "",
                        Surname2 = reader["Surname2"].ToString() ?? "",
                        Asset = Convert.ToInt32(reader["Asset"] as int?),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (Transaction.TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
                        Date_Transaction_Requested = Convert.ToDateTime((reader["Date_Transaction_Requested"] as DateTime?).GetValueOrDefault()),
                        Date_Transaction_Approved = Convert.ToDateTime((reader["Date_Transaction_Approved"] as DateTime?).GetValueOrDefault()),
                        Who_Approved = reader["Who_Approved"].ToString() ?? ""
                    };

                    transactionList.Add(tra);
                }
                con.Close();
            }

            return transactionList;
        }

        public List<Transaction> UnsuccessfulTransactions()
        {
            Transaction tra = null;
            List<Transaction> transactionList = new List<Transaction>();
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE [Status] = 3", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Name1 = reader["Name1"].ToString() ?? "",
                        Surname1 = reader["Surname1"].ToString() ?? "",
                        Seller = Convert.ToInt32(reader["Seller"] as int?),
                        Name2 = reader["Name2"].ToString() ?? "",
                        Surname2 = reader["Surname2"].ToString() ?? "",
                        Asset = Convert.ToInt32(reader["Asset"] as int?),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (Transaction.TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
                        Date_Transaction_Requested = Convert.ToDateTime((reader["Date_Transaction_Requested"] as DateTime?).GetValueOrDefault()),
                        Date_Transaction_Approved = Convert.ToDateTime((reader["Date_Transaction_Approved"] as DateTime?).GetValueOrDefault()),
                        Who_Approved = reader["Who_Approved"].ToString() ?? ""
                    };

                    transactionList.Add(tra);
                }
                con.Close();
            }

            return transactionList;
        }

        public int MaxTransactionsCheck()
        {
            int total = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE Date_Transaction_Requested = @Date", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", DateTime.Today.ToString("yyyy-MM-dd"));
                total = cmd.ExecuteNonQuery();
                
                con.Close();
            }

            return total;
        }

        public int MaxTransactionsBuyerCheck(int id)
        {
            int total = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [Transaction].[ID]
                                                ,[Buyer]
	                                            ,a1.[Name] AS 'Name1'
                                                ,a1.[Surname] AS 'Surname1'
                                                ,[Seller]
	                                            ,a2.[Name] AS 'Name2'
                                                ,a2.[Surname] AS 'Surname2'
                                                ,[Asset]
                                                ,[Amount]
                                                ,[Auto_Sale]
                                                ,[Status]
                                                ,[Date_Transaction_Requested]
                                                ,[Date_Transaction_Approved]
                                                ,[Who_Approved]
                                            FROM [dbo].[Transaction]
                                            INNER JOIN [Customers] a1 ON [Transaction].[Buyer] = a1.[ID]
                                            INNER JOIN [Customers] a2 ON [Transaction].[Seller] = a2.ID
                                          WHERE Date_Transaction_Requested = @Date AND [Buyer] = @Buyer", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", DateTime.Today.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Buyer", id);
                total = cmd.ExecuteNonQuery();

                con.Close();
            }

            return total;
        }

        public bool AddWaitingApprovalTransaction(Transaction tran)
        {
            int affectedRows = 0;
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
                                                   ,[Date_Transaction_Requested])
                                             VALUES
                                                   (@Buyer
                                                   ,@Seller
                                                   ,@Asset
                                                   ,@Amount
                                                   ,@Auto_Sale
                                                   ,@Status
                                                   ,@DateRequested)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Buyer", tran.Buyer);
                cmd.Parameters.AddWithValue("@Seller", tran.Seller);
                cmd.Parameters.AddWithValue("@Asset", tran.Asset);
                cmd.Parameters.AddWithValue("@Amount", tran.Amount);
                cmd.Parameters.AddWithValue("@Auto_Sale", tran.Auto_Sale);
                cmd.Parameters.AddWithValue("@Status", tran.Status);
                cmd.Parameters.AddWithValue("@DateRequested", tran.Date_Transaction_Requested);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }

            return affectedRows > 0;
        }

        public bool UpdateRejectedTransaction (Transaction tran)
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
                                         WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Buyer", tran.Buyer);
                cmd.Parameters.AddWithValue("@Seller", tran.Seller);
                cmd.Parameters.AddWithValue("@Asset", tran.Asset);
                cmd.Parameters.AddWithValue("@Amount", tran.Amount);
                cmd.Parameters.AddWithValue("@Auto_Sale", tran.Auto_Sale);
                cmd.Parameters.AddWithValue("@Status", tran.Status);
                cmd.Parameters.AddWithValue("@DateRequested", tran.Date_Transaction_Requested);
                cmd.Parameters.AddWithValue("@ID", tran.ID);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }
    }
}
