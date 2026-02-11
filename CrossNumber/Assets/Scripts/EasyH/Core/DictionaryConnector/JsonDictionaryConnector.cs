using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace EasyH {

    public class JsonDictionaryConnector<K, V> : IDictionaryConnector<K, V> {

        public JsonDictionaryConnector()
        {
            
        }

        public string GetDefaultPath() => "Json";

        public string GetExtensionName() => ".json";

        public IDictionary<K, V> ReadData(string path)
        {
            string json = FileManager.Instance.FileConnector.Read(
                string.Format("{0}/{1}", GetDefaultPath(), path));

            json ??= "{\"value\":[]}";

            Dictionary<K, V> dic = JsonConvert.DeserializeObject<Dictionary<K, V>>(json);

            return dic;

        }

        public void Save(IDictionary<K, V> data, string path)
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