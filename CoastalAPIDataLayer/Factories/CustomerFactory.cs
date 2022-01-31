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
    // Factory is used for Queries that return a LIST OF RESULTS
    public class CustomerFactory
    {
        //Initialize dbConnectionString to be used in the SQL Connection
        private readonly string dbConnectionString;
        public CustomerFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Customer Create(Action<Customer> initalizer)
        {
            var customer = new Customer(this.dbConnectionString);
            initalizer(customer);
            return customer;
        }

        public bool GetCustomer(string Id_no)
        {
            bool hasRows = false;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Identity_No]
                                          FROM [dbo].[Customers]
                                          WHERE [Identity_No] = @Identity_No", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Identity_No", Id_no);
                SqlDataReader reader = cmd.ExecuteReader();

                hasRows = reader.HasRows;
                con.Close();
            }

            return hasRows;
        }
    }
}
