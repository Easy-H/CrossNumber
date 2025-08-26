
using EHTool.DBKit;
using System.Collections.Generic;

[System.Serializable]
public class StageMetaData : IDictionaryable<StageMetaData>
{
    public string Name;
    public string Key;
    public string Type = "Local";
    public bool IsVerified;

    public StageMetaData() { }

    public StageMetaData(string n, string v, bool verified = true)
    {
        Name = n;
        Key = v;
        IsVerified = verified;
    }

    public IDictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> retval =
            new Dictionary<string, object>
            {
                {"name", Name},
                {"key", Key},
                {"verified", IsVerified}
            };
        return retval;
    }

    public void SetValueFromDictionary(IDictionary<string, object> value)
    {
        if (!value.ContainsKey("name") || !value.ContainsKey("key")) return;

        Name = value["name"].ToString();
        Key = value["key"].ToString();

    }

    public void SetType(string type)
    {
        Type = type;
    }

}