using System.Collections.Generic;
using System.IO;
using System.Xml;

public class ClearDataManager : MonoSingleton<ClearDataManager> {

    List<string> _clearData;

    string _path = "Assets/Resources/ClearData.xml";

    public void Clear(string stage)
    {
        if (_clearData.Contains(stage))
            return;

        _clearData.Add(stage);
        SaveFurnitureXML();
    }

    public void Null()
    {
        return;
    }
    public void SaveFurnitureXML()
    {
        Write(_path);
    }

    protected override void OnCreate()
    {
        _clearData = new List<string>();

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(_path);
        }
        catch (FileNotFoundException)
        {
            return;
        }

        XmlNodeList nodes = xmlDoc.SelectNodes("ClearData/Clear");

        for (int i = 0; i < nodes.Count; i++)
        {
            string stage = nodes[i].Attributes["stage"].Value;
            _clearData.Add(stage);

        }
    }

    public void Write(string filePath)
    {

        XmlDocument Document = new XmlDocument();
        XmlElement FList = Document.CreateElement("ClearData");
        Document.AppendChild(FList);

        for (int i = 0; i < _clearData.Count; i++)
        {
            XmlElement FElement = Document.CreateElement("Clear");
            FElement.SetAttribute("stage", _clearData[i]);
            FList.AppendChild(FElement);
        }
        Document.Save(filePath);
    }

}