using System;
using System.Collections.Generic;

namespace EHTool.DBKit {

    public interface IDictionaryable<T> {

        public IDictionary<string, object> ToDictionary();
        public void SetValueFromDictionary(IDictionary<string, object> value);

    }

    public interface IDatabaseConnector<K, T> where T : IDictionaryable<T>
    {
        public void Connect(string[] args);
        public void Connect(string authName, string databaseName);
        public bool IsDatabaseExist();
        public void AddRecord(T Record);
        public void GetAllRecord(Action<IDictionary<K, T>> callback, Action<string> fallback);
        public void GetRecordAt(K idx, Action<T> callback, Action<string> fallback);
        public void UpdateRecordAt(K idx, T Record);
        public void DeleteRecordAt(K idx);

    }
}