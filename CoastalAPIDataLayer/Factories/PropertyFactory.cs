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
    public class PropertyFactory
    {
        private readonly string dbConnectionString;
        public PropertyFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Property Create(Action<Property> initalizer)
        {
            var property = new Property(this.dbConnectionString);
            initalizer(property);
            return property;
        }

        public bool CheckPropAddress(string address)
        {
            bool hasRows = false;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Address]
                                          FROM [dbo].[Property]
                                          WHERE [Address] = @Address", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Address", address);
                SqlDataReader reader = cmd.ExecuteReader();

                hasRows = reader.HasRows;
                con.Close();
            }

            return hasRows;
        }
    }
}
