using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EHTool.LangKit {

    public class JsonLangPackReader : ILangPackReader {

        string _path;
        IDictionary<string, string> _dic;

        public JsonLangPackReader()
        {
            _dic = new Dictionary<string, string>();
        }

        public void AddKey(string key)
        {
            if (key != null && !_dic.ContainsKey(key))
            {
                _dic[key] = null;
            }

            if (Directory.Exists("Assets/Resources/Json/String"))
            {
                DirectoryInfo di = new DirectoryInfo("Assets/Resources/Json/String");
                foreach (FileInfo File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".json") != 0) continue;

                    string name = File.Name.Substring(0, File.Name.Length - 5);
                    if (name.CompareTo(_path) == 0) continue;

                    ReadData(name);
                    Save(name);
                }
            }
            ReadData(_path);
            Save(_path);
        }

        public string GetStringByKey(string key)
        {
            if (_dic.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        public void ReadData(string path)
        {
            _path = path;

            string json = AssetOpener.ReadTextAsset(string.Format("Json/String/{0}", path));

            json ??= "{\"value\":[]}";

            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            foreach (string str in dic.Keys) {
                _dic[str] = dic[str];
            }

        }

        public void ConverFromDictionary(IDictionary<string, string> dict)
        {
            _dic = dict;
        }

        public void ConvertType(ILangPackReader target)
        {
            if (Directory.Exists("Assets/Resources/Json/String"))
            {
                DirectoryInfo di = new DirectoryInfo("Assets/Resources/Json/String");
                foreach (FileInfo File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".json") != 0) continue;

                    string name = File.Name.Substring(0, File.Name.Length - 5);

                    if (name.CompareTo(_path) == 0) continue;

                    ReadData(name);
                    target.ConverFromDictionary(_dic);
                    target.Save(name);
                }
            }
            ReadData(_path);
            target.ConverFromDictionary(_dic);
            target.Save(_path);
        }


        public void Save(string path)
        {

            string json = JsonConvert.SerializeObject(_dic, Formatting.Indented);
            if (!Directory.Exists("Assets/Resources/Json/String"))
            {
                Directory.CreateDirectory("Assets/Resources/Json/String");
            }

            File.WriteAllText(string.Format("Assets/Resources/Json/String/{0}.json", path), json);

        }
    }
}