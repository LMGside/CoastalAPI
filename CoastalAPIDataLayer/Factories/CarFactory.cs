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
    public class CarFactory
    {
        private readonly string dbConnectionString;
        public CarFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Car Create(Action<Car> initalizer)
        {
            var car = new Car(this.dbConnectionString);
            initalizer(car);
            return car;
        }

        public bool CheckCarLicence(string licence)
        {
            bool hasRows = false;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Licence_No]
                                          FROM [dbo].[Cars]
                                          WHERE [Licence_No] = @Licence_No", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Licence_No", licence);
                SqlDataReader reader = cmd.ExecuteReader();

                hasRows = reader.HasRows;
                con.Close();
            }

            return hasRows;
        }
    }
}
