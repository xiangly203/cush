# 简单记账的项目

只有 Api 和 后端

技术框架：.NET8 + ef core + PostgreSql + redis + StackExchange.Redis

> /api/Cush POST 方法

入参

```json
{
  "amount": 0,
  "type": 0,
  "kind": 0
}
```

返回参数

```json
"ok"
```

> /api/Cush/range GET 方法

| Parameters ｜ type ｜ example |
| ----------------------------- | ------ | ------------ |
| StartDateStr                  | string | "2000-01-06" |
| endDateStr                    | string | "2000-01-06" |

返回参数

```json
{
  "total": 0,
  "amount": 0,
  "income": 0,
  "outcome": 0
}
```
