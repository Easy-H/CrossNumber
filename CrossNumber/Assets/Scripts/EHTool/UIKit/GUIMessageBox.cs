using UnityEngine;
using System;
using EHTool.LangKit;

namespace EHTool.UIKit {
    public class GUIMessageBox : GUIPopUp {

        [SerializeField] EHText _textField;

        Action _buttonMethod;

        public void SetMessage(string key)
        {
            SetMessage(key, CloseMessageBox);
        }

        public void SetMessage(string key, Action buttonMethod)
        {
            SetOn();
            _textField.SetText(key);

            _buttonMethod = buttonMethod;
        }

        public void MessageBoxButton()
        {
            _buttonMethod?.Invoke();

            if (_buttonMethod != CloseMessageBox) return;

            CloseMessageBox();
        }

        public override void Close()
        {

        }

        public void CloseMessageBox()
        {
            base.Close();
        }

    }
}