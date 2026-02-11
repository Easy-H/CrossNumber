using UnityEngine;
using System;
using UnityEngine.UI;

namespace EasyH.Unity.UI
{
    public class GUIDefaultMessageBox : GUIMessageBox
    {

        [SerializeField] Text _text;

        protected override void ShowMessage(string key)
        {
            _text.text = key;
        }

    }
}