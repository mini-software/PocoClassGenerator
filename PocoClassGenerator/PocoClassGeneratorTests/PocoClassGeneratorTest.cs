using System;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace PocoClassGeneratorTests
{
    public class PocoClassGeneratorTest
    {
        static readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=SSPI;Initial Catalog=GeneratorDataBase;";
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
            }
        }

        [Fact]
        public void DapperContrib_GenerateAllTables_Test()
        {
            using (var conn = GetConnection())
            {
                var result = conn.GenerateAllTables(GeneratorBehavior.DapperContrib);
                Console.WriteLine(result);

                Assert.Contains("[Dapper.Contrib.Extensions.ExplicitKey]", result);
                Assert.Contains("public int ID { get; set; }", result);
                Assert.Contains("[Dapper.Contrib.Extensions.Computed]", result);
                Assert.Contains("public int AutoIncrementColumn { get; set; }", result);
                Assert.Contains("[Dapper.Contrib.Extensions.Table(\"table1\")]", result);
                Assert.Contains("[Dapper.Contrib.Extensions.Key]", result);
                Assert.Contains("public int ID { get; set; }", result);
            }
        }
    }
}
