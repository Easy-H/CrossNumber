using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLang : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeLang(string lang)
    {
        StringManager.Instance.LangSet(lang);
    }
}
