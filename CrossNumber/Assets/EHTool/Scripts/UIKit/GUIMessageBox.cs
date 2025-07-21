using System;

namespace EHTool.UIKit {
    
    public abstract class GUIMessageBox : GUIPopUp
    {

        private Action _buttonMethod;

        protected abstract void ShowMessage(string key);

        public override void Close()
        {
            SetOff();
        }

        public void CloseMessageBox()
        {
            SetOff();
        }

        public void SetMessage(string key, Action buttonMethod = null)
        {
            _buttonMethod = buttonMethod;
            _buttonMethod ??= CloseMessageBox;

            SetOn();
            ShowMessage(key);
        }

        public void MessageBoxButton()
        {
            _buttonMethod?.Invoke();
        }
    }
}