using EasyH.Tool.DBKit;
using System.Collections.Generic;

public class UserData : IDictionaryable<UserData>
{
    public int stageCnt;
    public IList<string> mapIdList;

    public UserData()
    {
        stageCnt = 0;
        mapIdList = new List<string>();
    }

    public void SetValueFromDictionary(IDictionary<string, object> value)
    {
        if (value["stage-list"] is IList<object> list)
        {
            foreach (var v in list)
            {
                UnityEngine.Debug.LogFormat("{0}", v);
                mapIdList.Add(v.ToString());
            }
        }
        UnityEngine.Debug.Log(value["stage-list"]);
        /*
        */
        stageCnt = int.Parse(value["stage-cnt"].ToString());
        UnityEngine.Debug.LogFormat("{0}", stageCnt);
        //mapIdList = value["stage-list"] as List<string>;
    }

    public IDictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> taskResult = new Dictionary<string, object>
            {
                {"stage-cnt", stageCnt },
                {"stage-list", mapIdList }
            };

        return taskResult;
    }

}