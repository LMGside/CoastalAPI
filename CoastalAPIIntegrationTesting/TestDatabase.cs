using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIIntegrationTesting
{
    internal sealed class TestDatabase : IDisposable
    {
        // TODO: The instance name for LocalDB is not necessarily this, perhaps use env variable
        private const string LocalDbMaster =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        private const string TestConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;
              MultipleActiveResultSets=True;AttachDBFilename={1}";

        private readonly string _databaseName;

        private string _mdfPath;
        private string _ldfPath;

        public TestDatabase()
        {
            _databaseName = GetRandomName(8);
        }

        public TestDatabase(string databaseName)
        {
            _databaseName = databaseName;
        }

        private static readonly Random random = new Random();

        //Random Name for Database
        public static string GetRandomName(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }

        //Restore Database (Duplicate Database Area)
        public bool RestoreDatabase(string backupFilePath, string restoreBasePath)
        {
            using (var connection = new SqlConnection(LocalDbMaster))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = $"RESTORE FILELISTONLY FROM DISK = N'{backupFilePath}'";
                var reader = cmd.ExecuteReader();

                string dbLogicalName = null;
                string logLogicalName = null;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string logicalName = reader.GetString(0);
                        string physicalName = reader.GetString(1);
                        string type = reader.GetString(2);

                        if (type.Equals("D"))
                        {
                            dbLogicalName = logicalName;
                        }

                        if (type.Equals("L"))
                        {
                            logLogicalName = logicalName;
                        }
                    }
                }

                reader.Close();

                if (dbLogicalName == null || logLogicalName == null)
                {
                    return false;
                }

                _mdfPath = Path.Combine(restoreBasePath, _databaseName + ".mdf");
                _ldfPath = Path.Combine(restoreBasePath, _databaseName + ".ldf");

                cmd.CommandText =
                    $"RESTORE DATABASE [{_databaseName}] FROM DISK = N'{backupFilePath}' WITH REPLACE, RECOVERY, MOVE N'{dbLogicalName}' TO N'{_mdfPath}', MOVE N'{logLogicalName}' TO N'{_ldfPath}'";
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(string.Format(TestConnectionString, _databaseName, _mdfPath));
        }

        public string GetConnectionString()
        {
            return string.Format(TestConnectionString, _databaseName, _mdfPath);
        }

        private bool DetachDatabase(bool deleteFiles)
        {
            using (var connection = new SqlConnection(LocalDbMaster))
            {
                connection.Open();
                SqlCommand cmd;
                try
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandText = $"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"exec sp_detach_db '{_databaseName}', 'true', 'false'";
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }
            }

            if (!deleteFiles)
            {
                return true;
            }

            try
            {
                if (File.Exists(_mdfPath))
                {
                    File.Delete(_mdfPath);
                }

                if (File.Exists(_ldfPath))
                {
                    File.Delete(_ldfPath);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            DetachDatabase(true);
        }
    }
}
