using EHTool;
using UnityEngine;
using System.Xml;
using System.Collections.Generic;


public struct UnitInfor {
    public string type;
    public Vector3 pos;
}

public class StageMetaData {
    public string name;
    public string value;
}

public class StageData {

    public UnitInfor[] units;
    public string _nextStagePath;

    public StageData() { 
        units = new UnitInfor[0];
    }

    public StageData(string path) {
        _SetData(AssetOpener.ReadXML("StageData/" + path));
    }

    public StageData(Dictionary<string, object> data)
    {
        units = new UnitInfor[data.Count];

        int i = 0;
        foreach (object d in data.Values)
        {
            Dictionary<string, object> dd = d as Dictionary<string, object>;
            
            units[i].type = dd["type"].ToString();
            units[i].pos = new Vector3(int.Parse(dd["xPos"].ToString()), int.Parse(dd["yPos"].ToString()));
            i++;
        }

    }

    public StageData(XmlDocument xmlDoc)
    {
        _SetData(xmlDoc);

    }

    public Dictionary<string, object> ToDictionary() { 
        Dictionary<string, object> retval =  new Dictionary<string, object>();

        for (int i = 0; i < units.Length; i++) {
            Dictionary<string, object> data = new Dictionary<string, object>() {
                { "type", units[i].type },
                { "xPos", units[i].pos.x },
                { "yPos", units[i].pos.y }

            };
            retval.Add(i.ToString().ToString(), data);
        }
        
        return retval;
    }

    private void _SetData(XmlDocument xmlDoc) {

        XmlNodeList nodes = xmlDoc.SelectNodes("StageData/Unit");

        units = new UnitInfor[nodes.Count];

        for (int i = 0; i < nodes.Count; i++)
        {
            string unitValue = nodes[i].Attributes["value"].Value;
            int x = int.Parse(nodes[i].Attributes["xPos"].Value);
            int y = int.Parse(nodes[i].Attributes["yPos"].Value);

            units[i].type = unitValue;
            units[i].pos = new Vector3(x, y);
        }
    }

}