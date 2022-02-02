using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Art : Asset
    {
        private readonly string dbConnectionString;

        public Art(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int Art_ID { get; set; }
        public string Artist { get; set; }
        public string ArtTitle { get; set; }
        public int Year_Completed { get; set; }

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
                    cmd.CommandText = @"INSERT INTO [dbo].[Art]
                                               ([ID]
                                               ,[Artist]
                                               ,[Art_Title]
                                               ,[Year_Completed])
                                         VALUES
                                               (@ID
                                               ,@Artist
                                               ,@ArtTitle
                                               ,@Year);";

                    cmd.Parameters.AddWithValue("@ID", modified);
                    cmd.Parameters.AddWithValue("@Artist", Artist);
                    cmd.Parameters.AddWithValue("@ArtTitle", ArtTitle);
                    cmd.Parameters.AddWithValue("@Year", Year_Completed);

                    affected = cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            return affected > 0;
        }
    }
}
