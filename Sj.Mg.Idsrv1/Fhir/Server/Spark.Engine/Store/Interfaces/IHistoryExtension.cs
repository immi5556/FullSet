using Spark.Engine.Core;

namespace Spark.Engine.Store.Interfaces
{
    public interface IHistoryStore
    {
        Snapshot History(string typename, HistoryParameters parameters);
        Snapshot History(IKey key, HistoryParameters parameters);
        Snapshot History(HistoryParameters parameters);
    }
}