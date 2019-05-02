### version 0.9.2
- [X] Support Dapper Contrib
- [X] Support Generate Class Comment
- [X] If table/view name contains white space then replace it by empty string

### version 0.9.1
- [X]   Support current DataBase all tables and views generate POCO class code
- [X]   Support multiple RDBMS: `sqlserver`, `oracle`, `mysql`, `postgresql`
- [X]   Mini and faster (only in 5 seconds generate 100 tables code)
- [X]   Use appropriate dialect schema table SQL for each database query

------------------------

### 版本 0.9.2
- 支持Dapper Contrib
- 支持類別註解Comment生成,方便辨認欄位資料
- 如果表格名稱含有特殊white Space,將以空字串取代,避免無法使用情況

### 版本 0.9.1
- 支持當前資料庫`全部表格、View批量生成POCO類別代碼`
- 支持`多資料庫`SQLServer、Oracle、MySQL、PostgreSQL
- 效率挺快的,一百個表格5秒以內跑完
- 可以決定是否要生成View的Class(預設不生成)