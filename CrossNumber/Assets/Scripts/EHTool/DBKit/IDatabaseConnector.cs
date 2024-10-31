using System;
using System.Collections.Generic;

namespace EHTool.DBKit {

    public interface IDictionaryable<T> {

        IDictionary<string, object> ToDictionary();
        void SetValueFromDictionary(IDictionary<string, object> value);

    }

    public interface IDatabaseConnector<T> where T : IDictionaryable<T> {

        public void Connect(string authName, string databaseName);
        public bool IsDatabaseExist();
        public void AddRecord(T Record);
        public void GetRecordAt(CallbackMethod<T> callback, CallbackMethod<string> fallback, int idx);
        public void UpdateRecordAt(T Record, int idx);
        public void GetAllRecord(CallbackMethod<IList<T>> callback, CallbackMethod<string> fallback);

    }
}