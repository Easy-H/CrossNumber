using UnityEngine;
using System;
using EHTool.LangKit;
using UnityEngine.UI;

namespace EHTool.UIKit {
    public class GUIDefaultMessageBox : GUIMessageBox {

        [SerializeField] Text _text;

        protected override void ShowMessage(string key) { 
            _text.text = key;
        }

    }
}