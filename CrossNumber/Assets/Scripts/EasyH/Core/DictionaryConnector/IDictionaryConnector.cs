using System.Collections.Generic;

namespace EasyH {
    public interface IDictionaryConnector<K, V> {
        public string GetDefaultPath();
        public string GetExtensionName();
        public IDictionary<K, V> ReadData(string path);
        public void Save(IDictionary<K, V> data, string path);
    }
}