# Transaction Demo

Steps: 

1. Naive scaffolding
2. Add transactions (magic)
3. Add filter

See https://mariadb.com/kb/en/set-transaction/#isolation-level

* READ UNCOMMITTED
* READ COMMITTED
* REPEATABLE READ <= default
* SERIALIZABLE

A consistent read means that InnoDB uses multi-versioning to present to a query a snapshot of the database at a point in time. The query sees the changes made by transactions that committed before that point in time, and no changes made by later or uncommitted transactions. The exception to this rule is that the query sees the changes made by earlier statements within the same transaction. 

If the transaction isolation level is REPEATABLE READ (the default level), all consistent reads within the same transaction read the snapshot established by the first such read in that transaction. You can get a fresher snapshot for your queries by committing the current transaction and after that issuing new queries.

With READ COMMITTED isolation level, each consistent read within a transaction sets and reads its own fresh snapshot.