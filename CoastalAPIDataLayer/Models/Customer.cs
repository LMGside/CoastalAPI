using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Customer
    {
        private readonly string dbConnectionString;

        public Customer(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Identity_No { get; set; }
        public string Contact { get; set; }
        public bool Blocked { get; set; }
        public int Sales_Made { get; set; }
        public int Rating { get; set; }

        public bool Insert()
        {
            bool added = false;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"INSERT INTO [dbo].[Customers]
                                               ([Name]
                                               ,[Surname]
                                               ,[DOB]
                                               ,[Address]
                                               ,[Identity_No]
                                               ,[Contact_No]
                                               ,[Blocked]
                                               ,[Sales_Made]
                                               ,[Rating])
                                         VALUES
                                               (@Name
                                               ,@Surname
                                               ,@DOB
                                               ,@Address
                                               ,@Identity_No
                                               ,@Contact_No
                                               ,@Blocked
                                               ,@Sales_Made
                                               ,@Rating);

                                           SELECT SCOPE_IDENTITY();", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Surname", Surname);
                cmd.Parameters.AddWithValue("@DOB", DOB);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@Identity_No", Identity_No);
                cmd.Parameters.AddWithValue("@Contact_No", Contact);
                cmd.Parameters.AddWithValue("@Blocked", Blocked);
                cmd.Parameters.AddWithValue("@Sales_Made", Sales_Made);
                cmd.Parameters.AddWithValue("@Rating", Rating);

                int modified = Convert.ToInt32(cmd.ExecuteScalar());
                added = InsertWallet(modified);
                con.Close();
            }

            return added;
        }

        public bool InsertWallet(int id)
        {
            int affectedRows = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"INSERT INTO [dbo].[Wallet]
                                               ([ID]
                                               ,[Balance])
                                         VALUES
                                               (@ID
                                               ,@Balance)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Balance", 0);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }

        public bool Delete(int id)
        {
            int affectedRows = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"DELETE [dbo].[Customers]
                                         WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }

        public Customer Get(string id)
        {
            Customer customer = null;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Name]
                                              ,[Surname]
                                              ,[DOB]
                                              ,[Address]
                                              ,[Identity_No]
                                              ,[Contact_No]
                                              ,[Blocked]
                                              ,[Sales_Made]
                                              ,[Rating]
                                          FROM [CoastalFinancialDB_Mfundo].[dbo].[Customers]
                                          WHERE Identity_No = @ID ", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader =  cmd.ExecuteReader();

                while (reader.Read())
                {
                    customer = new Customer(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Name = reader["Name"].ToString() ?? "",
                        Surname = reader["Surname"].ToString() ?? "",
                        DOB = Convert.ToDateTime((reader["DOB"] as DateTime?).GetValueOrDefault()),
                        Address = reader["Address"].ToString() ?? "",
                        Identity_No = reader["Identity_No"].ToString() ?? "",
                        Contact = reader["Contact_No"].ToString() ?? "",
                        Blocked = Convert.ToBoolean(reader["Blocked"] as bool?),
                        Sales_Made = Convert.ToInt32(reader["Sales_Made"] as int?),
                        Rating = Convert.ToInt32(reader["Rating"] as int?)
                    };
                }

                con.Close();
            }
            return customer;
        }
    }
}
