PocoClassGenerator is based on `Necroskillz`'s POCO Generator [Generate C# POCOs from SQL statement in LINQPad ](http://www.necronet.org/archive/2012/10/09/generate-c-pocos-from-sql-statement-in-linqpad.aspx)

### Features

- Support current DataBase all tables and views generate POCO class code
- Support mutiple RDBMS : `sqlserver,oracle,mysql,postgresql`
- mini and faster (only in 5 seconds generate 100 tables code)
- Fix Dialect SchemaTable SQL for each database query
- Special fields for each database (such as SQLServer use [])

### GetStart

👇First :  Copy&Paste [PocoClassGenerator.cs](https://github.com/shps951023/PocoClassGenerator/blob/master/PocoClassGenerator.cs) Code to your project or LINQPad.

👇Second :  Use Connection to call `GenerateAllTables` and then print it.
```C#
	using (var connection = Connection)
	{
		Console.WriteLine(connection.GenerateAllTables());
	}
```

**The Online Demo : [POCO Class Generator GenerateAllTables  | .NET Fiddle](https://dotnetfiddle.net/GkdqsU)**

### Demo Photo
![20190430141947-image.png](https://raw.githubusercontent.com/shps951023/ImageHosting/master/img/20190430141947-image.png)

### Domain Logic
- ExecuteReader and `CommandBehavior.KeyInfo` and GetSchemaTable methods to get RDBMS's table/column data.
- query use `where  1=2`to get `0` data to improve query efficiency

### Domain Code

```C#
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

public static class PocoClassGenerator
{
	private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string> {
		{ typeof(int), "int" },
		{ typeof(short), "short" },
		{ typeof(byte), "byte" },
		{ typeof(byte[]), "byte[]" },
		{ typeof(long), "long" },
		{ typeof(double), "double" },
		{ typeof(decimal), "decimal" },
		{ typeof(float), "float" },
		{ typeof(bool), "bool" },
		{ typeof(string), "string" }
	};

	private static readonly Dictionary<string, string> QuerySqls = new Dictionary<string, string> {
		{"sqlconnection", "select  *  from [{0}] where 1=2" },
		{"sqlceserver", "select  *  from [{0}] where 1=2" },
		{"sqliteconnection", "select  *  from [{0}] where 1=2" },
		{"oracleconnection", "select  *  from \"{0}\" where 1=2" },
		{"mysqlconnection", "select  *  from `{0}` where 1=2" },
		{"npgsqlconnection", "select  *  from \"{0}\" where 1=2" }
	};

	private static readonly Dictionary<string, string> SchemaSqls = new Dictionary<string, string> {
		{"sqlconnection", "select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'" },
		{"sqlceserver", "select TABLE_NAME from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE = 'BASE TABLE'" },
		{"sqliteconnection", "SELECT name FROM sqlite_master where type = 'table'" },
		{"oracleconnection", "select TABLE_NAME from USER_TABLES where table_name not in (select View_name from user_views)" },
		{"mysqlconnection", "select TABLE_NAME from  information_schema.tables where TABLE_TYPE = 'BASE TABLE';" },
		{"npgsqlconnection", "select table_name from information_schema.tables where table_type = 'BASE TABLE'" }
	};

	private static readonly HashSet<Type> NullableTypes = new HashSet<Type> {
		typeof(int),
		typeof(short),
		typeof(long),
		typeof(double),
		typeof(decimal),
		typeof(float),
		typeof(bool),
		typeof(DateTime)
	};
	
	
	public static string GenerateAllTables(this System.Data.Common.DbConnection connection,bool containsView = false)
	{
		if (connection.State != ConnectionState.Open)
			connection.Open();
		
		var conneciontName = connection.GetType().Name.ToLower();
		var tables = new List<string>();
		using (var command = connection.CreateCommand())
		{
			command.CommandText = containsView ? Regex.Split(SchemaSqls[conneciontName],"where")[0]:SchemaSqls[conneciontName];			
			using (var reader = command.ExecuteReader())
			{
				while(reader.Read())
					tables.Add(reader.GetString(0));
			}
		}
		
		var sb = new StringBuilder();
		sb.AppendLine("namespace Models { ");
		tables.ForEach(table=> sb.Append(connection.GenerateClass(string.Format(QuerySqls[conneciontName],table))));
		sb.AppendLine("}");
		return sb.ToString();
	}

	public static string GenerateClass(this IDbConnection connection, string sql,string className = null)
	{
		if (connection.State != ConnectionState.Open)
			connection.Open();

		var cmd = connection.CreateCommand();
		cmd.CommandText = sql;


		using (var reader = cmd.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SingleRow ))
		{
			var builder = new StringBuilder();
			do
			{
				if (reader.FieldCount <= 1) continue;

				var schema = reader.GetSchemaTable();
				foreach (DataRow row in schema.Rows)
				{
					if (string.IsNullOrWhiteSpace(builder.ToString()))
					{
						var tableName = string.IsNullOrWhiteSpace(className) ? row["BaseTableName"] as string ?? "Info" : className;
						builder.AppendFormat("	public class {0}{1}", tableName, Environment.NewLine);
						builder.AppendLine("	{");
					}


					var type = (Type)row["DataType"];
					var name = TypeAliases.ContainsKey(type) ? TypeAliases[type] : type.Name;
					var isNullable = (bool)row["AllowDBNull"] && NullableTypes.Contains(type);
					var collumnName = (string)row["ColumnName"];

					builder.AppendLine(string.Format("		public {0}{1} {2} {{ get; set; }}", name, isNullable ? "?" : string.Empty, collumnName));
					builder.AppendLine();
				}

				builder.AppendLine("	}");
				builder.AppendLine();
			} while (reader.NextResult());

			return builder.ToString();
		}
	}
}
```
