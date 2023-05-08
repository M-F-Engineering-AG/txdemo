# Demo Project for DB Transaction Management with .NET Core
This small demo project consists of a .NET Core backend using EF Core and an SPA frontend. The transaction management of the backend can be switched at runtime from the frontend. The project was created to support a presentation on the subject.

There is a single entity containing a value. There are the following operations:
* **Double Read:** Read all entities and calculate the sum of the values. After a delay of 5 seconds the data is read again and both values are returned.
* **Sum and Insert:** Read all entities and calculate the sum of the values. After an optional delay of 5 seconds a new entity is inserted, with the value set to the sum+1 

There are the following Modes:
* **NONE:** No special transaction management, default situation when setting up a project
* **Repeatable Read:** All operations are wrapped in a repeatable read mariadb transaction
* **Serializable:** All operations are wrapped in a serializable mariadb transaction
* **Action Filter:** Transactions are managed using an action filter. Read operations are executed with repeatable read isolation, write operations are executed in serializable transactions.