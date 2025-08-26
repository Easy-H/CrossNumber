using System;
using System.Collections.Generic;

namespace EHTool.DBKit
{

    public interface IDictionaryable<T>
    {

        public IDictionary<string, object> ToDictionary();
        public void SetValueFromDictionary(IDictionary<string, object> value);

    }

    public class UpdateData<K, R> where R : IDictionaryable<R>
    {
        public string[] Location { get; private set; }
        public K Idx { get; private set; }
        public R Record { get; private set; }

        public UpdateData(K idx, R record)
        {
            Location = new string[0];
            Idx = idx;
            Record = record;
        }

        public UpdateData(string[] location, K idx, R record)
        {
            Location = location;
            Idx = idx;
            Record = record;
        }

    }

    public interface IDatabaseConnector<K, R> where R : IDictionaryable<R>
    {
        public void Connect(string[] args);
        public void Connect(string authName, string databaseName);
        public bool IsDatabaseExist();
        public void GetAllRecord(Action<IDictionary<K, R>> callback,
            Action<string> fallback);

        public void GetRecordAt(K idx,
            Action<R> callback, Action<string> fallback);

        public void AddRecord(R Record);

        public void UpdateRecordAt(K idx, R record);
        //public void UpdateRecords(UpdateData<K, R>[] updateData);

        public void DeleteRecordAt(K idx);

    }
}