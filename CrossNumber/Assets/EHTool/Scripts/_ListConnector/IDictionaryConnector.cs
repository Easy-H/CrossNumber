using System.Collections.Generic;

namespace EHTool {
    public interface IListConnector<K> {
        public string GetDefaultPath();
        public string GetExtensionName();
        public IList<K> ReadData(string path);
        public void Save(IList<K> data, string path);
    }
}