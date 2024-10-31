using UnityEngine.EventSystems;
using UnityEngine;

namespace EHTool.UIKit {
    public class GUIPanel : GUIWindow, IGUIPanel, IPointerEnterHandler, IPointerExitHandler {

        bool _mouseOver;

        public override void Open()
        {
            base.Open();
            UIManager.Instance.NowDisplay.AddPanel(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseOver = false;
        }

        public bool MouseOn() {
            return _mouseOver;
        }

        public override void Close()
        {
            UIManager.Instance.NowDisplay.ClosePanel();
            base.Close();

        }
    }
}