using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public struct UnitInfor {
    public string type;
    public Vector2Int pos;

    public UnitInfor(string t, Vector2Int p) {
        type = t;
        pos = p;
    }
}

public class Stage {

    public UnitInfor[] Units => _units;

    private UnitInfor[] _units;
    public string _nextStagePath;

    public Stage() { 
        _units = new UnitInfor[0];
    }

    public Stage(UnitInfor[] units) {
        _units = units;
    }

    public Stage(IDictionary<string, IDictionary<string, object>> data)
    {
        _units = new UnitInfor[data.Count];

        int i = 0;
        foreach (var d in data.Values)
        {
            _units[i].type = d["type"].ToString();
            _units[i].pos =new Vector2Int
                (int.Parse(d["xPos"].ToString()),
                int.Parse(d["yPos"].ToString()));
            i++;
        }

    }

    public Stage(XmlDocument xmlDoc)
    {
        XmlNodeList nodes = xmlDoc.SelectNodes("StageData/Unit");

        _units = new UnitInfor[nodes.Count];

        for (int i = 0; i < nodes.Count; i++)
        {
            string unitValue = nodes[i].Attributes["value"].Value;
            int x = int.Parse(nodes[i].Attributes["xPos"].Value);
            int y = int.Parse(nodes[i].Attributes["yPos"].Value);

            _units[i].type = unitValue;
            _units[i].pos = new Vector2Int(x, y);
        }

    }

    public IDictionary<string, object> ToDictionary() { 
        IDictionary<string, object> retval = 
            new Dictionary<string, object>();

        for (int i = 0; i < _units.Length; i++) {
            Dictionary<string, object> data = new Dictionary<string, object>() {
                { "type", _units[i].type },
                { "xPos", _units[i].pos.x },
                { "yPos", _units[i].pos.y }

            };
            retval.Add(i.ToString().ToString(), data);
        }
        
        return retval;
    }

    public XmlDocument ToXML() {
        
        XmlDocument Document = new XmlDocument();
        XmlElement FList = Document.CreateElement("StageData");
        
        Document.AppendChild(FList);

        for (int i = 0; i < _units.Length; i++)
        {
            XmlElement FElement = Document.CreateElement("Unit");
            FElement.SetAttribute("value", _units[i].type);
            FElement.SetAttribute("xPos",
                _units[i].pos.x.ToString());
            FElement.SetAttribute("yPos",
                _units[i].pos.y.ToString());

            FList.AppendChild(FElement);
        }

        return Document;
    }

}