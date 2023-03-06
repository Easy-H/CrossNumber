using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangSetting : MonoBehaviour
{
    public void ChangeLang(string lang) { 
        StringManager.Instance.ChangeLang(lang);
    }
}
