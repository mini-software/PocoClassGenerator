[![NuGet](https://img.shields.io/nuget/v/PocoClassGenerator.svg)](https://www.nuget.org/packages/HtmlTableHelper)
![](https://img.shields.io/nuget/dt/PocoClassGenerator.svg)

---

PocoClassGenerator is started with `Necroskillz`'s POCO Generator [Generate C# POCOs from SQL statement in LINQPad ](http://www.necronet.org/archive/2012/10/09/generate-c-pocos-from-sql-statement-in-linqpad.aspx)

### Features

- Support current DataBase all tables and views generate POCO class code
- Support [Dapper.Contrib](https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib)
- Support mutiple RDBMS : `sqlserver,oracle,mysql,postgresql`
- mini and faster (only in 5 seconds generate 100 tables code)
- Use appropriate dialect schema table SQL for each database query

### GetStart

👇First :  Copy&Paste [PocoClassGenerator.cs](PocoClassGenerator/PocoClassGenerator/PocoClassGenerator.cs) Code to your project or LINQPad.  
or Install from [NuGet](https://www.nuget.org/packages/PocoClassGenerator/)
```cmd
PM> install-package PocoClassGenerator
```

👇Second :  Use Connection to call `GenerateAllTables` and then print it.
```C#
using (var connection = Connection)
{
	Console.WriteLine(connection.GenerateAllTables());
}
```

**The Online Demo : [POCO Class Generator GenerateAllTables  | .NET Fiddle](https://dotnetfiddle.net/GkdqsU)**

![20190430141947-image.png](https://raw.githubusercontent.com/shps951023/ImageHosting/master/img/20190430141947-image.png)

#### Support Dapper Contrib POCO Class
- Just call method with `GeneratorBehavior.DapperContrib`

```C#
using (var conn = GetConnection())
{
    var result = conn.GenerateAllTables(GeneratorBehavior.DapperContrib);
    Console.WriteLine(result);
}
```
**The Online Demo : [POCO Dapper Contrib Class Generator GenerateAllTables | .NET Fiddle](https://dotnetfiddle.net/yeuK1E)
![20190502132948-image.png](https://raw.githubusercontent.com/shps951023/ImageHosting/master/img/20190502132948-image.png)**

#### Generate View
```C#
using (var conn = GetConnection())
{
    var result = conn.GenerateAllTables(GeneratorBehavior.View);
    Console.WriteLine(result);
}
```

#### Generate Comment

```C#
using (var conn = GetConnection())
{
    var result = conn.GenerateAllTables(GeneratorBehavior.Comment);
    Console.WriteLine(result);
}
```

#### Generate Comment and View and Dapper.Contrib

```C#
using (var conn = GetConnection())
{
    var result = conn.GenerateAllTables(GeneratorBehavior.Comment | GeneratorBehavior.View | GeneratorBehavior.DapperContrib);
    Console.WriteLine(result);
}
```

#### Generate one class by sql

1. Generate one class
```C#
using (var connection = Connection)
{
	var classCode = connection.GenerateClass("select * from Table");
	Console.WriteLine(classCode);
}
```

2. Specify class name
```C#
using (var connection = Connection)
{
	var classCode = connection.GenerateClass("with EMP as (select 1 ID,'WeiHan' Name,25 Age) select * from EMP", className: "EMP");
	Console.WriteLine(classCode);
}
```
