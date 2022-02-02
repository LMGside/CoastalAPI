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
    public class AssetFactory
    {
        private readonly string dbConnectionString;
        public AssetFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Asset Create(Action<Asset> initalizer)
        {
            var asset = new Asset(this.dbConnectionString);
            initalizer(asset);
            return asset;
        }

        public bool OwnerChange(int assetID, int newOwner)
        {
            int affected = 0;
            using (var con = new SqlConnection(this.dbConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(@"UPDATE [dbo].[Asset]
                                            SET [Owner] = @ID
                                            WHERE [ID] = @AssetID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", newOwner);
                cmd.Parameters.AddWithValue("@AssetID", assetID);
                affected = cmd.ExecuteNonQuery();

                con.Close();
            }

            return affected > 0;
        }
    }
}
