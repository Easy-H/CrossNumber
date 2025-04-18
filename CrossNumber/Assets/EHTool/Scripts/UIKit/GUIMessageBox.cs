using UnityEngine;
using System;
using EHTool.LangKit;

namespace EHTool.UIKit {
    public abstract class GUIMessageBox : GUIPopUp {

        Action _buttonMethod;

        public void SetMessage(string key)
        {
            SetMessage(key, CloseMessageBox);
        }

        public override void Close()
        {

        }

        public void CloseMessageBox()
        {
            base.Close();
        }

        public void SetMessage(string key, Action buttonMethod)
        {
            SetOn();
            ShowMessage(key);

            _buttonMethod = buttonMethod;
        }

        protected abstract void ShowMessage(string key);

        public void MessageBoxButton()
        {
            _buttonMethod?.Invoke();

            if (_buttonMethod != CloseMessageBox) return;

            CloseMessageBox();
        }
    }
}