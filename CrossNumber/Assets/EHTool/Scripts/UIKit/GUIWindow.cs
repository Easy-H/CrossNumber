using System;
using UnityEngine;

namespace EHTool.UIKit {
    public class GUIWindow : MonoBehaviour, IGUI {
        // Start is called before the first frame update

        protected Action _setOnEvent;

        [SerializeField] private uint priority = 0;
        public uint Priority => priority;

        virtual public void SetOn()
        {
            gameObject.SetActive(true);
            _setOnEvent?.Invoke();
            _setOnEvent = null;
        }

        virtual public void SetOff()
        {
            if (gameObject == null) return;
            gameObject.SetActive(false);

        }

        public virtual void Open()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();

            rect.SetParent(GameObject.Find("Canvas").transform);
            rect.localScale = Vector3.one;
            rect.offsetMin = Vector3.zero;
            rect.offsetMax = Vector3.one;
            rect.anchorMax = Vector2.one;
        }

        public virtual void Open(Action callback)
        {
            Open();
            _setOnEvent += callback;
        }

        public virtual void Close()
        {
            Destroy(gameObject);
        }

        public virtual void OpenWindow(string key)
        {
            UIManager.Instance.OpenGUI<GUIWindow>(key);
        }

        public int CompareTo(IGUI other)
        {
            return -priority.CompareTo(other.Priority);
        }
    }

}