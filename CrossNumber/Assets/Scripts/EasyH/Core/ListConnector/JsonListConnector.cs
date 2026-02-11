using System.Collections.Generic;
using Newtonsoft.Json;

namespace EasyH
{

    public class JsonListConnector<T> : IListConnector<T>
    {

        public string GetDefaultPath() => "Json";
        public string GetExtensionName() => ".json";

        public IList<T> ReadData(string path)
        {
            string json = FileManager.Instance.FileConnector.Read(
                string.Format("{0}/{1}", GetDefaultPath(), path));

            json ??= "[]";

            IList<T> ret = JsonConvert.
                DeserializeObject<List<T>>(json);

            return ret;
        }
    }
}