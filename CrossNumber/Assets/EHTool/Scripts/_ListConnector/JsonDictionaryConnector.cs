using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace EHTool {

    public class JsonListConnector<K> : IListConnector<K> {

        public JsonListConnector()
        {

        }

        public string GetDefaultPath() => "Json";

        public string GetExtensionName() => ".json";

        public IList<K> ReadData(string path)
        {
            string json = AssetOpener.ReadTextAsset(string.Format("{0}/{1}", GetDefaultPath(), path));

            json ??= "{\"value\":[]}";

            List<K> dic = JsonConvert.DeserializeObject<List<K>>(json);

            return dic;

        }

        public void Save(IList<K> data, string path)
        {
#if !UNITY_EDITOR
            return;
#endif

            string json = JsonUtility.ToJson(data, true);
            if (!Directory.Exists("Assets/Resources/Json"))
            {
                Directory.CreateDirectory("Assets/Resources/Json");
            }

            File.WriteAllText(string.Format("Assets/Resources/Json/{0}.json", path), json);

        }
    }
}