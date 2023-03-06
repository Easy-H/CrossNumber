using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUILangSelect : MonoBehaviour
{
    public void LangSet(string lang) {
        StringManager.Instance.ReadStringFromXml(lang);
    }
}
