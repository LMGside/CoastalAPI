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
                if(Auto_Valuation != null)
                {
                    cmd.Parameters.AddWithValue("@AutoVal", Auto_Valuation);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AutoVal", DBNull.Value);
                }
                
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

        public Art Get()
        {
            Art art = null;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT Art.ID
                                              ,[Artist]
                                              ,[Art_Title]
                                              ,[Year_Completed]
	                                          ,Asset.Auto_Sale
	                                          ,Asset.Auto_Valuation
	                                          ,Asset.Normal_Valuation
	                                          ,Asset.Owner
                                          FROM [CoastalFinancialDB_Mfundo].[dbo].[Art]
                                          INNER JOIN [Asset] ON [Art].ID = [Asset].ID
                                          WHERE [Art].ID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", Art_ID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    art = new Art(this.dbConnectionString)
                    {
                        Art_ID = Convert.ToInt32(reader["ID"] as int?),
                        Artist = reader["Artist"].ToString() ?? "",
                        ArtTitle = reader["Art_Title"].ToString() ?? "",
                        Year_Completed = Convert.ToInt32(reader["Year_Completed"] as int?),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Auto_Valuation = Convert.ToInt32(reader["Auto_Valuation"] as int?),
                        Normal_Valuation = Convert.ToInt32(reader["Normal_Valuation"] as int?),
                        Owner = Convert.ToInt32(reader["Owner"] as int?)
                    };
                }

                con.Close();
            }
            return art;
        }

        public bool DeleteArt ()
        {
            int affectedRows = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"DELETE [dbo].[Art]
                                         WHERE [ID] = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", Art_ID);

                affectedRows = cmd.ExecuteNonQuery();
                con.Close();
            }
            return affectedRows > 0;
        }
    }
}
