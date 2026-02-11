using System.Collections.Generic;
using UnityEngine;

namespace EasyH.Unity.UI
{
    public class GUIFullScreen : GUIWindow, IGUIFullScreen
    {

        protected IList<IGUIPopUp> _popupUI;
        protected IGUIPopUp _nowPopUp;
        protected IGUIPanel _nowPanel;

        protected bool _isSetting = false;

        virtual protected void Start()
        {
            if (_isSetting) return;

            Open();
        }

        public override void Open()
        {

            _isSetting = true;

            base.Open();
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
            _popupUI = new List<IGUIPopUp>();

            UIManager.Instance.OpenFullScreen(this);
        }

        public override void SetOn()
        {
            base.SetOn();
            _nowPopUp?.SetOn();
            _nowPanel?.SetOn();
        }
        public override void SetOff()
        {
            base.SetOff();

            _nowPopUp?.SetOff();
            _nowPanel?.SetOff();
        }

        public void AddPopUp(IGUIPopUp popUp)
        {
            if (_nowPopUp != null)
            {
                _popupUI.Add(_nowPopUp);
                _nowPopUp.SetOff();
            }
            _nowPopUp = popUp;
            _nowPopUp.SetOn();

        }

        public virtual void ClosePopUp(IGUIPopUp popUp)
        {

            if (_nowPopUp != popUp)
            {
                if (_popupUI.Contains(popUp))
                {
                    _popupUI.Remove(popUp);
                }
                return;
            }

            if (_popupUI.Count == 0)
            {
                _nowPopUp = null;
                return;
            }

            _nowPopUp = _popupUI[_popupUI.Count - 1];
            _nowPopUp.SetOn();
            _popupUI.RemoveAt(_popupUI.Count - 1);

        }

        public void AddPanel(IGUIPanel panel)
        {
            _nowPanel?.Close();
            _nowPanel = panel;
        }

        public void ClosePanel()
        {
            _nowPanel = null;
        }

        public override void Close()
        {
            UIManager.Instance.CloseFullScreen(this);

            foreach (IGUIPopUp popup in _popupUI)
            {
                popup.Close();
            }
            _nowPopUp?.Close();

            base.Close();
        }

    }
}