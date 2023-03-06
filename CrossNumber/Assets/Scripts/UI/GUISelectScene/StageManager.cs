using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.XR;
using UnityEngine.Purchasing;

public class StageData {
    public string name;
    public string value;
}

public class StageManager : MonoSingleton<StageManager> {
    // Start is called before the first frame update

    public static int WorldCount { get; private set; }
    public static int WorldIdx = 0;
    public static int StageIdx = 0;

    class OverWorldData {
        internal string name;
        internal int size = 0;
        internal List<StageData> _stages;

        public void Read(XmlNode node)
        {
            name = node.Attributes["name"].Value;
            _stages = new List<StageData>();

            XmlNodeList nodes = node.SelectNodes("Stage");
            for (int i = 0; i < nodes.Count; i++)
            {
                StageData stageData = new StageData();
                stageData.name = nodes[i].Attributes["name"].Value;
                stageData.value = nodes[i].Attributes["value"].Value;

                _stages.Add(stageData);

            }
            size = nodes.Count;
        }

        public StageData ReturnStageData(int i) {
            return _stages[i];
        }
    }

    List<OverWorldData> _dic;

    protected override void OnCreate()
    {
        _dic = new List<OverWorldData>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/XML/StageListData.xml");

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

    public StageData GetStageData(int idx)
    {
        return _dic[WorldIdx].ReturnStageData(idx);

    }
    public StageData GetStageData() {
        return GetStageData(StageIdx);
    }
}
