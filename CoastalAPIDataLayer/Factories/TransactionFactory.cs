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
                                          WHERE Date_Transaction_Requested = @Date", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tra = new Transaction(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Buyer = reader["Buyer"].ToString() ?? "",
                        Seller = reader["Seller"].ToString() ?? "",
                        Asset = reader["Asset"].ToString() ?? "",
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Status = (TransactionStatus)Convert.ToInt32((reader["Status"].ToString() ?? "0")),
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

        public List<Transaction> GetUsersTransactions(string buyer)
        {
            Transaction tra = null;
            List<Transaction> transactionList = new List<Transaction>();
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
                                          WHERE [Buyer] = @User", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@User", buyer);
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
                                          WHERE [Date_Transaction_Requested] BETWEEN @StartDate AND @EndDate", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
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
                                          WHERE [Status] = 2", con);
                cmd.CommandType = CommandType.Text;
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
                                          WHERE [Status] = 3", con);
                cmd.CommandType = CommandType.Text;
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
                                          WHERE Date_Transaction_Requested = @Date AND [Buyer] = @Buyer", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", DateTime.Today.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Buyer", id);
                total = cmd.ExecuteNonQuery();

                con.Close();
            }

            return total;
        }
    }
}
