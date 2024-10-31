using System.Xml;
using UnityEngine;

namespace EHTool {
    public class AssetOpener : MonoBehaviour {

        public static T Import<T>(string path) where T : Object
        {
            return ImportGameObject(path).GetComponent<T>();
        }
        public static GameObject ImportGameObject(string path)
        {
            GameObject source = Resources.Load(path) as GameObject;
            return Instantiate(source);
        }

        public static string ReadTextAsset(string path) { 
            TextAsset retval = (TextAsset)Resources.Load(path, typeof(TextAsset));

            return retval.text;
        }

        public static XmlDocument ReadXML(string path)
        {
            TextAsset xmlData = (TextAsset)Resources.Load("XML/" + path, typeof(TextAsset));

            if (xmlData == null) return null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData.text);

            return xmlDoc;
        }

        public static void SaveXML(XmlDocument doc)
        {

        }
    }

    public interface XMLNodeReader {
        public void Read(XmlNode node);
    }

}