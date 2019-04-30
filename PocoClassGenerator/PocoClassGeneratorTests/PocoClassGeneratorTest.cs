using System;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace PocoClassGeneratorTests
{
    public class PocoClassGeneratorTest
    {
        static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=GeneratorDataBase;";
        DbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        [Fact]
        public void GenerateAllTables()
        {

            using (var conn = GetConnection())
            {
                var result = conn.GenerateAllTables();
                Console.WriteLine(result);

                Assert.Contains("public class table1", result);
                Assert.Contains("public class table2", result);
                Assert.Contains("public class table3", result);
            }
        }
    }
}
