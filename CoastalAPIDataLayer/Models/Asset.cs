using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Asset
    {
        private readonly string dbConnectionString;

        public Asset(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Asset()
        {
            this.dbConnectionString = ConfigurationManager.ConnectionStrings["CoastalFinanceDB"].ConnectionString;
        }

        public int ID { get; set; }
        public AssetType Type { get; set; }
        public bool Auto_Sale { get; set; }
        public int Auto_Valuation { get; set; }
        public int Normal_Valuation { get; set; }
        public int Owner { get; set; }

        public enum AssetType
        {
            Property = 0,
            Car = 1,
            Art = 2
        }

        public Asset Get(int id)
        {
            Asset asset = null;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"SELECT [ID]
                                              ,[Type]
                                              ,[Auto_Sale]
                                              ,[Auto_Valuation]
                                              ,[Normal_Valuation]
                                              ,[Owner]
                                          FROM [CoastalFinancialDB_Mfundo].[dbo].[Asset]
                                          WHERE ID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    asset = new Asset(this.dbConnectionString)
                    {
                        ID = Convert.ToInt32(reader["ID"] as int?),
                        Type = (AssetType)Convert.ToInt32((reader["Type"].ToString() ?? "0")),
                        Auto_Sale = Convert.ToBoolean(reader["Auto_Sale"] as bool?),
                        Auto_Valuation = Convert.ToInt32(reader["Auto_Valuation"] as int?),
                        Normal_Valuation = Convert.ToInt32(reader["Normal_Valuation"] as int?),
                        Owner = Convert.ToInt32(reader["Owner"] as int?)
                    };
                }

                con.Close();
            }
            return asset;
        }
    }
}
