using UnityEngine;
using System;

namespace EHTool.UIKit {
    
    public abstract class GUIMessageBox : GUIPopUp
    {

        private Action _buttonMethod;

        protected abstract void ShowMessage(string key);

        public void CloseMessageBox()
        {
            SetOff();
        }

        public void SetMessage(string key, Action buttonMethod = null)
        {
            buttonMethod ??= CloseMessageBox;

            SetOn();
            ShowMessage(key);

            _buttonMethod = buttonMethod;
        }

        public void MessageBoxButton()
        {
            _buttonMethod?.Invoke();

            if (_buttonMethod != CloseMessageBox) return;

            CloseMessageBox();
        }
    }
}