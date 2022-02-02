using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Property : Asset
    {
        private readonly string dbConnectionString;

        public Property(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int Property_ID { get; set; }
        public string Address { get; set; }
        public int SQ { get; set; }
        public PropertyType Property_Type { get; set; }

        public enum PropertyType
        {
            House = 0,
            Apartment = 1,
            Commercial_Building = 2
        }

        public bool Insert()
        {
            int affected = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"INSERT INTO [dbo].[Asset]
                                                ([Type]
                                                ,[Auto_Sale]
                                                ,[Auto_Valuation]
                                                ,[Normal_Valuation]
                                                ,[Owner])
                                            VALUES
                                                (@Type
                                                ,@AutoSale
                                                ,@AutoVal
                                                ,@NormalVal
                                                ,@Owner);

                                           SELECT SCOPE_IDENTITY();", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@AutoSale", Auto_Sale);
                cmd.Parameters.AddWithValue("@AutoVal", Auto_Valuation);
                cmd.Parameters.AddWithValue("@NormalVal", Normal_Valuation);
                cmd.Parameters.AddWithValue("@Owner", Owner);

                int modified = Convert.ToInt32(cmd.ExecuteScalar());

                if (modified > 0)
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Property]
                                               ([ID]
                                               ,[Address]
                                               ,[SQ]
                                               ,[Property])
                                         VALUES
                                               (@ID
                                               ,@Address
                                               ,@SQ
                                               ,@Property)";

                    cmd.Parameters.AddWithValue("@ID", modified);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@SQ", SQ);
                    cmd.Parameters.AddWithValue("@Property", Property_Type);

                    affected = cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            return affected > 0;
        }
    }
}
