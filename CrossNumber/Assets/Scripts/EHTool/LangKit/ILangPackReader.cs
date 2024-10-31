using System.Collections.Generic;

namespace EHTool.LangKit {
    public interface ILangPackReader {
        public string GetStringByKey(string key);
        public void ReadData(string path);
        public void AddKey(string key);
        public void ConverFromDictionary(IDictionary<string, string> dict);
        public void ConvertType(ILangPackReader target);
        public void Save(string path);
    }
}