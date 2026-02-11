using System;
using System.Collections.Generic;

namespace EasyH.Tool.DBKit {

    public interface IDictionaryable<T> {

        public IDictionary<string, object> ToDictionary();
        public void SetValueFromDictionary(IDictionary<string, object> value);

    }

    public interface IDatabaseConnector<K, T> where T : IDictionaryable<T>
    {
        public class UpdateLog
        {
            public K Idx;
            public T Record;

            public UpdateLog(K idx, T record)
            {
                Idx = idx;
                Record = record;
            }
        }

        public void Connect(string[] args);
        public void Connect(string authName, string databaseName);
        public bool IsDatabaseExist();
        public void AddRecord(T Record);

        public void GetAllRecord(Action<IDictionary<K, T>> callback, Action<string> fallback);
        public void GetRecordAt(K idx, Action<T> callback, Action<string> fallback);

        public void UpdateRecordAt(K idx, T Record);
        public void UpdateRecord(UpdateLog[] updates);

        public void DeleteRecordAt(K idx);

    }
}