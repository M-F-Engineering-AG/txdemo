namespace txdemo.db;
public enum TxMode
{
    NONE,
    REPEATABLE_READ,
    SERIALIZABLE,
    ACTION_FILTER,
}