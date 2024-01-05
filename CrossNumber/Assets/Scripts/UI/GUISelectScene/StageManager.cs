using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StageMetaData {
    public string name;
    public string value;
}

public struct UnitInfor {
    public string type;
    public Vector3 pos;
}

public class LevelData {

    public UnitInfor[] units;
    public string _nextStagePath;

    public LevelData(string path) {
        _SetData(AssetOpener.ReadXML("StageData/" + path));
    }

    public LevelData(Dictionary<string, object> data)
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

    public LevelData(XmlDocument xmlDoc)
    {
        /*
        if (xmlDoc.Attributes["nextStage"] == null)
            _nextStagePath = null;
        else
            _nextStagePath = xmlDoc.Attributes["nextStage"].Value;
        */

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

public class StageManager : MonoSingleton<StageManager> {
    // Start is called before the first frame update

    public static int WorldCount { get; private set; }
    public static int WorldIdx = 0;

    class OverWorldData {
        internal string name;
        internal int size = 0;
        internal List<StageMetaData> _stages;

        public void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            _stages = new List<StageMetaData>();

            XmlNodeList nodes = node.SelectNodes("Stage");
            for (int i = 0; i < nodes.Count; i++)
            {
                StageMetaData stageData = new StageMetaData();
                stageData.name = nodes[i].Attributes["name"].Value;
                stageData.value = nodes[i].Attributes["value"].Value;

                _stages.Add(stageData);

            }
            size = nodes.Count;
        }

        public StageMetaData ReturnStageData(int i) {
            return _stages[i];
        }
    }

    List<OverWorldData> _dic;

    protected override void OnCreate()
    {
        _dic = new List<OverWorldData>();
        XmlDocument xmlDoc = AssetOpener.ReadXML("StageListData");

        XmlNodeList nodes = xmlDoc.SelectNodes("StageListData/OverWorld");

        for (int i = 0; i < nodes.Count; i++)
        {
            OverWorldData overworldData = new OverWorldData();
            overworldData.Read(nodes[i]);

            _dic.Add(overworldData);
        }

        WorldCount = nodes.Count;
    }

    public int GetStageCount() {
        return _dic[WorldIdx].size;
    }

    public string GetWorldName()
    {
        return _dic[WorldIdx].name;
    }

    public StageMetaData GetStageMetaData(int idx)
    {
        return _dic[WorldIdx].ReturnStageData(idx);
    }


}
