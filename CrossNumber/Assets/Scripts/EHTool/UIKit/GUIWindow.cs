using UnityEngine;

namespace EHTool.UIKit {
    public class GUIWindow : MonoBehaviour , IGUI {
        // Start is called before the first frame update

        public int priority = 0;

        virtual public void SetOn()
        {
            gameObject.SetActive(true);
        }

        virtual public void SetOff()
        {
            if (gameObject == null) return;
            gameObject.SetActive(false);

        }

        public virtual void Open()
        {
            SetOn();

            RectTransform rect = gameObject.GetComponent<RectTransform>();

            rect.SetParent(GameObject.Find("Canvas").transform);
            rect.localScale = Vector3.one;
            rect.offsetMin = Vector3.zero;
            rect.offsetMax = Vector3.one;
            rect.anchorMax = Vector2.one;
        }

        public virtual void Close()
        {
            Destroy(gameObject);
        }

        public virtual void OpenWindow(string key)
        {
            UIManager.Instance.OpenGUI<GUIWindow>(key);
        }

    }

}